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
                    var imagePaths = product.Image1?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                    var uploadedImageUrls = new List<string>();
                    bool isFirstImage = true;

                    foreach (var imagePath in imagePaths)
                    {
                        if (isFirstImage)
                        {
                            var image1000 = GenerateImageUrl("1000", imagePath);
                            var image75 = GenerateImageUrl("75", imagePath);
                            var image180 = GenerateImageUrl("180", imagePath);

                            product.Image1 = await _imageService.UploadImageAsync(image1000);
                            product.Image75 = await _imageService.UploadImageAsync(image75);
                            product.Image180 = await _imageService.UploadImageAsync(image180);

                            isFirstImage = false;
                        }
                        else
                        {
                            var uploadedImageUrl = await _imageService.UploadImageAsync(GenerateImageUrl("1000", imagePath));
                            uploadedImageUrls.Add(uploadedImageUrl);
                        }
                    }

                    if (uploadedImageUrls.Count > 1) product.Image2 = uploadedImageUrls.ElementAtOrDefault(1);
                    if (uploadedImageUrls.Count > 2) product.Image3 = uploadedImageUrls.ElementAtOrDefault(2);
                    if (uploadedImageUrls.Count > 3) product.Image4 = uploadedImageUrls.ElementAtOrDefault(3);
                    if (uploadedImageUrls.Count > 4) product.Image5 = uploadedImageUrls.ElementAtOrDefault(4);
                    if (uploadedImageUrls.Count > 5) product.Image6 = uploadedImageUrls.ElementAtOrDefault(5);
                    if (uploadedImageUrls.Count > 6) product.Image7 = uploadedImageUrls.ElementAtOrDefault(6);
                    if (uploadedImageUrls.Count > 7) product.Image8 = uploadedImageUrls.ElementAtOrDefault(7);

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

            for (int row = 2; row <= worksheet.Dimension.End.Row - 4845; row++) // מתחילים בשורה 2 כדי לדלג על הכותרות
            {
                var fullName = worksheet.Cells[row, 2].Text;

                if (string.IsNullOrEmpty(fullName))
                    continue;

                var product = new Product
                {
                    Name = fullName,
                    ShortDescription = worksheet.Cells[row, 3].Text,
                    Image1 = worksheet.Cells[row, 4].Text,
                    Price = double.TryParse(worksheet.Cells[row, 5].Text, out var price) ? price : 0,
                    OldPrice = double.TryParse(worksheet.Cells[row, 6].Text, out var oldPrice) ? oldPrice : 0,
                    Quantity = int.TryParse(worksheet.Cells[row, 8].Text, out var quantity) ? quantity : 0,
                    Badge = worksheet.Cells[row, 9].Text,
                    Rating = double.TryParse(worksheet.Cells[row, 10].Text, out var rating) ? rating : 0
                };

                products.Add(product);
            }

            return products;
        }

        private string GenerateImageUrl(string resolution, string sourcePath)
        {
            return $"https://static.wixstatic.com/media/{sourcePath}/v1/fill/w_{resolution},h_{resolution},al_c,q_100,usm_0.66_1.00_0.01,enc_auto/.webp";
        }

        private string ExtractHebrewText(string input) => string.Concat(input.Where(c => c >= 'א' && c <= 'ת'));

        private string ExtractEnglishText(string input) => string.Concat(input.Where(c => char.IsLetter(c) && c < 'א'));
    }
}