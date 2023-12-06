using Entity.CMS;
using Entity.FileManager;
using Entity.Order;

namespace EntityFramework.DBProvider;
public partial class ContextBase
{
    public DbSet<FileData> FileDatas { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Catalog> Catalogs { get; set; }
}
