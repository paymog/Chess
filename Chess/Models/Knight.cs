using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    class Knight: ChessPiece
    {
        private static readonly IReadOnlyCollection<BitArray> RAYS;

        static Knight()
        {
            var rays = new List<BitArray>(ChessPiece.NUM_BOARD_LOCATIONS);
            for (int i = 0; i < ChessPiece.NUM_BOARD_LOCATIONS; i++)
            {
                var upLeft = i - 16 - 1;
                var upRight = i - 16 + 1;

                var leftUp = i - 2 - 8;
                var leftDown = i - 2 + 8;

                var rightUp = i + 2 - 8;
                var rightDown = i + 2 + 8;

                var downLeft = i + 16 - 1;
                var downRight = i + 16 + 1;

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
    }
}
