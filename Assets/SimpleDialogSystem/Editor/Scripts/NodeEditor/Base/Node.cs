using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Controllers;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Base
{
	public abstract class Node : IInputHolder, IInputtable, IDrawable
	{
		protected readonly List<IInputHolder> _inputs = new List<IInputHolder>();
		private readonly Draggable _draggableTitle;
		private Rect _rect;
		private Rect _titleRect;

		protected Node(Vector2 position, float width, float height)
		{
			_rect = new Rect(position, new Vector2(width, height));
			_titleRect = new Rect(position, new Vector2(width, Settings.TitleHeight));

			_draggableTitle = new Draggable(_titleRect, Drag, this);

			_inputs.Add(_draggableTitle);
		}

		public event Action RepaintNeeded;
		public event Action<Event> Dragged;
		public int InputPriority => Utils.InputPriority.NODE;

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

		public void OnDragged(IUserInputtable userInputtable) { }

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
			OnDraw();
		}

		protected virtual GUIContent GetTitle()
		{
			return GUIContent.none;
		}

		protected virtual void OnDrag(Event current) { }
		protected virtual void OnDraw() { }

		private void Drag(Event current)
		{
			_rect.position += current.delta;
			_titleRect.position += current.delta;
			OnDrag(current);

			_draggableTitle.UpdateDragArea(_titleRect);

			Dragged?.Invoke(current);
			RepaintNeeded?.Invoke();
		}

		private void DrawBackground()
		{
			GUI.Box(_rect, GUIContent.none, Settings.NodeBackgroundStyle);
		}

		private void DrawTitle()
		{
			GUI.Box(_titleRect, GetTitle(), Settings.TitleStyle);
		}
	}
}