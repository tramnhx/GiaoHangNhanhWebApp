using GiaoHangNhanh.Utilities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GiaoHangNhanh.DAL.EF
{
    public class GiaoHangNhanhDbContextFactory : IDesignTimeDbContextFactory<GiaoHangNhanhDbContext>
    {
        public GiaoHangNhanhDbContext CreateDbContext(string[] args)
        {
            // cau hinh connecttion string
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString(SystemConstants.ConnectionStringConstants.MainConnectionString);

            var optionsBuilder = new DbContextOptionsBuilder<GiaoHangNhanhDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new GiaoHangNhanhDbContext(optionsBuilder.Options);
        }
    }
}
