using Mapster;

namespace Core.Utils;
public static partial class Extensions
{
    /// <summary>
    /// set source property value as merge type value, ignore null property
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TMerge"></typeparam>
    /// <param name="source"></param>
    /// <param name="merge"></param>
    /// <returns></returns>
    public static TSource Merge<TSource, TMerge>(this TSource source, TMerge merge)
    {
        TypeAdapterConfig<TMerge, TSource>
            .NewConfig()
            .IgnoreNullValues(true);
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
        TypeAdapterConfig<TSource, TDestination>
           .NewConfig()
           .IgnoreNullValues(true);
        return source.Adapt<TSource, TDestination>();
    }

    /// <summary>
    /// select dto properties
    /// important: dto and entity property(name and type) must same
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source)
    {
        var sourceType = typeof(TSource);
        var resultType = typeof(TResult);
        var parameter = Expression.Parameter(sourceType, "e");

        // 只构造都存在的属性
        var sourceNames = sourceType.GetProperties()
            .Select(s => s.Name).ToList();
        var props = resultType.GetProperties().ToList();
        props = props.Where(p => sourceNames.Contains(p.Name)).ToList();
        //props = props.Intersect(sourceProps).ToList();

        var bindings = props.Select(p =>
             Expression.Bind(p, Expression.PropertyOrField(parameter, p.Name))
        ).ToList();
        var body = Expression.MemberInit(Expression.New(resultType), bindings);
        var selector = Expression.Lambda(body, parameter);
        return source.Provider.CreateQuery<TResult>(
            Expression.Call(typeof(Queryable), "Select", new Type[] { sourceType, resultType },
                source.Expression, Expression.Quote(selector)));

    }


}
