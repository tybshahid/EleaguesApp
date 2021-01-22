namespace EleaguesApp.API.Dtos
{
    public class LeaderboardDto
    {
        public string Player { get; set; }
        public string Country { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public int Points { get; set; }
        public string History { get; set; }

        public int RunsScored { get; set; }
        public decimal OversPlayed { get; set; }
        public int WicketsLost { get; set; }
        public int RunsConceded { get; set; }
        public decimal OversBowled { get; set; }
        public int WicketsTaken { get; set; }

        public int Fifties { get; set; }
        public int Hundreds { get; set; }
        public int FiveWickets { get; set; }

        public double WinPercentage { get; set; }
        public decimal NRR { get; set; }
        public decimal Average { get; set; }
        public decimal Economy { get; set; }

        public string GroupName { get; set; }
    }
}