using System;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Nodes
{
	public class Draggable : IInputProcessor, IInputHolder
	{
		private readonly Action<Vector2> _onDrag;
		private bool _isDragging;
		private Rect _dragArea;

		public Draggable(Rect dragArea, Action<Vector2> onDrag)
		{
			_onDrag = onDrag;
			_dragArea = dragArea;
		}

		public void UpdateDragArea(Rect dragArea)
		{
			_dragArea = dragArea;
		}
		
		public bool IsHoldingInput()
		{
			return _isDragging;
		}

		public void Clear()
		{
			_isDragging = false;
		}

		public void ProcessInput(Event current)
		{
			if (current.type == EventType.MouseDown)
			{
				if (current.button == Settings.LeftMouseButton && _dragArea.Contains(current.mousePosition))
				{
					_isDragging = true;
				}
			}

			if (current.type == EventType.MouseUp)
			{
				_isDragging = false;
			}

			
			if (_isDragging && current.type == EventType.MouseDrag)
			{
				_onDrag?.Invoke(current.delta);
			}
		}
	}
}