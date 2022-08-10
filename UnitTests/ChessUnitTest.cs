using Chess.Model.Command;
using Chess.Model.Data;
using Chess.Model.Game;
using Chess.Model.Piece;
using Chess.ViewModel.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace UnitTests
{
    [TestClass]
public class ChessUnitTests
    {

        
        [TestMethod]
        public void CheckBoards()
        {
            IChessGameVM testgameVM;

            //test normal chess mode
            testgameVM = new ChessGameVM();
            if (testgameVM.Board == null)
            {
                Assert.Fail("Board does not exist for normal game");
            }

            //test chess960 mode
            testgameVM = new ChessGame960VM();
            if (testgameVM.Board == null)
            {
                Assert.Fail("Board does not exist for chess960 game");
            }

        }

        [TestMethod]
        public void CheckPieceCount()
        {
            IChessGameVM testgame;
            int numberOfPieces;

            //test normal chess mode
            testgame = new ChessGameVM();
            numberOfPieces = testgame.Board.Pieces.Count;
            if (numberOfPieces != 32)
            {
                Assert.Fail("Expected Piece count for normal game is 32, the real piece count is " + numberOfPieces);
                //throw error
            }

            //test chess960 mode
            testgame = new ChessGame960VM();
            numberOfPieces = testgame.Board.Pieces.Count; 
            if (numberOfPieces != 32)
            {
                Assert.Fail("Expected Piece count for chess960 game is 32, the real piece count is " + numberOfPieces);
                //throw error
            }
        }

        [TestMethod]
        public void AddPiece()
        {
            ChessGame testGame;
            IChessGameVM testGameVM;

            //test normal chess mode
            try
            {
                testGameVM = new ChessGameVM();
                testGame = testGameVM.Game;
                testGame.Board.Add(new Position(3, 3), new Queen(Color.White));
            }
            catch (System.Exception)
            {
                Assert.Fail("Couldn't Add White Queen at 3,3 in Normal Chess Game");
            }

            //test chess960 mode
            try
            {
                testGameVM = new ChessGame960VM();
                testGame = testGameVM.Game;
                testGame.Board.Add(new Position(3, 3), new Queen(Color.White));
            }
            catch (System.Exception)
            {
                Assert.Fail("Couldn't Add White Queen at 3,3 in Chess960 Game");
            }
        }

        [TestMethod]

        public void RemovePiece()
        {   
            ChessGame testGame;
            IChessGameVM testGameVM;

            //test normal chess mode
            try
            {
                testGameVM = new ChessGameVM();
                testGame = testGameVM.Game;
                testGame.Board.Remove(new Position(0, 0));
                //if no errors or breaks, it passes 
            }
            catch (System.Exception)
            {

                Assert.Fail("Couldn't Remove Piece at 0,0 in Normal Chess Game");
            }

            //test chess960 mode
            try
            {
                testGameVM = new ChessGame960VM();
                testGame = testGameVM.Game;
                testGame.Board.Remove(new Position(0, 0));
                //if no errors or breaks, it passes 
            }
            catch (System.Exception)
            {

                Assert.Fail("Couldn't Remove Piece at 0,0 in Chess960 Game");
            }
        }


        [TestMethod]
        public void DoesWhiteGoFirst()
        {
            ChessGame testGame;
            IChessGameVM testGameVM;
            Color firstColor;

            //test normal chess mode
            testGameVM = new ChessGameVM();
            testGame = testGameVM.Game;
            firstColor = testGame.ActivePlayer.Color;
            if (firstColor != Color.White)
            {
                Assert.Fail("First Player Color was not Color White. First Player Color was " + firstColor);
            }

            //test chess960 mode
            testGameVM = new ChessGame960VM();
            testGame = testGameVM.Game;
            firstColor = testGame.ActivePlayer.Color;
            if (firstColor != Color.White)
            {
                Assert.Fail("First Player Color was not Color White. First Player Color was " + firstColor);
            }
        }
    }

    
    
}
