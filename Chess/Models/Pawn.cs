﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Pawn: ChessPiece
    {
         private static readonly IReadOnlyCollection<BitArray> BLACK_RAYS;
         private static readonly IReadOnlyCollection<BitArray> WHITE_RAYS;

        static Pawn()
        {
            var blackRays = new List<BitArray>(ChessBoard.NUM_LOCATIONS);
            var whiteRays = new List<BitArray>(ChessBoard.NUM_LOCATIONS);

            for (int i = 0; i < ChessBoard.NUM_LOCATIONS; i++)
            {
                var currRow = i / ChessBoard.NUM_COLUMNS;
                var currCol = i % ChessBoard.NUM_COLUMNS;
                var blackLocations = new List<Tuple<int, int>>();
                var whiteLocations = new List<Tuple<int, int>>();

                blackLocations.Add(new Tuple<int, int>(currRow + 1, currCol));
                blackLocations.Add(new Tuple<int, int>(currRow + 1, currCol - 1));
                blackLocations.Add(new Tuple<int, int>(currRow + 1, currCol + 1));

                whiteLocations.Add(new Tuple<int, int>(currRow - 1, currCol));
                whiteLocations.Add(new Tuple<int, int>(currRow - 1, currCol - 1));
                whiteLocations.Add(new Tuple<int, int>(currRow - 1, currCol + 1));

                //home row can move two places
                if(currRow == 1)
                {
                    blackLocations.Add(new Tuple<int, int>(currRow + 2, currCol));
                }
                if(currRow == 6)
                {
                    whiteLocations.Add(new Tuple<int, int>(currRow - 2, currCol));
                }

                blackRays.Add(generateRay(blackLocations.ToArray()));
                whiteRays.Add(generateRay(whiteLocations.ToArray()));
            }
            BLACK_RAYS = blackRays;
            WHITE_RAYS = whiteRays;
        }
        public Pawn(ChessColour color): base(color)
        {
            
        }

        public override string ToString()
        {
            return "Pawn";
        }

        public override System.Collections.BitArray GetRay(int location)
        {
            switch(this.Colour)
            {
                case ChessColour.Black:
                    return BLACK_RAYS.ElementAt(location);
                case ChessColour.White:
                    return WHITE_RAYS.ElementAt(location);
                default:
                    return new BitArray(64, false);
            }
        }
    }
}
