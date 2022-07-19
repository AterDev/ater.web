namespace Ater.Web.Contract;
public static class Extensions
{
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, Dictionary<string, bool> dic)
    {
        IOrderedQueryable<T> orderQuery = default!;
        var parameter = Expression.Parameter(typeof(T), "e");
        foreach (var item in dic)
        {
            var prop = Expression.PropertyOrField(parameter, item.Key);
            var body = Expression.MakeMemberAccess(parameter, prop.Member);
            var selector = Expression.Lambda(body, parameter);


            MethodCallExpression expression;
            if (item.Value)
            {
                expression = Expression.Call(typeof(Queryable),
                                          "OrderBy",
                                          new Type[] { typeof(T), body.Type },
                                          query.Expression,
                                          Expression.Quote(selector));
            }
            else
            {
                expression = Expression.Call(typeof(Queryable),
                                          "OrderByDescending",
                                          new Type[] { typeof(T), body.Type },
                                          query.Expression,
                                          Expression.Quote(selector));
            }
            orderQuery = (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(expression);
            query = orderQuery;
        }
        return orderQuery;
    }
}
