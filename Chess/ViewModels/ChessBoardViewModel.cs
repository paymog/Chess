using Chess.Models;
using MVVMToolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private BoardLocation _selectedBoardLocation = null;
        public BoardLocation SelectedBoardLocation
        {
            get
            {
                return _selectedBoardLocation;
            }
            set
            {
                _selectedBoardLocation = value;
                base.RaisePropertyChanged(() => this.SelectedBoardLocation);
            }
        }

        public static readonly RoutedCommand SelectLocationCommand = new RoutedCommand();
        public static readonly RoutedCommand MovePieceHereCommand = new RoutedCommand();

        public ChessBoardViewModel()
        {
            _locations = CreateChessBoard();
            CreateChessPieces();

            base.RegisterCommand(SelectLocationCommand, this.CanSelectLocation, param => this.SelectLocation(param as BoardLocation));
            base.RegisterCommand(MovePieceHereCommand, this.CanMovePieceHere, param => this.MovePieceHere(param as BoardLocation));
        }

        #region Commands
       
        private bool CanMovePieceHere(object obj)
        {
            return this.SelectedBoardLocation != null;
        }

        private void MovePieceHere(BoardLocation location)
        {
            SelectedBoardLocation.IsSelected = false;
            location.IsSelected = false;
            location.Piece = SelectedBoardLocation.Piece;
            SelectedBoardLocation.Piece = null;


            this.SelectedBoardLocation = null;

        }

        private bool CanSelectLocation(object obj)
        {
            return true;
        }

        private void SelectLocation(BoardLocation location)
        {
            this.SelectedBoardLocation = location;
        }
        #endregion

        #region Initialization

        private void CreateChessPieces()
        {
            //create rooks
            this.ChangeChessPiece(0, 0, new Rook(ChessPieceColour.Black));
            this.ChangeChessPiece(0, 7, new Rook(ChessPieceColour.Black));
            this.ChangeChessPiece(7, 0, new Rook(ChessPieceColour.White));
            this.ChangeChessPiece(7, 7, new Rook(ChessPieceColour.White));

            //create knights
            this.ChangeChessPiece(0, 1, new Knight(ChessPieceColour.Black));
            this.ChangeChessPiece(0, 6, new Knight(ChessPieceColour.Black));
            this.ChangeChessPiece(7, 1, new Knight(ChessPieceColour.White));
            this.ChangeChessPiece(7, 6, new Knight(ChessPieceColour.White));

            //create bishops
            this.ChangeChessPiece(0, 2, new Bishop(ChessPieceColour.Black));
            this.ChangeChessPiece(0, 5, new Bishop(ChessPieceColour.Black));
            this.ChangeChessPiece(7, 2, new Bishop(ChessPieceColour.White));
            this.ChangeChessPiece(7, 5, new Bishop(ChessPieceColour.White));

            //create queens
            this.ChangeChessPiece(0, 3, new Queen( ChessPieceColour.Black));
            this.ChangeChessPiece(7, 3, new Queen( ChessPieceColour.White));

            //create kings
            this.ChangeChessPiece(0, 4, new King(ChessPieceColour.Black));
            this.ChangeChessPiece(7, 4, new King(ChessPieceColour.White));


            //create pawns
            for (int i = 0; i < 8; i++)
            {
                this.ChangeChessPiece(1, i, new Pawn(ChessPieceColour.Black));
                this.ChangeChessPiece(6, i, new Pawn( ChessPieceColour.White));
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
        #endregion
    }
}
