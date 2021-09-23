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
		public readonly Port InputPort;
		public readonly Port OutputPort;
		protected readonly List<IInputHolder> _inputs = new List<IInputHolder>();
		private readonly Draggable _draggableTitle;
		private Rect _rect;
		private Rect _titleRect;

		protected Node(Vector2 position, float width, float height)
		{
			_rect = new Rect(position, new Vector2(width, height));
			_titleRect = new Rect(position, new Vector2(width, Settings.TitleHeight));

			_draggableTitle = new Draggable(_titleRect, Drag, this);

			InputPort = new InputPort(position.x, position.y + (Settings.TitleHeight - Settings.PortHeight) / 2, this);
			OutputPort = new OutputPort(position.x + width - Settings.PortHeight, position.y + (Settings.TitleHeight - Settings.PortHeight) / 2, this);

			_inputs.Add(_draggableTitle);
			_inputs.Add(OutputPort);
			_inputs.Add(InputPort);

			InputPort.DragEnded += OnDragEnded;
			OutputPort.DragEnded += OnDragEnded;

			InputPort.SetAvailableOwnerTypes(InputOwnerTypes);
			OutputPort.SetAvailableOwnerTypes(OutputOwnerTypes);
		}

		public event Action RepaintNeeded;
		public event Action<Event> Dragged;
		public event Action<Port, Event> PortDragEnd;
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
			DrawPorts();
			OnDraw();
		}

		protected abstract List<Type> InputOwnerTypes { get; }
		protected abstract List<Type> OutputOwnerTypes { get; }

		protected virtual GUIContent GetTitle()
		{
			return GUIContent.none;
		}

		protected virtual void OnDrag(Event current) { }
		protected virtual void OnDraw() { }

		private void DrawPorts()
		{
			InputPort.Draw();
			OutputPort.Draw();
		}

		private void OnDragEnded(Port port, Event current)
		{
			PortDragEnd?.Invoke(port, current);
		}

		private void Drag(Event current)
		{
			_rect.position += current.delta;
			_titleRect.position += current.delta;
			InputPort.Position += current.delta;
			OutputPort.Position += current.delta;

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

		public abstract void OnNewConnection(Connection connection);
	}
}