using System;
using System.Collections.Generic;

namespace Monopoly.Core
{
    public class Player
    {
        private string _name;
        private int _position;
        private int _money;
        private List<Property> _properties;
        private bool _isInJail = false;
        private int _jailTurns = 0;
        private List<Monopoly.Cards.Card> _heldCards = new List<Monopoly.Cards.Card>();

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public int Position
        {
            get => _position;
            set => _position = value;
        }
        public int Money
        {
            get => _money;
            set => _money = value;
        }
        public List<Property> Properties
        {
            get => _properties;
            set => _properties = value;
        }
        public bool IsInJail
        {
            get => _isInJail;
            set => _isInJail = value;
        }
        public int JailTurns
        {
            get => _jailTurns;
            set => _jailTurns = value;
        }


    public List<Monopoly.Cards.Card> HeldCards => _heldCards;

        public Player(string name, int initialMoney)
        {
            _name = name;
            _position = 0;
            _money = initialMoney;
            _properties = new List<Property>();
        }

        public bool DecideToBuy(Property prop)
        {
            Console.WriteLine($"Comprar {prop.Name} por {prop.Price}? (s/n)");
            string input = Console.ReadLine()?.Trim().ToLower();
            return input == "s";
        }
    }
}
