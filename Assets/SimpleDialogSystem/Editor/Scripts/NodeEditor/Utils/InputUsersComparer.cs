using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils
{
	public class InputUsersComparer : IComparer<IInputProcessor>
	{
		public int Compare(IInputProcessor x, IInputProcessor y)
		{
			int equalType = y.InputPriority - x.InputPriority;

			if (equalType == 0)
			{
				return -1;
			}

			return equalType;
		}
	}
}