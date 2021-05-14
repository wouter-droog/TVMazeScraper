using System.Collections.Generic;
using System.Threading.Tasks;
using Rtl.TVMazeService.Domain.Models;
using Rtl.TVMazeService.Domain.Models.Dtos;

namespace Rtl.TVMazeService.Domain.Interfaces.Repositories
{
    public interface ICastMemberRepository
    {
        List<IdsDto> GetIds(IEnumerable<int> mazeIds);
        Task InsertRange(IEnumerable<ShowCastMember> showCastMembers);
        Task InsertCastMemberRelationsWithShow(int showId, List<IdsDto> castMemberIds);
    }
}
