using System;
using Moq;
using Shouldly;
using Tournament.Contracts;
using Tournament.Football.Contracts;
using Xunit;

namespace Tournament.Football.Tests
{
    public class FootballMatchTest
    {

        [Fact]
        public void Common()
        {
            var p1 = new Mock<IParticipantPlace<Team>>();
            var p2 = new Mock<IParticipantPlace<Team>>();
            var stage = new Mock<IStage>();
            var matchResult = new Mock<IFootballMatchResult<Team>>();

            var match = new FootballMatch(p1.Object, p2.Object, DateTime.Today, stage.Object);

            var completed = false;
            var resultSetted = false;
            match.OnCompleted += g => completed = true;
            match.OnResultSet += g => resultSetted = true;

            match.Result = matchResult.Object;

            completed.ShouldBeTrue();
            resultSetted.ShouldBeTrue();
            match.HasResult.ShouldBeTrue();
            match.Context.ShouldBe(stage.Object);
        }
    }
}
