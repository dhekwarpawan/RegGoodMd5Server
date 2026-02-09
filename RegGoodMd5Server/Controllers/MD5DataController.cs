using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using RegGoodMd5.Server.DB_Bridge;
using RegGoodMd5Server.Models;
using RegGoodMd5Server.Models.DB_Entities;
using RegGoodMd5Server.Models.DTOs;
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
        private readonly ApplicationDBContext _db;
        public MD5DataController(IConfiguration config, ILogger<DashboardController> logger, IGoodMd5Service goodmd5Service, IWormConflictMD5Services wormconflictmd5Service,ApplicationDBContext db)
        {
            _config = config;
            _logger = logger;
            _goodmd5Service = goodmd5Service;
            _wormconflictmd5Service = wormconflictmd5Service;
            _db = db;
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
                _logger.LogError(ex, "Failed to get data");
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

                string response = await _goodmd5Service.Removemd5Operation(postdata, loginId);

                return Ok(new { message = response });
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
            List<AllMD5Modal> list_wormconflict = new List<AllMD5Modal>();
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


        [HttpPost]
        [Route("updcmt")]
        public async Task<IActionResult> UpdateComment(UpdateComentDto obj) 
         {           
            try
            {
                var result = await _wormconflictmd5Service.UpdateCommentAsync(obj);
                return Ok(result);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to update data");

                throw;
            }
           
        }

        [HttpPost]
        [Route("updanlysis")]
        public async Task<IActionResult> UpdateAnalysis(UpdateAnalysisDto obj)
        {
            try
            {
                string result = await _wormconflictmd5Service.UpdateAnalysis(obj);
                return Ok(new { message = result});
            }
            catch (Exception)
            {
                _logger.LogError("Failed to update data");

                throw;
            }

        }

        [HttpGet]
        [Route("allremovedmd5")]
        public async Task<IActionResult> GetAllRemovedMD5()
        {
            List<AllMD5Modal> list = new List<AllMD5Modal>();
            var result = _goodmd5Service.Fn_GetAllRemovedmd5();
            return Ok(result);
        }

        [HttpGet]
        [Route("showdetails_removed/{id}")]
        public async Task<IActionResult> ShowDetailsAboutRemovedMd5(int id)
        {
            var response = await  _goodmd5Service.GetDetailsofRemovedmd5(id);
            if (response == null)
                return NotFound($"No removed MD5 found with id {id}");
            return Ok(new
            {
                regGMD5_ID = response.Value.regGMD5_ID,
                rmd5 = response.Value.rmd5,
                filename = response.Value.filename,
                addedDate = response.Value.addedDate,
                addedByIP = response.Value.addedByIP,
                info_comments = response.Value.info_comments,
                reason = response.Value.reason,
                name = response.Value.name


            });
        }

        [HttpPost]
        [Route("movegood")]
        public async Task<IActionResult> MoveRmd5ToGood(MoveRmd5ToGoodDto postdata)
        {
            string? loginId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loginId == null)
                return Unauthorized();

            string response = await _goodmd5Service.Fn_MovedToGood(postdata, loginId);
            return Ok(new { message = response });
        } 
    }
}
