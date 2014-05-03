using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    

    class ChessMove
    {
        public int FromIndex { get; set; }
        public int ToIndex { get; set; }
        public ChessPiece PieceMoved { get; set; }
        public ChessPiece PieceTaken { get; set; }


    }
}
