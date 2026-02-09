using RegGoodMd5Server.Models;
using RegGoodMd5Server.Models.DB_Entities;
using RegGoodMd5Server.Models.DTOs;

namespace RegGoodMd5Server.Repository.Interface
{
    public interface IGoodMd5Service
    {
        public Task<List<AllMD5Modal>> GetMD5();
        public Task<string> Removemd5Operation(RemoveMd5PostData postdata, string loginId);

        Task<List<AllMD5Modal>> Fn_GetAllRemovedmd5();


        // Below method uses tuple for return data
        Task<(int? regGMD5_ID, string? rmd5, string filename, DateTime? addedDate, string? addedByIP, string info_comments, string reason, string name)?> GetDetailsofRemovedmd5(int id);

        Task<string> Fn_MovedToGood(MoveRmd5ToGoodDto data,string loginid);
    }
}
