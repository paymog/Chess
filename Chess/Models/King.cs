using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    class King: ChessPiece
    {
        private static readonly IReadOnlyCollection<BitArray> RAYS;
     
        static King()
        {
            var rays = new List<BitArray>(ChessPiece.NUM_BOARD_LOCATIONS);
            for (int i = 0; i < ChessPiece.NUM_BOARD_LOCATIONS; i++)
            {
                var topLeft = i - 8 - 1;
                var top = i - 8;
                var topRight = i - 8 + 1;
                var left = i - 1;
                var right = i + 1;
                var bottomLeft = i + 8 - 1;
                var bottom = i + 8;
                var bottomRight = i + 8 + 1;

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
    }
}
