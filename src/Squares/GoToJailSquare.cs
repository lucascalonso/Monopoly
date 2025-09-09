using System;
using Monopoly.Core;

namespace Monopoly.Squares
{
    public class GoToJailSquare : Square
    {
        public int JailPosition { get; set; }

        public GoToJailSquare(string name, int position, int jailPosition)
            : base(name, position)
        {
            JailPosition = jailPosition;
        }

    public override void OnLand(Player player, Game game)
        {
            Console.WriteLine($"{player.Name} caiu em {Name} e vai direto para a prisão!");
            player.Position = JailPosition;
            player.IsInJail = true;
            player.JailTurns = 0;
        }
    }
}
