using System;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.Dialog.Nodes;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Base;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Controllers;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils;
using SimpleDialogSystem.Runtime.Data;
using SimpleDialogSystem.Runtime.Data.ScriptableObjects;
using UnityEditorInternal;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Dialog.Window
{
	public class DialogWindow
	{
		public DialogData Data;
		private readonly SortedSet<IDrawable> _drawables = new SortedSet<IDrawable>(new DrawableComparer());
		private readonly SortedSet<IInputable> _inputables = new SortedSet<IInputable>(new InputUsersComparer());
		private readonly Controller.Background _background;
		private IInputHolder _currentInputHolder;

		public event Action RepaintNeeded;
		
		public DialogWindow(DialogData dialogData)
		{
			Data = dialogData;
			
			_background = new Controller.Background();
			_background.NewNodeRequested += OnNewNodeRequested;

			temporaryCurve = new Curve(Vector2.zero, Vector2.zero);
			
			_inputables.Add(_background);
			_drawables.Add(_background);
			_drawables.Add(temporaryCurve);
			
			CreateLineNode(Data.OpeningLine.Position, Data.OpeningLine);

			foreach (var x in Data.OpeningLine.Response)
			{
				CreateResponseNode(x.Position, x);
			}
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

		private void CreateResponseNode(Vector2 position, Response response)
		{
			ResponseNode newResponse = new ResponseNode(position, Settings.ResponseNode.Width, Settings.ResponseNode.Height);

			newResponse.SetContent(response);
			newResponse.RepaintNeeded += RepaintNeeded;
			newResponse.InputPort.DragStarted += OnInputConnectionRequested;
			newResponse.OutputPort.DragStarted += OnInputConnectionRequested;
			_inputables.UnionWith(newResponse.GetAllInputables());
			_drawables.Add(newResponse);
			
			RepaintNeeded?.Invoke();
		}

		private Curve temporaryCurve;
		
		private void OnInputConnectionRequested(Port port)
		{
			temporaryCurve.SetStart(port.Position);
			temporaryCurve.SetEnd(port.Position);
			
			port.DragEnded += OnDragEnded;
			port.Drag += OnDrag;
		}

		private void OnDrag(Port port, Event current)
		{
			temporaryCurve.SetEnd(current.mousePosition);
		}

		private void OnDragEnded(Port port)
		{
			temporaryCurve.Clear();
			port.Drag -= OnDrag;
			port.DragEnded -= OnDragEnded;
		}

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

			foreach (IInputable x in _inputables)
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

		private void CreateLineNode(Vector2 position, Line content = default)
		{
			LineNode newLine = new LineNode(position, Settings.LineNode.Width, Settings.LineNode.Height);

			newLine.SetContent(content);
			newLine.RepaintNeeded += RepaintNeeded;

			_inputables.UnionWith(newLine.GetAllInputables());
			_drawables.Add(newLine);
			
			RepaintNeeded?.Invoke();
		}
	}
}