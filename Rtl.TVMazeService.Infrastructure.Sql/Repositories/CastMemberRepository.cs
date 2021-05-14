using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rtl.TVMazeService.Domain.Interfaces.Repositories;
using Rtl.TVMazeService.Domain.Models;
using Rtl.TVMazeService.Domain.Models.Dtos;

namespace Rtl.TVMazeService.Infrastructure.Sql.Repositories
{
    public class CastMemberRepository : ICastMemberRepository
    {
        private readonly TVMazeContext context;

        public CastMemberRepository(TVMazeContext context) => this.context = context;

        public List<IdsDto> GetIds(IEnumerable<int> mazeIds) =>
            this.context.CastMembers
                .Where(cm => mazeIds.Contains(cm.MazeId))
                .Select(cm => new IdsDto
                {
                    Id = cm.Id,
                    MazeId = cm.MazeId
                }).ToList();

        public async Task InsertCastMemberRelationsWithShow(int showId, List<IdsDto> castMemberIds)
        {
            var showCastMembers = castMemberIds.Select(m => new ShowCastMember
            {
                ShowId = showId,
                CastMemberId = m.Id
            });
            this.context.ShowCastMembers.AddRange(showCastMembers);
            _ = await this.context.SaveChangesAsync();
        }

        public async Task InsertRange(IEnumerable<ShowCastMember> showCastMembers)
        {
            await this.context.ShowCastMembers.AddRangeAsync(showCastMembers);
            _ = await this.context.SaveChangesAsync();
        }
    }
}
