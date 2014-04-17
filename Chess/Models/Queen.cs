using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Queen: ChessPiece
    {
         private static readonly IReadOnlyCollection<BitArray> RAYS;

        static Queen()
        {
            var rays = new List<BitArray>(Chessboard.NumLocations);
            var bishop = new Bishop(ChessColour.Black);
            var rook = new Rook(ChessColour.Black);
            for (int i = 0; i < Chessboard.NumLocations; i++)
            {
                rays.Add(bishop.GetRay(i).Or(rook.GetRay(i)));
            }
            RAYS = rays;
        }
        public Queen(ChessColour colour): base(colour)
        {

        }

        public override string ToString()
        {
            return "Queen";
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
