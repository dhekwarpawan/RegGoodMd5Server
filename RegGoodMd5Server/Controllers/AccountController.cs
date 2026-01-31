using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using RegGoodMd5.Server.DB_Bridge;
using RegGoodMd5.Server.Models;
using RegGoodMd5Server.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RegGoodMd5Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly ApplicationDBContext _db;
        private readonly IJwtServices _jwt;
        public AccountController(ApplicationDBContext db, IJwtServices jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(LoginPostData postdata)
        {
            try
            {
                if (postdata == null || string.IsNullOrWhiteSpace(postdata.userName))
                {
                    return BadRequest("Username is required.");
                }

                postdata.userName = postdata.userName.ToLowerInvariant();

                var result = await _db.loginModels.SingleOrDefaultAsync(u => u.userName == postdata.userName);

                if (result == null)
                {
                    return Unauthorized("Invalid Credencial");
                }
                var token = _jwt.GetGenerateToken(result);

                return Ok(new AuthResponse { Token = token, ExpiresAt = _jwt.GetExipryDate() });


                // var result = _db.loginModels.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
