using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Base;
using SimpleDialogSystem.Runtime.Data;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Dialog.Nodes
{
	public class LineContentNode : ContentNode<Line>
	{
		private readonly GUIContent _title;

		public LineContentNode(Vector2 position, float width, float height, GUIStyle style = default) : base(position, width, height, style)
		{
			_title = new GUIContent("Line");
		}

		protected override List<Type> InputOwnerTypes => new List<Type> {typeof(ResponseContentNode)};
		protected override List<Type> OutputOwnerTypes => new List<Type> {typeof(ResponseContentNode)};

		public override void DrawContent(Line content, Rect contentRect)
		{
			Content.Message = GUI.TextArea(contentRect, Content.Message);
		}

		protected override GUIContent GetTitle()
		{
			return _title;
		}

		public override void OnNewConnection(Connection connection)
		{
			if (connection.To.Owner is ResponseContentNode responseContentNode)
			{
				Content.ResponseIDs.Add(responseContentNode.Content.Guid);
			}
		}
	}
}