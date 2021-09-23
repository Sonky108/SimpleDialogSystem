using System;
using UnityEditor;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils
{
	public class ContextMenu
	{
		private readonly GenericMenu _menu;

		public ContextMenu()
		{
			_menu = new GenericMenu();
		}

		public void AddAction(string name, Action onSelected)
		{
			_menu.AddItem(new GUIContent(name), false, () => { onSelected?.Invoke(); });
		}

		public void AddAction(GUIContent content, Action onSelected)
		{
			_menu.AddItem(content, false, () => { onSelected?.Invoke(); });
		}

		public void Show()
		{
			_menu.ShowAsContext();
		}
	}
}