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
        public static readonly int MAX_PLY = 3;
        public int recursiveCount = 0;
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
            return FindBestMove(board, player, MAX_PLY).Item2;
        }

        public ChessMove FindBestAlphaBetaMove(Chessboard board, ChessColour player)
        {
            var moves = new List<Tuple<int, ChessMove>>();

            foreach(var move in FindAllMoves(board, player))
            {
                var newBoard = new Chessboard(board);
                newBoard.MakeMove(move.FromIndex, move.ToIndex);

                var score = AlphaBeta(newBoard, MAX_PLY - 1, int.MinValue, int.MaxValue, Utils.GetOtherColour(player));

                moves.Add(Tuple.Create( score, move));
            }

            if(player == ChessColour.White)
            {
                return FindMax(moves).Item2;
            }
            else
            {
                return FindMin(moves).Item2;
            }

        }

        private int AlphaBeta(Chessboard board, int depth, int alpha, int beta, ChessColour player)
        {
            recursiveCount += 1;
            if (depth == 0)
            {
                return Evaluator.Evaluate(board);
            }

            Chessboard originalBoard = new Chessboard(board);
            if(player == ChessColour.White)
            {
                foreach(var move in FindAllMoves(board, player))
                {
                    board.MakeMove(move.FromIndex, move.ToIndex);
                    alpha = Math.Max(alpha, AlphaBeta(board, depth - 1, alpha, beta, ChessColour.Black));
                    board.UndoMove(move);

                    if(beta <= alpha)
                    {
                        break;
                    }
                }
                return alpha;
            }
            else
            {
                foreach (var move in FindAllMoves(board, player))
                {
                    board.MakeMove(move.FromIndex, move.ToIndex);
                    beta = Math.Min(beta, AlphaBeta(board, depth - 1, alpha, beta, ChessColour.White));
                    board.UndoMove(move);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return beta;
            }
        }

        private Tuple<int, ChessMove> FindBestMove(Chessboard board, ChessColour player, int currentPly)
        {
            recursiveCount += 1;
            var movesAndScore = new List<Tuple<int, ChessMove>>();
            

            foreach(var move in FindAllMoves(board, player))
            {
                var newBoard = new Chessboard(board);
                newBoard.MakeMove(move.FromIndex, move.ToIndex);

                // base case
                if (currentPly == 1)
                {
                    movesAndScore.Add(new Tuple<int, ChessMove>(Evaluator.Evaluate(newBoard), move));
                }
                else //recursive case
                {
                    var bestMove = FindBestMove(newBoard, Utils.GetOtherColour(player), currentPly - 1);
                    movesAndScore.Add(Tuple.Create(bestMove.Item1, move));
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

        private IEnumerable<ChessMove> FindAllMoves(Chessboard board, ChessColour player)
        {
            var pieceLocations = board.GetLocations(player);

            foreach (var location in pieceLocations)
            {
                var locationIndex = board.Locations.IndexOf(location);
                var ray = board.GetRay(location);
                for (int i = 0; i < ray.Count; i++)
                {
                    if (ray[i])
                    {
                        yield return new ChessMove(locationIndex, i, location.Piece, board.Locations[i].Piece);
                    }
                }
            }
        }
    }
}
