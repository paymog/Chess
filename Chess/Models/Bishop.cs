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
        private static readonly IReadOnlyCollection<BitArray> RAYS;

        static Bishop()
        {
            var rays = new List<BitArray>(ChessBoard.NUM_LOCATIONS);
            for (int i = 0; i < ChessBoard.NUM_LOCATIONS; i++)
            {
                var currRow = i / ChessBoard.NUM_COLUMNS;
                var currCol = i % ChessBoard.NUM_COLUMNS;
                var locations = new List<Tuple<int, int>>();

                for (int j = 1; j < Math.Max(ChessBoard.NUM_COLUMNS, ChessBoard.NUM_ROWS); j++ )
                {
                    locations.Add(new Tuple<int, int>(currRow + j, currCol + j));
                    locations.Add(new Tuple<int, int>(currRow - j, currCol + j));
                    locations.Add(new Tuple<int, int>(currRow + j, currCol - j));
                    locations.Add(new Tuple<int, int>(currRow - j, currCol - j));
                }


                rays.Add(ChessPiece.generateRay(locations.ToArray()));
            }
            RAYS = rays;
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
            return RAYS.ElementAt(location);
        }

        public override BitArray GetRay(int location)
        {
            return new BitArray(RAYS.ElementAt(location));
        }
    }
}
