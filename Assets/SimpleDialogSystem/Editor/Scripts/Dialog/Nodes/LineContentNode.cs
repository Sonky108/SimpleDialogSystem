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
		private readonly GUIContent _openingLineTitle;

		private float _contentHeight;
		public LineContentNode(Vector2 position, float width, float height, GUIStyle style = default) : base(position, width, height, style)
		{
			_title = new GUIContent("Line");
			_openingLineTitle = new GUIContent("Opening line");
		}

		protected override List<Type> InputOwnerTypes => new List<Type> {typeof(ResponseContentNode)};
		protected override List<Type> OutputOwnerTypes => new List<Type> {typeof(ResponseContentNode)};

		public override void DrawContent(Line content, Rect contentRect)
		{
			_contentHeight = contentRect.height;
			
			contentRect.height = NodeEditor.Utils.Settings.ToggleHeight;
			
			Content.IsStartingLine = GUI.Toggle(contentRect, Content.IsStartingLine, "Is starting line?");
			
			contentRect.position += Vector2.up * (NodeEditor.Utils.Settings.ToggleHeight + NodeEditor.Utils.Settings.ToggleHeight);
			contentRect.height = _contentHeight;
			
			Content.Message = GUI.TextArea(contentRect, Content.Message);
		}

		protected override GUIContent GetTitle()
		{
			if (Content.IsStartingLine)
			{
				return _openingLineTitle;
			}
			
			return _title;
		}

		public override void OnNewConnection(Connection connection)
		{
			if (connection.To is InputPort && connection.To.Owner is ResponseContentNode responseContentNode && !Content.ResponseIDs.Contains(responseContentNode.Content.Guid))
			{
				Content.ResponseIDs.Add(responseContentNode.Content.Guid);
			}
		}
	}
}