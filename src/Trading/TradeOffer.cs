using System;
using System.Collections.Generic;

using Monopoly.Core;

namespace Monopoly.Trading
{
    public class TradeOffer
    {
        public Player From { get; set; }
        public Player To { get; set; }
        public List<Property> PropertiesOffered { get; set; } = new List<Property>();
        public List<Property> PropertiesRequested { get; set; } = new List<Property>();
        public int MoneyOffered { get; set; }
        public int MoneyRequested { get; set; }
        public bool GetOutOfJailOffered { get; set; }
        public bool GetOutOfJailRequested { get; set; }
        public string Description { get; set; }

        public TradeOffer(Player from, Player to)
        {
            From = from;
            To = to;
        }
    }
}
