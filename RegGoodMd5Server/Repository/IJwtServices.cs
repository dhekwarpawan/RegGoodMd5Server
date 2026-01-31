using RegGoodMd5.Server.Models.Db_model;

namespace RegGoodMd5Server.Repository
{
    public interface IJwtServices
    {

        string GetGenerateToken(LoginModel login);

        DateTime GetExipryDate();
    }
}
