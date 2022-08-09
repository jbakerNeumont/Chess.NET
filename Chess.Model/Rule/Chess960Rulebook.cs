//-----------------------------------------------------------------------
// <copyright file="StandardRulebook.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Rule
{
    using Chess.Model.Command;
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using Chess.Model.Visitor;
	using System;
	using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    /// <summary>
    /// Represents the standard chess rulebook.
    /// </summary>
    public class Chess960Rulebook : IRulebook
    {
        /// <summary>
        /// Represents the check rule of a standard chess game.
        /// </summary>
        private readonly CheckRule checkRule;

        /// <summary>
        /// Represents the end rule of a standard chess game.
        /// </summary>
        private readonly EndRule endRule;

        /// <summary>
        /// Represents the movement rule of a standard chess game.
        /// </summary>
        private readonly MovementRule movementRule;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardRulebook"/> class.
        /// </summary>
        public Chess960Rulebook()
        {
            var threatAnalyzer = new ThreatAnalyzer();
            var castlingRule = new CastlingRule(threatAnalyzer);
            var enPassantRule = new EnPassantRule();
            var promotionRule = new PromotionRule();

            this.checkRule = new CheckRule(threatAnalyzer);
            this.movementRule = new MovementRule(castlingRule, enPassantRule, promotionRule, threatAnalyzer);
            this.endRule = new EndRule(this.checkRule, this.movementRule);
        }

        /// <summary>
        /// Creates a new chess game according to the standard rulebook.
        /// </summary>
        /// <returns>The newly created chess game.</returns>
        public ChessGame CreateGame()
        {
            var DerivationValues = GetDerivationValues();
            var availableCols = new List<int> {0, 1, 2, 3, 4, 5, 6, 7};
            IEnumerable<PlacedPiece> makeBaseLine(int row, Color color)
            {
                var p1 = new Position(row, DerivationValues["b1"], "bishopOdd", availableCols);
				if (availableCols.Contains(p1.Column)) { availableCols.Remove(p1.Column); }
                var p2 = new Position(row, DerivationValues["b1"], "bishopEven", availableCols);
                if (availableCols.Contains(p2.Column)) { availableCols.Remove(p2.Column); }
                var p3 = new Position(row, DerivationValues["q"], "queen", availableCols);
                if (availableCols.Contains(p3.Column)) { availableCols.Remove(p3.Column); }
                var p4 = new Position(row, DerivationValues["n4"], "knight", availableCols);
                if (availableCols.Contains(p4.Column)) { availableCols.Remove(p4.Column); }
                var p5 = new Position(row, DerivationValues["n4"], "knight", availableCols);
                if ( availableCols.Contains(p5.Column)) { availableCols.Remove(p5.Column); }
                var p6 = new Position(row, 0, "rook", availableCols);
                if ( availableCols.Contains(p6.Column)) { availableCols.Remove(p6.Column); }
                var p7 = new Position(row, 0, "king", availableCols);
                if ( availableCols.Contains(p7.Column)) { availableCols.Remove(p7.Column); }
                var p8 = new Position(row, 0, "rook", availableCols);
                if ( availableCols.Contains(p8.Column)) { availableCols.Remove(p8.Column); }
                yield return new PlacedPiece(p1, new Bishop(color));
                yield return new PlacedPiece(p2, new Bishop(color));
                yield return new PlacedPiece(p3, new Queen(color));
                yield return new PlacedPiece(p4, new Knight(color));
                yield return new PlacedPiece(p5, new Knight(color));
                yield return new PlacedPiece(p6, new Rook(color));
                yield return new PlacedPiece(p7, new King(color));
                yield return new PlacedPiece(p8, new Rook(color));
            }

            IEnumerable<PlacedPiece> makePawns(int row, Color color) =>
                Enumerable.Range(0, 8).Select(
                    i => new PlacedPiece(new Position(row, i), new Pawn(color))
                );

            IImmutableDictionary<Position, ChessPiece> makePieces(int pawnRow, int baseRow, Color color)
            {
                var pawns = makePawns(pawnRow, color);
                var baseLine = makeBaseLine(baseRow, color);
                var pieces = baseLine.Union(pawns);
                var empty = ImmutableSortedDictionary.Create<Position, ChessPiece>(PositionComparer.DefaultComparer);
                return pieces.Aggregate(empty, (s, p) => s.Add(p.Position, p.Piece));
            }

            var whitePlayer = new Player(Color.White);
            var whitePieces = makePieces(1, 0, Color.White);
            var blackPlayer = new Player(Color.Black);
            var blackPieces = makePieces(6, 7, Color.Black);
            var board = new Board(whitePieces.AddRange(blackPieces));

            return new ChessGame(board, whitePlayer, blackPlayer);
        }

        /// <summary>
        /// Gets the status of a chess game, according to the standard rulebook.
        /// </summary>
        /// <param name="game">The game state to be analyzed.</param>
        /// <returns>The current status of the game.</returns>
        public Status GetStatus(ChessGame game)
        {
            return this.endRule.GetStatus(game);
        }

        /// <summary>
        /// Gets all possible updates (i.e., future game states) for a chess piece on a specified position,
        /// according to the standard rulebook.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <param name="position">The position to be analyzed.</param>
        /// <returns>A sequence of all possible updates for a chess piece on the specified position.</returns>
        public IEnumerable<Update> GetUpdates(ChessGame game, Position position)
        {
            var piece = game.Board.GetPiece(position, game.ActivePlayer.Color);
            var updates = piece.Map(
                p =>
                {
                    var moves = this.movementRule.GetCommands(game, p);
                    var turnEnds = moves.Select(c => new SequenceCommand(c, EndTurnCommand.Instance));
                    var records = turnEnds.Select
                    (
                        c => new SequenceCommand(c, new SetLastUpdateCommand(new Update(game, c)))
                    );
                    var futures = records.Select(c => c.Execute(game).Map(g => new Update(g, c)));
                    return futures.FilterMaybes().Where
                    (
                        e => !this.checkRule.Check(e.Game, e.Game.PassivePlayer)
                    );
                }
            );

            return updates.GetOrElse(Enumerable.Empty<Update>());
        }

        public Dictionary<string, int> GetDerivationValues()
		{
            Random random = new Random();
            var result = new Dictionary<string, int>();
            var n = random.Next(959);
            result.Add("n", n);
            var b1 = (n % 4);
            result.Add("b1", b1);
            var n2 = (n - b1) / 4;
            var b2 = (n2 % 4);
            result.Add("b2", b2);
            var n3 = (n2 - b2) / 4;
            var q = (n3 % 6);
            result.Add("q", q);
            var n4 = (n3 % 6) / 6;
            result.Add("n4", n4);
            return result;
		}
    }
}

