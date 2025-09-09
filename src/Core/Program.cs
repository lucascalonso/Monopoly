using System;
using System.Collections.Generic;
using Monopoly;
using Monopoly.Cards;
using Monopoly.Trading;
using System.Linq;

namespace Monopoly.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Dice dice = new Dice();
            var jogadores = new List<Player>
        {
            new Player("Ana", 1500),
            new Player("Bruno", 1500),
            new Player("Carlos", 1500),
            new Player("Daniel", 1500)
        };

            var communityDeck = new Deck(CommunityChestCard.GetDefaultCards().Cast<Card>().ToList());
            var chanceDeck = new Deck(ChanceCard.GetDefaultCards().Cast<Card>().ToList());
            var game = new Game(board, jogadores, communityDeck, chanceDeck);
            var tradeManager = new TradeManager(game);

            int rodada = 1;
            while (true)
            {
                Console.WriteLine($"\n--- Rodada {rodada} ---");
                
                var jogadoresAtivos = jogadores.ToList();
                foreach (var player in jogadoresAtivos)
                {
                    Console.WriteLine($"\nVez de {player.Name} (Dinheiro: {player.Money})");
                    var turn = new PlayerTurn(player, board, dice, game);
                    turn.Execute();
                }
                rodada++;
            }
        }

    }
}