using System.Reflection;

namespace Ater.Web.Core.Utils;
/// <summary>
/// 枚举帮助类
/// </summary>
public static class EnumHelper
{
    /// <summary>
    /// 枚举转数组
    /// </summary>
    /// <returns></returns>
    public static List<EnumDictionary> ToList(Type type)
    {
        List<EnumDictionary> result = new();
        var enumNames = Enum.GetNames(type);
        Array values = Enum.GetValues(type);

        if (enumNames != null)
        {
            for (var i = 0; i < enumNames.Length; i++)
            {
                FieldInfo? fi = type.GetField(enumNames[i]);
                if (fi != null)
                {
                    if (fi.GetCustomAttribute(typeof(DescriptionAttribute), true) is DescriptionAttribute attribute)
                    {
                        result.Add(new EnumDictionary
                        {
                            Name = fi.Name,
                            Description = attribute.Description,
                            Value = Convert.ToInt32(values.GetValue(i))
                        });
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 获取程序集中所有枚举类型
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static Type[] GetEnumTypes(this Assembly assembly)
    {
        return assembly.GetTypes().Where(t => t.IsEnum).ToArray();
    }

    /// <summary>
    /// 获取程序集所有枚举信息
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, List<EnumDictionary>> GetAllEnumInfo()
    {
        Dictionary<string, List<EnumDictionary>> res = new();
        Type[] types = Assembly.GetExecutingAssembly().GetEnumTypes();
        foreach (Type type in types)
        {
            List<EnumDictionary> infos = ToList(type);
            res.Add(type.Name, infos);
        }
        return res;
    }

}

public struct EnumDictionary
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Value { get; set; }
}
