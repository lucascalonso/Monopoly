using System;
using Monopoly.Core;

namespace Monopoly.Squares
{
    public class TaxSquare : Square
    {
        public int TaxAmount { get; set; }

        public TaxSquare(string name, int position, int taxAmount)
            : base(name, position)
        {
            TaxAmount = taxAmount;
        }

    public override void OnLand(Player player, Game game)
        {
            int tax = TaxAmount;
            AssetManager assetManager = new AssetManager(player);
            if (assetManager.TryRaiseFunds(tax))
            {
                player.Money -= tax;
                Console.WriteLine($"{player.Name} pagou {tax} de imposto.");
            }
            else
            {
                Console.WriteLine($"{player.Name} não conseguiu pagar o imposto e está eliminado!");
                // Elimine o jogador do jogo, se quiser
            }
        }
    }
}