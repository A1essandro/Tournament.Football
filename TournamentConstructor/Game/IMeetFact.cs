﻿using System;
using TournamentConstructor.GameUnit;

namespace TournamentConstructor.Game
{

    [Obsolete]
    public interface IMeetFact
    {
        bool IsDraft { get; }

        IGameUnit Winner { get; }

        IGameUnit Loser { get; }
    }
}