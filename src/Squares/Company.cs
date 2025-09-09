using System;
using Monopoly.Core;

namespace Monopoly.Squares
{
    public class Company : Property
    {
        public Company(string name, int position, int price)
            : base(name, position, price)
        {
        }

    public override void OnLand(Player player, Game game)
        {
            if (Owner == null)
            {
                Console.WriteLine($"{player.Name} caiu na companhia {Name}, disponível por {Price}.");
                if (player.Money >= Price)
                {
                    Console.WriteLine($"{player.Name} comprou {Name} por {Price}.");
                    player.Money -= Price;
                    Owner = player;
                    player.Properties.Add(this);
                }
                else
                {
                    Console.WriteLine($"{player.Name} não tem dinheiro suficiente para comprar {Name}.");
                }
            }
            else if (Owner != player)
            {
                Dice dice = new Dice();
                var (roll1, roll2) = dice.RollTwo();
                int roll = roll1 + roll2;
                int ownedCompanies = Owner.Properties.FindAll(p => p is Company).Count;
                int rent = (ownedCompanies == 2) ? roll * 10 : roll * 4;
                Console.WriteLine($"{player.Name} deve pagar aluguel de {rent} para {Owner.Name}.");
                if (player.Money >= rent)
                {
                    player.Money -= rent;
                    Owner.Money += rent;
                }
                else
                {
                    Console.WriteLine($"{player.Name} não tem dinheiro suficiente para pagar o aluguel!");
                }
            }
            else
            {
                Console.WriteLine($"{player.Name} caiu em sua própria companhia ({Name}).");
            }
        }
    }
}