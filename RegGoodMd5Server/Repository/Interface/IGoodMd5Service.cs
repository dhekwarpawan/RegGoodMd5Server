using RegGoodMd5Server.Models;
using RegGoodMd5Server.Models.DB_Entities;

namespace RegGoodMd5Server.Repository.Interface
{
    public interface IGoodMd5Service
    {
        public Task<List<AllMD5Modal>> GetMD5();
        public Task<string> Removemd5Operation(RemoveMd5PostData postdata,string loginId);
    }
}
