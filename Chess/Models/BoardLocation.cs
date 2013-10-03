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


    class BoardLocation: ObservableObject
    {
        private BoardLocationColour _colour;
        public BoardLocationColour Colour
        {
            get
            {
                return _colour;
            }
            private set
            {
                this._colour = value;
                base.RaisePropertyChanged(() => this.Colour);
            }
        }

        public BoardLocation(BoardLocationColour color)
        {
            this.Colour = color;
        }
    }
}
