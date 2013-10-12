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

        private ChessColour _currentPlayerColour = ChessColour.White;
        public ChessColour CurrentPlayerColour
        {
            get
            {
                return this._currentPlayerColour;
            }
            set
            {
                this._currentPlayerColour = value;
                base.RaisePropertyChanged(() => this.CurrentPlayerColour);
            }
        }

        public static readonly RoutedCommand SelectLocationCommand = new RoutedCommand();
        public static readonly RoutedCommand MovePieceHereCommand = new RoutedCommand();

        public ChessBoardViewModel()
        {
            _locations = CreateChessBoard();
            CreateChessPieces();

            base.RegisterCommand(SelectLocationCommand,param => this.CanSelectLocation(param as BoardLocation), param => this.SelectLocation(param as BoardLocation));
            base.RegisterCommand(MovePieceHereCommand, param => this.CanMovePieceHere(param as BoardLocation), param => this.MovePieceHere(param as BoardLocation));
        }

        #region Commands
       
        private bool CanMovePieceHere(BoardLocation obj)
        {
            return this.SelectedBoardLocation != null;
        }

        private void MovePieceHere(BoardLocation location)
        {
            if (this.CurrentPlayerColour == ChessColour.Black)
            {
                this.CurrentPlayerColour = ChessColour.White;
            }
            else
            {
                this.CurrentPlayerColour = ChessColour.Black;
            }
            SelectedBoardLocation.IsSelected = false;
            location.IsSelected = false;

            location.Piece = SelectedBoardLocation.Piece;
            SelectedBoardLocation.Piece = null;


            this.SelectedBoardLocation = null;

        }

        private bool CanSelectLocation(BoardLocation obj)
        {
            return obj.PieceColour() == this.CurrentPlayerColour;
        }

        private void SelectLocation(BoardLocation location)
        {
            if (this.SelectedBoardLocation != null)
            {
                this.SelectedBoardLocation.IsSelected = false;
            }
            this.SelectedBoardLocation = location;
        }
        #endregion

        #region Initialization

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
            this.ChangeChessPiece(0, 3, new Queen( ChessColour.Black));
            this.ChangeChessPiece(7, 3, new Queen( ChessColour.White));

            //create kings
            this.ChangeChessPiece(0, 4, new King(ChessColour.Black));
            this.ChangeChessPiece(7, 4, new King(ChessColour.White));


            //create pawns
            for (int i = 0; i < 8; i++)
            {
                this.ChangeChessPiece(1, i, new Pawn(ChessColour.Black));
                this.ChangeChessPiece(6, i, new Pawn( ChessColour.White));
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
        #endregion

    }
}
