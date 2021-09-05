using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.Dialog.Nodes;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Base;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils;
using SimpleDialogSystem.Runtime.Data;
using SimpleDialogSystem.Runtime.Data.ScriptableObjects;
using UnityEngine;
using Background = SimpleDialogSystem.Editor.Scripts.Dialog.Controller.Background;

namespace SimpleDialogSystem.Editor.Scripts.Dialog.Window
{
	public class DialogWindow
	{
		public DialogData Data;
		private readonly Background _background;
		private readonly SortedSet<IDrawable> _drawables = new SortedSet<IDrawable>(new DrawableComparer());
		private readonly SortedSet<IInputtable> _inputables = new SortedSet<IInputtable>(new InputUsersComparer());
		private IInputHolder _currentInputHolder;

		public DialogWindow(DialogData dialogData)
		{
			Data = dialogData;

			_background = new Background();
			_background.NewNodeRequested += OnNewNodeRequested;

			TemporaryCurve = new Curve(Vector2.zero, Vector2.zero);

			_inputables.Add(_background);
			_drawables.Add(_background);
			_drawables.Add(TemporaryCurve);

			var lineNode = CreateLineNode(Data.OpeningLine.Position, Data.OpeningLine);

			foreach (Response x in Data.OpeningLine.Response)
			{
				var node = CreateResponseNode(x.Position, x);
				
				lineNode.OutputPort.ConnectPort(node.InputPort);

				OnPortsConnected(node.InputPort, lineNode.OutputPort);
			}
		}

		public event Action RepaintNeeded;
		public static Curve TemporaryCurve { get; private set; }

		public void Draw(Event current)
		{
			foreach (IDrawable x in _drawables)
			{
				x.Draw();
			}

			if (_currentInputHolder != null && _currentInputHolder.IsHoldingInput())
			{
				if (_currentInputHolder is IInputProcessor inputProcessor)
				{
					inputProcessor.ProcessInput(current);
				}

				return;
			}

			_currentInputHolder = null;

			foreach (IInputtable x in _inputables)
			{
				if (x.CanUseInput(current))
				{
					x.ProcessInput(current);

					if (x is IInputHolder inputHolder && inputHolder.IsHoldingInput())
					{
						_currentInputHolder = inputHolder;
					}

					break;
				}
			}
		}

		public void OnLostFocus()
		{
			_currentInputHolder?.Clear();
		}

		private void OnNewNodeRequested(Vector2 position, NodeTypes nodeTypes)
		{
			switch (nodeTypes)
			{
				case NodeTypes.Line:
					CreateLineNode(position, new Line());
					break;
				case NodeTypes.Response:
					CreateResponseNode(position, new Response());
					break;
			}
		}

		private ResponseContentNode CreateResponseNode(Vector2 position, Response response)
		{
			ResponseContentNode newResponseContent = new ResponseContentNode(position, Settings.ResponseNode.Width, Settings.ResponseNode.Height);

			newResponseContent.SetContent(response);
			newResponseContent.RepaintNeeded += RepaintNeeded;
			newResponseContent.PortDragEnd += OnPortDragEnd;
			newResponseContent.PortsConnected += OnPortsConnected;
			_inputables.UnionWith(newResponseContent.GetAllInputables());
			_drawables.Add(newResponseContent);

			RepaintNeeded?.Invoke();

			return newResponseContent;
		}

		private void OnPortsConnected(Port input, Port output)
		{
			Curve drawable = new Curve(input.Position, output.Position);
			input.Owner.Dragged += current => { drawable.MoveStart(current.delta); };
			output.Owner.Dragged += current => { drawable.MoveEnd(current.delta); };

			input.ConnectionRemoved += port =>
			                           {
					                           _drawables.Remove(drawable);
			                           };
			
			output.ConnectionRemoved += port =>
			                            {
					                            _drawables.Remove(drawable);
			                            };
			
			_drawables.Add(drawable);
		}

		private void OnPortDragEnd(Port port, Event current)
		{
			foreach (IInputtable x in _inputables)
			{
				if (x.CanUseInput(current))
				{
					x.OnDragged(port);
					break;
				}
			}
		}

		private LineContentNode CreateLineNode(Vector2 position, Line content = default)
		{
			LineContentNode newLineContent = new LineContentNode(position, Settings.LineNode.Width, Settings.LineNode.Height);

			newLineContent.SetContent(content);
			newLineContent.RepaintNeeded += RepaintNeeded;
			newLineContent.PortDragEnd += OnPortDragEnd;
			newLineContent.PortsConnected += OnPortsConnected;

			_inputables.UnionWith(newLineContent.GetAllInputables());
			_drawables.Add(newLineContent);

			RepaintNeeded?.Invoke();

			return newLineContent;
		}
	}
}