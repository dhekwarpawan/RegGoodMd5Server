using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegGoodMd5.Server.Models.Db_model
{
    [Table("login_master")]
    public class LoginModel
    {
        //LoginID, name, userName, pwd, loginType, inDate, LoginStatus, WMSID
        [Key]
        public int LoginID { get; set; }
        public string? name { get; set; }
        public string? userName { get; set; }
        public string? pwd { get; set; }
        public string? loginType { get; set; }
        public DateTime? inDate { get; set; }
        public bool LoginStatus { get; set; }
        public int WMSID { get; set; }

    }

    [Table("wormconflictrows")]
    public class Wormconflictrows
    {
        [Key]
        public int WCR_ID { get; set; }

        public int? ID_WCC { get; set; }     
        public DateTime? EntryDtTm { get; set; }  

        public string? DetectionbyAVs { get; set; }
        public string? FullMD5_FileNm { get; set; }
        public string? SampleRegMD5 { get; set; }
        public string? ourDetectionNm { get; set; }
        public string? Comments { get; set; }
        public string? analysisComments { get; set; }
        public int? actionTaken { get; set; }
        public DateTime? whenActionTaken { get; set; }
        public string? WCR_ByName { get; set; }
    }

}
