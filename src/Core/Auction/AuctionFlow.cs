using System;
using System.Collections.Generic;
using Monopoly.Core;

namespace Monopoly.Core.Auction
{
    public static class AuctionFlow
    {
    public static void StartAuction(Property property, List<Player> players, int startIndex = 0)
        {
            var auction = new Auction(property, players);
            int currentBid = 1;
            Player lastBidder = null;
            int n = players.Count;
            int passes = 0;
            int currentIndex = (startIndex + 1) % n;

            Console.WriteLine($"Leilão iniciado para {property.Name}! Lance inicial: 1");
            // O leilão só termina quando todos, exceto o último a dar lance, passarem
            while (true)
            {
                var player = players[currentIndex];
                if (player == lastBidder && passes > 0) // só resta o último licitante
                    break;

                // Não perguntar ao último que deu o maior lance
                if (player != lastBidder)
                {
                    Console.WriteLine($"{player.Name}, deseja dar um lance maior que {currentBid}? (s/n)");
                    string input = Console.ReadLine()?.Trim().ToLower();
                    if (input == "s" || input == "y")
                    {
                        Console.WriteLine($"Qual é o seu lance? (mínimo: {currentBid + 1}, máximo: {player.Money})");
                        if (int.TryParse(Console.ReadLine(), out int value) && value > currentBid && value <= player.Money)
                        {
                            auction.PlaceBid(player, value);
                            currentBid = value;
                            lastBidder = player;
                            passes = 0;
                        }
                        else
                        {
                            Console.WriteLine("Lance inválido!");
                        }
                    }
                    else
                    {
                        passes++;
                    }
                }
                currentIndex = (currentIndex + 1) % n;
            }

            auction.Finish();
            if (auction.Winner != null)
            {
                Console.WriteLine($"{auction.Winner.Name} venceu o leilão de {property.Name} por {auction.FinalBid}!");
            }
            else
            {
                Console.WriteLine($"Ninguém arrematou {property.Name}!");
            }
        }
    }
}
