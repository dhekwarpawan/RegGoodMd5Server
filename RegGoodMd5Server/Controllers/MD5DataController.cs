using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using RegGoodMd5Server.Models;
using RegGoodMd5Server.Models.DB_Entities;
using RegGoodMd5Server.Repository.Interface;
using System.Data;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace RegGoodMd5Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MD5DataController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<DashboardController> _logger;
        private readonly IGoodMd5Service _goodmd5Service;
        private readonly IWormConflictMD5Services _wormconflictmd5Service;

        public MD5DataController(IConfiguration config, ILogger<DashboardController> logger, IGoodMd5Service goodmd5Service, IWormConflictMD5Services wormconflictmd5Service)
        {
            _config = config;
            _logger = logger;
            _goodmd5Service = goodmd5Service;
            _wormconflictmd5Service = wormconflictmd5Service;
        }
        [HttpGet]
        [Route("getgoodmd5")]
        public async Task<IActionResult> GetAllGoodMD5()
        {
            List<AllMD5Modal> listallgd = new List<AllMD5Modal>();   
            try
            {
                listallgd = await _goodmd5Service.GetMD5();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to get data");
                throw;
            }

            return Ok(listallgd);
        }


        [HttpPost]
        [Route("removemd5")]
        public async Task<IActionResult> RemoveMD5(RemoveMd5PostData postdata)
        {
            string? loginId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loginId == null)
                return Unauthorized();
            try
            {

                string response = await _goodmd5Service.Removemd5Operation(postdata,loginId);

                return Ok(new {message= response});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove");
                throw;
            }
           
        }



        [HttpGet]
        [Route("getwormconflictmd5")]

        public async Task<IActionResult> GetWormConflictMd5()
        {
            List<AllMD5Modal> list_wormconflict= new List<AllMD5Modal>();
            try
            {
                list_wormconflict = await _wormconflictmd5Service.GetWormConflictMd5();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get data");
                throw;
            }

            return Ok(list_wormconflict);
        }


        [HttpPost]
        [Route("getWormConclictRows")]
        public async Task<IActionResult> GetWormConfilctRows([FromBody] int wccid)
        {
            List<Wormconflictrows> wormconflictrows_basedon_wccid = new List<Wormconflictrows>();
            try
            {
                wormconflictrows_basedon_wccid = await _wormconflictmd5Service.GetWormConflictRows(wccid);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get data");
                throw;
            }

            return Ok(wormconflictrows_basedon_wccid);
        }
    }
}
