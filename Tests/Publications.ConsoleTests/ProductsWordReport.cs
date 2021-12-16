using TemplateEngine.Docx;

namespace Publications.ConsoleTests
{
    public class ProductsWordReport
    {
        private const string __CatalogNameField = "CatalogName";
        private const string __ProductsCountField = "ProductsCount";
        private const string __CreationDateField = "CreationDate";

        private const string __ProductsField = "Products";
        private const string __ProductIdField = "ProductId";
        private const string __ProductNameField = "ProductName";
        private const string __ProductDescriptionField = "ProductDescription";
        private const string __ProductPriceField = "ProductPrice";
        private const string __TotalPriceField = "TotalPrice";

        private readonly FileInfo _TemplateFile;

        public string CatalogName { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public IReadOnlyCollection<Product> Products { get; set; }

        public ProductsWordReport(FileInfo TemplateFile) => _TemplateFile = TemplateFile;

        public FileInfo Create(string ReportFile)
        {
            var report_file = new FileInfo(ReportFile);
            if(report_file.Exists) 
                report_file.Delete();

            _TemplateFile.CopyTo(report_file.FullName);

            var content = new Content(
                new FieldContent(__CatalogNameField, CatalogName),
                new FieldContent(__ProductsCountField, Products.Count.ToString()),
                new FieldContent(__CreationDateField, CreationDate.ToShortDateString())
                );

            using var doc = new TemplateProcessor(report_file.FullName)
               .SetRemoveContentControls(true);

            doc.FillContent(content);
            doc.SaveChanges();

            report_file.Refresh();
            return report_file;
        }
    }
}
