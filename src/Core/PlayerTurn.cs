using System;
using System.Linq;

namespace Monopoly.Core
{
        public class PlayerTurn
{
    private Player _player;
    private Board _board;
    private Dice _dice;
    private Game _game;

    public PlayerTurn(Player player, Board board, Dice dice, Game game)
    {
        _player = player;
        _board = board;
        _dice = dice;
        _game = game;
    }

        public void Execute()
        {
            bool hasRolled = false;
            bool endTurn = false;
            AssetManager assetManager = new AssetManager(_player);
            while (!endTurn)
            {
                Console.WriteLine("\nEscolha uma ação:");
                if (_player.IsInJail && !hasRolled)
                {
                    Console.WriteLine("1 - Tentar sair (rolar dados)");
                    Console.WriteLine("2 - Pagar 50 para sair");
                    bool temCarta = _player.HeldCards.Any(c => c.Description.ToLower().Contains("livre da prisão") || c.Description.ToLower().Contains("get out of jail"));
                    if (temCarta)
                        Console.WriteLine("3 - Usar carta 'Saída Livre da Prisão'");
                    Console.WriteLine("4 - Negociar");
                    Console.WriteLine("5 - Hipotecar/Desipotecar");
                    Console.WriteLine("6 - Construir casas/hotéis");
                }
                else if (!hasRolled)
                {
                    Console.WriteLine("1 - Jogar dados");
                    Console.WriteLine("2 - Negociar");
                    Console.WriteLine("3 - Hipotecar/Desipotecar");
                    Console.WriteLine("4 - Construir casas/hotéis");
                }
                else
                {
                    Console.WriteLine("1 - Negociar");
                    Console.WriteLine("2 - Hipotecar/Desipotecar");
                    Console.WriteLine("3 - Construir casas/hotéis");
                    Console.WriteLine("4 - Passar a vez");
                }
                Console.Write("Opção: ");
                string opcao = Console.ReadLine()?.Trim();
                if (_player.IsInJail && !hasRolled)
                {
                    switch (opcao)
                    {
                        case "1": // Tentar sair (rolar dados)
                            bool saiu = HandleJailTurnStandard();
                            hasRolled = true;
                            if (!saiu && _player.JailTurns < 3)
                            {
                                // Não saiu, turno termina
                                endTurn = true;
                            }
                            break;
                        case "2": // Pagar 50 para sair
                            if (_player.Money >= 50)
                            {
                                _player.Money -= 50;
                                _player.IsInJail = false;
                                _player.JailTurns = 0;
                                Console.WriteLine("Você pagou 50 e saiu da prisão. Jogue os dados normalmente.");
                                HandleRegularTurn();
                                hasRolled = true;
                            }
                            else
                            {
                                Console.WriteLine("Dinheiro insuficiente!");
                            }
                            break;
                        case "3": // Usar carta
                            var carta = _player.HeldCards.FirstOrDefault(c => c.Description.ToLower().Contains("livre da prisão") || c.Description.ToLower().Contains("get out of jail"));
                            if (carta != null)
                            {
                                _player.HeldCards.Remove(carta);
                                // Devolver ao baralho correto
                                if (_game != null)
                                {
                                    if (carta.Description.ToLower().Contains("community") || carta.Description.ToLower().Contains("comunidade"))
                                        _game.CommunityChestDeck?.Shuffle(); // devolve ao deck, se necessário
                                    else
                                        _game.ChanceDeck?.Shuffle();
                                }
                                _player.IsInJail = false;
                                _player.JailTurns = 0;
                                Console.WriteLine("Você usou uma carta 'Saída Livre da Prisão'. Jogue os dados normalmente.");
                                HandleRegularTurn();
                                hasRolled = true;
                            }
                            else
                            {
                                Console.WriteLine("Você não possui carta 'Saída Livre da Prisão'.");
                            }
                            break;
                        case "4": // Negociar
                            Monopoly.Trading.TradeFlow.OferecerNegociacao(new Monopoly.Trading.TradeManager(_game), _player, _game.Players);
                            break;
                        case "5": // Hipotecar/Desipotecar
                            assetManager.ShowMortgageMenu();
                            break;
                        case "6": // Construir
                            assetManager.ShowUpgradeMenu(_board);
                            break;
                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }
                }
                else if (!hasRolled)
                {
                    switch (opcao)
                    {
                        case "1": // Jogar dados
                            HandleRegularTurn();
                            hasRolled = true;
                            break;
                        case "2": // Negociar
                            Monopoly.Trading.TradeFlow.OferecerNegociacao(new Monopoly.Trading.TradeManager(_game), _player, _game.Players);
                            break;
                        case "3": // Hipotecar/Desipotecar
                            assetManager.ShowMortgageMenu();
                            break;
                        case "4": // Construir
                            assetManager.ShowUpgradeMenu(_board);
                            break;
                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }
                }
                else
                {
                    switch (opcao)
                    {
                        case "1": // Negociar
                            Monopoly.Trading.TradeFlow.OferecerNegociacao(new Monopoly.Trading.TradeManager(_game), _player, _game.Players);
                            break;
                        case "2": // Hipotecar/Desipotecar
                            assetManager.ShowMortgageMenu();
                            break;
                        case "3": // Construir
                            assetManager.ShowUpgradeMenu(_board);
                            break;
                        case "4": // Passar a vez
                            endTurn = true;
                            break;
                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }
                }
            }
        }

    // Retorna true se saiu da prisão e andou, false se ficou preso
    private bool HandleJailTurnStandard()
        {
            var (roll1, roll2) = _dice.RollTwo();
            Console.WriteLine($"{_player.Name} está na prisão e rolou {roll1} e {roll2}.");
            if (_dice.IsDouble(roll1, roll2))
            {
                Console.WriteLine("Você tirou doubles e saiu da prisão!");
                _player.IsInJail = false;
                _player.JailTurns = 0;
                int totalRoll = roll1 + roll2;
                MoveAndHandleSquare(totalRoll);
                return true;
            }
            else
            {
                _player.JailTurns++;
                if (_player.JailTurns >= 3)
                {
                    Console.WriteLine("Você ficou 3 turnos na prisão e agora é obrigado a pagar 50 para sair.");
                    if (_player.Money >= 50)
                    {
                        _player.Money -= 50;
                    }
                    else
                    {
                        Console.WriteLine("Dinheiro insuficiente! Você sai devendo.");
                        _player.Money = 0;
                    }
                    _player.IsInJail = false;
                    _player.JailTurns = 0;
                    int totalRoll = roll1 + roll2;
                    MoveAndHandleSquare(totalRoll);
                    return true;
                }
                else
                {
                    Console.WriteLine($"Você não saiu da prisão. Turnos na prisão: {_player.JailTurns}/3");
                    return false;
                }
            }
        }

    private void HandleRegularTurn()
    {
        int doublesCount = 0;
        bool inJail = false;

        do
        {
            var (roll1, roll2) = _dice.RollTwo();
            int totalRoll = roll1 + roll2;
            Console.WriteLine($"{_player.Name} rolou {roll1} e {roll2} (total: {totalRoll})");

            if (_dice.IsDouble(roll1, roll2))
            {
                doublesCount++;
                Console.WriteLine($"{_player.Name} tirou doubles! Joga novamente.");
                if (doublesCount == 3)
                {
                    Console.WriteLine($"{_player.Name} tirou doubles três vezes seguidas e vai para a prisão!");
                    _player.Position = 10; // posição da prisão
                    _player.IsInJail = true;
                    _player.JailTurns = 0;
                    inJail = true;
                    break;
                }
            }
            else
            {
                doublesCount = 0;
            }

            MoveAndHandleSquare(totalRoll);

        } while (doublesCount > 0 && !inJail);
    }

    private void MoveAndHandleSquare(int totalRoll)
    {
        _player.Position = (_player.Position + totalRoll) % _board.Squares.Count;
        var square = _board.GetSquare(_player.Position);
        Console.WriteLine($"{_player.Name} caiu em {square.Name} ({square.GetType().Name})");

        if (square is Property prop && prop.Owner == null)
        {
            if (_player.DecideToBuy(prop))
            {
                prop.Buy(_player, _game.Players);
            }
            else
            {
                Console.WriteLine($"{_player.Name} decidiu não comprar {prop.Name}.");
                // Inicia leilão começando pelo próximo jogador
                int currentIndex = _game.Players.IndexOf(_player);
                Auction.AuctionFlow.StartAuction(prop, _game.Players, currentIndex);
            }
        }
        else
        {
            square.OnLand(_player, _game);
        }

        Console.WriteLine($"{_player.Name} está agora na posição {_player.Position} ({square.Name}) e tem {_player.Money} dinheiro.");

        AssetManager assetManager = new AssetManager(_player);
        var mortgagedProps = _player.Properties.FindAll(p => p.IsMortgaged);
        if (mortgagedProps.Count > 0)
        {
            Console.WriteLine("Deseja desipotecar alguma propriedade? (s/n)");
            string input = Console.ReadLine()?.Trim().ToLower();
            if (input == "s")
            {
                assetManager.UnmortgageProperty();
            }
        }
    }
}
}