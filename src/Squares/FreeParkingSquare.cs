using System;
using Monopoly.Core;

namespace Monopoly.Squares
{
    public class FreeParkingSquare : Square
    {
        public FreeParkingSquare(string name, int position)
            : base(name, position)
        {
        }

    public override void OnLand(Player player, Game game)
        {
            Console.WriteLine($"{player.Name} está no Estacionamento Livre. Nada acontece.");
        }
    }
}
