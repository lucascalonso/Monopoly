using System.Linq;
using System;
using System.Collections.Generic;

using Monopoly.Core;
using Monopoly.Interfaces;

namespace Monopoly.Cards
{
    public class CommunityChestCard : Card
    {
        public CommunityChestCard(string description, Action<Monopoly.Core.Player, IGameContext> action) : base(description)
        {
            this.action = action;
        }
        private Action<Monopoly.Core.Player, IGameContext> action;
        public override void Execute(Monopoly.Core.Player player, IGameContext gameContext)
        {
            action(player, gameContext);
        }

        public static List<CommunityChestCard> GetDefaultCards()
        {
            return new List<CommunityChestCard>
            {
                new CommunityChestCard("Receba $200 do banco.", (Monopoly.Core.Player p, IGameContext g) => g.AddMoney(p, 200)),
                new CommunityChestCard("Pague $50 ao banco.", (Monopoly.Core.Player p, IGameContext g) => g.RemoveMoney(p, 50)),
                new CommunityChestCard("Vá para a prisão.", (Monopoly.Core.Player p, IGameContext g) => g.SendPlayerToJail(p)),
                new CommunityChestCard("Receba $100 de herança.", (Monopoly.Core.Player p, IGameContext g) => g.AddMoney(p, 100)),
                new CommunityChestCard("Pague $100 de despesas médicas.", (Monopoly.Core.Player p, IGameContext g) => g.RemoveMoney(p, 100)),
                new CommunityChestCard("Receba $25 de cada jogador.", (Monopoly.Core.Player p, IGameContext g) => {
                    foreach (Monopoly.Core.Player other in g.GetPlayers()) if (other != p) { g.RemoveMoney(other, 25); g.AddMoney(p, 25); }
                }),
                new CommunityChestCard("Pague $10 de multa por atraso.", (Monopoly.Core.Player p, IGameContext g) => g.RemoveMoney(p, 10)),
                new CommunityChestCard("Receba $50 de reembolso de imposto.", (Monopoly.Core.Player p, IGameContext g) => g.AddMoney(p, 50)),
                new CommunityChestCard("Avance até o Go e receba $200.", (Monopoly.Core.Player p, IGameContext g) => { g.MovePlayer(p, 0); g.AddMoney(p, 200); }),
                new CommunityChestCard("Seu aniversário! Receba $10 de cada jogador.", (Monopoly.Core.Player p, IGameContext g) => {
                    foreach (Monopoly.Core.Player other in g.GetPlayers()) if (other != p) { g.RemoveMoney(other, 10); g.AddMoney(p, 10); }
                }),
                new CommunityChestCard("Carta 'Saída Livre da Prisão' (Community Chest)", (Monopoly.Core.Player p, IGameContext g) => {
                    if (!p.HeldCards.Any(c => c.Description.Contains("Saída Livre da Prisão") && c.Description.Contains("Community Chest")))
                    {
                        p.HeldCards.Add(g as Card);
                        Console.WriteLine($"{p.Name} recebeu uma carta 'Saída Livre da Prisão' (Community Chest). Guarde até precisar ou negocie.");
                    }
                })
            };
        }
    }
}