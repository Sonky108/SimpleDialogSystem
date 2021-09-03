using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Controllers;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils;
using SimpleDialogSystem.Runtime.Data;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Base
{
	public abstract class Node<T> : IInputHolder, IInputable, IDrawable where T : NodeContent
	{
		protected GUIContent Title;
		protected GUIStyle GUIStyle;
		protected T Content;
		private readonly Draggable _draggableTitle;
		private readonly List<IInputHolder> _inputs = new List<IInputHolder>();
		public readonly Port InputPort;
		public readonly Port OutputPort;
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

			_draggableTitle = new Draggable(_titleRect, Drag, this);
			InputPort = new InputPort(position.x, position.y + (Settings.TitleHeight - Settings.PortHeight) / 2);
			OutputPort = new OutputPort(position.x + width - Settings.PortHeight, position.y + (Settings.TitleHeight - Settings.PortHeight) / 2);

			_inputs.Add(_draggableTitle);
			_inputs.Add(OutputPort);
			_inputs.Add(InputPort);
		}

		public event Action RepaintNeeded;
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
			DrawContent(Content, GetContentRect());
			DrawPorts();
		}

		public List<IInputable> GetAllInputables()
		{
			return new List<IInputable> {InputPort, OutputPort, this};
		}

		public virtual void SetTitle(GUIContent title)
		{
			Title = title;
		}

		public virtual void SetContent(T content)
		{
			Content = content;
		}

		public abstract void DrawContent(T content, Rect contentRect);

		private Rect GetContentRect()
		{
			return _contentRect;
		}

		private void DrawPorts()
		{
			InputPort.Draw();
			OutputPort.Draw();
		}

		private void Drag(Event current)
		{
			_rect.position += current.delta;
			_titleRect.position += current.delta;
			_contentRect.position += current.delta;
			Content.Position += current.delta;
			InputPort.Position += current.delta;
			OutputPort.Position += current.delta;

			_draggableTitle.UpdateDragArea(_titleRect);

			RepaintNeeded?.Invoke();
		}

		private void DrawBackground()
		{
			GUI.Box(_rect, GUIContent.none, Settings.NodeBackgroundStyle);
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