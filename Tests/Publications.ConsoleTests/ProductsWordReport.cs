namespace Publications.ConsoleTests
{
    public class ProductsWordReport
    {
        private const string __CatalogNameField = "CatalogName";
        private const string __ProductsCountField = "ProductsCount";
        private const string __CreationDateField = "CreationDate";

        private FileInfo _TemplateFile;

        public string CatalogName { get; set; } = null!;

        public int ProductsCount { get; set; }

        public DateTime CreationDate { get; set; }

        public ProductsWordReport(FileInfo TemplateFile) => _TemplateFile = TemplateFile;

        public FileInfo Create(string ReportFile)
        {
            var report_file = new FileInfo(ReportFile);
            if(report_file.Exists) 
                report_file.Delete();

            _TemplateFile.CopyTo(report_file.FullName);




            report_file.Refresh();
            return report_file;
        }
    }
}
