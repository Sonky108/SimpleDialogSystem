using System;
using System.CodeDom;
using SimpleDialogSystem.Editor.Scripts.Dialog.Nodes;
using SimpleDialogSystem.Runtime.Data;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils
{
	public abstract class Drawer<T> where T : NodeContent
	{
		public abstract void Draw(T content, Rect contentRect);
	}

	public class LineNodeDrawer : Drawer<Line>
	{
		public override void Draw(Line content, Rect contentRect) { }
	}
}