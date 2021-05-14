using System.Collections.Generic;
using System.Linq;
using Rtl.TVMazeService.Domain.Models;
using Rtl.TVMazeService.Domain.Models.Dtos;
using Rtl.TVMazeService.Domain.Models.Maze;

namespace Rtl.TVMazeService.Domain.Utilities
{
    public static class ModelMappers
    {
        public static List<Show> ConvertShowsFromMazeToShows(List<ShowDataMaze> showDataMaze)
        {
            var listOfShows = showDataMaze
                .Select(t => new Show
                {
                    MazeId = t.Id,
                    Name = t.Name,
                }).ToList();
            return listOfShows;
        }

        public static IEnumerable<ShowCastMember> ConvertCastFromMazeToCastMembers(Show show, IEnumerable<CastDataMaze> castFromMaze)
        {
            var castMembersOfShow = castFromMaze
                .Select(c => new ShowCastMember
                {
                    Show = show,
                    CastMember = new CastMember
                    {
                        MazeId = c.Person.Id,
                        Name = c.Person.Name,
                        BirthDay = c.Person.BirthDay
                    }
                });
            return castMembersOfShow;
        }

        public static IList<ShowDto> ConvertShowToShowDto(List<Show> shows)
        {
            var showDtos = shows
                .Select(s => new ShowDto
                {
                    Id = s.MazeId,
                    Name = s.Name,
                    Cast = s.ShowCastMembers.Select(scm => new CastMemberDto
                    {
                        Id = scm.CastMember.MazeId,
                        Name = scm.CastMember.Name,
                        Birthdate = scm.CastMember.BirthDay
                    }).ToList()
                }).ToList();

            return showDtos;
        }
    }
}
