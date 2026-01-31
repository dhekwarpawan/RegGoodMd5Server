namespace RegGoodMd5Server.Models
{
    public class RemoveMd5PostData
    {
        public string? md5 { get; set; }
        public string? fileName { get; set; }
        public string? removedby  { get; set; }
        public string? reason  { get; set; }
        public int regGMD5_ID { get; set; }
        public int reasonID { get; set; }

    }
}
