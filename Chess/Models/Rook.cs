using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Rook : ChessPiece
    {
        private static readonly IReadOnlyCollection<BitArray> RAYS;

        static Rook()
        {
            var rays = new List<BitArray>(Chessboard.NumLocations);
            for (int i = 0; i < Chessboard.NumLocations; i++)
            {
                int currRow = i / Chessboard.Dimension;
                int currCol = i % Chessboard.Dimension;
                var locations = new List<Tuple<int, int>>();

                //vertical moves
                for (int j = 1; j < Chessboard.Dimension; j++)
                {
                    //moves above
                    var newRow = currRow - j;
                    locations.Add(new Tuple<int, int>(newRow, currCol));

                    //moves below
                    newRow = currRow + j;
                    locations.Add(new Tuple<int, int>(newRow, currCol));
                }

                //horizontal moves
                for (int j = 1; j < Chessboard.Dimension; j++)
                {
                    //moves to left
                    var newCol = currCol - j;
                    locations.Add(new Tuple<int, int>(currRow, newCol));

                    //moves to right
                    newCol = currCol + j;
                    locations.Add(new Tuple<int, int>(currRow, newCol));
                }

                rays.Add(ChessPiece.GenerateRay(locations.ToArray()));
            }
            RAYS = rays;
        }
        public Rook(ChessColour colour)
            : base(colour)
        {

        }

        public override string ToString()
        {
            return "Rook";
        }

        public override BitArray GetCorrectedRay(int location, BitArray whiteLocations, BitArray blackLocations)
        {
            BitArray myColourLocations = this.Colour == ChessColour.Black ? blackLocations : whiteLocations;
            var unblockedLocations = GetBlockedLocations(location, whiteLocations, blackLocations).Not();
            return new BitArray(this.GetRay(location).And(myColourLocations.Not()).And(unblockedLocations));
        }

        public override BitArray GetRay(int location)
        {
            return new BitArray(RAYS.ElementAt(location));
        }
    }
}
