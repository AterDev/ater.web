﻿
namespace Ater.Web.Core.Attributes;
/// <summary>
/// 模块标记
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ModuleAttribute : Attribute
{
    /// <summary>
    /// 模块名称，区分大小写
    /// </summary>
    public string Name { get; init; }

    public ModuleAttribute(string name)
    {
        Name = name;
    }
}