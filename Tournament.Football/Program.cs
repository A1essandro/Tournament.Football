using System;
using System.Collections.Generic;
using System.Linq;
using Tournament.Contracts;
using Tournament.Football.Contracts;

namespace Tournament.Football
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var teams = new[]{
                    new Team("Bayern Munich", 0.8),
                    new Team("Atlético Madrid", 0.7),
                    new Team("Red Bull Salzburg", 0.6),
                    new Team("Lokomotiv Moscow", 0.55),
                    new Team("Manchester City", 0.8),
                    new Team("Porto Porto", 0.65),
                    new Team("Olympiacos", 0.55),
                    new Team("Marseille", 0.55),
                    new Team("Inter Milan", 0.65),
                    new Team("Real Madrid", 0.8),
                    new Team("Borussia Monchengladbach", 0.7),
                    new Team("Shakhtar Donetsk", 0.6),
                    new Team("Juventus Torino", 0.7),
                    new Team("Barcelona Barcelona", 0.7),
                    new Team("Ferencvaros Budapest", 0.5),
                    new Team("Dynamo Kyiv", 0.55)
                };
            var rand = new Random();
            Action<IEnumerable<GroupStage>> groupsDraw = groupStages =>
            {
                var stack = new Stack<Team>(teams.OrderBy(x => rand.Next()));
                foreach (var gs in groupStages)
                {
                    gs.ParticipantPlaces[0].Participant = stack.Pop();
                    gs.ParticipantPlaces[1].Participant = stack.Pop();
                    gs.ParticipantPlaces[2].Participant = stack.Pop();
                    gs.ParticipantPlaces[3].Participant = stack.Pop();
                }
            };

            Action<IEnumerable<KnockoutStage>, IEnumerable<Team>> playoffDraw = (kstages, teams) =>
                        {
                            var stack = new Stack<Team>(teams.OrderBy(x => rand.Next()));
                            foreach (var ks in kstages)
                            {
                                foreach (var place in ks.ParticipantPlaces)
                                {
                                    place.Participant = stack.Pop();
                                }
                            }
                        };

            while (true)
            {
                System.Console.WriteLine(Environment.NewLine + Environment.NewLine);

                var lc = new ChampionsLeague(groupsDraw, playoffDraw);

                IStage previuosStage = null;
                foreach (var g in lc.Schedule)
                {
                    if (g.Context.GetType() != previuosStage?.GetType())
                        System.Console.WriteLine($"========\t{g.Context.GetType().Name}\t========");
                    g.Result = GetResult(rand, g);
                    if (!g.Result.IsDraw)
                    {
                        g.Result.Winner.Rating += 0.1;
                    }
                    else
                    {
                        g.ParticipantPlaces.Item2.Participant.Rating += 0.05;
                        g.ParticipantPlaces.Item1.Participant.Rating += 0.05;
                    }
                    System.Console.WriteLine($"{g.Date}\t{g.ParticipantPlaces.Item1.Participant.Name} - {g.ParticipantPlaces.Item2.Participant.Name}\t{g.Result}");
                    previuosStage = g.Context;
                }

                System.Console.WriteLine();
                foreach (var t in teams.OrderByDescending(x => x.Rating))
                {
                    t.Rating = Math.Log(1 + t.Rating);
                    System.Console.WriteLine($"({t.Winner})\t{t.Name}:\t{t.Rating}");
                }

                System.Console.ReadKey();
            }

        }

        private static void Penalties(object res)
        {
            var rand = new Random();
            var stage = 0;
            var p1 = 0;
            var p2 = 0;
            while (stage <= 5 || p1 == p2)
            {
                if (rand.NextDouble() > 0.15)
                    p1++;
                if (rand.NextDouble() > 0.15)
                    p2++;
                stage++;
            }

            var game = (res as KnockoutStageResult).Stage.Schedule.Last();
            System.Console.WriteLine($"Penalties! {game.ParticipantPlaces.Item1.Participant} - {game.ParticipantPlaces.Item2.Participant} \t {p1}:{p2}");

            game.Result.Penalties = Pair<int>.Create(p1, p2);
        }

        private static FootballGameResult GetResult(Random rand, IFootballMatch<Team> game)
        {
            try
            {
                var homeStr = Math.Log(1 + game.ParticipantPlaces.Item1.Participant.Rating / game.ParticipantPlaces.Item2.Participant.Rating);
                var awayStr = Math.Log(1 + game.ParticipantPlaces.Item2.Participant.Rating / game.ParticipantPlaces.Item1.Participant.Rating);
                var home = GetPoints(rand, homeStr);
                var away = GetPoints(rand, awayStr * 0.9);
                return new FootballGameResult(game, Pair<int>.Create(home, away));
            }
            catch
            {
                throw;
            }
        }

        private static int GetPoints(Random rand, double str)
        {
            var points = 0;

            var factor = 1 / (1 + str);
            while (rand.NextDouble() < (str * factor))
            {
                str *= 0.83;
                points++;
            }

            return points;
        }
    }
}
