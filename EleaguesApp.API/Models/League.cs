using System;
using System.Collections.Generic;

namespace EleaguesApp.API.Models
{
    public class League
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
        public string RulesFileName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ICollection<Game> Games { get; set; }
        public League()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(2);
            CreatedOn = DateTime.Now;
        }


    }
}