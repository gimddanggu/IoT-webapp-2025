using Microsoft.EntityFrameworkCore;

namespace WebApiApp03.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }

        // 제일중요!!
        // DB 이름이랑 같게
        public DbSet<IoT_Datas> iot_datas { get; set; }

    }
}
