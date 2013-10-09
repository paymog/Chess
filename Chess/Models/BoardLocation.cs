using MVVMToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public enum BoardLocationColour
    {
        Light,
        Dark
    }


    internal class BoardLocation: ObservableObject
    {
        private readonly BoardLocationColour _colour;
        private bool _isSelected = false;
        private ChessPiece _piece = null;

        
        public BoardLocationColour Colour
        {
            get
            {
                return _colour;
            }
        }

        
        public bool IsSelected 
        { 
            get 
            { 
                return _isSelected; 
            } 
            set
            {
                _isSelected = value;
                base.RaisePropertyChanged(() => this.IsSelected);
            }
        }

        public ChessPiece Piece
        {
            get { return _piece; }
            set
            {
                _piece = value;
                base.RaisePropertyChanged(() => this.Piece);
            }
        }

        public BoardLocation(BoardLocationColour color, ChessPiece piece = null)
        {
            this._colour = color;
            this.Piece = piece;
        }
    }
}
