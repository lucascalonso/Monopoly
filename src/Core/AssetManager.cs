using System;
using System.Collections.Generic;

namespace Monopoly.Core
{

    public class AssetManager
    {
        private Player _player;

        public AssetManager(Player player)
        {
            _player = player;
        }

        public void ShowMortgageMenu()
        {
            Console.WriteLine("Deseja hipotecar (h) ou desipotecar (d) uma propriedade? (h/d)");
            string input = Console.ReadLine()?.Trim().ToLower();
            if (input == "h")
            {
                MortgageProperty();
            }
            else if (input == "d")
            {
                UnmortgageProperty();
            }
        }

        public void ShowUpgradeMenu(Board board)
        {
            string input;
            var props = _player.Properties.FindAll(p => !p.IsMortgaged);
            if (props.Count == 0)
            {
                Console.WriteLine("Você não possui propriedades para upgrade.");
                return;
            }

            while (true)
            {
                for (int i = 0; i < props.Count; i++)
                {
                    var propr = props[i];
                    Console.WriteLine($"{i}: {propr.Name} | Casas: {propr.Houses} | Hotel: {(propr.HasHotel ? "Sim" : "Não")} | Grupo: {propr.ColorGroup}");
                }
                Console.WriteLine("Digite o número da propriedade para upgrade ou ENTER para cancelar:");
                input = Console.ReadLine();
                if (!int.TryParse(input, out int idx) || idx < 0 || idx >= props.Count)
                    break;

                var prop = props[idx];

                // Decide automaticamente o tipo de upgrade
                if (prop.Houses < 4 && !prop.HasHotel)
                {
                    prop.BuildHouse(_player, board);
                }
                else if (prop.Houses == 4 && !prop.HasHotel)
                {
                    prop.BuildHotel(_player, board);
                }
                else
                {
                    Console.WriteLine("Esta propriedade já possui um hotel. Não é possível fazer mais upgrades.");
                }

                Console.WriteLine("Deseja construir outro upgrade? (s/n)");
                string again = Console.ReadLine()?.Trim().ToLower();
                if (again != "s") break;
            }
        }

        public void ShowAssets()
        {
            Console.WriteLine($"{_player.Name} possui as seguintes propriedades:");
            for (int i = 0; i < _player.Properties.Count; i++)
            {
                var prop = _player.Properties[i];
                string status = prop.IsMortgaged ? "Hipotecada" : "Livre";
                Console.WriteLine($"{i}: {prop.Name} (Valor: {prop.Price}, {status})");
            }
        }

        public bool SellProperty()
        {
            ShowAssets();
            Console.WriteLine("Digite o número da propriedade que deseja vender ou ENTER para cancelar:");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int idx) && idx >= 0 && idx < _player.Properties.Count)
            {
                var prop = _player.Properties[idx];
                int saleValue = prop.Price / 2;
                _player.Money += saleValue;
                prop.Owner = null;
                _player.Properties.RemoveAt(idx);
                Console.WriteLine($"{_player.Name} vendeu {prop.Name} por {saleValue}.");
                return true;
            }
            Console.WriteLine("Venda cancelada.");
            return false;
        }

        public bool TryRaiseFunds(int requiredAmount)
        {
            while (_player.Money < requiredAmount && _player.Properties.Count > 0)
            {
                Console.WriteLine($"Você precisa de {requiredAmount}, mas só tem {_player.Money}.");
                Console.WriteLine("Deseja vender ou hipotecar propriedades? (v/h/n)");
                string choice = Console.ReadLine()?.Trim().ToLower();
                if (choice == "v")
                {
                    SellProperty();
                }
                else if (choice == "h")
                {
                    MortgageProperty();
                }
                else
                {
                    break;
                }
            }
            return _player.Money >= requiredAmount;
        }

        public bool MortgageProperty()
        {
            ShowAssets();
            Console.WriteLine("Digite o número da propriedade que deseja hipotecar ou ENTER para cancelar:");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int idx) && idx >= 0 && idx < _player.Properties.Count)
            {
                var prop = _player.Properties[idx];
                if (!prop.IsMortgaged)
                {
                    int mortgageValue = prop.Price / 2;
                    _player.Money += mortgageValue;
                    prop.IsMortgaged = true;
                    Console.WriteLine($"{_player.Name} hipotecou {prop.Name} por {mortgageValue}.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"{prop.Name} já está hipotecada.");
                }
            }
            Console.WriteLine("Hipoteca cancelada.");
            return false;
        }

        public bool UnmortgageProperty()
        {
            var mortgagedProps = _player.Properties.FindAll(p => p.IsMortgaged);
            if (mortgagedProps.Count == 0)
            {
                Console.WriteLine("Você não possui propriedades hipotecadas.");
                return false;
            }

            Console.WriteLine("Propriedades hipotecadas:");
            for (int i = 0; i < mortgagedProps.Count; i++)
            {
                Console.WriteLine($"{i}: {mortgagedProps[i].Name} (Valor para desipotecar: {mortgagedProps[i].Price / 2})");
            }
            Console.WriteLine("Digite o número da propriedade que deseja desipotecar ou ENTER para cancelar:");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int idx) && idx >= 0 && idx < mortgagedProps.Count)
            {
                var prop = mortgagedProps[idx];
                int unmortgageValue = prop.Price / 2;
                if (_player.Money >= unmortgageValue)
                {
                    _player.Money -= unmortgageValue;
                    prop.IsMortgaged = false;
                    Console.WriteLine($"{_player.Name} desipotecou {prop.Name} por {unmortgageValue}.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Você não tem dinheiro suficiente para desipotecar.");
                }
            }
            Console.WriteLine("Desipoteca cancelada.");
            return false;
        }

    public bool TryPay(int amount)
        {
            while (_player.Money < amount)
            {
                Console.WriteLine($"{_player.Name} não tem dinheiro suficiente para pagar {amount}. Dinheiro atual: {_player.Money}");
                Console.WriteLine("Você pode (v)ender ou (h)ipotecar propriedades.");
                string input = Console.ReadLine()?.Trim().ToLower();
                if (input == "v")
                {
                    SellProperty();
                }
                else if (input == "h")
                {
                    MortgageProperty();
                }
                else
                {
                    Console.WriteLine("Opção inválida.");
                }
                if (_player.Money < amount && _player.Properties.Count == 0)
                {
                    // Não exibe mensagem de falência aqui, pois será tratada por Game.RemovePlayer
                    return false;
                }
            }
            _player.Money -= amount;
            Console.WriteLine($"{_player.Name} pagou {amount}. Dinheiro restante: {_player.Money}");
            return true;
        }
    }
}