using RegGoodMd5Server.Models.DB_Entities;

namespace RegGoodMd5Server.Repository.Interface
{
    public interface IWormConflictMD5Services
    {
        public Task<List<AllMD5Modal>> GetWormConflictMd5();

        public Task<List<Wormconflictrows>> GetWormConflictRows(int wccid);
    }
}
