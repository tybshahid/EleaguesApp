using System.Collections.Generic;

namespace EleaguesApp.API.Dtos
{
    public class GameManagerDto
    {
        public IEnumerable<DropdownListDto> Leagues { get; set; }
        public IEnumerable<DropdownListDto> Winners { get; set; }
        public IEnumerable<DropdownListDto> Opponents { get; set; }
    }
}