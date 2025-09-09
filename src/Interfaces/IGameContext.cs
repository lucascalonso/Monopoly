using System.Collections.Generic;

namespace Monopoly.Interfaces
{
    public interface IGameContext
    {
    List<Monopoly.Core.Player> GetPlayers();
    Monopoly.Core.Board GetBoard();
    void MovePlayer(Monopoly.Core.Player player, int newPosition);
    void AddMoney(Monopoly.Core.Player player, int amount);
    void RemoveMoney(Monopoly.Core.Player player, int amount);
    void SendPlayerToJail(Monopoly.Core.Player player);
    void TransferProperty(Monopoly.Core.Player from, Monopoly.Core.Player to, Monopoly.Core.Property property);
    void MortgageProperty(Monopoly.Core.Player player, Monopoly.Core.Property property);
    void UnmortgageProperty(Monopoly.Core.Player player, Monopoly.Core.Property property);
    void RemovePlayer(Monopoly.Core.Player player);
    void AddProperty(Monopoly.Core.Player player, Monopoly.Core.Property property);
    void RemoveProperty(Monopoly.Core.Player player, Monopoly.Core.Property property);
    Monopoly.Core.Property GetPropertyByName(string name);
    Monopoly.Core.Player GetCurrentPlayer();
    Monopoly.Cards.Deck GetDeck(string type);
    void BroadcastMessage(string message);
    bool CanPlayerPay(Monopoly.Core.Player player, int amount);
    Monopoly.Core.Player GetPlayerByName(string name);

    }
}