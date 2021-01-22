using System;

namespace EleaguesApp.API.Dtos
{
    public class GameForListDto
    {
        public int Id { get; set; }
        public string League { get; set; }
        public string Winner { get; set; }
        public string WinnerPhotoUrl { get; set; }
        public string Opponent { get; set; }
        public string Stage { get; set; }
        public int Round { get; set; }
        public int Runs { get; set; }
        public decimal Overs { get; set; }
        public int Wickets { get; set; }
        public int OpponentRuns { get; set; }
        public decimal OpponentOvers { get; set; }
        public int OpponentWickets { get; set; }
        public string Result { get; set; }
        public string FileName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}