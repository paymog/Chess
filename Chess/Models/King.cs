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
            var rays = new List<BitArray>(Chessboard.NumLocations);
            for (int i = 0; i < Chessboard.NumLocations; i++)
            {
                var currRow = i / Chessboard.Dimension;
                var currCol = i % Chessboard.Dimension;
                var topLeft = new Tuple<int, int>(currRow - 1, currCol - 1);
                var top = new Tuple<int, int>(currRow - 1, currCol);
                var topRight = new Tuple<int, int>(currRow - 1, currCol + 1);
                var left = new Tuple<int, int>(currRow, currCol - 1);
                var right = new Tuple<int, int>(currRow, currCol + 1);
                var bottomLeft = new Tuple<int, int>(currRow + 1, currCol - 1);
                var bottom = new Tuple<int, int>(currRow + 1, currCol);
                var bottomRight = new Tuple<int, int>(currRow + 1, currCol + 1);

                rays.Add(ChessPiece.GenerateRay(topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight));
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
            BitArray arrayToUse = this.Colour == ChessColour.Black ? blackLocations : whiteLocations;
            return this.GetRay(location).And(arrayToUse.Not());
        }

        public override BitArray GetRay(int location)
        {
            return new BitArray(RAYS.ElementAt(location));
        }
    }
}
