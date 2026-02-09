using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegGoodMd5Server.Models.Db_model
{
    [Table("reason_master")]
    public class reasonmaster
    {
        //reason_ID, reason, inDate
        [Key]
        public int reason_ID { get; set; }
        public string? reason { get; set; }
        public DateTime inDate { get; set; }
    }
}
