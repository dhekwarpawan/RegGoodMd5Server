using RegGoodMd5Server.Models;
using RegGoodMd5Server.Models.DB_Entities;
using RegGoodMd5Server.Models.DTOs;

namespace RegGoodMd5Server.Repository.Interface
{
    public interface IWormConflictMD5Services
    {
        Task<List<AllMD5Modal>> GetWormConflictMd5();

        Task<List<Wormconflictrows>> GetWormConflictRows(int wccid);

        Task<ApiResponse<string>> UpdateCommentAsync(UpdateComentDto dto);

        Task<string> UpdateAnalysis(UpdateAnalysisDto dto);
    }
}
