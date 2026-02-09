using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegGoodMd5Server.Models.Db_model
{
    [Table("removedmd5_master")]
    public class RemovedMd5Master
    {
        [Key]
        public int? rmvd_ID { get; set; }
        public string? rmvd_reason { get; set; }
        public int? LoginID { get; set; }
        public DateTime? rmvd_dateTime { get; set; }
        public int? regGMD5_ID { get; set; }
        public string? removedByIP { get; set; }
    }


    public class RemovedMd5DetailsModal()
    {
            //regGMD5_ID,
            //rmd5,
            //fileName,
            //addedDate,
            //addedByIP,
            //info_comments,
            //Reason = reason.reason,
            //Name = login.name
    }
}
