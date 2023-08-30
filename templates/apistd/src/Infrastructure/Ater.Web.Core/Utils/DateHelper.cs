namespace Ater.Web.Core.Utils;
/// <summary>
/// 日期帮助类
/// </summary>
public class DateHelper
{
    /// <summary>
    /// 获取本周日期范围
    /// </summary>
    /// <returns></returns>
    public static (DateOnly startDate, DateOnly endDate) GetCurrentWeekDate()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var weekDay = now.GetWeekDay();
        var startDate = now.AddDays(-weekDay + 1).ToDateOnly();
        var endDate = now.AddDays(7 - weekDay).ToDateOnly();
        return (startDate, endDate);
    }

    /// <summary>
    /// 获取本月日期范围
    /// </summary>
    /// <returns></returns>
    public static (DateOnly startDate, DateOnly endDate) GetCurrentMonthDate()
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var startDate = new DateOnly(now.Year, now.Month, 1);
        DateOnly endDate = startDate.AddMonths(1).AddDays(-1);
        return (startDate, endDate);
    }
}
