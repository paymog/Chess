using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chess.ViewModels;
using Chess.Models;

namespace ChessTest.ViewModels
{
    [TestClass]
    public class ChessBoardViewModelTest
    {
        private ChessBoardViewModel model;
        private bool handled;

        [TestInitialize]
        public void testInitialize()
        {
            this.model = new ChessBoardViewModel();
            this.handled = false;
        }

        [TestMethod]
        public void CanMovePieceHere_NoSelectedLocation()
        {
            model.SelectedBoardLocation = null;
            Assert.IsFalse(model.CanExecuteCommand(ChessBoardViewModel.MovePieceHereCommand, new BoardLocation(ChessColour.Black), out handled));
        }

        [TestMethod]
        public void CanMoviePieceHere_WithSameLocation()
        {
            BoardLocation location = new BoardLocation(ChessColour.Black);
            model.SelectedBoardLocation = location;
            Assert.IsFalse(model.CanExecuteCommand(ChessBoardViewModel.MovePieceHereCommand, location, out handled));
        }

        [TestMethod]
        public void CanMovePieceHere_WithDifferentLocation()
        {
            model.SelectedBoardLocation = new BoardLocation(ChessColour.Black);
            Assert.IsTrue(model.CanExecuteCommand(ChessBoardViewModel.MovePieceHereCommand, new BoardLocation(ChessColour.White), out handled));
        }
    }
}
