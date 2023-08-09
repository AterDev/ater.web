namespace Ater.Web.Core.Utils;
/// <summary>
/// 日期时间扩展
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// dateOnly转DateTimeOffset
    /// </summary>
    /// <param name="dateOnly"></param>
    /// <param name="zone"></param>
    /// <returns></returns>
    public static DateTimeOffset ToDateTimeOffset(this DateOnly dateOnly, TimeZoneInfo? zone = null)
    {
        zone ??= TimeZoneInfo.Local;
        var dateTime = dateOnly.ToDateTime(new TimeOnly(0));
        return new DateTimeOffset(dateTime, zone.GetUtcOffset(dateTime));
    }

    public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime, TimeZoneInfo? zone = null)
    {
        zone ??= TimeZoneInfo.Local;
        return new DateTimeOffset(dateTime, zone.GetUtcOffset(dateTime));
    }

}
