using System.Collections.Generic;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Rtl.TVMazeService.Domain.Interfaces.Repositories;
using Rtl.TVMazeService.Domain.Models;
using Rtl.TVMazeService.Domain.Models.Dtos;
using Rtl.TVMazeService.Domain.Services;
using Xunit;

namespace Rtl.TVMazeService.Domain.UnitTests.Services
{
    public class ShowServiceTests
    {
        private readonly AutoMocker mocker;
        private readonly Mock<IShowRepository> showRepositoryMock;

        public ShowServiceTests()
        {
            this.mocker = new AutoMocker();
            this.showRepositoryMock = this.mocker.GetMock<IShowRepository>();
        }

        [Fact]
        public async Task GetPagedResult_WhenShorRepoReturnShows_ShoulReturnShowDtos()
        {
            // Arrange
            _ = this.showRepositoryMock.Setup(s => s.GetPagedShows(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(GetShows() as List<Show>);

            var sut = this.mocker.CreateInstance<ShowService>();

            // Act
            var result = await sut.GetPagedResult(1, 2);

            // Assert
            _ = result.Should().BeOfType<List<ShowDto>>();
        }

        private static IList<Show> GetShows()
        {
            var shows = Builder<Show>.CreateListOfSize(3)
            .All().With(s => s.Id = 0)
                .All().With(d => d.ShowCastMembers = Builder<ShowCastMember>.CreateListOfSize(1)
                .All().With(scm => scm.CastMember = Builder<CastMember>.CreateNew()
                .Build())
                .Build())
            .Build();

            return shows;
        }
    }
}
