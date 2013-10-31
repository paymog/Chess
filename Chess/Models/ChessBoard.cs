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
        public static readonly int NUM_ROWS = 8;
        public static readonly int NUM_COLUMNS = 8;
        public static readonly int NUM_LOCATIONS = NUM_ROWS * NUM_COLUMNS;


        private readonly ObservableCollection<BoardLocation> _locations;
        public ObservableCollection<BoardLocation> Locations
        {
            get { return this._locations; }
        }

        public ChessBoard()
        {
            _locations = CreateChessBoard();
            CreateChessPieces();
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
            for (int i = 0; i < NUM_ROWS; i++)
            {
                for (int j = 0; j < NUM_COLUMNS / 2; j++)
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
    }
}
