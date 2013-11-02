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
    public class ChessBoardViewModel : BaseViewModel
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

        public ChessBoardViewModel()
        {
            base.RegisterCommand(SelectLocationCommand, param => this.CanSelectLocation(param as BoardLocation), param => this.SelectLocation(param as BoardLocation));
        }

        #region Commands

        private bool CanSelectLocation(BoardLocation location)
        {
            if (location.HasPiece && location.PieceColour() == this.CurrentPlayerColour)
            {
                return true;
            }
            else if(this.SelectedBoardLocation != null && !location.HasPiece)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void SelectLocation(BoardLocation location)
        {
            UntargetAllLocations();
            if (this.SelectedBoardLocation == null)
            {
                this.SelectedBoardLocation = location;
            }
            else
            {
                if(location == this.SelectedBoardLocation)
                {
                    this.SelectedBoardLocation = null;
                }
                else if(location.HasPiece)
                {
                    this.SelectedBoardLocation.IsSelected = false;
                    this.SelectedBoardLocation = location;
                }
                else
                {
                    this.MovePiece(this.SelectedBoardLocation, location);
                    this.ToggleCurrentPlayerColour();
                    this.SelectedBoardLocation.IsSelected = false;
                    location.IsSelected = false;
                    this.SelectedBoardLocation = null;
                }
            }

            TargetLocations(this.SelectedBoardLocation);
        }

        

        private void TargetLocations(BoardLocation location)
        {
            if (location == null)
            {
                return;
            }

            ChessPiece piece = location.Piece;
            var ray = piece.GetRay(this.Locations.IndexOf(location));
            for (int i = 0; i < ray.Count; i++)
            {
                if (ray[i])
                {
                    this.Locations[i].IsTargeted = true;
                }
            }
        }

        private void UntargetAllLocations()
        {
            foreach (BoardLocation location in this.Locations)
            {
                location.IsTargeted = false;
            }
        }
        #endregion

        private void ToggleCurrentPlayerColour()
        {
            if(this.CurrentPlayerColour == ChessColour.White)
            {
                this.CurrentPlayerColour = ChessColour.Black;
            }
            else
            {
                this.CurrentPlayerColour = ChessColour.White;
            }
        }

        private void MovePiece(BoardLocation from, BoardLocation to)
        {
            to.Piece = from.Piece;
            from.Piece = null;
        }
    }
}
