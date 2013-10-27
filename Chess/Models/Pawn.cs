using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    class Pawn: ChessPiece
    {
        public Pawn(ChessColour color): base(color)
        {
            
        }

        public override string ToString()
        {
            return "Pawn";
        }
    }
}
