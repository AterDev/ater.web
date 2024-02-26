﻿namespace Entity.System;
/// <summary>
/// 组织结构
/// </summary>
[Module(Modules.System)]
[Index(nameof(Name))]
public class SystemOrganization : IEntityBase, ITreeNode<SystemOrganization>
{
    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// 子目录
    /// </summary>
    public List<SystemOrganization> Children { get; set; } = [];

    /// <summary>
    /// 父目录
    /// </summary>
    [ForeignKey(nameof(ParentId))]
    public SystemOrganization? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public ICollection<SystemUser> Users { get; set; } = [];
    public Guid Id { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset UpdatedTime { get; set; }
    public bool IsDeleted { get; set; }
}
