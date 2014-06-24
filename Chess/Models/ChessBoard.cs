using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Chessboard
    {
        public static readonly int Dimension = 8;
        public static readonly int NumLocations = Dimension * Dimension;

        private BitArray _whiteLocations = new BitArray(Chessboard.NumLocations, false);
        private BitArray _blackLocations = new BitArray(Chessboard.NumLocations, false);

        private readonly ObservableCollection<BoardLocation> _locations;
        public ObservableCollection<BoardLocation> Locations
        {
            get { return this._locations; }
        }

        public BitArray WhiteLocations
        {
            get { return new BitArray(this._whiteLocations); }
            private set { this._whiteLocations = new BitArray(value); }
        }

        public BitArray BlackLocations
        {
            get { return new BitArray(this._blackLocations); }
            private set { this._blackLocations = new BitArray(value); }
        }

        public Chessboard()
        {
            _locations = CreateChessBoard();
            CreateChessPieces();
            UpdatePieceLocations();
        }

        /// <summary>
        /// Private copy constructor. Used for copying.
        /// </summary>
        /// <param name="board"></param>
        public Chessboard(Chessboard board)
        {
            _locations = CreateChessBoard();
            for (int i = 0; i < board.Locations.Count; i++)
            {
                Locations[i].Piece = board.Locations[i].Piece;
            }
            UpdatePieceLocations();
        }

        public void MovePiece(BoardLocation from, BoardLocation to)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            this.MovePiece(from, Locations.IndexOf(from), to, Locations.IndexOf(to));
        }

        public void MovePiece(int fromIndex, int toIndex)
        {
            this.MovePiece(Locations[fromIndex], fromIndex, Locations[toIndex], toIndex);
        }

        public bool IsPlayerInCheck(ChessColour playerToCheck)
        {
            int kingIndex = FindKingIndex(playerToCheck);
            BitArray otherPlayerAttackVectors = GetAttackVectors(playerToCheck == ChessColour.Black ? ChessColour.White : ChessColour.Black);
            return otherPlayerAttackVectors[kingIndex];
        }

        public bool IsPlayerInCheckmate(ChessColour player)
        {
            if (IsPlayerInCheck(player))
            {
                return !GetAllPossibleMoves(player).HasTrue();
            }
            else
            {
                return false;
            }
        }

        public bool IsPlayerInStalemate(ChessColour player)
        {
            if (!IsPlayerInCheck(player))
            {
                return !GetAllPossibleMoves(player).HasTrue();
            }
            else
            {
                // if the player is in check he can't possibly be in a stalemate
                return false;
            }
        }

        public static int GetIndex(int row, int column)
        {
            return row * Chessboard.Dimension + column;
        }

        public static bool IsValidBoardLocation(int row, int column)
        {
            return row >= 0 && row < Chessboard.Dimension
                && column >= 0 && column < Chessboard.Dimension;
        }

        public BitArray GetRay(BoardLocation location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            var piece = location.Piece;
            if (piece == null)
            {
                throw new ArgumentException("Expected location to have a piece but it doesn't");
            }

            var index = this.Locations.IndexOf(location);
            var ray = piece.GetCorrectedRay(index, this.WhiteLocations, this.BlackLocations);

            return this.GetCheckPreventionRay(ray, index, piece.Colour);
        }

        /// <summary>
        /// Gets all of the board location objects with a piece of a specified colour
        /// </summary>
        /// <param name="pieceColour">The piece color to filter by</param>
        /// <returns></returns>
        public IEnumerable<BoardLocation> GetLocations(ChessColour pieceColour)
        {
            // TODO method needs to be renamed, I think

            return from location in Locations
                   where location.HasPiece && location.PieceColour == pieceColour
                   select location;

        }


        /// <summary>
        /// Updates WhiteLocations and BlackLocations to accurately reflect the locations of the black and white pieces
        /// </summary>
        private void UpdatePieceLocations()
        {
            var whiteLocations = new BitArray(Chessboard.NumLocations, false);
            var blackLocations = new BitArray(Chessboard.NumLocations, false);
            for (int i = 0; i < Chessboard.NumLocations; i++)
            {
                if (this.Locations[i].HasPiece)
                {
                    if (this.Locations[i].PieceColour == ChessColour.Black)
                    {
                        blackLocations[i] = true;
                    }
                    else
                    {
                        whiteLocations[i] = true;
                    }
                }
            }

            this.WhiteLocations = whiteLocations;
            this.BlackLocations = blackLocations;
        }

        

        private void MovePiece(BoardLocation from, int fromIndex, BoardLocation to, int toIndex)
        {

            if (fromIndex < 0 || fromIndex >= Chessboard.NumLocations)
            {
                throw new ArgumentOutOfRangeException("fromIndex");
            }
            if (toIndex < 0 || toIndex >= Chessboard.NumLocations)
            {
                throw new ArgumentOutOfRangeException("toIndex");
            }

            var oldBlackLocations = this.BlackLocations;
            var oldWhiteLocations = this.WhiteLocations;

            if (from.PieceColour == ChessColour.Black)
            {
                oldBlackLocations[fromIndex] = false;
                oldBlackLocations[toIndex] = true;

                // if a piece of the other player is being taken
                if (to.HasPiece)
                {
                    oldWhiteLocations[toIndex] = false;
                }
            }
            else
            {
                oldWhiteLocations[fromIndex] = false;
                oldWhiteLocations[toIndex] = true;

                if (to.HasPiece)
                {
                    oldBlackLocations[toIndex] = false;
                }
            }


            BlackLocations = oldBlackLocations;
            WhiteLocations = oldWhiteLocations;

            to.Piece = from.Piece;
            from.Piece = null;

        }



        private void CreateChessPieces()
        {
            //create rooks
            this.ChangeChessPiece(0, 0, new Rook(ChessColour.Black));
            this.ChangeChessPiece(0, 7, new Rook(ChessColour.Black));
            this.ChangeChessPiece(7, 0, new Rook(ChessColour.White));
            this.ChangeChessPiece(7, 7, new Rook(ChessColour.White));

            //create knights
            this.ChangeChessPiece(0, 1, new Knight(ChessColour.Black));
            this.ChangeChessPiece(0, 6, new Knight(ChessColour.Black));
            this.ChangeChessPiece(7, 1, new Knight(ChessColour.White));
            this.ChangeChessPiece(7, 6, new Knight(ChessColour.White));

            //create bishops
            this.ChangeChessPiece(0, 2, new Bishop(ChessColour.Black));
            this.ChangeChessPiece(0, 5, new Bishop(ChessColour.Black));
            this.ChangeChessPiece(7, 2, new Bishop(ChessColour.White));
            this.ChangeChessPiece(7, 5, new Bishop(ChessColour.White));

            //create queens
            this.ChangeChessPiece(0, 3, new Queen(ChessColour.Black));
            this.ChangeChessPiece(7, 3, new Queen(ChessColour.White));

            //create kings
            this.ChangeChessPiece(0, 4, new King(ChessColour.Black));
            this.ChangeChessPiece(7, 4, new King(ChessColour.White));


            //create pawns
            for (int i = 0; i < 8; i++)
            {
                this.ChangeChessPiece(1, i, new Pawn(ChessColour.Black));
                this.ChangeChessPiece(6, i, new Pawn(ChessColour.White));
            }
        }

        private void ChangeChessPiece(int row, int column, ChessPiece chessPiece)
        {
            this.Locations[row * 8 + column].Piece = chessPiece;
        }

        private static ObservableCollection<BoardLocation> CreateChessBoard()
        {
            var result = new List<BoardLocation>(Chessboard.NumLocations);
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension / 2; j++)
                {
                    if (i % 2 == 0)
                    {
                        result.Add(new BoardLocation(ChessColour.White));
                        result.Add(new BoardLocation(ChessColour.Black));
                    }
                    else
                    {
                        result.Add(new BoardLocation(ChessColour.Black));
                        result.Add(new BoardLocation(ChessColour.White));
                    }
                }
            }
            return new ObservableCollection<BoardLocation>(result);
        }



        private int FindKingIndex(ChessColour chessColour)
        {
            for (int i = 0; i < this.Locations.Count; i++)
            {
                var piece = this.Locations[i].Piece;
                if (piece is King && piece.Colour == chessColour)
                {
                    return i;
                }
            }

            throw new System.InvalidOperationException("No king was found. A king should always be present.");
        }

        private BitArray GetAttackVectors(ChessColour chessColour)
        {
            BitArray result = new BitArray(Chessboard.NumLocations, false);
            for (int i = 0; i < this.Locations.Count; i++)
            {
                var piece = this.Locations[i].Piece;
                if (piece != null && piece.Colour == chessColour)
                {
                    result.Or(piece.GetCorrectedRay(i, WhiteLocations, BlackLocations));
                }
            }

            return result;
        }



        /// <summary>
        /// Given a ray of potential moves (assuming the player isn't in check), this method will generate a ray
        /// a new ray which eliminates any of the potential moves which result in check.
        /// </summary>
        /// <param name="potentialMoves">The potential moves of a piece</param>
        /// <param name="index">The location of the piece</param>
        /// <param name="pieceColour">The colour of the piece</param>
        /// <returns>A bit array of moves which will NOT result in check for <paramref name="pieceColour"/></returns>
        private BitArray GetCheckPreventionRay(BitArray potentialMoves, int index, ChessColour pieceColour)
        {
            var ray = new BitArray(potentialMoves.Count, false);

            for (int i = 0; i < potentialMoves.Count; i++)
            {
                var bit = potentialMoves[i];
                if (bit)
                {
                    var board = new Chessboard(this);
                    board.MovePiece(index, i);
                    if (!board.IsPlayerInCheck(pieceColour))
                    {
                        ray[i] = true;
                    }
                }
            }

            return ray;

        }

        private BitArray GetAllPossibleMoves(ChessColour colour)
        {
            var possibleMoves = new BitArray(Chessboard.NumLocations, false);
            foreach (var location in Locations)
            {
                if (location.HasPiece && location.PieceColour == colour)
                {
                    possibleMoves.Or(GetRay(location));
                }
            }
            return possibleMoves;
        }

        


    }
}
