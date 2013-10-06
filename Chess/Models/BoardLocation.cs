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
        private bool _canBeSelected = true;
        private ChessPiece _piece;

        
        public BoardLocationColour Colour
        {
            get
            {
                return _colour;
            }
        }

        
        public bool CanBeSelected 
        { 
            get 
            { 
                return _canBeSelected; 
            } 
            set
            {
                _canBeSelected = value;
                base.RaisePropertyChanged(() => this.CanBeSelected);
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
