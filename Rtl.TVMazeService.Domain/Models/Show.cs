using System.Collections.Generic;

namespace Rtl.TVMazeService.Domain.Models
{
    public class Show
    {
        public int Id { get; set; }
        public int MazeId { get; set; }
        public string Name { get; set; }
        public ICollection<ShowCastMember> ShowCastMembers { get; set; }
    }
}
