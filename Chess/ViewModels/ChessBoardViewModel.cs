﻿using Chess.Models;
using MVVMToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Chess;
using Chess.AI;
using System.Diagnostics;

namespace Chess.ViewModels
{
    public class ChessboardViewModel : BaseViewModel
    {
        private readonly Chessboard board = new Chessboard();
        private readonly AIEngine engine = new AIEngine();

        private ChessColour _currentPlayerColour = ChessColour.White;
        private BoardLocation _selectedBoardLocation = null;
        public static readonly RoutedCommand SelectLocationCommand = new RoutedCommand();
        private bool _blackInCheck = false;
        private bool _whiteInCheck = false;
        private bool _checkmate = false;
        private bool _stalemate = false;
        private int _count = 0;


        #region Properties

        public static int RowCount
        {
            get { return Chessboard.Dimension; }
        }
        public static int ColumnCount
        {
            get { return Chessboard.Dimension; }
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

        public bool Checkmate
        {
            get { return _checkmate; }
            set
            {
                this._checkmate = value;
                base.RaisePropertyChanged(() => this.Checkmate);
            }
        }

        public bool Stalemate
        {
            get { return _stalemate; }
            set
            {
                this._stalemate = value;
                base.RaisePropertyChanged(() => this.Stalemate);
            }
        }

        public int BranchCount
        {
            get { return _count; }
            set
            {
                this._count = value;
                base.RaisePropertyChanged(() => this.BranchCount);
            }
        }

        #endregion

        public ChessboardViewModel()
        {
            base.RegisterCommand(SelectLocationCommand, param => this.CanSelectLocation(param as BoardLocation), param => this.SelectLocation(param as BoardLocation));
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
                    // select another location of the player
                    this.SelectedBoardLocation.IsSelected = false;
                    this.SelectedBoardLocation = location;
                }
                else
                {
                    // move piece. Can only be moved to a location that is targeted.
                    board.MakeMove(this.SelectedBoardLocation, location);
                    this.ToggleCurrentPlayerColour();
                    this.SelectedBoardLocation.IsSelected = false;
                    location.IsSelected = false;
                    this.SelectedBoardLocation = null;
                }
            }

            TargetLocations(this.SelectedBoardLocation);
            DetectCheck();
            DetectCheckmate();
            DetectStalemate();

            if(CurrentPlayerColour == ChessColour.Black)
            {
                var move = engine.FindBestAlphaBetaMove(board, CurrentPlayerColour);
                board.MakeMove(move.FromIndex, move.ToIndex);
                this.ToggleCurrentPlayerColour();
                DetectCheck();
                DetectCheckmate();
                DetectStalemate();
                this.BranchCount = engine.recursiveCount;
            }
        }
        #endregion

        private void DetectCheck()
        {
            this.BlackInCheck = board.IsPlayerInCheck(ChessColour.Black);
            this.WhiteInCheck = board.IsPlayerInCheck(ChessColour.White);
        }

        private void DetectCheckmate()
        {
            Checkmate = board.IsPlayerInCheckmate(CurrentPlayerColour);
        }

        private void DetectStalemate()
        {
            Stalemate = board.IsPlayerInStalemate(CurrentPlayerColour);
        }

        private void TargetLocations(BoardLocation location)
        {
            if (location == null)
            {
                return;
            }

            ChessPiece piece = location.Piece;
            var ray = board.GetRay(location);

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




    }
}
