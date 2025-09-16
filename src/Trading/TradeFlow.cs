// ...existing code...
// Exemplo de uso: inserir no início do turno do jogador
// tradeManager.OferecerNegociacao(player);
using System;
using System.Collections.Generic;

using Monopoly.Core;

namespace Monopoly.Trading
{
    public static class TradeFlow
    {
        public static void OferecerNegociacao(TradeManager tradeManager, Player currentPlayer, List<Player> allPlayers)
        {
            // Escolher jogador alvo
            Console.WriteLine("Para qual jogador deseja negociar? (digite o nome)");
            foreach (var p in allPlayers)
                if (p != currentPlayer) Console.WriteLine(p.Name);
            string nomeAlvo = Console.ReadLine()?.Trim();
            var alvo = allPlayers.Find(p => p.Name.Equals(nomeAlvo, StringComparison.OrdinalIgnoreCase));
            if (alvo == null || alvo == currentPlayer)
            {
                Console.WriteLine("Jogador inválido.");
                return;
            }

            // Propriedades a oferecer
            List<Property> propsOferecidas = new List<Property>();
            if (currentPlayer.Properties.Count > 0)
            {
                Console.WriteLine("Suas propriedades (digite índices separados por vírgula, ou ENTER para nenhuma):");
                for (int i = 0; i < currentPlayer.Properties.Count; i++)
                    Console.WriteLine($"{i}: {currentPlayer.Properties[i].Name}");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    foreach (var idxStr in input.Split(','))
                        if (int.TryParse(idxStr.Trim(), out int idx) && idx >= 0 && idx < currentPlayer.Properties.Count)
                            propsOferecidas.Add(currentPlayer.Properties[idx]);
                }
            }

            // Propriedades a pedir
            List<Property> propsPedidas = new List<Property>();
            if (alvo.Properties.Count > 0)
            {
                Console.WriteLine($"Propriedades de {alvo.Name} (digite índices separados por vírgula, ou ENTER para nenhuma):");
                for (int i = 0; i < alvo.Properties.Count; i++)
                    Console.WriteLine($"{i}: {alvo.Properties[i].Name}");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    foreach (var idxStr in input.Split(','))
                        if (int.TryParse(idxStr.Trim(), out int idx) && idx >= 0 && idx < alvo.Properties.Count)
                            propsPedidas.Add(alvo.Properties[idx]);
                }
            }

            // Dinheiro a oferecer
            Console.WriteLine("Quanto dinheiro você quer oferecer? (0 para nenhum)");
            int dinheiroOferecido = 0;
            int.TryParse(Console.ReadLine(), out dinheiroOferecido);

            // Dinheiro a pedir
            Console.WriteLine($"Quanto dinheiro você quer pedir de {alvo.Name}? (0 para nenhum)");
            int dinheiroPedido = 0;
            int.TryParse(Console.ReadLine(), out dinheiroPedido);

            var offer = new TradeOffer(currentPlayer, alvo)
            {
                PropertiesOffered = propsOferecidas,
                PropertiesRequested = propsPedidas,
                MoneyOffered = dinheiroOferecido,
                MoneyRequested = dinheiroPedido
            };
            tradeManager.ProposeTrade(offer);
        }
    }
}
