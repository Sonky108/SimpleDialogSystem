using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Base;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using Background = SimpleDialogSystem.Editor.Scripts.Dialog.Controller.Background;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils
{
	public class DrawableComparer : IComparer<IDrawable>
	{
		private static readonly Dictionary<Type, int> _renderOrder = new Dictionary<Type, int>
		{
			{typeof(Background), 0}, {typeof(Curve), 1}, {typeof(Connection), 2}, {typeof(Node), 3},
		};

		public int Compare(IDrawable x, IDrawable y)
		{
			if (x.Equals(y))
			{
				return 0;
			}

			int xOrder = x is Node ? _renderOrder[typeof(Node)] : _renderOrder[x.GetType()];
			int yOrder = y is Node ? _renderOrder[typeof(Node)] : _renderOrder[y.GetType()];

			if (xOrder - yOrder == 0)
			{
				return 1;
			}

			return xOrder - yOrder;
		}
	}
}