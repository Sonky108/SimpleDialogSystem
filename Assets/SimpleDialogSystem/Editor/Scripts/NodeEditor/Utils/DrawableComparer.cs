using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Base;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils
{
	public class DrawableComparer : IComparer<IDrawable>
	{
		public int Compare(IDrawable x, IDrawable y)
		{
			if (x.Equals(y))
			{
				return 0;
			}
			
			if (x is Background)
			{
				return 1;
			}

			if (x is Curve)
			{
				return 1;
			}
			
			if (x.GetType() == typeof(ContentNode<>))
			{
				return 1;
			}

			
			
			return 1;
		}
	}
}