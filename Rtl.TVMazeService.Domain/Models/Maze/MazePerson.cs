using System;

namespace Rtl.TVMazeService.Domain.Models.Maze
{
    public class MazePerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}