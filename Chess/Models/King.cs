﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    class King: ChessPiece
    {
        public King(ChessPieceColour colour): base(colour)
        {

        }

        public override string ToString()
        {
            return base.ToString() + "King";
        }
    }
}