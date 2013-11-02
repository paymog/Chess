using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class King: ChessPiece
    {
        private static readonly IReadOnlyCollection<BitArray> RAYS;
     
        static King()
        {
            var rays = new List<BitArray>(ChessBoard.NUM_LOCATIONS);
            for (int i = 0; i < ChessBoard.NUM_LOCATIONS; i++)
            {
                var currRow = i / ChessBoard.NUM_COLUMNS;
                var currCol = i % ChessBoard.NUM_COLUMNS;
                var topLeft = new Tuple<int, int>(currRow - 1, currCol - 1);
                var top = new Tuple<int, int>(currRow - 1, currCol);
                var topRight = new Tuple<int, int>(currRow - 1, currCol + 1);
                var left = new Tuple<int, int>(currRow, currCol - 1);
                var right = new Tuple<int, int>(currRow, currCol + 1);
                var bottomLeft = new Tuple<int, int>(currRow + 1, currCol - 1);
                var bottom = new Tuple<int, int>(currRow + 1, currCol);
                var bottomRight = new Tuple<int, int>(currRow + 1, currCol + 1);

                rays.Add(ChessPiece.generateRay(topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight));
            }
            RAYS = rays;
        }

        public King(ChessColour colour): base(colour)
        {

        }

        public override string ToString()
        {
            return "King";
        }

        public override BitArray GetCorrectedRay(int location, BitArray whiteLocations, BitArray blackLocations)
        {
            return RAYS.ElementAt(location);
        }

        public override BitArray GetRay(int location)
        {
            return new BitArray(RAYS.ElementAt(location));
        }
    }
}
