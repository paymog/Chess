using Chess.Models;
using MVVMToolkit;
using System;
using System.Collections;
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
        private readonly ChessBoard board = new ChessBoard();
        private ChessColour _currentPlayerColour = ChessColour.White;
        private BoardLocation _selectedBoardLocation = null;

        public int NumRows
        {
            get { return ChessBoard.NUM_ROWS; }
        }
        public int NumColumns
        {
            get { return ChessBoard.NUM_COLUMNS; }
        }

        
        public ObservableCollection<BoardLocation> Locations
        {
            get
            {
                return board.Locations;
            }
        }

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

            base.RegisterCommand(SelectLocationCommand,param => this.CanSelectLocation(param as BoardLocation), param => this.SelectLocation(param as BoardLocation));
            base.RegisterCommand(MovePieceHereCommand, param => this.CanMovePieceHere(param as BoardLocation), param => this.MovePieceHere(param as BoardLocation));
        }

        #region Commands
       
        private bool CanMovePieceHere(BoardLocation location)
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

        private bool CanSelectLocation(BoardLocation location)
        {
            return location.PieceColour() == this.CurrentPlayerColour;
        }

        private void SelectLocation(BoardLocation location)
        {
            if (this.SelectedBoardLocation != null)
            {
                this.SelectedBoardLocation.IsSelected = false;
            }
            this.SelectedBoardLocation = location;


            ChessPiece piece = location.Piece;
            BitArray ray = piece.GetRay(this.Locations.IndexOf(location));
            for(int i = 0; i < this.Locations.Count; i++)
            {
                if(ray[i])
                {
                    this.Locations[i].IsTargeted = true;
                }
            }
        }
        #endregion

    }
}
