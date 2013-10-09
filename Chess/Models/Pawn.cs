using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    class Pawn: ChessPiece
    {
        public Pawn(ChessPieceColour color): base(color)
        {
            
        }

        public override string ToString()
        {
            return base.ToString() + "Pawn";
        }
    }
}
