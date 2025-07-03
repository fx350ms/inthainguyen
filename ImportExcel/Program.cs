


// Required NuGet: DocumentFormat.OpenXml (install via NuGet)

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ImportExcel;
using InTN.EntityFrameworkCore;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Start Begin");

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var basePath = AppContext.BaseDirectory;
                config.SetBasePath(basePath);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("Default");
                services.AddDbContext<InTNDbContext>(options =>
                    options.UseSqlServer(connectionString));
            })
            .Build();

        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<InTNDbContext>();

        try
        {
            Console.WriteLine("Kết nối cơ sở dữ liệu thành công!");

            // Đường dẫn file Excel
            var filePath = Path.Combine(AppContext.BaseDirectory, "products.xlsx");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Không tìm thấy file Excel: " + filePath);
                return;
            }

            Console.WriteLine("Đang đọc dữ liệu từ Excel...");
            var reader = new ExcelReader();
            var rows = reader.ReadExcel(filePath);

            if (rows.Count == 0)
            {
                Console.WriteLine("Không có dữ liệu để import.");
                return;
            }

            Console.WriteLine($"Tổng số dòng đọc được: {rows.Count}");

            var importer = new ProductImporter(dbContext);
            await importer.ImportAsync(rows);

          //  importer.CreatePropertyMatrix("LOẠI:Decal Nhựa Trắng/ trong|CÁN MÀNG:K màng|SỐ LƯỢNG:1-6 tờ");

            Console.WriteLine("Hoàn tất import sản phẩm.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi trong quá trình import: {ex.Message}\n{ex.StackTrace}");
        }
    }
}