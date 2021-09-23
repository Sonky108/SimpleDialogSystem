using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.Dialog.Window;
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
		private readonly List<Connection> _connections;
		private Rect _rect;

		public Port(float x, float y, Node owner)
		{
			_rect = new Rect(x, y, Settings.PortHeight, Settings.PortHeight);

			Owner = owner;

			_connections = new List<Connection>();

			_draggable = new Draggable(_rect, OnDrag, this);
			_draggable.DragStarted += OnDragStarted;
			_draggable.DragEnded += OnDragEnded;

			_contextMenu = new ContextMenu();
			_contextMenu.AddAction(Names.ClearAllConnections, ClearAllConnections);
		}

		private void ClearAllConnections()
		{
			for (int i = _connections.Count - 1; i >= 0; i--)
			{
				var x = _connections[i];
				RemoveConnection(x);
			}

			_connections.Clear();
		}

		public void RemoveConnection(Connection connection)
		{
			if (_connections.Remove(connection))
			{
				ConnectionRemoved?.Invoke(connection);
				connection.To.RemoveConnection(connection);
			}
		}
		
		public event Action<Port, Event> DragEnded;
		public event Action<Connection> PortsConnected;
		public event Action<Connection> ConnectionRemoved;
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
			if (userInputtable is Port port && !IsConnectedTo(port))
			{
				ConnectPort(port);
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
			GUI.Box(_rect, new GUIContent(_connections.Count.ToString()), Settings.PortStyle);
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
			foreach (var x in _connections)
			{
				if (x.To == port)
				{
					return true;
				}
			}

			return false;
		}
		
		public Connection ConnectPort(Port port)
		{
			if (CanConnectPort(port))
			{
				Connection connection = new Connection(this, port);
				_connections.Add(connection);

				if (!port.IsConnectedTo(this))
				{
					port.ConnectPort(this);
				}

				PortsConnected?.Invoke(connection);
			
				return connection;
			}

			return null;
		}

		private bool CanConnectPort(Port port)
		{
			return !IsConnectedTo(port) && port.Owner != Owner;
		}

		private void OnDragStarted()
		{
			DialogWindow.TemporaryCurve.SetStart(Position);
			DialogWindow.TemporaryCurve.SetEnd(Position);
		}

		private void OnDragEnded(Event current)
		{
			DialogWindow.TemporaryCurve.Clear();

			DragEnded?.Invoke(this, current);
		}

		private void OnDrag(Event current)
		{
			DialogWindow.TemporaryCurve.SetEnd(current.mousePosition);
		}

		// public List<Node> GetConnectedNodes()
		// {
		// 	var result = new List<Node>();
		// 	
		// 	foreach (var x in _connectedPorts)
		// 	{
		// 		result.Add(x.Owner);
		// 	}
		//
		// 	return result;
		// }
	}
}