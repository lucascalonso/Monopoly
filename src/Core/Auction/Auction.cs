using System.Collections.Generic;

namespace Monopoly.Core.Auction
{
    public class Auction
    {
        public Property Property { get; }
        public List<Bid> Bids { get; }
        public List<Player> Participants { get; }
        public bool Finished { get; private set; }
        public Player Winner { get; private set; }
        public int FinalBid { get; private set; }

        public Auction(Property property, List<Player> participants)
        {
            Property = property;
            Participants = new List<Player>(participants);
            Bids = new List<Bid>();
            Finished = false;
        }

        public bool PlaceBid(Player player, int value)
        {
            if (Finished || !Participants.Contains(player))
                return false;
            int highestBid = Bids.Count > 0 ? Bids[^1].Value : 0;
            if (value > highestBid && player.Money >= value)
            {
                Bids.Add(new Bid(player, value));
                return true;
            }
            return false;
        }

        public void Finish()
        {
            if (Bids.Count > 0)
            {
                var lastBid = Bids[^1];
                Winner = lastBid.Player;
                FinalBid = lastBid.Value;
                Property.Owner = Winner;
                Winner.Money -= FinalBid;
                Winner.Properties.Add(Property);
            }
            Finished = true;
        }
    }
}
