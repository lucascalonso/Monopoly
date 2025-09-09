using System;
using Monopoly.Core;

namespace Monopoly.Squares
{
    public class GoSquare : Square
    {
        public int Bonus { get; set; }

        public GoSquare(string name, int position, int bonus)
            : base(name, position)
        {
            Bonus = bonus;
        }

    public override void OnLand(Player player, Game game)
        {
            Console.WriteLine($"{player.Name} passou pelo {Name} e recebe {Bonus}.");
            player.Money += Bonus;
        }
    }
}