namespace EntityFramework;

public partial class IdentityContext : DbContext
{
    public IdentityContext()
    {
    }

    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }

}
