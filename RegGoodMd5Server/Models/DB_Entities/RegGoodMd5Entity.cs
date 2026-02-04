namespace RegGoodMd5Server.Models.DB_Entities
{
    public class RegGoodMd5Entity
    {
        public int FNm_ID { get; set; }
        public string? FileName { get; set; }
        public int Md5Count { get; set; }
        public string? LastestAddedDate { get; set; }
        public string? LastestRemovedDate { get; set; }
        public string? LastestModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }


    public class AllMD5Modal
    {
        //regGMD5_ID, rmd5, fileName, addedDate, LoginID, addedByIP, reasonID, info_comments, removed, YNo, RemovedChangeDate, IsRMD5RemoveConflict, ConflictDate, reason, name, WCC_ID, WormDetected, lastestWhen, firstWhen, WormDetectHitCount, ID_regGMD5, InDateTime, addedDate

        public int regGMD5_ID { get; set; }

        public string? rmd5 { get; set; }
        public string? fileName { get; set; }
        public string? addedDate { get; set; }
        public int LoginID { get; set; }
        public string? addedByIP { get; set; }
        public int reasonID { get; set; }
        public string? info_comments { get; set; }
        public bool removed { get; set; }
        public int YNo { get; set; }
        public DateTime? RemovedChangeDate { get; set; }
        public bool IsRMD5RemoveConflict { get; set; }
        public DateTime? ConflictDate { get; set; }
        public string? reason { get; set; }
        public string? name { get; set; }
        public int WCC_ID { get; set; }
        public bool WormDetected { get; set; }
        public string? lastestWhen { get; set; }
        public string? firstWhen { get; set; }
        public int? WormDetectHitCount { get; set; }
        public int ID_regGMD5 { get; set; }
        public DateTime? InDateTime { get; set; }
    }



    public class Wormconflictrows
    {

        //WCR_ID, ID_WCC, EntryDtTm, DetectionbyAVs, FullMD5_FileNm, SampleRegMD5, ourDetectionNm, Comments, analysisComments, actionTaken, whenActionTaken, WCR_ByName, FEntryDtTm, FwhenActionTaken


            public int WCR_ID           {get;set;}      
            public int ID_WCC           {get;set;}
            public string? EntryDtTm        {get;set;}
            public string? DetectionbyAVs   {get;set;}
            public string? FullMD5_FileNm   {get;set;}
            public string? SampleRegMD5     {get;set;}
            public string? ourDetectionNm   {get;set;}
            public string? Comments         {get;set;}
            public string? analysisComments {get;set;}
            public string? actionTaken      {get;set;}
            public string? whenActionTaken  {get;set;}
            public string? WCR_ByName       {get;set;}
            public string? FEntryDtTm       {get;set;}
            public string? FwhenActionTaken { get; set;}



    }
}
