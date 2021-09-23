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
		public override List<Type> InputOwnerTypes => new List<Type>() {typeof(ResponseContentNode)};
		public override List<Type> OutputOwnerTypes => new List<Type>() {typeof(ResponseContentNode)};
		public LineContentNode(Vector2 position, float width, float height, GUIStyle style = default) : base(position, width, height, style)
		{
			_title = new GUIContent("Line");
		}

		public override void DrawContent(Line content, Rect contentRect)
		{
			Content.Message = GUI.TextArea(contentRect, Content.Message);
		}

		protected override GUIContent GetTitle()
		{
			return _title;
		}
	}
}