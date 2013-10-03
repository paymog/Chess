using Chess.Models;
using MVVMToolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private ObservableCollection<BoardLocation> _locations;
        public ObservableCollection<BoardLocation> Locations
        {
            get
            {
                //Create the board the first time it is "gotten"
                if (_locations == null)
                {
                    _locations = new ObservableCollection<BoardLocation>();
                    for (int i = 0; i < NumRows; i++)
                    {
                        for (int j = 0; j < NumColumns / 2; j++)
                        {
                            if (i % 2 == 0)
                            {
                                _locations.Add(new BoardLocation(BoardLocationColour.Light));
                                _locations.Add(new BoardLocation(BoardLocationColour.Dark));
                            }
                            else
                            {
                                _locations.Add(new BoardLocation(BoardLocationColour.Dark));
                                _locations.Add(new BoardLocation(BoardLocationColour.Light));
                            }
                        }
                    }
                }
                return _locations;
            }
        }

        public ChessBoardViewModel()
        {

        }

    }
}
