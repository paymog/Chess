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
        private static readonly IReadOnlyCollection<BitArray> RAYS = InitializeRays();

        private static IReadOnlyCollection<BitArray> InitializeRays()
        {
            var rays = new List<BitArray>(Chessboard.NumLocations);
            for (int i = 0; i < Chessboard.NumLocations; i++)
            {
                var currRow = i / Chessboard.Dimension;
                var currCol = i % Chessboard.Dimension;

                var upLeft = new Tuple<int, int>(currRow - 2, currCol - 1);
                var upRight = new Tuple<int, int>(currRow - 2, currCol + 1);

                var leftUp = new Tuple<int, int>(currRow - 1, currCol - 2);
                var leftDown = new Tuple<int, int>(currRow + 1, currCol - 2);

                var rightUp = new Tuple<int, int>(currRow - 1, currCol + 2);
                var rightDown = new Tuple<int, int>(currRow + 1, currCol + 2);

                var downLeft = new Tuple<int, int>(currRow + 2, currCol - 1);
                var downRight = new Tuple<int, int>(currRow + 2, currCol + 1);

                rays.Add(ChessPiece.GenerateRay(upLeft, upRight, leftUp, leftDown, rightUp, rightDown, downLeft, downRight));
            }
            return rays;
        }

        public Knight(ChessColour colour): base(colour)
        {

        }

        public override string ToString()
        {
            return "Knight";
        }

        public override BitArray GetCorrectedRay(int location, BitArray whiteLocations, BitArray blackLocations)
        {
            BitArray myColourLocations = this.Colour == ChessColour.Black ? blackLocations : whiteLocations;

            return GetRay(location).And(myColourLocations.Not());
        }

        public override BitArray GetRay(int location)
        {
            return new BitArray(RAYS.ElementAt(location));
        }
    }
}
