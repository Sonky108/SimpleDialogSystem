using SimpleDialogSystem.Editor.Scripts.NodeEditor.Base;
using SimpleDialogSystem.Runtime.Data;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Dialog.Nodes
{
	public class ResponseContentNode : ContentNode<Response>
	{
		public ResponseContentNode(Vector2 position, float width, float height, GUIStyle style = default) : base(position, width, height, style) { }
		public override void DrawContent(Response content, Rect contentRect)
		{
			Content.Message = GUI.TextArea(contentRect, Content.Message);
		}
	}
}