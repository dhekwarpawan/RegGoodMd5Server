using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;
using RegGoodMd5.Server.DB_Bridge;
using RegGoodMd5Server.Models;
using RegGoodMd5Server.Models.DB_Entities;
using RegGoodMd5Server.Models.DTOs;
using RegGoodMd5Server.Repository.Interface;
using System.Data;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RegGoodMd5Server.Repository.Services
{
    public class WormConflictMD5Services : IWormConflictMD5Services
    {
        private readonly string _connectionstring;
        private readonly ApplicationDBContext _db;
        public WormConflictMD5Services(IConfiguration config, ApplicationDBContext db)
        {
                _connectionstring = config.GetConnectionString("mycon") ?? throw new InvalidOperationException("Connection string 'mycon' not found.");
            _db = db;
        }
        public async Task<List<AllMD5Modal>> GetWormConflictMd5()
        {

            List<AllMD5Modal> list = new List<AllMD5Modal>();
            try
            {
                    using var conn = new MySqlConnection(_connectionstring);
                    await conn.OpenAsync();
                    string query = @$"
                                    SELECT goodmd5_master.*, reason_master.reason, login_master.name, wormconflictcounter.*, DATE_FORMAT(lastestWhen,'%d-%b-%Y %h:%m %p') AS FlastestWhen , DATE_FORMAT(firstWhen,'%d-%b-%Y %h:%m %p') AS FfirstWhen, DATE_FORMAT(addedDate,'%d-%b-%Y %h:%m %p') AS addedDate FROM goodmd5_master  LEFT JOIN reason_master ON goodmd5_master.reasonID = reason_master.reason_ID  LEFT JOIN wormconflictcounter ON goodmd5_master.regGMD5_ID = wormconflictcounter.ID_regGMD5  LEFT JOIN login_master ON goodmd5_master.LoginID = login_master.LoginID WHERE removed=false  ORDER BY WormDetectHitCount DESC;
                                ";

                    using var cmd = new MySqlCommand(query, conn);
                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                         list.Add(new AllMD5Modal
                        {
                            regGMD5_ID = reader.GetInt32(reader.GetOrdinal("regGMD5_ID")),
                             WormDetectHitCount = reader.IsDBNull("WormDetectHitCount")
    ? 0
    : reader.GetInt32("WormDetectHitCount"),
                             rmd5 = reader["rmd5"]?.ToString(),
                            fileName = reader["fileName"]?.ToString(),
                            reason = reader["reason"]?.ToString(),
                             lastestWhen = reader["lastestWhen"].ToString(),

                             firstWhen = reader["firstWhen"].ToString(),
                             //addedDate = reader.IsDBNull("addedDate") ? DateTime.MinValue : reader.GetDateTime("addedDate"),
                             addedDate = reader["addedDate"].ToString(),

                             addedByIP = reader["addedByIP"]?.ToString(),

                             name = reader["name"]?.ToString(),

                            // LoginID = reader.IsDBNull("LoginID") ? 0 : reader.GetInt32("LoginID"),
                           
                            //reasonID = reader.IsDBNull("reasonID") ? 0 : reader.GetInt32("reasonID"),
                            //info_comments = reader["info_comments"]?.ToString(),
                            //RemovedChangeDate = reader.IsDBNull("RemovedChangeDate")
                            //    ? (DateTime?)null
                            //    : reader.GetDateTime("RemovedChangeDate"),
                            //IsRMD5RemoveConflict = !reader.IsDBNull("IsRMD5RemoveConflict")
                            //    && reader.GetBoolean("IsRMD5RemoveConflict"),
                            //ConflictDate = reader.IsDBNull("ConflictDate")
                            //    ? (DateTime?)null
                            //    : reader.GetDateTime("ConflictDate"),
                       
                             WCC_ID = reader.IsDBNull("WCC_ID") ? 0 : reader.GetInt32("WCC_ID"),
                            //WormDetected = !reader.IsDBNull("WormDetected")
                            //    && reader.GetBoolean("WormDetected"),
                         
                 
                            //WormDetectHitCount = reader.IsDBNull("WormDetectHitCount")
                            //    ? 0
                            //    : reader.GetInt32("WormDetectHitCount"),
                            //ID_regGMD5 = reader.IsDBNull("ID_regGMD5") ? 0 : reader.GetInt32("ID_regGMD5"),
                            //InDateTime = reader.IsDBNull("InDateTime")
                            //    ? (DateTime?)null
                            //    : reader.GetDateTime("InDateTime"),
                        });
                    }

              

            }
            catch (Exception)
            {

                throw;
            }


            return list;
        }


        public async Task<List<Wormconflictrows>> GetWormConflictRows(int wccid)
        {
            List<Wormconflictrows> list = new List<Wormconflictrows>();
            try
            {
                using var conn = new MySqlConnection(_connectionstring);
                await conn.OpenAsync();
                string query = @$"SELECT WCR_ID, ID_WCC, EntryDtTm, DetectionbyAVs, FullMD5_FileNm, SampleRegMD5, ourDetectionNm, Comments, analysisComments, actionTaken, whenActionTaken, WCR_ByName, DATE_FORMAT(EntryDtTm,'%d-%b %h:%m %p') AS FEntryDtTm, DATE_FORMAT(whenActionTaken,'%d-%b %h:%m %p') AS FwhenActionTaken FROM wormconflictrows where ID_WCC=@wccid;";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@wccid", wccid);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new Wormconflictrows
                    {
                        WCR_ID = reader.GetInt32(reader.GetOrdinal("WCR_ID")),
                        ID_WCC = reader.IsDBNull("ID_WCC")? 0 : reader.GetInt32("ID_WCC"),
                        EntryDtTm = reader["EntryDtTm"]?.ToString(),
                        DetectionbyAVs = reader["DetectionbyAVs"]?.ToString(),
                        FullMD5_FileNm = reader["FullMD5_FileNm"]?.ToString(),
                        SampleRegMD5 = reader["SampleRegMD5"].ToString(),

                        ourDetectionNm = reader["ourDetectionNm"].ToString(),
                        Comments = reader["Comments"].ToString(),

                        analysisComments = reader["analysisComments"]?.ToString(),

                        actionTaken = reader["actionTaken"]?.ToString(),
                        whenActionTaken = reader["whenActionTaken"]?.ToString(),
                        WCR_ByName = reader["WCR_ByName"]?.ToString(),
                        FEntryDtTm = reader["FEntryDtTm"]?.ToString(),
                        FwhenActionTaken = reader["FwhenActionTaken"]?.ToString(),
                    });
                }



            }
            catch (Exception)
            {

                throw;
            }


            return list;

        }


        public async Task<ApiResponse<string>> UpdateCommentAsync(UpdateComentDto obj)
        {
            Wormconflictrows wormconflictrows = new Wormconflictrows();
            if (obj == null || string.IsNullOrWhiteSpace(obj.Comment))
                return ApiResponse<string>.Fail("Comment cannot be empty.");


            var entity = await _db.wormconflictrows
                      .FirstOrDefaultAsync(x => x.WCR_ID == obj.Id);
            if( entity == null)
            {
                return ApiResponse<string>.Fail("Record not found.");
            }
            entity.Comments = obj.Comment;
            await _db.SaveChangesAsync();
            return ApiResponse<string>.Ok(obj.Comment, "Comment updated successfully.");

        }

        public async Task<string> UpdateAnalysis(UpdateAnalysisDto dto)
        {
            var data = await _db.wormconflictrows.Where(x => x.WCR_ID == dto.ID).FirstOrDefaultAsync();
            if (data == null) throw new KeyNotFoundException("Data not found");

            data.whenActionTaken = DateTime.Now;
            data.analysisComments = dto.analysisComments;
            data.actionTaken = dto.action ?? 0; 
            await _db.SaveChangesAsync();

            return "Successfully Update";

        }
    }
}
