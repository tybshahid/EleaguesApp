using System;
using Microsoft.AspNetCore.Hosting;

namespace EleaguesApp.API.Dtos
{
    public class LeagueForListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public string Format { get; set; }
        public int Rounds { get; set; }
        public string TeamFormat { get; set; }
        public string MatchTiming { get; set; }
        public string Powerplay { get; set; }
        public string NormalizeSkills { get; set; }
        public string RopeSettings { get; set; }
        public string PitchSettings { get; set; }
        public int WinPoints { get; set; }
        public int LostPoints { get; set; }
        public int ParticipationPoints { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
        public string Winner { get; set; }
        public bool IsGroupLeague { get; set; }
        public bool IsDefaultLeague { get; set; }
        public string RulesFilePath { get; set; }
    }
}