using System;
using Monopoly.Core;

namespace Monopoly.Squares
{
    public class Railroad : Property
    {
        public Railroad(string name, int position, int price)
            : base(name, position, price)
        {
        }

    public override void OnLand(Player player, Game game)
        {
            if (Owner == null)
            {
                Console.WriteLine($"{player.Name} caiu na ferrovia {Name}, disponível por {Price}.");
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
                int ownedRailroads = Owner.Properties.FindAll(p => p is Railroad).Count;
                int rent = 25 * ownedRailroads; // Regra clássica do Monopoly
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
                Console.WriteLine($"{player.Name} caiu em sua própria ferrovia ({Name}).");
            }
        }
    }
}