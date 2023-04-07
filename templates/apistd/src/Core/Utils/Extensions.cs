using Mapster;

namespace Core.Utils;
public static partial class Extensions
{
    /// <summary>
    /// 将被合并对象中非空属性值，合并到源对象
    /// </summary>
    /// <typeparam name="TSource">源对象</typeparam>
    /// <typeparam name="TMerge">被合并对象</typeparam>
    /// <param name="source"></param>
    /// <param name="merge"></param>
    /// <param name="ignoreNull">是否忽略null</param>
    /// <returns></returns>
    public static TSource Merge<TSource, TMerge>(this TSource source, TMerge merge, bool ignoreNull = true)
    {
        if (ignoreNull)
        {
            _ = TypeAdapterConfig<TMerge, TSource>
                .NewConfig()
                .IgnoreNullValues(true);
        }
        return merge.Adapt(source);
    }

    /// <summary>
    /// 类型转换
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static TDestination MapTo<TSource, TDestination>(this TSource source) where TDestination : class
    {
        _ = TypeAdapterConfig<TSource, TDestination>
           .NewConfig()
           .IgnoreNullValues(true);
        return source.Adapt<TSource, TDestination>();
    }

    /// <summary>
    /// 构造查询Dto
    /// 重要: dto中属性名称和类型必须与实体一致
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source)
    {
        Type sourceType = typeof(TSource);
        Type resultType = typeof(TResult);
        ParameterExpression parameter = Expression.Parameter(sourceType, "e");

        // 只构造都存在的属性
        List<string> sourceNames = sourceType.GetProperties()
            .Select(s => s.Name).ToList();
        List<System.Reflection.PropertyInfo> props = resultType.GetProperties().ToList();
        props = props.Where(p => sourceNames.Contains(p.Name)).ToList();
        //props = props.Intersect(sourceProps).ToList();

        List<MemberAssignment> bindings = props.Select(p =>
             Expression.Bind(p, Expression.PropertyOrField(parameter, p.Name))
        ).ToList();
        MemberInitExpression body = Expression.MemberInit(Expression.New(resultType), bindings);
        LambdaExpression selector = Expression.Lambda(body, parameter);
        return source.Provider.CreateQuery<TResult>(
            Expression.Call(typeof(Queryable), "Select", new Type[] { sourceType, resultType },
                source.Expression, Expression.Quote(selector)));
    }


    /// <summary>
    /// 不为空时执行的条件
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="field">不为空的字段</param>
    /// <param name="expression">不为空时执行的条件</param>
    /// <returns></returns>
    public static IQueryable<TSource> WhereNotNull<TSource>(this IQueryable<TSource> source, object? field, Expression<Func<TSource, bool>> expression)
    {
        return field != null ? source.Where(expression) : source;
    }

    public static IQueryable<TResult> ProjectTo<TResult>(this IQueryable source)
    {
        return source.ProjectToType<TResult>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, Dictionary<string, bool> dic)
    {
        IOrderedQueryable<T> orderQuery = default!;
        ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
        int count = 0;
        foreach (KeyValuePair<string, bool> item in dic)
        {
            MemberExpression prop = Expression.PropertyOrField(parameter, item.Key);
            MemberExpression body = Expression.MakeMemberAccess(parameter, prop.Member);
            LambdaExpression selector = Expression.Lambda(body, parameter);
            MethodCallExpression expression = count > 0
                ? item.Value
                    ? Expression.Call(typeof(Queryable),
                                              "ThenBy",
                                              new Type[] { typeof(T), body.Type },
                                              query.Expression,
                                              Expression.Quote(selector))
                    : Expression.Call(typeof(Queryable),
                                              "ThenByDescending",
                                              new Type[] { typeof(T), body.Type },
                                              query.Expression,
                                              Expression.Quote(selector))
                : item.Value
                    ? Expression.Call(typeof(Queryable),
                                              "OrderBy",
                                              new Type[] { typeof(T), body.Type },
                                              query.Expression,
                                              Expression.Quote(selector))
                    : Expression.Call(typeof(Queryable),
                                              "OrderByDescending",
                                              new Type[] { typeof(T), body.Type },
                                              query.Expression,
                                              Expression.Quote(selector));
            orderQuery = (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(expression);
            query = orderQuery;
            count++;
        }
        return orderQuery;
    }


    /// <summary>
    /// 列表转成树型结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public static List<T> BuildTree<T>(this List<T> nodes) where T : ITreeNode<T>
    {
        nodes.ForEach(n => { n.Children = null; });
        Dictionary<Guid, T> nodeDict = nodes.ToDictionary(n => n.Id);
        List<T> res = new();

        foreach (T node in nodes)
        {
            if (node.ParentId == null)
            {
                res.Add(node);
            }
            else
            {
                if (nodeDict[node.ParentId.Value].Children == null)
                {
                    nodeDict[node.ParentId.Value].Children = new List<T>();
                }
                nodeDict[node.ParentId.Value].Children!.Add(node);
            }
        }
        return res;
    }
}
