using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public enum ChessPieceColour
    {
        Black,
        White
    }

    public enum ChessPieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    internal abstract class ChessPiece
    {
        private readonly ChessPieceColour _colour;
        public ChessPieceColour Colour { get { return this._colour; } }

        //private readonly ChessPieceType _type;
        //public ChessPieceType Type { get { return this._type; } }

        public ChessPiece(/*ChessPieceType type, */ChessPieceColour colour)
        {
            this._colour = colour;
            //this._type = type;
        }

        public override string ToString()
        {
            return this.Colour.ToString();// +" " + this.Type.ToString();
        }
    }
}
