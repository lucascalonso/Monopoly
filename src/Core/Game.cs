
using System;
using System.Collections.Generic;
using Monopoly.Cards;
using Monopoly.Squares;
using Monopoly.Trading;
using Monopoly.Interfaces;

namespace Monopoly.Core
{
    public class Game : IGameContext
    {
        public Board Board { get; set; }
        public List<Player> Players { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public Deck CommunityChestDeck { get; set; }
        public Deck ChanceDeck { get; set; }

        public Game(Board board, List<Player> players, Deck communityChestDeck, Deck chanceDeck)
        {
            Board = board;
            Players = players;
            CommunityChestDeck = communityChestDeck;
            ChanceDeck = chanceDeck;
            CurrentPlayerIndex = 0;
        }

        public void PlayTurn()
        {
            var currentPlayer = Players[CurrentPlayerIndex];
            var playerTurn = new PlayerTurn(currentPlayer, Board, new Dice(), this);
            playerTurn.Execute();
        }

        public void NextTurn()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
        }

        // Implementação de IGameContext
        public List<Player> GetPlayers() => Players;
        public Board GetBoard() => Board;
        public void MovePlayer(Player player, int newPosition) => player.Position = newPosition;
        public void AddMoney(Player player, int amount) => player.Money += amount;
        public void RemoveMoney(Player player, int amount) => player.Money -= amount;
        public void SendPlayerToJail(Player player)
        {
            player.Position = 10; // posição da prisão
            player.IsInJail = true;
            player.JailTurns = 0;
        }
        public void TransferProperty(Player from, Player to, Property property)
        {
            if (from.Properties.Contains(property))
            {
                from.Properties.Remove(property);
                to.Properties.Add(property);
                property.Owner = to;
            }
        }

        public void RemoveBankruptPlayer(Player player)
        {
            RemovePlayer(player);
        }

        public void MortgageProperty(Player player, Property property)
        {
            if (player.Properties.Contains(property) && !property.IsMortgaged)
            {
                property.IsMortgaged = true;
                player.Money += property.MortgageValue;
            }
        }

        public void UnmortgageProperty(Player player, Property property)
        {
            if (player.Properties.Contains(property) && property.IsMortgaged)
            {
                int cost = (int)(property.MortgageValue * 1.1); // 10% juros
                if (player.Money >= cost)
                {
                    player.Money -= cost;
                    property.IsMortgaged = false;
                }
            }
        }

        public void RemovePlayer(Player player)
        {
            if (Players.Contains(player))
            {
                Players.Remove(player);
                foreach (var prop in Board.Squares)
                {
                    if (prop is Property p && p.Owner == player)
                    {
                        p.Owner = null;
                    }
                }
                Console.WriteLine($"{player.Name} está falido e foi removido do jogo!");
            }
        }

        public void AddProperty(Player player, Property property)
        {
            if (!player.Properties.Contains(property))
            {
                player.Properties.Add(property);
                property.Owner = player;
            }
        }

        public void RemoveProperty(Player player, Property property)
        {
            if (player.Properties.Contains(property))
            {
                player.Properties.Remove(property);
                property.Owner = null;
            }
        }

        public Property GetPropertyByName(string name)
        {
            foreach (var square in Board.Squares)
            {
                if (square is Property prop && prop.Name == name)
                    return prop;
            }
            return null;
        }

        public Player GetCurrentPlayer() => Players[CurrentPlayerIndex];

        public Deck GetDeck(string type)
        {
            return type.ToLower() switch
            {
                "chance" => ChanceDeck,
                "communitychest" => CommunityChestDeck,
                _ => null
            };
        }

        public void BroadcastMessage(string message)
        {
            // Simplesmente imprime, mas pode ser expandido para logs/eventos
            System.Console.WriteLine(message);
        }

        public bool CanPlayerPay(Player player, int amount) => player.Money >= amount;

        public Player GetPlayerByName(string name)
        {
            foreach (var p in Players)
                if (p.Name == name) return p;
            return null;
        }
    }
}
