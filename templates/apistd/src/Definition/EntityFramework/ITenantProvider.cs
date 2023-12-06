namespace EntityFramework;
public interface ITenantProvider
{
    public Guid TenantId { get; set; }
}
