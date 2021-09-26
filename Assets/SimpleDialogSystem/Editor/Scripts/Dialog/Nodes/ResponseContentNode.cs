using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Base;
using SimpleDialogSystem.Runtime.Data;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Dialog.Nodes
{
	public class ResponseContentNode : ContentNode<Response>
	{
		public ResponseContentNode(Vector2 position, float width, float height, GUIStyle style = default) : base(position, width, height, style) { }
		protected override List<Type> InputOwnerTypes => new List<Type> {typeof(LineContentNode)};
		protected override List<Type> OutputOwnerTypes => new List<Type> {typeof(LineContentNode)};
		public override void OnNewConnection(Connection connection)
		{
			if (connection.To is InputPort && connection.To.Owner is LineContentNode lineContentNode)
			{
				Content.LineGuid = lineContentNode.Content.Guid;
			}
		}

		public override void DrawContent(Response content, Rect contentRect)
		{
			Content.Message = GUI.TextArea(contentRect, Content.Message);
		}
	}
}