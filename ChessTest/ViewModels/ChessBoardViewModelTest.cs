using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chess.ViewModels;
using Chess.Models;

namespace ChessTest.ViewModels
{
    [TestClass]
    public class ChessBoardViewModelTest
    {
        private ChessboardViewModel model;
        private bool handled;

        [TestInitialize]
        public void testInitialize()
        {
            this.model = new ChessboardViewModel();
            this.handled = false;
        }

        [TestMethod]
        public void CanSelectLocation_WithoutCurrentSelection_NoPiece()
        {
            var location = new BoardLocation(ChessColour.Black);
            Assert.IsFalse(CanSelect(location));
        }

        [TestMethod]
        public void CanSelectLocation_WithoutCurrentSelection_RightPieceColour()
        {
            var location = new BoardLocation(ChessColour.Black, new Pawn(ChessColour.White));
            Assert.IsTrue(CanSelect(location));

            model.CurrentPlayerColour = ChessColour.Black;
            location.Piece = new Pawn(ChessColour.Black);
            Assert.IsTrue(CanSelect(location));
        }

        [TestMethod]
        public void CanSelectLocation_WithoutCurrentSelection_WrongPieceColour()
        {
            var location = new BoardLocation(ChessColour.Black, new Pawn(ChessColour.Black));
            Assert.IsFalse(CanSelect(location));

            model.CurrentPlayerColour = ChessColour.Black;
            location.Piece = new Pawn(ChessColour.White);
            Assert.IsFalse(CanSelect(location));
        }

        [TestMethod]
        public void CanSelectLocation_WithCurrentSelection_WithoutPiece()
        {
            var location = new BoardLocation(ChessColour.Black) { IsTargeted = true };
            model.SelectedBoardLocation = new BoardLocation(ChessColour.Black);
            Assert.IsTrue(CanSelect(location));

            location.IsTargeted = false;
            Assert.IsFalse(CanSelect(location));
        }

        [TestMethod]
        public void CanSelectLocation_WithCurrentSelection_WithPiece_WrongColour()
        {
            var location = new BoardLocation(ChessColour.Black, new Pawn(ChessColour.Black)) { IsTargeted = false };
            model.SelectedBoardLocation = new BoardLocation(ChessColour.Black);
            Assert.IsFalse(CanSelect(location));

            location.IsTargeted = true;
            Assert.IsTrue(CanSelect(location));
        }

        [TestMethod]
        public void CanSelectLocation_WithCurrentSelection_WithPiece_RightColour()
        {
            var location = new BoardLocation(ChessColour.Black, new Pawn(ChessColour.White));
            model.SelectedBoardLocation = new BoardLocation(ChessColour.White);
            Assert.IsTrue(CanSelect(location));
        }

        [TestMethod]
        public void SelectLocation_WithoutCurrentSelection()
        {
            var location = model.Locations[0];
            Select(location);
            Assert.AreSame(location, model.SelectedBoardLocation);
        }

        [TestMethod]
        public void SelectLocation_WithCurrentSelection_SameLocationWithPiece()
        {
            var location = model.Locations[0];
            Select(location);
            Select(location);
            Assert.IsNull(model.SelectedBoardLocation);
        }

        [TestMethod]
        public void SelectLocation_WithCurrentSelection_DifferenLocationWithPiece()
        {
            //two cases, select another piece of same colour vs different colour
            //same colour
            var firstLocation = model.Locations[0];
            var secondLocation = model.Locations[1];
            model.CurrentPlayerColour = ChessColour.Black;

            Select(firstLocation);
            Select(secondLocation);

            Assert.AreSame(secondLocation, model.SelectedBoardLocation);
            Assert.IsFalse(firstLocation.IsSelected);

            //different colour
            this.testInitialize();
            model.CurrentPlayerColour = ChessColour.Black;
            firstLocation = model.Locations[0]; //select black rook
            Select(firstLocation);

            secondLocation = model.Locations[56]; //select white rook in same column, this should be targeted
            Select(secondLocation);
            Assert.IsNull(model.SelectedBoardLocation);
            Assert.IsFalse(firstLocation.IsSelected);
            Assert.IsFalse(secondLocation.IsSelected);
            
        }

        [TestMethod]
        public void SelectLocation_WithCurrentSelection_DifferLocationWithoutPiece()
        {
            var firstLocation = model.Locations[0];
            var secondLocation = model.Locations[17];

            Select(firstLocation);
            Select(secondLocation);

            Assert.IsNull(model.SelectedBoardLocation);
            Assert.IsFalse(firstLocation.IsSelected);
            Assert.IsFalse(secondLocation.IsSelected);
        }


        private bool CanSelect(BoardLocation location)
        {
            return model.CanExecuteCommand(ChessboardViewModel.SelectLocationCommand, location, out handled);
        }

        private void Select(BoardLocation location)
        {
            model.ExecuteCommand(ChessboardViewModel.SelectLocationCommand, location, out handled);
        }
    }
}
