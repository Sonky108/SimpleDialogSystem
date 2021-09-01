using SimpleDialogSystem.Runtime.Data;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Nodes
{
	public class ResponseNode : Node<Response>
	{
		public ResponseNode(Vector2 position, float width, float height, GUIStyle style = default) : base(position, width, height, style) { }
		public override void DrawContent(Response content)
		{
			Content.Message = GUI.TextArea(GetContentRect(), Content.Message);
		}
	}
}