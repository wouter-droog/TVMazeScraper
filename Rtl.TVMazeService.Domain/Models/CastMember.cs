using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rtl.TVMazeService.Domain.Models
{
    public class CastMember
    {
        public int Id { get; set; }
        public int MazeId { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDay { get; set; }

        public ICollection<ShowCastMember> ShowCastMembers { get; set; }
    }
}
