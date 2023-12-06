using EntityFramework.DBProvider;

namespace EntityFramework;
/// <summary>
/// 数据访问层抽象
/// </summary>
public class DataAccessContext<TEntity> where TEntity : class, IEntityBase
{
    private readonly CommandDbContext _commandDbContext;
    private readonly QueryDbContext _queryDbContext;

    public QueryDbContext QueryContext { get; init; }
    public CommandDbContext CommandContext { get; init; }

    public int MyProperty { get; set; }
    public DataAccessContext(CommandDbContext commandDbContext, QueryDbContext queryDbContext)
    {
        _commandDbContext = commandDbContext;
        _queryDbContext = queryDbContext;
        QueryContext = queryDbContext;
        CommandContext = commandDbContext;
    }

    public QuerySet<TEntity> QuerySet() => new(_queryDbContext);
    public CommandSet<TEntity> CommandSet() => new(_commandDbContext);
}
