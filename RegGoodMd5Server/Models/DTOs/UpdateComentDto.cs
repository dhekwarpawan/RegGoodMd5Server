namespace RegGoodMd5Server.Models.DTOs
{
    public class UpdateComentDto
    {
      
            public int Id { get; set; }
            public string? Comment { get; set; }
     

    }
    public class UpdateAnalysisDto() {
        public int ID { get; set; }
        public string? analysisComments { get; set; }
        public int? action { get; set; }
    }
}
