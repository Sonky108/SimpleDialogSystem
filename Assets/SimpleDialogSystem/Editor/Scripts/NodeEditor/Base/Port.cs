using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Controllers;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils;
using UnityEngine;
using ContextMenu = SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils.ContextMenu;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Base
{
	public abstract class Port : IInputable, IDrawable, IInputHolder
	{
		private readonly ContextMenu _contextMenu;
		private readonly Draggable _draggable;
		private readonly List<Port> _connectedPorts;
		private Rect _rect;

		public Port(float x, float y)
		{
			_rect = new Rect(x, y, Settings.PortHeight, Settings.PortHeight);

			_connectedPorts = new List<Port>();

			_draggable = new Draggable(_rect, OnDrag, this);
			_draggable.DragStarted += OnDragStarted;
			_draggable.DragEnded += OnDragEnded;

			_contextMenu = new ContextMenu();
			_contextMenu.AddAction(Names.ClearAllConnections, () => { _connectedPorts.Clear(); });
		}

		public event Action<Port> DragStarted;
		public event Action<Port> DragEnded;
		public event Action<Port, Event> Drag;
		public int InputPriority => Utils.InputPriority.PORT;

		public bool CanUseInput(Event current)
		{
			return _rect.Contains(current.mousePosition);
		}

		public bool IsHoldingInput()
		{
			return _draggable.IsHoldingInput();
		}

		public void ProcessInput(Event current)
		{
			_draggable.ProcessInput(current);

			if (_draggable.IsHoldingInput())
			{
				return;
			}

			if (current.type == EventType.MouseDown && current.button == Settings.RightMouseButton)
			{
				_contextMenu.Show();
			}
		}

		public void Draw()
		{
			GUI.Box(_rect, GUIContent.none, Settings.PortStyle);
		}

		public void Clear() { }
		public Vector2 Position
		{
			get => _rect.position;
			set
			{
				_rect.position = value;
				_draggable.UpdateDragArea(value);
			}
		}

		public void ConnectPort(Port port)
		{
			if (_connectedPorts.Contains(port))
			{
				return;
			}

			_connectedPorts.Add(port);
		}

		private void OnDragStarted()
		{
			DragStarted?.Invoke(this);
		}

		private void OnDragEnded()
		{
			DragEnded?.Invoke(this);
		}

		private void OnDrag(Event current)
		{
			Drag?.Invoke(this, current);
		}
	}
}