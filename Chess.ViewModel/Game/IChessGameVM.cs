
using Chess.Model.Command;
using Chess.Model.Game;
using Chess.ViewModel.Command;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ViewModel.Game
{
	public interface IChessGameVM
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public BoardVM Board { get; }

		public Status Status { get; }
		public GenericCommand NewCommand { get; }
		public GenericCommand UndoCommand { get; }

		public void Select(int row, int column);

		public void Visit(SequenceCommand command);

		public void Visit(EndTurnCommand command);

		public void Visit(MoveCommand command);

		public void Visit(RemoveCommand command);

		public void Visit(SetLastUpdateCommand command);

		public void Visit(SpawnCommand command);

	}
}
