using Chess.Models;
using MVVMToolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ViewModels
{
    class ChessBoardViewModel : BaseViewModel
    {
        private const int NUM_ROWS = 8;
        public int NumRows
        {
            get { return NUM_ROWS; }
        }

        private const int NUM_COLUMNS = 8;
        public int NumColumns
        {
            get { return NUM_COLUMNS; }
        }

        private readonly ObservableCollection<BoardLocation> _locations;
        public ObservableCollection<BoardLocation> Locations
        {
            get
            {
                return _locations;
            }
        }

        public ChessBoardViewModel()
        {
            _locations = CreateChessBoard();
            CreateChessPieces();
        }

        private void CreateChessPieces()
        {
            //create rooks
            this.ChangeChessPiece(0, 0, new ChessPiece(ChessPieceType.Rook, ChessPieceColour.Black));
            this.ChangeChessPiece(0, 7, new ChessPiece(ChessPieceType.Rook, ChessPieceColour.Black));
            this.ChangeChessPiece(7, 0, new ChessPiece(ChessPieceType.Rook, ChessPieceColour.White));
            this.ChangeChessPiece(7, 7, new ChessPiece(ChessPieceType.Rook, ChessPieceColour.White));

            //create knights
            this.ChangeChessPiece(0, 1, new ChessPiece(ChessPieceType.Knight, ChessPieceColour.Black));
            this.ChangeChessPiece(0, 6, new ChessPiece(ChessPieceType.Knight, ChessPieceColour.Black));
            this.ChangeChessPiece(7, 1, new ChessPiece(ChessPieceType.Knight, ChessPieceColour.White));
            this.ChangeChessPiece(7, 6, new ChessPiece(ChessPieceType.Knight, ChessPieceColour.White));

            //create bishops
            this.ChangeChessPiece(0, 2, new ChessPiece(ChessPieceType.Bishop, ChessPieceColour.Black));
            this.ChangeChessPiece(0, 5, new ChessPiece(ChessPieceType.Bishop, ChessPieceColour.Black));
            this.ChangeChessPiece(7, 2, new ChessPiece(ChessPieceType.Bishop, ChessPieceColour.White));
            this.ChangeChessPiece(7, 5, new ChessPiece(ChessPieceType.Bishop, ChessPieceColour.White));

            //create queens
            this.ChangeChessPiece(0, 3, new ChessPiece(ChessPieceType.Queen, ChessPieceColour.Black));
            this.ChangeChessPiece(7, 3, new ChessPiece(ChessPieceType.Queen, ChessPieceColour.White));

            //create kings
            this.ChangeChessPiece(0, 4, new ChessPiece(ChessPieceType.King, ChessPieceColour.Black));
            this.ChangeChessPiece(7, 4, new ChessPiece(ChessPieceType.King, ChessPieceColour.White));


            //create pawns
            for (int i = 0; i < 8; i++)
            {
                this.ChangeChessPiece(1, i, new ChessPiece(ChessPieceType.Pawn, ChessPieceColour.Black));
                this.ChangeChessPiece(6, i, new ChessPiece(ChessPieceType.Pawn, ChessPieceColour.White));
            }
        }

        private void ChangeChessPiece(int row, int column, ChessPiece chessPiece)
        {
            this.Locations[row * 8 + column].Piece = chessPiece;
        }

        private ObservableCollection<BoardLocation> CreateChessBoard()
        {
            var result = new ObservableCollection<BoardLocation>();
            for (int i = 0; i < NumRows; i++)
            {
                for (int j = 0; j < NumColumns / 2; j++)
                {
                    if (i % 2 == 0)
                    {
                        result.Add(new BoardLocation(BoardLocationColour.Light));
                        result.Add(new BoardLocation(BoardLocationColour.Dark));
                    }
                    else
                    {
                       result.Add(new BoardLocation(BoardLocationColour.Dark));
                       result.Add(new BoardLocation(BoardLocationColour.Light));
                    }
                }
            }
            return result;
        }

    }
}
