using RazorEngine;
using RazorEngine.Templating;

namespace Publications.ConsoleTests;

public class ProductsRazorReport
    : IProductsReport
{
    public string CatalogName { get; set; } = null!;

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public IReadOnlyCollection<Product> Products { get; set; }

    public FileInfo Create(string ReportFile)
    {
        var report_file = new FileInfo(ReportFile);
        if(report_file.Exists)
            report_file.Delete();

        const string template = @"Каталог товаров
Название: @Model.CatalogName
Число товаров: @Model.Products.Count

Дата формирования: @Model.CreationDate

id    Name    Description    Price
@foreach(var product in Model.Products)
{
@product.Id    @product.Name    @product.Description    @product.Price.ToString(""C"")
}
";
        var xml_template = File.ReadAllText("xml.template.txt");
        var result = Engine.Razor.RunCompile(xml_template, "ProductsReport", null, new
        {
            CatalogName,
            CreationDate,
            Products,
        });

        using (var writer = report_file.CreateText())
            writer.Write(result);

        report_file.Refresh();
        return report_file;
    }
}