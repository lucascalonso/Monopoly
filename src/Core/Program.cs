using System;
using System.Collections.Generic;
using Monopoly.Squares;
using Monopoly.Cards;
using Monopoly.Trading;
using System.Linq;
// Add the following using if PropertySquare is in a different namespace
// using Monopoly.Squares; // Already present, ensure PropertySquare is defined in this namespace

namespace Monopoly.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Selecione o modo de jogo para rodar:");
            Console.WriteLine("1 - Jogo normal");
            Console.WriteLine("2 - Todos jogadores começam presos");
            Console.WriteLine("3 - Mario possui todas as propriedades de uma cor (teste de upgrade)");
            Console.WriteLine("4 - Mario e Peach começam com uma propriedade cada (teste de troca)");
            Console.Write("Opção: ");
            string opcao = Console.ReadLine()?.Trim();

            Board board = new Board();
            Dice dice = new Dice();
            var jogadores = new List<Player>
            {
                new Player("Mario", 1500),
                new Player("Luigi", 1500),
                new Player("Peach", 1500),
                new Player("Daisy", 1500)
            };
            var communityDeck = new Deck(CommunityChestCard.GetDefaultCards().Cast<Card>().ToList());
            var chanceDeck = new Deck(ChanceCard.GetDefaultCards().Cast<Card>().ToList());
            var game = new Game(board, jogadores, communityDeck, chanceDeck);
            var tradeManager = new TradeManager(game);

            switch (opcao)
            {
                case "1":
                    // Jogo normal
                    game.StartGameLoop();
                    break;
                case "2":
                    // Todos presos
                    foreach (var p in jogadores)
                    {
                        p.Position = 10; // posição da prisão
                        p.IsInJail = true;
                        p.JailTurns = 0;
                    }
                    // Adiciona carta de saída da prisão para Bruno
                    var cartaSaida = CommunityChestCard.GetDefaultCards().FirstOrDefault(c => c.Description.Contains("Saída Livre da Prisão"));
                    if (cartaSaida != null)
                    {
                        jogadores[1].HeldCards.Add(cartaSaida);
                    }
                    Console.WriteLine("Todos os jogadores começaram presos!");
                    game.StartGameLoop();
                    break;
                case "3":
                    // Mario possui todas as propriedades de uma cor
                    var grupoCor = "Marrom"; // Exemplo: grupo marrom
                    var propriedades = board.Squares
                        .OfType<Property>()
                        .Where(p => p.ColorGroup == grupoCor)
                        .ToList();
                    foreach (var prop in propriedades)
                    {
                        prop.Owner = jogadores[0]; // Mario
                        jogadores[0].Properties.Add(prop);
                    }
                    Console.WriteLine("Mario agora possui todas as propriedades do grupo " + grupoCor + "!");
                    Console.WriteLine("Teste: Mario pode fazer upgrade de casas nessas propriedades.");
                    // Aqui você pode chamar o método de upgrade ou mostrar as opções
                    game.StartGameLoop();
                    break;
                case "4":
                    // Mario e Peach começam com uma propriedade cada e testam troca
                    var propMario = board.Squares.OfType<Property>().First();
                    var propPeach = board.Squares.OfType<Property>().Skip(1).First();
                    propMario.Owner = jogadores[0]; // Mario
                    jogadores[0].Properties.Add(propMario);
                    propPeach.Owner = jogadores[2]; // Peach
                    jogadores[2].Properties.Add(propPeach);
                    Console.WriteLine($"Mario possui {propMario.Name}, Peach possui {propPeach.Name}.");
                    Console.WriteLine("Teste: Troca entre Mario e Peach.");
                    // Aqui você pode chamar o método de troca ou mostrar as opções
                    game.StartGameLoop();
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }
}