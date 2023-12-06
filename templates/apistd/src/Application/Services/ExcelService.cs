﻿using OfficeOpenXml;
using OfficeOpenXml.Export.ToCollection;

namespace Application.Services;
/// <summary>
/// excel 操作类
/// </summary>
public class ExcelService
{
    public const string MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public ExcelService()
    {

    }

    /// <summary>
    /// 快捷导出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="sheetName"></param>
    /// <param name="hasTitle">是否包含标题</param>
    /// <returns></returns>
    public static async Task<Stream> ExportAsync<T>(IEnumerable<T> data, string sheetName = "sheet1", bool hasTitle = true)
    {
        var stream = new MemoryStream();
        using (var package = new ExcelPackage(stream))
        {
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add(sheetName);
            var list = data.ToList();
            sheet.Cells[1, 1].LoadFromCollection(list, hasTitle);
            await package.SaveAsync();
        }
        stream.Position = 0;
        return stream;
    }

    /// <summary>
    /// Stream to List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"></param>
    /// <param name="sheetName"></param>
    /// <param name="hasTitle">是否包含标题</param>
    /// <returns></returns>
    public static List<T> ImportAsync<T>(Stream stream, string? sheetName = null, bool hasTitle = true)
    {
        var data = new List<T>();
        using var package = new ExcelPackage(stream);
        ExcelWorksheet sheet = sheetName == null ? package.Workbook.Worksheets[0] : package.Workbook.Worksheets[sheetName];

        int rows = sheet.Dimension.Rows;
        int columns = sheet.Dimension.Columns;

        var range = sheet.Dimension.Address;

        data = sheet.Cells[range].ToCollection<T>(options =>
        {
            options.HeaderRow = hasTitle ? 0 : 1;
            options.DataStartRow = hasTitle ? 1 : 0;
            options.ConversionFailureStrategy = ToCollectionConversionFailureStrategy.SetDefaultValue;
        });
        return data;
    }
}
