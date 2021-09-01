using System;
using System.CodeDom;
using System.Collections.Generic;
using SimpleDialogSystem.Editor.Scripts.Nodes;
using SimpleDialogSystem.Runtime.Data;
using SimpleDialogSystem.Runtime.Data.ScriptableObjects;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Window
{
	public class DialogWindow
	{
		public DialogData Data;
		private readonly HashSet<IDrawable> _drawables = new HashSet<IDrawable>();
		private readonly SortedSet<IInputable> _inputables = new SortedSet<IInputable>(new InputUsersPriority());
		private readonly Background _background;
		private IInputHolder _currentInputHolder;

		public event Action RepaintNeeded;
		
		public DialogWindow(DialogData dialogData)
		{
			Data = dialogData;

			_background = new Background();
			_background.NewNodeRequested += OnNewNodeRequested;
			
			_inputables.Add(_background);

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

			_inputables.Add(newResponse);
			_drawables.Add(newResponse);
			
			RepaintNeeded?.Invoke();
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

				current.Use();
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

			_inputables.Add(newLine);
			_drawables.Add(newLine);
			
			RepaintNeeded?.Invoke();
		}
	}
}