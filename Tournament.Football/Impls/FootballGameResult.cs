using System;
using Tournament.Contracts;
using Tournament.Football.Contracts;

namespace Tournament.Football
{

    public class FootballGameResult : IFootballMatchResult<Team>
    {

        public FootballGameResult(IFootballMatch<Team> game, IPair<int> points)
        {
            Game = game;
            Points = points;
        }

        public bool IsDraw => Points.Item1 == Points.Item2;

        public Team Winner
        {
            get
            {
                if (Penalties != null)
                {
                    return Penalties.Item1 > Penalties.Item2
                                            ? Game.ParticipantPlaces.Item1.Participant
                                            : Penalties.Item1 < Penalties.Item2
                                                ? Game.ParticipantPlaces.Item2.Participant
                                                : throw new Exception(); //TODO:
                }
                return Points.Item1 > Points.Item2
                        ? Game.ParticipantPlaces.Item1.Participant
                        : Points.Item1 < Points.Item2
                            ? Game.ParticipantPlaces.Item2.Participant
                            : null;
            }
        }

        public Team Loser
        {
            get
            {
                if (Penalties != null)
                {
                    return Penalties.Item1 > Penalties.Item2
                                        ? Game.ParticipantPlaces.Item2.Participant
                                        : Penalties.Item1 < Penalties.Item2
                                            ? Game.ParticipantPlaces.Item1.Participant
                                            : throw new Exception(); //TODO:
                }
                return Points.Item1 > Points.Item2
                                        ? Game.ParticipantPlaces.Item2.Participant
                                        : Points.Item1 < Points.Item2
                                            ? Game.ParticipantPlaces.Item1.Participant
                                            : null;
            }
        }

        public IPair<int> Points { get; }

        public override string ToString() => $"{Points.Item1}:{Points.Item2}";

        public IFootballMatch<Team> Game { get; }

        public IPair<int> Penalties { get; set; } = null;

        public void SetPenalties()
        {
            var rand = new Random();
            var attempt = 0;
            var home = 0;
            var away = 0;

            while (attempt < 5 || home == away)
            {
                if (rand.NextDouble() > 0.15) home++;
                if (rand.NextDouble() > 0.15) away++;

                attempt++;
            }

            Penalties = Pair<int>.Create(home, away);
        }

    }
}