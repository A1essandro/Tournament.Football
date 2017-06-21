﻿using TournamentConstructor.Game;

namespace TournamentConstructor.Structure
{
    public class Tour<TMeetType> : ITour<TMeetType> where TMeetType : IMeetFact
    {
        public Tour(IMeet<TMeetType>[] games)
        {
            Games = games;
        }

        public IMeet<TMeetType>[] Games { get; }

        public IMeet<TMeetType> this[int index] => Games[index];
    }
}