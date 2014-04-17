using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Bishop: ChessPiece
    {
        private static readonly IReadOnlyCollection<BitArray> RAYS = InitializeRays();

        private static IReadOnlyCollection<BitArray> InitializeRays()
        {
            var rays = new List<BitArray>(Chessboard.NumLocations);
            for (int i = 0; i < Chessboard.NumLocations; i++)
            {
                var currRow = i / Chessboard.Dimension;
                var currCol = i % Chessboard.Dimension;
                var locations = new List<Tuple<int, int>>();

                for (int j = 1; j < Math.Max(Chessboard.Dimension, Chessboard.Dimension); j++)
                {
                    locations.Add(new Tuple<int, int>(currRow + j, currCol + j));
                    locations.Add(new Tuple<int, int>(currRow - j, currCol + j));
                    locations.Add(new Tuple<int, int>(currRow + j, currCol - j));
                    locations.Add(new Tuple<int, int>(currRow - j, currCol - j));
                }


                rays.Add(ChessPiece.GenerateRay(locations.ToArray()));
            }
            return rays;
        }

        public Bishop(ChessColour colour): base(colour)
        {

        }

        public override string ToString()
        {
            return "Bishop";
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
