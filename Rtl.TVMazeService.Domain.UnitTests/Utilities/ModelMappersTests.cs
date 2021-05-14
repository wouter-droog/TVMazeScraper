using System.Collections.Generic;
using FizzWare.NBuilder;
using FluentAssertions;
using Rtl.TVMazeService.Domain.Models;
using Rtl.TVMazeService.Domain.Models.Maze;
using Rtl.TVMazeService.Domain.Utilities;
using Xunit;

namespace Rtl.TVMazeService.Domain.UnitTests.Utilities
{
    public class ModelMappersTests
    {
        [Fact]
        public void ConvertShowsFromMazeToShows()
        {
            // Arrange
            var expected = GetShows() as List<Show>;
            var showsFromMaze = GetShowsFromMaze();

            // Act
            var result = ModelMappers.ConvertShowsFromMazeToShows(showsFromMaze as List<ShowDataMaze>);

            // Assert
            _ = result.Should().BeEquivalentTo(expected);
        }

        private static IList<Show> GetShows() =>
            Builder<Show>.CreateListOfSize(3)
            .All().With(s => s.Id = 0)
            .Build();

        private static IList<ShowDataMaze> GetShowsFromMaze() =>
            Builder<ShowDataMaze>.CreateListOfSize(3)
            .Build();
    }
}
