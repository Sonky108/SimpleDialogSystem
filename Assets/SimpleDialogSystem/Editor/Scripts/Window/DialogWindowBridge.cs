using System;
using SimpleDialogSystem.Runtime.Data.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleDialogSystem.Editor.Scripts.Window
{
	public class DialogWindowBridge : EditorWindow
	{
		private static DialogWindow _window;
		private static Object _data;
		public static event Action OnWindowClosed;

		private void OnGUI()
		{
			Event current = Event.current;

			_window.Draw(current);

			current.Use();
		}

		private void OnLostFocus()
		{
			_window.OnLostFocus();
		}

		public static void ShowWindow(Object data)
		{
			_data = data;
			_window = new DialogWindow((DialogData) data);

			DialogWindowBridge dialogWindowBridge = (DialogWindowBridge) GetWindow(typeof(DialogWindowBridge));
			dialogWindowBridge.titleContent = new GUIContent(data.name);

			_window.RepaintNeeded += () => dialogWindowBridge.Repaint();
		}

		private void OnDestroy()
		{
			OnWindowClosed?.Invoke();
			OnWindowClosed = null;
		}
	}
}