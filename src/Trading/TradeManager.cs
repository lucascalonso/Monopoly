using System;
using System.Collections.Generic;

using Monopoly.Core;

namespace Monopoly.Trading
{
    public class TradeManager
    {
        private Game _game;
        public TradeManager(Game game)
        {
            _game = game;
        }

        public void ProposeTrade(TradeOffer offer)
        {
            // Exibe a proposta de forma clara
            Console.WriteLine($"\n--- Proposta de Troca ---");
            Console.WriteLine($"De: {offer.From.Name}  Para: {offer.To.Name}");
            if (offer.PropertiesOffered.Count > 0)
                Console.WriteLine($"Propriedades oferecidas: {string.Join(", ", offer.PropertiesOffered.ConvertAll(p => p.Name))}");
            if (offer.PropertiesRequested.Count > 0)
                Console.WriteLine($"Propriedades pedidas: {string.Join(", ", offer.PropertiesRequested.ConvertAll(p => p.Name))}");
            if (offer.MoneyOffered > 0)
                Console.WriteLine($"Dinheiro oferecido: {offer.MoneyOffered}");
            if (offer.MoneyRequested > 0)
                Console.WriteLine($"Dinheiro pedido: {offer.MoneyRequested}");
            if (offer.GetOutOfJailOffered)
                Console.WriteLine($"Carta 'Get Out of Jail' oferecida");
            if (offer.GetOutOfJailRequested)
                Console.WriteLine($"Carta 'Get Out of Jail' pedida");

            // Pergunta ao jogador alvo
            Console.WriteLine($"{offer.To.Name}, você aceita a proposta? (s/n/c para contraproposta)");
            string resposta = Console.ReadLine()?.Trim().ToLower();

            if (resposta == "s")
            {
                // Validação de saldo no momento da aceitação
                if (offer.MoneyRequested > 0 && offer.To.Money < offer.MoneyRequested)
                {
                    Console.WriteLine($"{offer.To.Name} não tem dinheiro suficiente para aceitar a proposta.");
                    return;
                }
                if (offer.MoneyOffered > 0 && offer.From.Money < offer.MoneyOffered)
                {
                    Console.WriteLine($"{offer.From.Name} não tem dinheiro suficiente para oferecer a proposta.");
                    return;
                }
                ExecuteTrade(offer);
                Console.WriteLine("Troca realizada!");
            }
            else if (resposta == "c")
            {
                Console.WriteLine($"{offer.To.Name}, defina sua contraproposta.");
                // Listar propriedades do jogador alvo
                if (offer.To.Properties.Count > 0)
                {
                    Console.WriteLine("Propriedades disponíveis para oferecer:");
                    for (int i = 0; i < offer.To.Properties.Count; i++)
                        Console.WriteLine($"{i}: {offer.To.Properties[i].Name}");
                    Console.WriteLine("Digite o número da propriedade para oferecer (ou ENTER para nenhuma):");
                    string propInput = Console.ReadLine();
                    List<Property> propsOferecidas = new List<Property>();
                    if (int.TryParse(propInput, out int idx) && idx >= 0 && idx < offer.To.Properties.Count)
                        propsOferecidas.Add(offer.To.Properties[idx]);

                    Console.WriteLine("Digite o valor em dinheiro a oferecer (ou 0):");
                    int dinheiroOferecido = 0;
                    int.TryParse(Console.ReadLine(), out dinheiroOferecido);

                    Console.WriteLine("Digite o valor em dinheiro a pedir (ou 0):");
                    int dinheiroPedido = 0;
                    int.TryParse(Console.ReadLine(), out dinheiroPedido);

                    var contra = new TradeOffer(offer.To, offer.From)
                    {
                        PropertiesOffered = propsOferecidas,
                        PropertiesRequested = offer.PropertiesOffered,
                        MoneyOffered = dinheiroOferecido,
                        MoneyRequested = dinheiroPedido
                    };
                    ProposeTrade(contra);
                }
                else
                {
                    Console.WriteLine("Você não possui propriedades para oferecer. Contraproposta cancelada.");
                }
            }
            else
            {
                Console.WriteLine("Troca recusada.");
            }
        }

        public void ExecuteTrade(TradeOffer offer)
        {
            foreach (var prop in offer.PropertiesOffered)
            {
                offer.From.Properties.Remove(prop);
                offer.To.Properties.Add(prop);
                prop.Owner = offer.To;
            }
            foreach (var prop in offer.PropertiesRequested)
            {
                offer.To.Properties.Remove(prop);
                offer.From.Properties.Add(prop);
                prop.Owner = offer.From;
            }
            if (offer.MoneyOffered > 0)
            {
                offer.From.Money -= offer.MoneyOffered;
                offer.To.Money += offer.MoneyOffered;
            }
            if (offer.MoneyRequested > 0)
            {
                offer.To.Money -= offer.MoneyRequested;
                offer.From.Money += offer.MoneyRequested;
            }
            // Implementar lógica para cartas "Get Out of Jail" se necessário
        }
    }
}
