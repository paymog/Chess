using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    class Rook : ChessPiece
    {
        private static readonly IReadOnlyCollection<BitArray> RAYS;

        static Rook()
        {
            var rays = new List<BitArray>(ChessPiece.NUM_BOARD_LOCATIONS);
            for (int i = 0; i < ChessPiece.NUM_BOARD_LOCATIONS; i++)
            {
                int currRow = i / 8;
                int currCol = i % 8;
                var locations = new List<int>();

                //vertical moves
                for (int j = 1; j < ChessBoard.NUM_ROWS; j++)
                {
                    //moves above
                    var newRow = currRow - j;
                    locations.Add(newRow * ChessBoard.NUM_COLUMNS + currCol);

                    //moves below
                    newRow = currRow + j;
                    locations.Add(newRow * ChessBoard.NUM_COLUMNS + currCol);
                }

                //horizontal moves
                for (int j = 1; j < ChessBoard.NUM_COLUMNS; j++)
                {
                    //moves to left
                    var newCol = currCol - j;
                    locations.Add(currRow * ChessBoard.NUM_COLUMNS + newCol);

                    //moves to right
                    newCol = currCol + j;
                    locations.Add(currRow * ChessBoard.NUM_COLUMNS + newCol);
                }

                rays.Add(ChessPiece.generateRay(locations.ToArray()));
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
    }
}
