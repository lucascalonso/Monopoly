using System.Linq;
using System;
using System.Collections.Generic;
using Monopoly.Core;
using Monopoly.Interfaces;

namespace Monopoly.Cards
{
    public class ChanceCard : Card
    {
        public ChanceCard(string description, Action<Monopoly.Core.Player, IGameContext> action) : base(description)
        {
            this.action = action;
        }
        private Action<Monopoly.Core.Player, IGameContext> action;
        public override void Execute(Monopoly.Core.Player player, IGameContext gameContext)
        {
            action(player, gameContext);
        }

        public static List<ChanceCard> GetDefaultCards()
        {
            return new List<ChanceCard>
            {
                new ChanceCard("Avance até o Go e receba $200.", (Monopoly.Core.Player p, IGameContext g) => { g.MovePlayer(p, 0); g.AddMoney(p, 200); }),
                new ChanceCard("Vá para a prisão.", (Monopoly.Core.Player p, IGameContext g) => g.SendPlayerToJail(p)),
                new ChanceCard("Avance até a Boardwalk.", (Monopoly.Core.Player p, IGameContext g) => g.MovePlayer(p, 39)),
                new ChanceCard("Pague $15 de multa.", (Monopoly.Core.Player p, IGameContext g) => g.RemoveMoney(p, 15)),
                new ChanceCard("Receba $150 do banco.", (Monopoly.Core.Player p, IGameContext g) => g.AddMoney(p, 150)),
                new ChanceCard("Volte 3 casas.", (Monopoly.Core.Player p, IGameContext g) => g.MovePlayer(p, (p.Position - 3 + g.GetBoard().Squares.Count) % g.GetBoard().Squares.Count)),
                new ChanceCard("Avance até a Reading Railroad.", (Monopoly.Core.Player p, IGameContext g) => g.MovePlayer(p, 5)),
                new ChanceCard("Avance até Illinois Avenue.", (Monopoly.Core.Player p, IGameContext g) => g.MovePlayer(p, 24)),
                new ChanceCard("Pague $50 ao banco.", (Monopoly.Core.Player p, IGameContext g) => g.RemoveMoney(p, 50)),
                new ChanceCard("Receba $50 do banco.", (Monopoly.Core.Player p, IGameContext g) => g.AddMoney(p, 50)),
                new ChanceCard("Carta 'Saída Livre da Prisão' (Chance)", (Monopoly.Core.Player p, IGameContext g) => {
                    if (!p.HeldCards.Any(c => c.Description.Contains("Saída Livre da Prisão") && c.Description.Contains("Chance")))
                    {
                        p.HeldCards.Add(g as Card);
                        Console.WriteLine($"{p.Name} recebeu uma carta 'Saída Livre da Prisão' (Chance). Guarde até precisar ou negocie.");
                    }
                })
            };
        }
    }
}