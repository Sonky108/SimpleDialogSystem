using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Controllers;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils;
using UnityEngine;
using ContextMenu = SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils.ContextMenu;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Base
{
	public abstract class Port : IInputtable, IDrawable, IInputHolder
	{
		private readonly ContextMenu _contextMenu;
		private readonly Draggable _draggable;
		private readonly List<Port> _connectedPorts;
		private Rect _rect;

		public Port(float x, float y, Node owner)
		{
			_rect = new Rect(x, y, Settings.PortHeight, Settings.PortHeight);

			Owner = owner;

			_connectedPorts = new List<Port>();

			_draggable = new Draggable(_rect, OnDrag, this);
			_draggable.DragStarted += OnDragStarted;
			_draggable.DragEnded += OnDragEnded;

			_contextMenu = new ContextMenu();
			_contextMenu.AddAction(Names.ClearAllConnections, ClearAllConnections);
		}

		private void ClearAllConnections()
		{
			foreach (var x in _connectedPorts)
			{
				ConnectionRemoved?.Invoke(x);
				x.RemoveConnection(this);
			}
			
			_connectedPorts.Clear();
		}

		public void RemoveConnection(Port port)
		{
			if (_connectedPorts.Remove(port))
			{
				ConnectionRemoved?.Invoke(port);
			}
		}
		
		public event Action<Port, Event> DragEnded;
		public event Action<Port, Event> Drag;
		public event Action<Port, Port> PortsConnected;
		public event Action<Port> DragStarted;
		public event Action<Port> ConnectionRemoved;
		public int InputPriority => Utils.InputPriority.PORT;

		public bool CanUseInput(Event current)
		{
			return _rect.Contains(current.mousePosition);
		}

		public bool IsHoldingInput()
		{
			return _draggable.IsHoldingInput();
		}

		public void OnDragged(IUserInputtable userInputtable)
		{
			if (this is InputPort inputPort && userInputtable is OutputPort outputPort)
			{
				if (inputPort.Owner != outputPort.Owner)
				{
					PortsConnected?.Invoke(inputPort, outputPort);
					outputPort.PortsConnected?.Invoke(inputPort, outputPort);
				}
			}

			if (this is OutputPort output && userInputtable is InputPort input)
			{
				if (input.Owner != output.Owner)
				{
					PortsConnected?.Invoke(input, output);
					input.PortsConnected?.Invoke(input, output);
				}
			}
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
			GUI.Box(_rect, new GUIContent(_connectedPorts.Count.ToString()), Settings.PortStyle);
		}

		public void Clear() { }
		public Node Owner { get; }
		public Vector2 Position
		{
			get => _rect.position;
			set
			{
				_rect.position = value;
				_draggable.UpdateDragArea(value);
			}
		}

		public bool IsConnectedTo(Port port)
		{
			return _connectedPorts.Contains(port);
		}
		
		public bool ConnectPort(Port port)
		{
			if (IsConnectedTo(port))
			{
				return false;
			}
			
			_connectedPorts.Add(port);

			if (!port.IsConnectedTo(this))
			{
				port.ConnectPort(this);
			}

			return true;
		}

		private void OnDragStarted()
		{
			DragStarted?.Invoke(this);
		}

		private void OnDragEnded(Event current)
		{
			DragEnded?.Invoke(this, current);
		}

		private void OnDrag(Event current)
		{
			Drag?.Invoke(this, current);
		}

		public List<Node> GetConnectedNodes()
		{
			var result = new List<Node>();
			
			foreach (var x in _connectedPorts)
			{
				result.Add(x.Owner);
			}

			return result;
		}
	}
}