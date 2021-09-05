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
		public readonly Port InputPort;
		public readonly Port OutputPort;
		protected GUIStyle GUIStyle;
		protected T Content;
		private Rect _contentRect;

		public event Action<Port, Event> PortDragEnd;
		public event Action<Port, Port> PortsConnected;
		
		public ContentNode(Vector2 position, float width, float height, GUIStyle style = default) : base(position, width, height)
		{
			if (style != default)
			{
				GUIStyle = style;
			}

			_contentRect = new Rect(new Vector2(position.x, position.y + Settings.TitleHeight), new Vector2(width, height - Settings.TitleHeight));

			InputPort = new InputPort(position.x, position.y + (Settings.TitleHeight - Settings.PortHeight) / 2, this);
			OutputPort = new OutputPort(position.x + width - Settings.PortHeight, position.y + (Settings.TitleHeight - Settings.PortHeight) / 2, this);

			_inputs.Add(OutputPort);
			_inputs.Add(InputPort);

			InputPort.DragStarted += OnInputConnectionRequested;
			OutputPort.DragStarted += OnInputConnectionRequested;
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
			DrawPorts();
		}

		protected override void OnDrag(Event current)
		{
			_contentRect.position += current.delta;
			Content.Position += current.delta;
			InputPort.Position += current.delta;
			OutputPort.Position += current.delta;
		}

		private Rect GetContentRect()
		{
			return _contentRect;
		}

		private void OnInputConnectionRequested(Port port)
		{
			DialogWindow.TemporaryCurve.SetStart(port.Position);
			DialogWindow.TemporaryCurve.SetEnd(port.Position);

			port.PortsConnected += OnPortsConnected;
			port.DragEnded += OnDragEnded;
			port.Drag += OnDrag;
		}

		private void OnPortsConnected(Port input, Port output)
		{
			if (input.ConnectPort(output))
			{
				PortsConnected?.Invoke(input, output);
			}
		}

		private void OnDrag(Port port, Event current)
		{
			DialogWindow.TemporaryCurve.SetEnd(current.mousePosition);
		}

		private void OnDragEnded(Port port, Event current)
		{
			port.Drag -= OnDrag;
			port.DragEnded -= OnDragEnded;

			PortDragEnd?.Invoke(port, current);
			
			port.PortsConnected -= OnPortsConnected;
			
			DialogWindow.TemporaryCurve.Clear();
		}

		private void DrawPorts()
		{
			InputPort.Draw();
			OutputPort.Draw();
		}
	}
}