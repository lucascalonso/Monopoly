using System.Collections.Generic;
using Monopoly.Squares;
using Monopoly;

namespace Monopoly.Core
{
    public class Board
    {
    public List<Square> Squares { get; set; }

        public Board()
        {
            Squares = new List<Square>();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Marrom
            Squares.Add(new GoSquare("Go", 0, 200));
            Squares.Add(new Property("Mediterranean Avenue", 1, 60) { ColorGroup = "Marrom" });
            Squares.Add(new CommunityChestSquare("Community Chest", 2));
            Squares.Add(new Property("Baltic Avenue", 3, 60) { ColorGroup = "Marrom" });
            Squares.Add(new TaxSquare("Income Tax", 4, 200));

            // Ferrovias e Azul Claro
            Squares.Add(new Railroad("Reading Railroad", 5, 200));
            Squares.Add(new Property("Oriental Avenue", 6, 100) { ColorGroup = "Azul Claro" });
            Squares.Add(new ChanceSquare("Chance", 7));
            Squares.Add(new Property("Vermont Avenue", 8, 100) { ColorGroup = "Azul Claro" });
            Squares.Add(new Property("Connecticut Avenue", 9, 120) { ColorGroup = "Azul Claro" });

            // Prisão
            Squares.Add(new Square("Jail", 10));

            // Rosa
            Squares.Add(new Property("St. Charles Place", 11, 140) { ColorGroup = "Rosa" });
            Squares.Add(new Company("Electric Company", 12, 150));
            Squares.Add(new Property("States Avenue", 13, 140) { ColorGroup = "Rosa" });
            Squares.Add(new Property("Virginia Avenue", 14, 160) { ColorGroup = "Rosa" });

            // Ferrovia
            Squares.Add(new Railroad("Pennsylvania Railroad", 15, 200));

            // Laranja
            Squares.Add(new Property("St. James Place", 16, 180) { ColorGroup = "Laranja" });
            Squares.Add(new CommunityChestSquare("Community Chest", 17));
            Squares.Add(new Property("Tennessee Avenue", 18, 180) { ColorGroup = "Laranja" });
            Squares.Add(new Property("New York Avenue", 19, 200) { ColorGroup = "Laranja" });

            // Estacionamento Livre
            Squares.Add(new FreeParkingSquare("Free Parking", 20));

            // Vermelho
            Squares.Add(new Property("Kentucky Avenue", 21, 220) { ColorGroup = "Vermelho" });
            Squares.Add(new ChanceSquare("Chance", 22));
            Squares.Add(new Property("Indiana Avenue", 23, 220) { ColorGroup = "Vermelho" });
            Squares.Add(new Property("Illinois Avenue", 24, 240) { ColorGroup = "Vermelho" });

            // Ferrovia
            Squares.Add(new Railroad("B&O Railroad", 25, 200));

            // Amarelo
            Squares.Add(new Property("Atlantic Avenue", 26, 260) { ColorGroup = "Amarelo" });
            Squares.Add(new Property("Ventnor Avenue", 27, 260) { ColorGroup = "Amarelo" });
            Squares.Add(new Company("Water Works", 28, 150));
            Squares.Add(new Property("Marvin Gardens", 29, 280) { ColorGroup = "Amarelo" });

            // Vá para a prisão
            Squares.Add(new GoToJailSquare("Go To Jail", 30, 10));

            // Verde
            Squares.Add(new Property("Pacific Avenue", 31, 300) { ColorGroup = "Verde" });
            Squares.Add(new Property("North Carolina Avenue", 32, 300) { ColorGroup = "Verde" });
            Squares.Add(new CommunityChestSquare("Community Chest", 33));
            Squares.Add(new Property("Pennsylvania Avenue", 34, 320) { ColorGroup = "Verde" });

            // Ferrovia
            Squares.Add(new Railroad("Short Line", 35, 200));

            // Azul Escuro
            Squares.Add(new ChanceSquare("Chance", 36));
            Squares.Add(new Property("Park Place", 37, 350) { ColorGroup = "Azul Escuro" });
            Squares.Add(new TaxSquare("Luxury Tax", 38, 100));
            Squares.Add(new Property("Boardwalk", 39, 400) { ColorGroup = "Azul Escuro" });
        }

        public Square GetSquare(int position)
        {
            return Squares[position % Squares.Count];
        }
    }
}