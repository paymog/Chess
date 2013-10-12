using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    /// <summary>
    /// Board location, chess piece and player colours
    /// </summary>
    public enum ChessColour
    {
        Black,
        White
    }

    internal abstract class ChessPiece
    {
        private readonly ChessColour _colour;
        public ChessColour Colour { get { return this._colour; } }

        public ChessPiece(ChessColour colour)
        {
            this._colour = colour;
        }

        public override string ToString()
        {
            return this.Colour.ToString();
        }
    }
}
