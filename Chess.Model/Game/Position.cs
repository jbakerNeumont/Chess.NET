﻿//-----------------------------------------------------------------------
// <copyright file="Position.cs">
//     Copyright (c) Michael Szvetits. All rights reserved.
// </copyright>
// <author>Michael Szvetits</author>
//-----------------------------------------------------------------------
namespace Chess.Model.Game
{
    using Chess.Model.Data;
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents a position on a chess board.
	/// </summary>
	public class Position : IEquatable<Position>
    {
        /// <summary>
        /// Represents the row of the position, where 0 represents the bottom row.
        /// </summary>
        public readonly int Row;

        /// <summary>
        /// Represents the column of the position, where 0 represents the leftmost column.
        /// </summary>
        public readonly int Column;

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        /// <param name="row">The row of the position, where 0 represents the bottom row.</param>
        /// <param name="column">The column of the position, where 0 represents the leftmost column.</param>
        public Position(int row, int column)
        {
            Validation.InRange(row, 0, 7, nameof(row));
            Validation.InRange(column, 0, 7, nameof(column));

            this.Row = row;
            this.Column = column;
        }

        public Position(int row, int derVal, string pieceName, List<int> availableCols)
		{
            this.Row = row;
			switch (pieceName)
			{
                case ("bishopOdd"):
					switch (derVal)
					{
                        case 0:
                            this.Column = 1;
                            break;
                        case 1:
                            this.Column = 3;
                            break;
                        case 2:
                            this.Column = 5;
                            break;
                        case 3:
                            this.Column = 7;
                            break;
                    }
                    break;
                case ("bishopEven"):
                    switch (derVal)
                    {
                        case 0:
                            this.Column = 0;
                            break;
                        case 1:
                            this.Column = 2;
                            break;
                        case 2:
                            this.Column = 4;
                            break;
                        case 3:
                            this.Column = 6;
                            break;
                    }
                    break;
                case ("queen"):
                    this.Column = availableCols[derVal];
                    break;
                case ("knight1"):
					switch (derVal)
					{
                        case 0:
                            this.Column = availableCols[0];
                            break;
                        case 1:
                            this.Column = availableCols[0];
                            break;
                        case 2:
                            this.Column = availableCols[0];
                            break;
                        case 3:
                            this.Column = availableCols[0];
                            break;
                        case 4:
                            this.Column = availableCols[1];
                            break;
                        case 5:
                            this.Column = availableCols[1];
                            break;
                        case 6:
                            this.Column = availableCols[1];
                            break;
                        case 7:
                            this.Column = availableCols[2];
                            break;
                        case 8:
                            this.Column = availableCols[2];
                            break;
                        case 9:
                            this.Column = availableCols[3];
                            break;
                    }
                    break;
                case ("knight2"):
                    switch (derVal)
                    {
                        case 0:
                            this.Column = availableCols[0];
                            break;
                        case 1:
                            this.Column = availableCols[1];
                            break;
                        case 2:
                            this.Column = availableCols[2];
                            break;
                        case 3:
                            this.Column = availableCols[3];
                            break;
                        case 4:
                            this.Column = availableCols[1];
                            break;
                        case 5:
                            this.Column = availableCols[2];
                            break;
                        case 6:
                            this.Column = availableCols[3];
                            break;
                        case 7:
                            this.Column = availableCols[2];
                            break;
                        case 8:
                            this.Column = availableCols[3];
                            break;
                        case 9:
                            this.Column = availableCols[3];
                            break;
                    }
                    break;
                case ("rook1"):
                    this.Column = availableCols[0];
                    break;
                case ("rook2"):
                    this.Column = availableCols[^1];
                    break;
                case ("king"):
                    this.Column = availableCols[0];
                    break;
            }

        }

        /// <summary>
        /// Adds an offset to the position.
        /// </summary>
        /// <param name="offset">The offset to be added to the position.</param>
        /// <returns>The new position, if it is within the bounds of the chess board.</returns>
        public IMaybe<Position> Offset(Direction offset)
        {
            return this.Offset(offset.RowDelta, offset.ColumnDelta);
        }

        /// <summary>
        /// Adds an offset to the position.
        /// </summary>
        /// <param name="rowDelta">The row offset to be added to the position.</param>
        /// <param name="columnDelta">The column offset to be added to the position.</param>
        /// <returns>The new position, if it is within the bounds of the chess board.</returns>
        public IMaybe<Position> Offset(int rowDelta, int columnDelta)
        {
            var newRow = this.Row + rowDelta;
            var newColumn = this.Column + columnDelta;

            if (Validation.IsInRange(newRow, 0, 7) &&
                Validation.IsInRange(newColumn, 0, 7))
            {
                return new Just<Position>(new Position(newRow, newColumn));
            }

            return Nothing<Position>.Instance;
        }

        /// <summary>
        /// Indicates whether the current position is equal to another position.
        /// </summary>
        /// <param name="other">The position to compare with this position.</param>
        /// <returns>True if the current position is equal to the other one, or else false.</returns>
        public bool Equals(Position other)
        {
            return
                this.Row == other.Row &&
                this.Column == other.Column;
        }

        /// <summary>
        /// Indicates whether the current position is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with this position.</param>
        /// <returns>True if the current position is equal to the other object, or else false.</returns>
        public override bool Equals(object obj)
        {
            return
                obj is Position other &&
                this.Row == other.Row &&
                this.Column == other.Column;
        }

        /// <summary>
        /// Calculates a hash code which represents the position.
        /// </summary>
        /// <returns>A hash code for the position.</returns>
        public override int GetHashCode()
        {
            var hashCodeBuilder = new HashCode();
            hashCodeBuilder.Add(this.Row);
            hashCodeBuilder.Add(this.Column);
            return hashCodeBuilder.ToHashCode();
        }
    }
}