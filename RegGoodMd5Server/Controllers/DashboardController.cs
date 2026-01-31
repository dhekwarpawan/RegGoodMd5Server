using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using RegGoodMd5Server.Models;
using RegGoodMd5Server.Models.DB_Entities;
using System.Data;
using System.Security.Cryptography.Xml;

namespace RegGoodMd5Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IConfiguration configuration, ILogger<DashboardController> logger)
        {
            _configuration = configuration;
            _logger = logger;

        }

        [Authorize]
        [HttpGet]
        [Route("getfilewisemd5")]
        public async Task<IActionResult> GetFilesMdData()
        {
            var connectionString = _configuration.GetConnectionString("mycon");
            var entitylist = new List<RegGoodMd5Entity>();
            try
            {
                using (var con = new MySqlConnection(connectionString))
                {
                    await con.OpenAsync();

                    string query = @"
                                SELECT 
                                    md.FNm_ID,
                                    md.FileName,
                                    t.cntAddedmd5,
                                    t.FaddedDate
                                FROM md5filenm_master md
                                LEFT JOIN 
                                (
                                    SELECT 
                                        tblMD5.fileName AS tblMD5FileName,
                                        COUNT(tblMD5.rmd5) AS cntAddedmd5,
                                        DATE_FORMAT(tblMD5.addedDate,'%d-%b-%Y %h:%i %p') AS FaddedDate
                                    FROM 
                                    (
                                        SELECT 
                                            regGMD5_ID, rmd5, fileName, addedDate, LoginID 
                                        FROM goodmd5_master 
                                        WHERE removed = FALSE OR removed IS NULL
                                        ORDER BY addedDate DESC
                                    ) AS tblMD5
                                    GROUP BY tblMD5.fileName
                                ) AS t 
                                ON md.FileName = t.tblMD5FileName;";
                    using var cmd = new MySqlCommand(query, con);

                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        entitylist.Add(new RegGoodMd5Entity
                        {
                            //FNm_ID, FileName, AddedBy, InDate, lstMD5ModifiedDtTm, releasedFlag, lstModifiedBy, regGMD5_ID, rmd5, tblMD5FileName, cntAddedmd5, FaddedDate, LoginID
                            FileName = reader["FileName"]?.ToString() ?? "",
                            Md5Count = reader["cntAddedmd5"] == DBNull.Value ? 0 : Convert.ToInt32(reader["cntAddedmd5"]),
                            LastestAddedDate = reader["FaddedDate"]?.ToString() ?? ""
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetFilesMdData()");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Something went wrong while fetching MD5 data.");
            }

            return Ok(entitylist);
        }

        [HttpGet]
        [Route("getremovedmd5")]
        public async Task<IActionResult> GetRemovedMD5Data()
        {
            var connectionString = _configuration.GetConnectionString("mycon");

            var entitylist = new List<RegGoodMd5Entity>();
            try
            {
                using var con = new MySqlConnection(connectionString);

                await con.OpenAsync();
                string query = @"SELECT 
                                 tbl3.fileName,
                                 count(tbl3.rmd5) as cntRmd5,
                                 DATE_FORMAT(tbl3.rmvd_dateTime,'%d-%b-%Y %h:%m %p') AS Frmvd_dateTime 
                                 FROM (
                                       SELECT
                                          tb1.rmd5,
                                          tb1.fileName, 
                                          DATE_FORMAT(tb1.addedDate,'%d-%b-%Y %h:%m %p') AS FaddedDate, 
                                          tb2.regGMD5_ID,
                                          rmvd_dateTime 
                                          FROM goodmd5_master as tb1, removedmd5_master as tb2 
                                          WHERE tb1.regGMD5_ID = tb2.regGMD5_ID 
                                          ORDER BY tb2.rmvd_dateTime DESC
                                    ) as tbl3 
                                   GROUP BY tbl3.fileName;";
                using var cmd = new MySqlCommand(query, con);

                using var reader = await cmd.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    entitylist.Add(new RegGoodMd5Entity
                    {

                        FileName = reader["fileName"]?.ToString() ?? "",
                        Md5Count = reader["cntRmd5"] == DBNull.Value ? 0 : Convert.ToInt32(reader["cntRmd5"]),
                        LastestRemovedDate = reader["Frmvd_dateTime"]?.ToString() ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetFilesMdData()");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Something went wrong while fetching MD5 data.");
            }
            return Ok(entitylist);
        }

        [HttpGet]
        [Route("getnewrealeasefile")]
        public async Task<IActionResult> GetNewReleaseFile()
        {
            var connectionString = _configuration.GetConnectionString("mycon");

            var entitylist = new List<RegGoodMd5Entity>();
            try
            {

                using var con = new MySqlConnection(connectionString);

                await con.OpenAsync();
                string query = @"SELECT md5filenm_master.FNm_ID,
                                  md5filenm_master.FileName, 
                                 DATE_FORMAT(lstMD5ModifiedDtTm,'%d-%b-%Y %h:%m %p') AS FlstMD5ModifiedDtTm,
                                 login_master.name as lstModifiedByName 
                                 FROM md5filenm_master
                                 LEFT JOIN login_master
                                 ON md5filenm_master.lstModifiedBy = login_master.loginID 
                                 WHERE releasedFlag=false and lstMD5ModifiedDtTm is not null;";
                using var cmd = new MySqlCommand(query, con);

                using var reader = await cmd.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    entitylist.Add(new RegGoodMd5Entity
                    {
                        FNm_ID = Convert.ToInt32(reader["FNm_ID"]),
                        FileName = reader["FileName"]?.ToString() ?? "",
                        LastestModifiedDate = reader["FlstMD5ModifiedDtTm"]?.ToString() ?? "",
                        ModifiedBy = reader["lstModifiedByName"]?.ToString() ?? "",

                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetFilesMdData()");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Something went wrong while fetching MD5 data.");
            }

            return Ok(entitylist);



        }
        [HttpGet]
        [Route("getlastrealeasefile")]
        public async Task<IActionResult> GetLastReleaseFile()
        {
            var connectionString = _configuration.GetConnectionString("mycon");

            var entitylist = new List<RegGoodMd5Entity>();
            try
            {
                using var con = new MySqlConnection(connectionString);
                await con.OpenAsync();
                string query = @"SELECT md5filenm_master.*, 
                                  DATE_FORMAT(lstMD5ModifiedDtTm,'%d-%b-%Y %h:%m %p') AS FlstMD5ModifiedDtTm, 
                                  login_master.loginID, 
                                  login_master.name as lstModifiedByName
                                  FROM md5filenm_master
                                  LEFT JOIN login_master ON md5filenm_master.lstModifiedBy = login_master.loginID
                                  WHERE releasedFlag=true and lstMD5ModifiedDtTm is not null;";
                using var cmd = new MySqlCommand(query, con);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    entitylist.Add(new RegGoodMd5Entity
                    {
                        FNm_ID = Convert.ToInt32(reader["FNm_ID"]),
                        FileName = reader["FileName"]?.ToString() ?? "",
                        LastestModifiedDate = reader["FlstMD5ModifiedDtTm"]?.ToString() ?? "",
                        ModifiedBy = reader["lstModifiedByName"]?.ToString() ?? "",
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetFilesMdData()");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Something went wrong while fetching MD5 data.");
            }

            return Ok(entitylist);
        }


        [HttpPost]
        [Route("markasreleased")]
        public async Task<IActionResult> Srv_MarkAsRelased([FromBody] MarkAs_Released_Unreleased_Request req)
        {
            var connectionString = _configuration.GetConnectionString("mycon");
            string query = string.Empty;
            try
            {
                if (req.Ids == null && string.IsNullOrEmpty(req.flg)) return BadRequest(new { message = "Please pass required credential" });
                using var con = new MySqlConnection(connectionString);
                await con.OpenAsync();
                if (req.flg == "1")
                {
                    query = $"UPDATE md5filenm_master SET releasedFlag=true WHERE FNm_ID=@id";
                }
                else
                {
                    query = $"UPDATE md5filenm_master SET releasedFlag=false WHERE FNm_ID=@id";
                }
                foreach (var id in req.Ids)
                {
                    using var cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id",id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(new { message = "Pawan Mark as Realesed fuctionality properly work" });
        }

    }
}
