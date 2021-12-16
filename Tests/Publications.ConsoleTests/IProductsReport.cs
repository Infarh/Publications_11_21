namespace Publications.ConsoleTests;

public interface IProductsReport
{
    string CatalogName { get; set; }
    DateTime CreationDate { get; set; }
    IReadOnlyCollection<Product> Products { get; set; }
    FileInfo Create(string ReportFile);
}
