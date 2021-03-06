﻿using System;
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

    public abstract class ChessPiece
    {
        //abstract stuff
        public abstract BitArray GetRay(int location);
        public abstract BitArray GetCorrectedRay(int location, BitArray whiteLocations, BitArray blackLocations);

        private readonly ChessColour _colour;

        public ChessColour Colour { get { return this._colour; } }

        protected ChessPiece(ChessColour colour)
        {
            this._colour = colour;
        }

        public override string ToString()
        {
            return this.Colour.ToString();
        }

        protected static BitArray GenerateRay(params Tuple<int, int>[] potentialLocations)
        {
            var result = new BitArray(Chessboard.NumLocations, false);
            foreach (Tuple<int, int> location in potentialLocations)
            {
                if (Chessboard.IsValidBoardLocation(location.Item1, location.Item2))
                {
                    result[Chessboard.GetIndex(location.Item1, location.Item2)] = true;
                }
            }

            return result;
        }



        /// <summary>
        /// Not proud of this function (it's ugly, has poorly named variables, and is horrible for maintenence). Finds all blocked locations. It's hard to define being blocked so I'll give an example. Suppose you're at
        /// you're at (3,4) (row, column). If there's a piece at (5,4) then (6,4) and (7,4) are blocked. This function
        /// finds all blocked locations found on the column, row and diagonals of a given location.
        /// 
        /// Put another way this function finds all locations which are unreachable by any piece which moves in a straight line.
        /// If there is a piece immediately to the left of a queen then all locations to the left of the piece are blocked and thus
        /// unreachable by the queen. Similar arguments apply to diagonals and verticals.
        /// </summary>
        /// <param name="location">Based on the description above, the location of the queen</param>
        /// <param name="whiteLocations">A bitarray representing the locations of the white piece</param>
        /// <param name="blackLocations">A bitarray representing the locations of the black pieces</param>
        /// <returns>A bitarray where a 1 means that index is blocked and 0 means that index is not blocked</returns>
        protected BitArray GetBlockedLocations(int location, BitArray whiteLocations, BitArray blackLocations)
        {
            var result = new BitArray(Chessboard.NumLocations, false);
            var pieceLocations = new BitArray(whiteLocations).Or(blackLocations);

            var row = location / Chessboard.Dimension;
            var column = location % Chessboard.Dimension;

            //I can't think of a good name or a succinct description for this variable. Sorry
            var t = new List<Tuple<Func<int, int, int>, Func<int, int, int>>>();
            t.Add(new Tuple<Func<int, int, int>, Func<int, int, int>>((r, i) => r - i, (c, i) => c - i)); //aboveleft
            t.Add(new Tuple<Func<int, int, int>, Func<int, int, int>>((r, i) => r - i, (c, i) => c)); //above
            t.Add(new Tuple<Func<int, int, int>, Func<int, int, int>>((r, i) => r - i, (c, i) => c + i)); //aboveright
            t.Add(new Tuple<Func<int, int, int>, Func<int, int, int>>((r, i) => r, (c, i) => c + i)); //right
            t.Add(new Tuple<Func<int, int, int>, Func<int, int, int>>((r, i) => r + i, (c, i) => c + i)); //belowright
            t.Add(new Tuple<Func<int, int, int>, Func<int, int, int>>((r, i) => r + i, (c, i) => c)); //below
            t.Add(new Tuple<Func<int, int, int>, Func<int, int, int>>((r, i) => r + i, (c, i) => c - i)); //below left
            t.Add(new Tuple<Func<int, int, int>, Func<int, int, int>>((r, i) => r, (c, i) => c - i)); //left

            foreach (var tuple in t)
            {
                var rowFunc = tuple.Item1;
                var columnFunc = tuple.Item2;
                var foundBlockingPiece = false;
                for (int i = 1; i < Chessboard.Dimension; i++)
                {
                    var newRow = rowFunc(row, i);
                    var newCol = columnFunc(column, i);
                    var index = Chessboard.GetIndex(newRow, newCol);
                    if (Chessboard.IsValidBoardLocation(newRow, newCol))
                    {
                        if (foundBlockingPiece)
                        {
                            result[index] = true;
                        }
                        else
                        {
                            foundBlockingPiece = pieceLocations[index];
                        }
                    }
                }
            }

            return result;
        }
    }
}
