using Core.Entities.SystemEntities;
namespace Application.Implement;
public class DataStoreContext
{
    public QueryDbContext QueryContext { get; init; }
    public CommandDbContext CommandContext { get; init; }



    /// <summary>
    /// 绑在对象
    /// </summary>
    private readonly Dictionary<string, object> StoreCache = new();

    public DataStoreContext(
        BlogQueryStore blogQuery,
        SystemRoleQueryStore systemRoleQuery,
        SystemUserQueryStore systemUserQuery,
        BlogCommandStore blogCommand,
        SystemRoleCommandStore systemRoleCommand,
        SystemUserCommandStore systemUserCommand,

        QueryDbContext queryDbContext,
        CommandDbContext commandDbContext
    )
    {
        QueryContext = queryDbContext;
        CommandContext = commandDbContext;
        AddCache(blogQuery);
        AddCache(systemRoleQuery);
        AddCache(systemUserQuery);
        AddCache(blogCommand);
        AddCache(systemRoleCommand);
        AddCache(systemUserCommand);

    }

    public async Task<int> SaveChangesAsync()
    {
        return await CommandContext.SaveChangesAsync();
    }

    public QuerySet<TEntity> QuerySet<TEntity>() where TEntity : EntityBase
    {
        var typename = typeof(TEntity).Name + "QueryStore";
        var set = GetSet(typename);
        return set == null 
            ? throw new ArgumentNullException($"{typename} class object not found") 
            : (QuerySet<TEntity>)set;
    }
    public CommandSet<TEntity> CommandSet<TEntity>() where TEntity : EntityBase
    {
        var typename = typeof(TEntity).Name + "CommandStore";
        var set = GetSet(typename);
        return set == null 
            ? throw new ArgumentNullException($"{typename} class object not found") 
            : (CommandSet<TEntity>)set;
    }

    private void AddCache(object set)
    {
        var typeName = set.GetType().Name;
        if (!StoreCache.ContainsKey(typeName))
        {
            StoreCache.Add(typeName, set);
        }
    }

    private object GetSet(string type)
    {
        return StoreCache[type];
    }
}
