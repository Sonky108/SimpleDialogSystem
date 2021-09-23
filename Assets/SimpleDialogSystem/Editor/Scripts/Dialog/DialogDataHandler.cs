using SimpleDialogSystem.Editor.Scripts.Dialog.Window;
using SimpleDialogSystem.Runtime.Data.ScriptableObjects;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Dialog
{
	public class DialogDataHandler
	{
		private static readonly string lastopened = "lastOpened";

		[OnOpenAsset]
		public static bool Open(int instanceID, int line)
		{
			Object instance = EditorUtility.InstanceIDToObject(instanceID);
			EditorPrefs.SetInt(lastopened, instanceID);

			if (instance.GetType() == typeof(DialogData))
			{
				DialogWindowBridge.ShowWindow(instance);
				DialogWindowBridge.OnWindowClosed += ClearPrefs;
			}

			return false;
		}

		[DidReloadScripts]
		public static void ReopenOnCompiled()
		{
			if (EditorPrefs.HasKey(lastopened))
			{
				Open(EditorPrefs.GetInt(lastopened), 0);
			}
		}

		private static void ClearPrefs()
		{
			EditorPrefs.DeleteKey(lastopened);
		}
	}
}