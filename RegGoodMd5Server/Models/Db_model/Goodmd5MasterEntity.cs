using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegGoodMd5Server.Models.Db_model
{
    [Table("goodmd5_master")]
    public class Goodmd5MasterEntity
    {
        [Key]
        public int? regGMD5_ID { get; set; }
        public string? rmd5 { get; set; }
        public string? fileName { get; set; }
        public DateTime? addedDate { get; set; }
        public int? LoginID { get; set; }
        public string? addedByIP { get; set; }
        public int? reasonID { get; set; }
        public string? info_comments { get; set; }
        public bool removed { get; set; }
        public int? YNo { get; set; }
        public DateTime? RemovedChangeDate { get; set; }
        public bool IsRMD5RemoveConflict { get; set; }
        public DateTime? ConflictDate { get; set; }

    }

}
