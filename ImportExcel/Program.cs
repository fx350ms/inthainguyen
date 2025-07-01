//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using InTN.EntityFrameworkCore;
//using Abp.Domain.Repositories;
//using InTN.Entities;

//namespace ImportExcel
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Start Begin");

//            // Tạo host để cấu hình dịch vụ và đọc cấu hình
//            var host = Host.CreateDefaultBuilder(args)
//                .ConfigureAppConfiguration((context, config) =>
//                {
//                    // Đảm bảo đường dẫn file appsettings.json chính xác
//                    var basePath = AppContext.BaseDirectory;
//                    config.SetBasePath(basePath);
//                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//                })
//                .ConfigureServices((context, services) =>
//                {
//                    // Đọc chuỗi kết nối từ appsettings.json
//                    var connectionString = context.Configuration.GetConnectionString("Default");

//                    // Đăng ký DbContext
//                    services.AddDbContext<InTNDbContext>(options =>
//                        options.UseSqlServer(connectionString));
//                })
//                .Build();

//            // Sử dụng DbContext
//            using var scope = host.Services.CreateScope();
//            var dbContext = scope.ServiceProvider.GetRequiredService<InTNDbContext>();

//            // Kiểm tra kết nối cơ sở dữ liệu
//            try
//            {


//                // dbContext.Database.EnsureCreated(); // Đảm bảo cơ sở dữ liệu đã được tạo
//                Console.WriteLine("Kết nối cơ sở dữ liệu thành công!");

//                var products = dbContext.Products.ToList(); // Thực hiện truy vấn đơn giản để kiểm tra kết nối
//                Console.Clear();

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Lỗi kết nối cơ sở dữ liệu: {ex.Message}");
//            }
//        }
//    }
//}


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
            var filePath = Path.Combine(AppContext.BaseDirectory, "Products.xlsx");

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

            Console.WriteLine("Hoàn tất import sản phẩm.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi trong quá trình import: {ex.Message}\n{ex.StackTrace}");
        }
    }
}

// (ExcelReader, ExcelProductRow và ProductImporter được giữ nguyên như trước...)

