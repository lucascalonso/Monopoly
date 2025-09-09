using System;

namespace Monopoly.Core
{
    public class Dice
    {
        private Random _random = new Random();

        public int Roll()
        {
            return _random.Next(1, 7);
        }
        public (int, int) RollTwo()
        {
            int roll1 = _random.Next(1, 7);
            int roll2 = _random.Next(1, 7);
            return (roll1, roll2);
        }

        public bool IsDouble(int roll1, int roll2)
        {
            return roll1 == roll2;
        }
    }
}
