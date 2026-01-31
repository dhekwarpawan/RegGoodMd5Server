using MySql.Data.MySqlClient;
using RegGoodMd5Server.Models;
using RegGoodMd5Server.Models.DB_Entities;
using RegGoodMd5Server.Repository.Interface;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace RegGoodMd5Server.Repository.Services
{
    public class GoodMd5Services : IGoodMd5Service
    {
        //private readonly IConfiguration _config;
        private readonly string _connectionString;
        public GoodMd5Services(IConfiguration config)
        {
            //_config = config;
            _connectionString = config.GetConnectionString("mycon")
          ?? throw new InvalidOperationException("Connection string 'mycon' not found.");
        }

        public async Task<List<AllMD5Modal>> GetMD5()
        {
            List<AllMD5Modal> listallgd = new List<AllMD5Modal>();
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync();
                string query = @$"
                                 SELECT goodmd5_master.*, 
                                    reason_master.reason,
                                    login_master.name, 
                                    wormconflictcounter.*,
                                    DATE_FORMAT(addedDate,'%d-%b-%Y %h:%m %p') AS addedDate 
                                    FROM goodmd5_master LEFT JOIN reason_master 
                                        ON goodmd5_master.reasonID = reason_master.reason_ID 
                                    LEFT JOIN wormconflictcounter 
                                       ON goodmd5_master.regGMD5_ID = wormconflictcounter.ID_regGMD5 
                                    LEFT JOIN login_master
                                       ON goodmd5_master.LoginID = login_master.LoginID 
                                    WHERE removed=false
                                    ORDER BY regGMD5_ID DESC;
                                ";

                using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    listallgd.Add(new AllMD5Modal
                    {
                        regGMD5_ID = reader.GetInt32(reader.GetOrdinal("regGMD5_ID")),
                        rmd5 = reader["rmd5"]?.ToString(),
                        fileName = reader["fileName"]?.ToString(),
                        addedDate = reader["addedDate"].ToString(),
                        LoginID = reader.IsDBNull("LoginID") ? 0 : reader.GetInt32("LoginID"),
                        addedByIP = reader["addedByIP"]?.ToString(),
                        reasonID = reader.IsDBNull("reasonID") ? 0 : reader.GetInt32("reasonID"),
                        info_comments = reader["info_comments"]?.ToString(),
                        removed = !reader.IsDBNull("removed") && reader.GetBoolean("removed"),
                        YNo = reader.IsDBNull("YNo") ? 0 : reader.GetInt32("YNo"),
                        RemovedChangeDate = reader.IsDBNull("RemovedChangeDate")
                            ? (DateTime?)null
                            : reader.GetDateTime("RemovedChangeDate"),
                        IsRMD5RemoveConflict = !reader.IsDBNull("IsRMD5RemoveConflict")
                            && reader.GetBoolean("IsRMD5RemoveConflict"),
                        ConflictDate = reader.IsDBNull("ConflictDate")
                            ? (DateTime?)null
                            : reader.GetDateTime("ConflictDate"),
                        reason = reader["reason"]?.ToString(),
                        name = reader["name"]?.ToString(),
                        WCC_ID = reader.IsDBNull("WCC_ID") ? 0 : reader.GetInt32("WCC_ID"),
                        WormDetected = !reader.IsDBNull("WormDetected")
                            && reader.GetBoolean("WormDetected"),
                        lastestWhen = reader["lastestWhen"].ToString(),
                         
                        firstWhen = reader["firstWhen"].ToString(),
                         
                        WormDetectHitCount = reader.IsDBNull("WormDetectHitCount")
                            ? 0
                            : reader.GetInt32("WormDetectHitCount"),
                        ID_regGMD5 = reader.IsDBNull("ID_regGMD5") ? 0 : reader.GetInt32("ID_regGMD5"),
                        InDateTime = reader.IsDBNull("InDateTime")
                            ? (DateTime?)null
                            : reader.GetDateTime("InDateTime"),
                    });
                }

            }
            catch (Exception)
            {

                throw;
            }
           

            
            return listallgd;
        }


        public async Task<string> Removemd5Operation(RemoveMd5PostData postdata, string loginId)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            IPAddress ipAddress = ipHostInfo.AddressList[1];

            try
            {
                var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                using var cmd = new MySqlCommand($"UPDATE goodmd5_master SET removed=True,RemovedChangeDate=Now() WHERE regGMD5_ID = @regGMD5_ID ", con);
                cmd.Parameters.Add("@regGMD5_ID", MySqlDbType.Int32).Value = postdata.regGMD5_ID;
                await cmd.ExecuteNonQueryAsync();


                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO removedmd5_master(rmvd_reason, LoginID, rmvd_dateTime, regGMD5_ID, removedByIP) VALUES(@removedreason,@removedby,NOW(),@Gid,@removedByIP)";
                cmd.Parameters.Add("@removedreason", MySqlDbType.VarChar).Value = postdata.reason;
                cmd.Parameters.Add("@removedby", MySqlDbType.Int32).Value = loginId;
                cmd.Parameters.Add("@Gid", MySqlDbType.Int32).Value = postdata.regGMD5_ID;
                cmd.Parameters.Add("@removedByIP", MySqlDbType.VarChar).Value = ipAddress.ToString();
                await cmd.ExecuteNonQueryAsync();

                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT FileName FROM goodmd5_master WHERE regGMD5_ID = @Gid";
                cmd.Parameters.Add("@Gid", MySqlDbType.Int32).Value = postdata.regGMD5_ID;

                string? filename = null;

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        filename = reader.GetString("FileName");
                    }
                }

                if (!string.IsNullOrEmpty(filename))
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "UPDATE md5filenm_master SET lstMD5ModifiedDtTm=NOW(),releasedFlag=false WHERE FileName= @Filename";
                    cmd.Parameters.Add("@Filename", MySqlDbType.VarChar).Value = filename;
                    await cmd.ExecuteNonQueryAsync();
                }
                return "Removed successfully";
            }
            catch (Exception ex)
            {
               
                throw;
            }
            
        }

    }
}
