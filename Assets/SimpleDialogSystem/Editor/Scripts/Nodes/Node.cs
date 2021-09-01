using System;
using System.Collections.Generic;
using SimpleDialogSystem.Runtime.Data;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Nodes
{
	public abstract class Node<T> : IInputHolder, IInputable, IDrawable where T : NodeContent
	{
		protected GUIContent Title;
		protected GUIStyle GUIStyle;
		protected T Content;
		private readonly Draggable _draggableTitle;
		private readonly List<IInputHolder> _inputs = new List<IInputHolder>();
		private Rect _rect;
		private Rect _titleRect;
		private Rect _contentRect;

		public Node(Vector2 position, float width, float height, GUIStyle style = default)
		{
			if (style != default)
			{
				GUIStyle = style;
			}

			_rect = new Rect(position, new Vector2(width, height));
			_titleRect = new Rect(position, new Vector2(width, Settings.TitleHeight));
			_contentRect = new Rect(new Vector2(position.x, position.y + Settings.TitleHeight), new Vector2(width, height - Settings.TitleHeight));

			_draggableTitle = new Draggable(_titleRect, Drag);

			_inputs.Add(_draggableTitle);
		}

		public event Action RepaintNeeded;

		public bool IsHoldingInput()
		{
			foreach (IInputHolder x in _inputs)
			{
				if (x.IsHoldingInput())
				{
					return true;
				}
			}

			return false;
		}

		public bool CanUseInput(Event current)
		{
			return _rect.Contains(current.mousePosition);
		}

		public void Clear()
		{
			foreach (IInputHolder x in _inputs)
			{
				x.Clear();
			}
		}

		public void ProcessInput(Event current)
		{
			_draggableTitle.ProcessInput(current);
		}

		public void Draw()
		{
			DrawBackground();
			DrawTitle();
			DrawContent(Content);
		}

		public Rect GetContentRect()
		{
			return _contentRect;
		}

		public virtual void SetTitle(GUIContent title)
		{
			Title = title;
		}

		public virtual void SetContent(T content)
		{
			Content = content;
		}

		public abstract void DrawContent(T content);

		private void Drag(Vector2 delta)
		{
			_rect.position += delta;
			_titleRect.position += delta;
			_contentRect.position += delta;
			Content.Position += delta;
			
			_draggableTitle.UpdateDragArea(_titleRect);

			RepaintNeeded?.Invoke();
		}

		private void DrawBackground()
		{
			GUI.Box(_rect, GUIContent.none, Settings.BackgroundStyle);
		}

		private void DrawTitle()
		{
			if (Title != null)
			{
				GUI.Box(_titleRect, Title, Settings.TitleStyle);
			}
			else
			{
				GUI.Box(_titleRect, typeof(T).Name, Settings.TitleStyle);
			}
		}
	}
}