﻿using System.Linq.Expressions;
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
        // get merge properties(not null) and values
        var mergeProps = typeof(TMerge).GetProperties().ToList();
        mergeProps = mergeProps
            .Where(p => p.GetValue(merge) != null)
            .ToList();
        // set source properties's value
        mergeProps.ForEach(p =>
        {
            var sourceProp = typeof(TSource).GetProperty(p.Name);
            if (sourceProp != null)
                sourceProp.SetValue(source, p.GetValue(merge), null);
        });
        return source;
    }


    /// <summary>
    /// select dto properties
    /// important: dto and entity property(name & type) must same
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source)
    {
        var sourceType = typeof(TSource);
        var resultType = typeof(TResult);
        var parameter = Expression.Parameter(sourceType, "e");

        var props = resultType.GetProperties().ToList();
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
