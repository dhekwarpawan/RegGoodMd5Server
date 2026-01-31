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
}
