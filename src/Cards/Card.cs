using System;
using Monopoly.Core;
using Monopoly.Interfaces;

namespace Monopoly.Cards
{
    public abstract class Card
    {
        public string Description { get; protected set; }
        public Card(string description)
        {
            Description = description;
        }
    public abstract void Execute(Monopoly.Core.Player player, IGameContext gameContext);
    }
}