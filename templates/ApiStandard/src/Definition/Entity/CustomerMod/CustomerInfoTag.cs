namespace Entity.CustomerMod;
/// <summary>
/// 客户标签关联表
/// </summary>
[Module(Modules.Customer)]
public class CustomerInfoTag
{
    public Guid CustomerInfoId { get; set; }
    public Guid CustomerTagId { get; set; }
}
