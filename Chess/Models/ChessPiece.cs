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
        private readonly ChessColour _colour;
        public ChessColour Colour { get { return this._colour; } }

        public static readonly int NUM_BOARD_LOCATIONS = 64;

        public ChessPiece(ChessColour colour)
        {
            this._colour = colour;
        }

        public override string ToString()
        {
            return this.Colour.ToString();
        }

        private static bool isValidBoardLocation(int location)
        {
            return location >= 0 && location < NUM_BOARD_LOCATIONS;
        }

        protected static BitArray generateRay(params int[] potentialLocations)
        {
            var result = new BitArray(NUM_BOARD_LOCATIONS, false);
            foreach(int location in potentialLocations)
            {
                if(isValidBoardLocation(location))
                {
                    result[location] = true;
                }
            }

            return result;
        }


    }
}
