using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Base
{
	public class Connection : IDrawable
	{
		private readonly Curve _drawableImplementation;

		public Connection(Port from, Port to)
		{
			From = from;
			To = to;

			_drawableImplementation = new Curve(from.Position, to.Position);
			From.Owner.Dragged += current => { _drawableImplementation.MoveStart(current.delta); };
			To.Owner.Dragged += current => { _drawableImplementation.MoveEnd(current.delta); };
		}

		public void Draw()
		{
			_drawableImplementation.Draw();
		}

		public override bool Equals(object obj)
		{
			if (obj is Connection connection)
			{
				return From == connection.From && To == connection.To || From == connection.To && To == connection.From;
			}
			
			return base.Equals(obj);
		}

		public Port From { get; }
		public Port To { get; }
	}
}