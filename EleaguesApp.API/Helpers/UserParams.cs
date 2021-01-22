namespace EleaguesApp.API.Helpers
{
    public class UserParams
    {
        public UserParams()
        {
            // Users
            UserName = "";
            KnownAs = "";
            Country = "";
        }

        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 6;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        // Users
        public string UserName { get; set; }
        public string KnownAs { get; set; }
        public string Country { get; set; }

        // Games
        public int LeagueId { get; set; } = 0;
        public int WinnerId { get; set; } = 0;
        public int OpponentId { get; set; } = 0;
    }
}