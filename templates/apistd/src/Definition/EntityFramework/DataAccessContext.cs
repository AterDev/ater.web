using EntityFramework.DBProvider;

namespace EntityFramework;
/// <summary>
/// 数据访问层抽象
/// </summary>
public class DataAccessContext<TEntity>(CommandDbContext commandDbContext, QueryDbContext queryDbContext) where TEntity : class, IEntityBase
{
    private readonly CommandDbContext _commandDbContext = commandDbContext;
    private readonly QueryDbContext _queryDbContext = queryDbContext;

    public QueryDbContext QueryContext { get; init; } = queryDbContext;
    public CommandDbContext CommandContext { get; init; } = commandDbContext;

    public int MyProperty { get; set; }

    public QuerySet<TEntity> QuerySet() => new(_queryDbContext);
    public CommandSet<TEntity> CommandSet() => new(_commandDbContext);
}
