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


    class BoardLocation: INotifyPropertyChanged
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
                //this.OnPropertyChanged("Colour");
            }
        }

        public BoardLocation(BoardLocationColour color)
        {
            this.Colour = color;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
