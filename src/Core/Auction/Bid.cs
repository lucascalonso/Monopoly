namespace Monopoly.Core.Auction
{
    public class Bid
    {
        public Player Player { get; }
        public int Value { get; }

        public Bid(Player player, int value)
        {
            Player = player;
            Value = value;
        }
    }
}
