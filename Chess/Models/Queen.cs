using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    class Queen: ChessPiece
    {
         private static readonly IReadOnlyCollection<BitArray> RAYS;

        static Queen()
        {
            var rays = new List<BitArray>(ChessBoard.NUM_LOCATIONS);
            var bishop = new Bishop(ChessColour.Black);
            var rook = new Rook(ChessColour.Black);
            for (int i = 0; i < ChessBoard.NUM_LOCATIONS; i++)
            {
                var tempRay = new BitArray(bishop.GetRay(i));
                var ray = tempRay.Or(rook.GetRay(i));
                rays.Add(ray);
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

        public override System.Collections.BitArray GetRay(int location)
        {
            return RAYS.ElementAt(location);
        }
    }
}
