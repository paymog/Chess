using Chess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AI
{
    class ChessboardEvaluator
    {
        public int Score { get; private set; }

        private static Dictionary<Type, int> PieceValues = InitializePieceValues();

        private static Dictionary<Type, int> InitializePieceValues()
        {
            var pieceValues = new Dictionary<Type, int>();

            pieceValues.Add(typeof(Pawn), 1);
            pieceValues.Add(typeof(Bishop), 5);
            pieceValues.Add(typeof(Knight), 5);
            pieceValues.Add(typeof(Rook), 5);
            pieceValues.Add(typeof(Queen), 9);
            pieceValues.Add(typeof(King), 1000);

            return pieceValues;
        }

        public int Evaluate(Chessboard board)
        {
            int value = 0;

            value += EvaluatePieceValues(board);

            return value;
        }

        private int EvaluatePieceValues(Chessboard board)
        {
            int value = 0;

            foreach(var location in board.Locations)
            {
                value += GetValueOfPiece(location.Piece);
            }

            return value;
        }

        private int GetValueOfPiece(ChessPiece piece)
        {
            if(piece == null)
            {
                return 0;
            }
            else
            {
                return piece.Colour == ChessColour.Black ? -1 * PieceValues[piece.GetType()] : PieceValues[piece.GetType()];
            }
        }
    }
}
