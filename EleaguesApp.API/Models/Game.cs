using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EleaguesApp.API.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public int WinnerId { get; set; }
        public int OpponentId { get; set; }
        public string Stage { get; set; }
        public int Round { get; set; }
        public int Runs { get; set; }
        public decimal Overs { get; set; }
        public int Wickets { get; set; }
        public int Fifties { get; set; }
        public int Hundreds { get; set; }
        public int FiveWickets { get; set; }
        public int InningsHigh { get; set; }
        public int OpponentRuns { get; set; }
        public decimal OpponentOvers { get; set; }
        public int OpponentWickets { get; set; }
        public int OpponentFifties { get; set; }
        public int OpponentHundreds { get; set; }
        public int OpponentFiveWickets { get; set; }
        public int OpponentInningsHigh { get; set; }
        public string Result { get; set; }
        public bool IsApproved { get; set; }
        public string FileName { get; set; }
        public string WinnerGroup { get; set; }
        public string OpponentGroup { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual League League { get; set; }
        public virtual User Winner { get; set; }
        public virtual User Opponent { get; set; }
        public Game()
        {
            CreatedOn = DateTime.Now;
        }
    }
}