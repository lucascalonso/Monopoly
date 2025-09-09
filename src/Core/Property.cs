using System.Collections.Generic;
using System;
using System.Linq;

namespace Monopoly.Core
{
    public class Property : Square
    {
        public int Price { get; set; }
        public Player Owner { get; set; }
        public bool IsMortgaged { get; set; } = false;
        public int Houses { get; set; } = 0;
        public bool HasHotel { get; set; } = false;
        public string ColorGroup { get; set; }
        public int MortgageValue { get; set; }

        public Property(string name, int position, int price, int mortgageValue = 0)
            : base(name, position)
        {
            Price = price;
            Owner = null;
            MortgageValue = mortgageValue > 0 ? mortgageValue : price / 2;
        }
    public override void OnLand(Player player, Game game)
        {
            if (Owner == null)
            {
                Console.WriteLine($"{player.Name} caiu em {Name}, que está disponível por {Price}.");
                // Compra é tratada em Buy()
            }
            else if (Owner != player)
            {
                if (IsMortgaged)
                {
                    Console.WriteLine($"{player.Name} caiu em {Name}, que está hipotecada. Não há cobrança de aluguel.");
                    // Não paga aluguel nem ao banco
                    return;
                }

                int rent = CalculateRent();
                AssetManager assetManager = new AssetManager(player);
                if (assetManager.TryPay(rent))
                {
                    Owner.Money += rent;
                    Console.WriteLine($"{player.Name} pagou {rent} de aluguel para {Owner.Name}.");
                }
                else
                {
                    game.RemoveBankruptPlayer(player);
                }
            }
            else
            {
                Console.WriteLine($"{player.Name} caiu em sua própria propriedade.");
            }
        }

        public void Buy(Player player, List<Player> allPlayers)
        {
            AssetManager assetManager = new AssetManager(player);
            if (assetManager.TryPay(Price))
            {
                Owner = player;
                player.Properties.Add(this);
                Console.WriteLine($"{player.Name} comprou {Name} por {Price}.");
            }
            else
            {
                Console.WriteLine($"{player.Name} não conseguiu comprar {Name}. A propriedade permanece disponível.");
            }
        }
        public int CalculateRent()
        {
            // Exemplo simples: aluguel é metade do preço
            return Price / 2;
        }
        public bool CanBuildHouse(Player player, Board board)
        {
            // Verifica se o jogador tem todas as propriedades do grupo
            var groupProps = board.Squares
                .OfType<Property>()
                .Where(p => p.ColorGroup == this.ColorGroup)
                .ToList();
            bool ownsAll = groupProps.All(p => p.Owner == player);
            if (!ownsAll || Houses >= 4 || HasHotel || IsMortgaged)
                return false;

            // Regra de construção equilibrada
            int minHouses = groupProps.Min(p => p.Houses);
            return Houses <= minHouses;
        }

        public void BuildHouse(Player player, Board board)
        {
            if (CanBuildHouse(player, board))
            {
                int houseCost = 50; // Exemplo, ajuste conforme o grupo
                if (player.Money >= houseCost)
                {
                    player.Money -= houseCost;
                    Houses++;
                    Console.WriteLine($"{player.Name} construiu uma casa em {Name}. Total de casas: {Houses}");
                }
                else
                {
                    Console.WriteLine("Dinheiro insuficiente para construir casa.");
                }
            }
            else
            {
                Console.WriteLine("Não é possível construir casa nesta propriedade (verifique regras de construção equilibrada).");
            }
        }

        public bool CanBuildHotel(Player player, Board board)
        {
            var groupProps = board.Squares
                .OfType<Property>()
                .Where(p => p.ColorGroup == this.ColorGroup)
                .ToList();

            bool ownsAll = groupProps.All(p => p.Owner == player);
            bool allHaveAtLeast4Houses = groupProps.All(p => p.Houses >= 4);
            bool noneMortgaged = groupProps.All(p => !p.IsMortgaged);

            return ownsAll && allHaveAtLeast4Houses && noneMortgaged && Houses == 4 && !HasHotel;
        }

        public void BuildHotel(Player player, Board board)
        {
            var groupProps = board.Squares
                .OfType<Property>()
                .Where(p => p.ColorGroup == this.ColorGroup)
                .ToList();

            if (CanBuildHotel(player, board))
            {
                int hotelCost = 100; // Ajuste conforme o grupo
                if (player.Money >= hotelCost)
                {
                    player.Money -= hotelCost;
                    HasHotel = true;
                    Houses = 0;
                    Console.WriteLine($"{player.Name} construiu um hotel em {Name}!");

                    // Remove as 4 casas de todas as propriedades do grupo
                    foreach (var prop in groupProps)
                    {
                        if (prop != this && prop.Houses > 0)
                            prop.Houses = prop.Houses; // Mantém as casas nas outras, pois só esta vira hotel
                    }
                }
                else
                {
                    Console.WriteLine("Dinheiro insuficiente para construir hotel.");
                }
            }
            else
            {
                Console.WriteLine("Não é possível construir hotel nesta propriedade. Todas as propriedades do grupo precisam ter 4 casas e não podem estar hipotecadas.");
            }
        }
    }
}