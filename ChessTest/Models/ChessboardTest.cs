using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chess.Models;

namespace ChessTest.Models
{
    [TestClass]
    public class ChessboardTest
    {
        private Chessboard board;

        [TestInitialize]
        public void initialize()
        {
            this.board = new Chessboard();
        }

        private void makeMovePieceAssertions(int fromIndex, int toIndex, Type pieceType, ChessColour pieceColour)
        {
            Assert.IsInstanceOfType(board.Locations[toIndex].Piece, pieceType);
            Assert.AreEqual(board.Locations[toIndex].PieceColour, pieceColour);
            Assert.IsNull(board.Locations[fromIndex].Piece);
        }

        [TestMethod]
        public void testMovePiece_toBlankSpot()
        {
            int toIndex = 17;
            int fromIndex = 0;

            movePiece(fromIndex, toIndex);
            makeMovePieceAssertions(fromIndex, toIndex, typeof(Rook), ChessColour.Black);
            Assert.IsFalse(board.BlackLocations[fromIndex]);
            Assert.IsTrue(board.BlackLocations[toIndex]);
            Assert.IsFalse(board.WhiteLocations[fromIndex]);
            Assert.IsFalse(board.WhiteLocations[toIndex]);


            toIndex = 18;
            fromIndex = 63;

            movePiece(fromIndex, toIndex);
            makeMovePieceAssertions(fromIndex, toIndex, typeof(Rook), ChessColour.White);
            Assert.IsTrue(board.WhiteLocations[toIndex]);
            Assert.IsFalse(board.WhiteLocations[fromIndex]);
            Assert.IsFalse(board.BlackLocations[toIndex]);
            Assert.IsFalse(board.BlackLocations[fromIndex]);
        }

        
        [TestMethod]
        public void testMovePiece_toEnemySpot()
        {
            int toIndex = 61;
            int fromIndex = 0;

            movePiece(fromIndex, toIndex);
            makeMovePieceAssertions(fromIndex, toIndex, typeof(Rook), ChessColour.Black);
            Assert.IsTrue(board.BlackLocations[toIndex]);
            Assert.IsFalse(board.BlackLocations[fromIndex]);
            Assert.IsFalse(board.WhiteLocations[toIndex]);
            Assert.IsFalse(board.BlackLocations[fromIndex]);


            toIndex = 4;
            fromIndex = 63;

            movePiece(fromIndex, toIndex);
            makeMovePieceAssertions(fromIndex, toIndex, typeof(Rook), ChessColour.White);
            Assert.IsTrue(board.WhiteLocations[toIndex]);
            Assert.IsFalse(board.WhiteLocations[fromIndex]);
            Assert.IsFalse(board.BlackLocations[toIndex]);
            Assert.IsFalse(board.BlackLocations[fromIndex]);

        }

        [TestMethod]
        public void testMovePiece_toOwnSpot()
        {
            int toIndex = 1;
            int fromIndex = 0;

            movePiece(fromIndex, toIndex);
            makeMovePieceAssertions(fromIndex, toIndex, typeof(Rook), ChessColour.Black);
            Assert.IsTrue(board.BlackLocations[toIndex]);
            Assert.IsFalse(board.BlackLocations[fromIndex]);
            Assert.IsFalse(board.WhiteLocations[toIndex]);
            Assert.IsFalse(board.WhiteLocations[fromIndex]);


            toIndex = 62;
            fromIndex = 63;

            movePiece(fromIndex, toIndex);
            makeMovePieceAssertions(fromIndex, toIndex, typeof(Rook), ChessColour.White);
            Assert.IsTrue(board.WhiteLocations[toIndex]);
            Assert.IsFalse(board.WhiteLocations[fromIndex]);
            Assert.IsFalse(board.BlackLocations[toIndex]);
            Assert.IsFalse(board.BlackLocations[fromIndex]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testMovePiece_NullFirstParameter()
        {
            board.MovePiece(null, board.Locations[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testMovePiece_NullSecondParameter()
        {
            board.MovePiece(board.Locations[0], null);
        }


        /// <summary>
        /// See http://goo.gl/L9bcdy to make sense of the numbers
        /// </summary>
        [TestMethod]
        public void testIsPlayerInCheck_BlackInCheck()
        {
            movePiece(13, 21); //move black pawn pawn
            movePiece(59, 31); //move white queen to put black in check

            Assert.IsTrue(board.IsPlayerInCheck(ChessColour.Black));
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));

            movePiece(31, 59); //move white queen back to original position
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));
            movePiece(57, 10); //check with knight

            Assert.IsTrue(board.IsPlayerInCheck(ChessColour.Black));
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));

            movePiece(10, 57);
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));
            movePiece(51, 11); //check with pawn

            Assert.IsTrue(board.IsPlayerInCheck(ChessColour.Black));
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));

            movePiece(11, 51);
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));
            movePiece(58, 31); //check with bishop - same location as queen previously was

            Assert.IsTrue(board.IsPlayerInCheck(ChessColour.Black));
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));

            movePiece(31, 58);
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));
            movePiece(12, 19); //move black pawn out of way
            movePiece(63, 44); //check with rook

            Assert.IsTrue(board.IsPlayerInCheck(ChessColour.Black));
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));
        }

        /// <summary>
        /// See http://goo.gl/L9bcdy to make sense of the numbers
        /// </summary>
        [TestMethod]
        public void testIsPlayerInCheck_WhiteInCheck()
        {
            movePiece(51, 43); //move white pawn
            movePiece(3, 24); //move black queen

            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));
            Assert.IsTrue(board.IsPlayerInCheck(ChessColour.White));

            movePiece(24, 3);
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));
            movePiece(1, 50); // check with knight

            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));
            Assert.IsTrue(board.IsPlayerInCheck(ChessColour.White));

            movePiece(50, 1);
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));
            movePiece(11, 51); // check with pawn

            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));
            Assert.IsTrue(board.IsPlayerInCheck(ChessColour.White));

            movePiece(51, 11);
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));
            movePiece(2, 24); // check with bishop

            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));
            Assert.IsTrue(board.IsPlayerInCheck(ChessColour.White));

            movePiece(24, 2);
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));
            movePiece(52, 45); //move pawn out of way
            movePiece(0, 20); // check with rook

            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));
            Assert.IsTrue(board.IsPlayerInCheck(ChessColour.White));
        }


        [TestMethod]
        public void testIsPlayerInCheck_NobodyInCheck()
        {
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));

            movePiece(59, 31); //move white queen. Pawn is still in the way

            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));

            movePiece(57, 20); // move knight, close to king but not in check positiion

            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.White));
            Assert.IsFalse(board.IsPlayerInCheck(ChessColour.Black));

            //hopefully that's good enough1
        }

        private void movePiece(int fromIndex, int toIndex)
        {
            board.MovePiece(board.Locations[fromIndex], board.Locations[toIndex]);
        }
    }
}
