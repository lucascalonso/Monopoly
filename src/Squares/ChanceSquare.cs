using System;
using Monopoly.Core;

namespace Monopoly.Squares
{
    public class ChanceSquare : Square
    {
        public ChanceSquare(string name, int position)
            : base(name, position)
        {
        }

    public override void OnLand(Player player, Game game)
        {
            Console.WriteLine($"{player.Name} caiu em {Name} e deve pegar uma carta de sorte/revés.");
            if (game?.ChanceDeck != null)
            {
                var card = game.ChanceDeck.Draw();
                if (card != null)
                {
                    Console.WriteLine($"Carta: {card.Description}");
            card.Execute(player, game); // O método Execute já aceita Game, que implementa IGameContext
                }
                else
                {
                    Console.WriteLine("Não há cartas no deck de Chance.");
                }
            }
        }
    }
}