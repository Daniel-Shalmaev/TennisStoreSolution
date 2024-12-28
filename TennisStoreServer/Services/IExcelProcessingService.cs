using OfficeOpenXml;
using TennisStoreServer.Repositories;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreServer.Services
{
    public interface IExcelProcessingService
    {
        Task<ServiceResponse> ProcessExcelFileAsync(IFormFile file);
    }

    public class ExcelProcessingService : IExcelProcessingService
    {
        private readonly IImageService _imageService;
        private readonly IProduct _productService;

        public ExcelProcessingService(
            IImageService imageService,
            IProduct productService)
        {
            _imageService = imageService;
            _productService = productService;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<ServiceResponse> ProcessExcelFileAsync(IFormFile file)
        {
            try
            {
                var products = ReadProductsFromExcel(file);

                foreach (var product in products)
                {
                    // פיצול התמונות לשדות Image1 עד Image8
                    var images = product.Image1?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                    var imageLinks = new List<string>();

                    foreach (var base64Image in images)
                    {
                        // העלאת התמונה ל-Imgur
                        var uploadedImageUrl = await _imageService.UploadImageAsync(base64Image);
                        imageLinks.Add(uploadedImageUrl);
                    }

                    // עדכון השדות Image1 עד Image8 עם הקישורים שהתקבלו
                    if (imageLinks.Count > 0) product.Image1 = imageLinks.ElementAtOrDefault(0);
                    if (imageLinks.Count > 1) product.Image2 = imageLinks.ElementAtOrDefault(1);
                    if (imageLinks.Count > 2) product.Image3 = imageLinks.ElementAtOrDefault(2);
                    if (imageLinks.Count > 3) product.Image4 = imageLinks.ElementAtOrDefault(3);
                    if (imageLinks.Count > 4) product.Image5 = imageLinks.ElementAtOrDefault(4);
                    if (imageLinks.Count > 5) product.Image6 = imageLinks.ElementAtOrDefault(5);
                    if (imageLinks.Count > 6) product.Image7 = imageLinks.ElementAtOrDefault(6);
                    if (imageLinks.Count > 7) product.Image8 = imageLinks.ElementAtOrDefault(7);

                    // שמירת המוצר
                    await _productService.AddProduct(product);
                }

                return new ServiceResponse(true, "Excel processing completed successfully.");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, $"Error processing Excel: {ex.Message}");
            }
        }

        private IEnumerable<Product> ReadProductsFromExcel(IFormFile file)
        {
            var products = new List<Product>();

            using var stream = file.OpenReadStream();
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0]; // גישה לדף הראשון

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // מתחילים בשורה 2 כדי לדלג על הכותרות
            {
                var fullName = worksheet.Cells[row, 2].Text;

                // חלוקה של השם לשם בעברית (SubName) ובאנגלית (Name)
                var subName = ExtractHebrewText(fullName);
                var name = ExtractEnglishText(fullName);

                var product = new Product
                {
                    Name = name,
                    SubName = subName,
                    ShortDescription = worksheet.Cells[row, 3].Text,
                    LongDescription = worksheet.Cells[row, 4].Text,
                    Price = double.TryParse(worksheet.Cells[row, 5].Text, out var price) ? price : 0,
                    OldPrice = double.TryParse(worksheet.Cells[row, 6].Text, out var oldPrice) ? oldPrice : 0,
                    Image1 = worksheet.Cells[row, 7].Text,
                    Quantity = int.TryParse(worksheet.Cells[row, 8].Text, out var quantity) ? quantity : 0,
                    Badge = worksheet.Cells[row, 9].Text,
                    Rating = double.TryParse(worksheet.Cells[row, 10].Text, out var rating) ? rating : 0
                };

                products.Add(product);
            }

            return products;
        }

        private string ExtractHebrewText(string input) => string.Concat(input.Where(c => c >= 'א' && c <= 'ת'));

        private string ExtractEnglishText(string input) => string.Concat(input.Where(c => char.IsLetter(c) && c < 'א'));
    }
}