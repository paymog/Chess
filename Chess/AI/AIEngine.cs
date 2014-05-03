using Chess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AI
{
    class AIEngine
    {
        public static readonly int MAX_PLY = 2;

        public ChessboardEvaluator Evaluator { get; private set; }

        public AIEngine()
        {
            Evaluator = new ChessboardEvaluator();

        }

        /// <summary>
        /// Performs minimax search for best move. No pruning
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public ChessMove FindBestMove(Chessboard board, ChessColour player)
        {
            return FindBestMove(board, player, 1).Item2;
        }

        private Tuple<int, ChessMove> FindBestMove(Chessboard board, ChessColour player, int currentPly)
        {
            var pieceLocations = board.GetLocations(player);
            var movesAndScore = new List<Tuple<int, ChessMove>>();
            

            foreach (var location in pieceLocations)
            {
                var locationIndex = board.Locations.IndexOf(location);
                var ray = board.GetRay(location);
                for (int i = 0; i < ray.Count; i++)
                {
                    if (ray[i])
                    {
                        var move = new ChessMove { FromIndex = locationIndex, ToIndex = i, PieceMoved = location.Piece, PieceTaken = board.Locations[i].Piece };
                        var newBoard = new Chessboard(board);
                        newBoard.MovePiece(locationIndex, i);

                        if (currentPly == MAX_PLY)
                        {
                            movesAndScore.Add(new Tuple<int, ChessMove>(Evaluator.Evaluate(newBoard), move));
                        }
                        else
                        {
                            movesAndScore.Add(Tuple.Create(FindBestMove(newBoard, Utils.GetOtherColour(player), currentPly + 1).Item1, move));
                        }
                    }
                }
            }

            if (player == ChessColour.Black)
            {
                return FindMin(movesAndScore);
            }
            else
            {
                return FindMax(movesAndScore);
            }
        }



        private Tuple<int, ChessMove> FindMin(List<Tuple<int, ChessMove>> moves)
        {
            var bestScore = moves[0].Item1;
            var minMoves = new List<Tuple<int, ChessMove>>();
            minMoves.Add(moves[0]);

            foreach(var t in moves)
            {
                if(t.Item1 < bestScore)
                {
                    minMoves.Clear();
                    minMoves.Add(t);
                    bestScore = t.Item1;
                }
                else if(t.Item1 == bestScore)
                {
                    minMoves.Add(t);
                }
            }

            var rnd = new Random();
            return minMoves[rnd.Next(minMoves.Count)];

        }

        private Tuple<int, ChessMove> FindMax(List<Tuple<int, ChessMove>> moves)
        {
            var bestScore = moves[0].Item1;
            var minMoves = new List<Tuple<int, ChessMove>>();
            minMoves.Add(moves[0]);

            foreach (var t in moves)
            {
                if (t.Item1 > bestScore)
                {
                    minMoves.Clear();
                    minMoves.Add(t);
                    bestScore = t.Item1;
                }
                else if (t.Item1 == bestScore)
                {
                    minMoves.Add(t);
                }
            }

            var rnd = new Random();
            return minMoves[rnd.Next(minMoves.Count)];
        }
    }
}
