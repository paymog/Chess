using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    

    public class ChessMove
    {
        public int FromIndex { get; set; }
        public int ToIndex { get; set; }
        public ChessPiece PieceMoved { get; set; }
        public ChessPiece PieceTaken { get; set; }

        public ChessMove(int fromIndex, int toIndex, ChessPiece pieceMoved, ChessPiece pieceTaken = null)
        {
            this.FromIndex = fromIndex;
            this.ToIndex = toIndex;
            this.PieceMoved = pieceMoved;
            this.PieceTaken = pieceTaken;
        }

    }
}
