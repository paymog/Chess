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

            movePiece(toIndex, fromIndex);
            makeMovePieceAssertions(fromIndex, toIndex, typeof(Rook), ChessColour.Black);
            Assert.IsFalse(board.BlackLocations[fromIndex]);
            Assert.IsTrue(board.BlackLocations[toIndex]);
            Assert.IsFalse(board.WhiteLocations[fromIndex]);
            Assert.IsFalse(board.WhiteLocations[toIndex]);


            toIndex = 18;
            fromIndex = 63;

            movePiece(toIndex, fromIndex);
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

            movePiece(toIndex, fromIndex);
            makeMovePieceAssertions(fromIndex, toIndex, typeof(Rook), ChessColour.Black);
            Assert.IsTrue(board.BlackLocations[toIndex]);
            Assert.IsFalse(board.BlackLocations[fromIndex]);
            Assert.IsFalse(board.WhiteLocations[toIndex]);
            Assert.IsFalse(board.BlackLocations[fromIndex]);


            toIndex = 4;
            fromIndex = 63;

            movePiece(toIndex, fromIndex);
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

            movePiece(toIndex, fromIndex);
            makeMovePieceAssertions(fromIndex, toIndex, typeof(Rook), ChessColour.Black);
            Assert.IsTrue(board.BlackLocations[toIndex]);
            Assert.IsFalse(board.BlackLocations[fromIndex]);
            Assert.IsFalse(board.WhiteLocations[toIndex]);
            Assert.IsFalse(board.WhiteLocations[fromIndex]);


            toIndex = 62;
            fromIndex = 63;

            movePiece(toIndex, fromIndex);
            makeMovePieceAssertions(fromIndex, toIndex, typeof(Rook), ChessColour.White);
            Assert.IsTrue(board.WhiteLocations[toIndex]);
            Assert.IsFalse(board.WhiteLocations[fromIndex]);
            Assert.IsFalse(board.BlackLocations[toIndex]);
            Assert.IsFalse(board.BlackLocations[fromIndex]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testMovePiece_NullFirstParameters()
        {
            board.MovePiece(null, board.Locations[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testMovePiece_NullSecondParameters()
        {
            board.MovePiece(board.Locations[0], null);
        }



        private void movePiece(int toIndex, int fromIndex)
        {
            board.MovePiece(board.Locations[fromIndex], board.Locations[toIndex]);
        }
    }
}
