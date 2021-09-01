using System;
using SimpleDialogSystem.Editor.Scripts.Nodes;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Window
{
	public class Background : IInputable, IInputHolder
	{
		private readonly ContextMenu _contextMenu;
		private Event _lastEvent;

		public Background()
		{
			_contextMenu = new ContextMenu();
			_contextMenu.AddAction(Names.ContextMenu.AddNode, () => NewNodeRequested?.Invoke(_lastEvent.mousePosition));
		}

		public event Action<Vector2> NewNodeRequested;

		public bool IsHoldingInput()
		{
			return false;
		}

		public bool CanUseInput(Event current)
		{
			return true;
		}

		public void ProcessInput(Event current)
		{
			_lastEvent = current;
			if (current.type == EventType.MouseDown)
			{
				if (current.button == Settings.RightMouseButton)
				{
					_contextMenu.Show();
				}
			}
		}

		public void Clear() { }
	}
}