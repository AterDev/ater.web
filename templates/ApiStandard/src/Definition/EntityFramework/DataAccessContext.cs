using Ater.Web.Abstraction.EntityFramework;

using EntityFramework.DBProvider;

namespace EntityFramework;
/// <summary>
/// 数据访问上下文
/// </summary>
public class DataAccessContext<TEntity>(CommandDbContext commandDbContext, QueryDbContext queryDbContext)
    : DataAccessContextBase<CommandDbContext, QueryDbContext, TEntity>(commandDbContext, queryDbContext)
    where TEntity : class, IEntityBase
{
}
