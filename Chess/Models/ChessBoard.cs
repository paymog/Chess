using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class ChessBoard
    {
        public static readonly int DIMENSION = 8;
        public static readonly int NUM_LOCATIONS = DIMENSION * DIMENSION;

        private BitArray _whiteLocations = new BitArray(ChessBoard.NUM_LOCATIONS, false);
        private BitArray _blackLocations = new BitArray(ChessBoard.NUM_LOCATIONS, false);

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

        public ChessBoard()
        {
            _locations = CreateChessBoard();
            CreateChessPieces();
            GeneratePieceLocations();
        }

        public void GeneratePieceLocations()
        {
            for (int i = 0; i < ChessBoard.NUM_LOCATIONS; i++)
            {
                _blackLocations[i] = _whiteLocations[i] = false;
                if (this.Locations[i].HasPiece)
                {
                    if (this.Locations[i].PieceColour == ChessColour.Black)
                    {
                        _blackLocations[i] = true;
                    }
                    else
                    {
                        _whiteLocations[i] = true;
                    }
                }
            }
        }

        public void MovePiece(BoardLocation from, BoardLocation to)
        {
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

        private ObservableCollection<BoardLocation> CreateChessBoard()
        {
            var result = new ObservableCollection<BoardLocation>();
            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION / 2; j++)
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
            return result;
        }

        public bool IsPlayerInCheck(ChessColour playerToCheck)
        {
            int kingIndex = FindKingLocation(playerToCheck);
            BitArray otherPlayerAttackVectors = GetAttackVectors(playerToCheck == ChessColour.Black ? ChessColour.White : ChessColour.Black);
            return otherPlayerAttackVectors[kingIndex];
        }

        private int FindKingLocation(ChessColour chessColour)
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
            BitArray result = new BitArray(ChessBoard.NUM_LOCATIONS, false);
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
    }
}
