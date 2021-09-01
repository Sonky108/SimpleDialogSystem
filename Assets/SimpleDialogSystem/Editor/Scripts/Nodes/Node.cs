using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Nodes
{
	public abstract class Node<T> : IInputHolder, IInputable, IDrawable
	{
		protected T Content;
		protected GUIContent Title;
		protected GUIStyle GUIStyle;
		protected Rect Rect;
		private readonly Draggable _draggableTitle;
		private readonly List<IInputHolder> _inputs = new List<IInputHolder>();
		private Rect _titleRect;

		public Node(Vector2 position, float width, float height, GUIStyle style = default)
		{
			if (style != default)
			{
				GUIStyle = style;
			}

			Rect = new Rect(position, new Vector2(width, height));
			_titleRect = new Rect(position, new Vector2(width, Settings.TitleHeight));

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

		public void Clear()
		{
			foreach (var x in _inputs)
			{
				x.Clear();
			}
		}

		public virtual void SetTitle(GUIContent title)
		{
			Title = title;
		}

		public virtual void SetContent(T content)
		{
			Content = content;
		}
		
		public bool CanUseInput(Event current)
		{
			return Rect.Contains(current.mousePosition);
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

		public abstract void DrawContent(T content);

		private void Drag(Vector2 delta)
		{
			Rect.position += delta;
			_titleRect.position += delta;
			_draggableTitle.UpdateDragArea(_titleRect);
			
			RepaintNeeded?.Invoke();
		}

		private void DrawBackground()
		{
			GUI.Box(Rect, GUIContent.none, Settings.BackgroundStyle);
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