using System.ComponentModel.DataAnnotations;

namespace Ater.Web.Contract.Models;
public class EntityBase<TId>
{
    [Key]
    public TId Id { get; set; } = default!;

}
public class EntityBase : EntityBase<Guid>
{
}


