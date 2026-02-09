using Microsoft.EntityFrameworkCore;
using RegGoodMd5.Server.Models.Db_model;
using RegGoodMd5Server.Models.Db_model;

namespace RegGoodMd5.Server.DB_Bridge
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> option):base(option)
        {
                
        }

        public DbSet<LoginModel> loginModels { get; set; }
        public DbSet<WormconflictrowsEntity> wormconflictrows { get; set; }
        public DbSet<Md5FilenameMaster> md5filenamemaster { get; set; }
        public DbSet<RemovedMd5Master> removedmdmaster { get; set; }
        public DbSet<Goodmd5MasterEntity> goodmdmaster { get; set; }
        public DbSet<reasonmaster> reasonmaster { get; set; }
    }
}
