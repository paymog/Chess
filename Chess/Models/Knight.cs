using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Knight: ChessPiece
    {
        private static readonly IReadOnlyCollection<BitArray> RAYS;

        static Knight()
        {
            var rays = new List<BitArray>(ChessBoard.NUM_LOCATIONS);
            for (int i = 0; i < ChessBoard.NUM_LOCATIONS; i++)
            {
                var currRow = i / ChessBoard.NUM_COLUMNS;
                var currCol = i % ChessBoard.NUM_COLUMNS;

                var upLeft = new Tuple<int, int>(currRow - 2, currCol - 1);
                var upRight = new Tuple<int, int>(currRow - 2, currCol + 1);

                var leftUp = new Tuple<int, int>(currRow - 1, currCol - 2);
                var leftDown = new Tuple<int, int>(currRow + 1, currCol - 2);

                var rightUp = new Tuple<int, int>(currRow - 1, currCol + 2);
                var rightDown = new Tuple<int, int>(currRow + 1, currCol + 2);

                var downLeft = new Tuple<int, int>(currRow + 2, currCol - 1);
                var downRight = new Tuple<int, int>(currRow + 2, currCol + 1);

                rays.Add(ChessPiece.generateRay(upLeft, upRight, leftUp, leftDown, rightUp, rightDown, downLeft, downRight));
            }
            RAYS = rays;
        }

        public Knight(ChessColour colour): base(colour)
        {

        }

        public override string ToString()
        {
            return "Knight";
        }

        public override BitArray GetRay(int location)
        {
            return RAYS.ElementAt(location);
        }
    }
}
