using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegGoodMd5Server.Models.Db_model
{
    [Table("md5filenm_master")]
    public class Md5FilenameMaster
    {

        [Key]
        public int? FNm_ID { get; set; }
        public string? Filename { get; set; }
        public string? AddedBy { get; set; }
        public DateTime? InDate { get; set; }
        public DateTime? lstMD5ModifiedDtTm { get; set; }
        public bool? releasedFlag { get; set; }
        public int? lstModifiedBy { get; set; }
    }
}
