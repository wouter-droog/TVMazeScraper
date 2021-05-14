using System.Collections.Generic;

namespace Rtl.TVMazeService.Domain.Models.Dtos
{
    public class ShowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<CastMemberDto> Cast { get; set; }
    }
}
