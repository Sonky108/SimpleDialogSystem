using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.Dialog.Window;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils;
using SimpleDialogSystem.Runtime.Data;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Base
{
	public abstract class ContentNode<T> : Node where T : NodeContent
	{
		protected GUIStyle GUIStyle;
		public T Content { get; private set; }
		private Rect _contentRect;

		public ContentNode(Vector2 position, float width, float height, GUIStyle style = default) : base(position, width, height)
		{
			if (style != default)
			{
				GUIStyle = style;
			}

			_contentRect = new Rect(new Vector2(position.x, position.y + Settings.TitleHeight), new Vector2(width, height - Settings.TitleHeight));
		}

		public List<IInputtable> GetAllInputables()
		{
			return new List<IInputtable> {InputPort, OutputPort, this};
		}

		public void SetContent(T content)
		{
			Content = content;
		}

		public abstract void DrawContent(T content, Rect contentRect);

		protected override void OnDraw()
		{
			DrawContent(Content, GetContentRect());
		}

		protected override void OnDrag(Event current)
		{
			_contentRect.position += current.delta;
			Content.Position += current.delta;
		}

		private Rect GetContentRect()
		{
			return _contentRect;
		}
	}
}