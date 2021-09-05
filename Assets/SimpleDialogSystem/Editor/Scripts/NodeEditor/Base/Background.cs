using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using UnityEngine;
using ContextMenu = SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils.ContextMenu;
using Settings = SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils.Settings;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Base
{
	public class Background : IInputtable, IInputHolder, IDrawable
	{
		protected readonly ContextMenu ContextMenu;
		protected Event LastEvent;

		public Background()
		{
			ContextMenu = new ContextMenu();
		}

		public int InputPriority => Utils.InputPriority.BACKGROUND;

		public bool IsHoldingInput()
		{
			return false;
		}

		public bool CanUseInput(Event current)
		{
			return true;
		}

		public void OnDragged(IUserInputtable userInputtable)
		{
			
		}

		public void Draw()
		{
			GUI.Box(new Rect(0, 0, 2000, 2000), GUIContent.none, Settings.BackgroundStyle);
		}

		public void ProcessInput(Event current)
		{
			LastEvent = current;

			if (current.type == EventType.MouseDown)
			{
				if (current.button == Dialog.Settings.RightMouseButton)
				{
					ContextMenu.Show();
				}
			}
		}

		public void Clear() { }
	}
}