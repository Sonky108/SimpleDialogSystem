using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.Nodes;

namespace SimpleDialogSystem.Editor.Scripts.Window
{
	public class InputUsersPriority : IComparer<IInputProcessor>
	{
		public int Compare(IInputProcessor x, IInputProcessor y)
		{
			if (x.GetType() == typeof(Node<>))
			{
				return 1;
			}

			return -1;
		}
	}
}