namespace Rtl.TVMazeService.Domain.Models
{
    public class ShowCastMember
    {
        public int ShowId { get; set; }
        public Show Show { get; set; }
        public int CastMemberId { get; set; }

        public CastMember CastMember { get; set; }
    }
}
