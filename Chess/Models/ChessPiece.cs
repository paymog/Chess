using System;
using System.Collections;
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
        //abstract stuff
        public abstract BitArray GetRay(int location);

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

        private static bool isValidBoardLocation(int row, int column)
        {
            return row >= 0 && row < ChessBoard.NUM_ROWS
                && column >= 0 && column < ChessBoard.NUM_COLUMNS;
        }

        protected static BitArray generateRay(params Tuple<int, int>[] potentialLocations)
        {
            var result = new BitArray(ChessBoard.NUM_LOCATIONS, false);
            foreach(Tuple<int, int> location in potentialLocations)
            {
                if(isValidBoardLocation(location.Item1, location.Item2))
                {
                    result[location.Item1*ChessBoard.NUM_COLUMNS + location.Item2] = true;
                }
            }

            return result;
        }


    }
}
