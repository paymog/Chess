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
        private BitArray _whiteLocations = new BitArray(ChessBoard.NUM_LOCATIONS, false);
        private BitArray _blackLocations = new BitArray(ChessBoard.NUM_LOCATIONS, false);
        public static readonly RoutedCommand SelectLocationCommand = new RoutedCommand();
        private bool _blackInCheck;
        private bool _whiteInCheck;

        public int NumRows
        {
            get { return ChessBoard.DIMENSION; }
        }
        public int NumColumns
        {
            get { return ChessBoard.DIMENSION; }
        }

        public ObservableCollection<BoardLocation> Locations
        {
            get
            {
                return board.Locations;
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

        public bool BlackInCheck
        {
            get
            {
                return _blackInCheck;
            }
            set
            {
                this._blackInCheck = value;
                base.RaisePropertyChanged(() => this.BlackInCheck);
            }
        }

        public bool WhiteInCheck
        {
            get
            {
                return _whiteInCheck;
            }
            set
            {
                this._whiteInCheck = value;
                base.RaisePropertyChanged(() => this.WhiteInCheck);
            }
        }

        private BitArray WhiteLocations
        {
            get { return new BitArray(this._whiteLocations); }
            set { this._whiteLocations = new BitArray(value); }
        }

        private BitArray BlackLocations
        {
            get { return new BitArray(this._blackLocations); }
            set { this._blackLocations = new BitArray(value); }
        }


        public ChessBoardViewModel()
        {
            base.RegisterCommand(SelectLocationCommand, param => this.CanSelectLocation(param as BoardLocation), param => this.SelectLocation(param as BoardLocation));

            GeneratePieceLocations();
        }

        private void GeneratePieceLocations()
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

        #region Commands

        private bool CanSelectLocation(BoardLocation location)
        {
            //any piece of the current player can be selected
            if (location.HasPiece && location.PieceColour == this.CurrentPlayerColour)
            {
                return true;
            }
            //if a piece is already selected, then any location without a piece can be selected
            else if (this.SelectedBoardLocation != null && location.IsTargeted)
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
                //nothing is currently selected
                this.SelectedBoardLocation = location;
            }
            else
            {
                if (location == this.SelectedBoardLocation)
                {
                    //deselect the current selection
                    this.SelectedBoardLocation = null;
                }
                else if (location.HasPiece && !location.IsTargeted && location.PieceColour == this.CurrentPlayerColour)
                {
                    this.SelectedBoardLocation.IsSelected = false;
                    this.SelectedBoardLocation = location;
                }
                else
                {
                    this.MovePiece(this.SelectedBoardLocation, location);
                    this.ToggleCurrentPlayerColour();
                    this.GeneratePieceLocations();
                    this.SelectedBoardLocation.IsSelected = false;
                    location.IsSelected = false;
                    this.SelectedBoardLocation = null;
                }
            }

            TargetLocations(this.SelectedBoardLocation);
            CheckForCheck();
        }

        private void CheckForCheck()
        {
            int blackKingIndex = FindKingLocation(ChessColour.Black);
            int whiteKingIndex = FindKingLocation(ChessColour.White);

            BitArray blackAttackVectors = GetAttackVectors(ChessColour.Black);
            BitArray whiteAttackVectors = GetAttackVectors(ChessColour.White);

            this.BlackInCheck = whiteAttackVectors[blackKingIndex];
            this.WhiteInCheck = blackAttackVectors[whiteKingIndex];
        }

        private BitArray GetAttackVectors(ChessColour chessColour)
        {
            BitArray result = new BitArray(ChessBoard.NUM_LOCATIONS, false);
            for(int i = 0; i < this.Locations.Count; i++)
            {
                var piece = this.Locations[i].Piece;
                if(piece != null && piece.Colour == chessColour)
                {
                    result.Or(piece.GetCorrectedRay(i, WhiteLocations, BlackLocations));
                }
            }

            return result;
        }

        private int FindKingLocation(ChessColour chessColour)
        {
            for(int i = 0; i < this.Locations.Count; i++)
            {
                var piece = this.Locations[i].Piece;
                 if(piece is King && piece.Colour == chessColour)
                 {
                     return i;
                 }
            }

            return -1;
        }



        private void TargetLocations(BoardLocation location)
        {
            if (location == null)
            {
                return;
            }

            ChessPiece piece = location.Piece;
            var ray = piece.GetCorrectedRay(this.Locations.IndexOf(location), this.WhiteLocations, this.BlackLocations);
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
            if (this.CurrentPlayerColour == ChessColour.White)
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
