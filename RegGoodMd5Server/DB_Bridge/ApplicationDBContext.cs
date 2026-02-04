using Microsoft.EntityFrameworkCore;
using RegGoodMd5.Server.Models.Db_model;

namespace RegGoodMd5.Server.DB_Bridge
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option):base(option)
        {
                
        }

        public DbSet<LoginModel> loginModels { get; set; }
        public DbSet<Wormconflictrows> wormconflictrows { get; set; }
    }
}
