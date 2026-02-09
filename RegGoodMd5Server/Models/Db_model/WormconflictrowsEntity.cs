using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegGoodMd5Server.Models.Db_model
{
    [Table("wormconflictrows")]
    public class WormconflictrowsEntity
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
