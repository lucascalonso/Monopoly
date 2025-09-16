using System;
using Monopoly.Core;

namespace Monopoly.Squares
{
    public class Square
    {
        public string Name { get; set; }
        public int Position { get; set; }

        public Square(string name, int position)
        {
            Name = name;
            Position = position;
        }
    public virtual void OnLand(Player player, Game game)
        {
            Console.WriteLine($"{player.Name} caiu em {Name}.");
        }
    }
}
