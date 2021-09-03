using System;
using SimpleDialogSystem.Editor.Scripts.Dialog.Window;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Dialog.Controller
{
	public class Background : NodeEditor.Base.Background
	{
		public Background()
		{
			ContextMenu.AddAction(Names.ContextMenu.AddLineNode, () => NewNodeRequested?.Invoke(LastEvent.mousePosition, NodeTypes.Line));
			ContextMenu.AddAction(Names.ContextMenu.AddResponseNode, () => NewNodeRequested?.Invoke(LastEvent.mousePosition, NodeTypes.Response));
		}

		public event Action<Vector2, NodeTypes> NewNodeRequested;
	}
}