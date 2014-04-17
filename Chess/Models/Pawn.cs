using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Pawn : ChessPiece
    {
        private static readonly IReadOnlyCollection<BitArray> BLACK_MOVEMENT_RAYS = InitializeBlackMovementRays();
        private static readonly IReadOnlyCollection<BitArray> WHITE_MOVEMENT_RAYS = InitializeWhiteMovementRays();
        private static readonly IReadOnlyCollection<BitArray> BLACK_ATTACK_RAYS = InitializeBlackAttackRays();
        private static readonly IReadOnlyCollection<BitArray> WHITE_ATTACK_RAYS = InitializeWhiteAttackRays();

        #region Ray Initialization
        private static IReadOnlyCollection<BitArray> InitializeBlackMovementRays()
        {
            var blackMovementRays = new List<BitArray>(Chessboard.NumLocations);

            for (int i = 0; i < Chessboard.NumLocations; i++)
            {
                var currRow = i / Chessboard.Dimension;
                var currCol = i % Chessboard.Dimension;
                var blackMovementLocations = new List<Tuple<int, int>>();

                blackMovementLocations.Add(new Tuple<int, int>(currRow + 1, currCol));

                //home row can move two places
                if (currRow == 1)
                {
                    blackMovementLocations.Add(new Tuple<int, int>(currRow + 2, currCol));
                }

                blackMovementRays.Add(GenerateRay(blackMovementLocations.ToArray()));
            }
            return blackMovementRays;
        }
        private static IReadOnlyCollection<BitArray> InitializeWhiteMovementRays()
        {
            var whiteMovementRays = new List<BitArray>(Chessboard.NumLocations);

            for (int i = 0; i < Chessboard.NumLocations; i++)
            {
                var currRow = i / Chessboard.Dimension;
                var currCol = i % Chessboard.Dimension;
                var whiteMovementLocations = new List<Tuple<int, int>>();


                whiteMovementLocations.Add(new Tuple<int, int>(currRow - 1, currCol));

                //home row can move two places
                if (currRow == 6)
                {
                    whiteMovementLocations.Add(new Tuple<int, int>(currRow - 2, currCol));
                }

                whiteMovementRays.Add(GenerateRay(whiteMovementLocations.ToArray()));
            }
            return whiteMovementRays;
        }
        private static IReadOnlyCollection<BitArray> InitializeBlackAttackRays()
        {
            var blackAttackRays = new List<BitArray>(Chessboard.NumLocations);

            for (int i = 0; i < Chessboard.NumLocations; i++)
            {
                var currRow = i / Chessboard.Dimension;
                var currCol = i % Chessboard.Dimension;
                var blackAttackLocations = new List<Tuple<int, int>>();

                blackAttackLocations.Add(new Tuple<int, int>(currRow + 1, currCol - 1));
                blackAttackLocations.Add(new Tuple<int, int>(currRow + 1, currCol + 1));

                blackAttackRays.Add(GenerateRay(blackAttackLocations.ToArray()));
            }
            return blackAttackRays;
        }
        private static IReadOnlyCollection<BitArray> InitializeWhiteAttackRays()
        {
            var whiteAttackRays = new List<BitArray>(Chessboard.NumLocations);

            for (int i = 0; i < Chessboard.NumLocations; i++)
            {
                var currRow = i / Chessboard.Dimension;
                var currCol = i % Chessboard.Dimension;
                var whiteAttackLocations = new List<Tuple<int, int>>();

                whiteAttackLocations.Add(new Tuple<int, int>(currRow - 1, currCol - 1));
                whiteAttackLocations.Add(new Tuple<int, int>(currRow - 1, currCol + 1));

                whiteAttackRays.Add(GenerateRay(whiteAttackLocations.ToArray()));
            }
            return whiteAttackRays;
        }

        #endregion

        public Pawn(ChessColour color)
            : base(color)
        {

        }

        public override string ToString()
        {
            return "Pawn";
        }

        public override BitArray GetCorrectedRay(int location, BitArray whiteLocations, BitArray blackLocations)
        {
            BitArray myColourLocations = this.Colour == ChessColour.Black ? blackLocations : whiteLocations;
            BitArray otherColourLocations = this.Colour != ChessColour.Black ? blackLocations : whiteLocations;
            BitArray unblockedLocations = GetBlockedLocations(location, whiteLocations, blackLocations).Not();

            BitArray movementRay = GetMovementRay(location);
            movementRay.And(unblockedLocations); //can't jump 2 if I'm blocked
            movementRay.And(myColourLocations.Not()); //can't move where my pieces are
            movementRay.And(otherColourLocations.Not()); //can't move where opposing peices are

            BitArray attackRay = GetAttackRay(location);
            attackRay.And(myColourLocations); //can't attack my own pieces (this was NOT-ed in the previous section)
            attackRay.And(otherColourLocations.Not()); //can attack other pieces (have to undo the NOT from before)

            return movementRay.Or(attackRay);
        }

        public override BitArray GetRay(int location)
        {
            return GetAttackRay(location).And(GetMovementRay(location));
        }

        private BitArray GetMovementRay(int location)
        {
            switch (this.Colour)
            {
                case ChessColour.Black:
                    return new BitArray(BLACK_MOVEMENT_RAYS.ElementAt(location));
                case ChessColour.White:
                    return new BitArray(WHITE_MOVEMENT_RAYS.ElementAt(location));
                default:
                    return new BitArray(64, false);
            }
        }

        private BitArray GetAttackRay(int location)
        {
            switch (this.Colour)
            {
                case ChessColour.Black:
                    return new BitArray(BLACK_ATTACK_RAYS.ElementAt(location));
                case ChessColour.White:
                    return new BitArray(WHITE_ATTACK_RAYS.ElementAt(location));
                default:
                    return new BitArray(64, false);
            }
        }
    }
}
