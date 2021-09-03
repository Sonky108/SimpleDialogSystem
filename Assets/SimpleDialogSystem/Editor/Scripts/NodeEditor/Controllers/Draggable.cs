using System;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using UnityEngine;
using Settings = SimpleDialogSystem.Editor.Scripts.Dialog.Settings;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Controllers
{
	public class Draggable : IInputProcessor, IInputHolder
	{
		private readonly Action<Event> _onDrag;
		private bool _isDragging;
		private Rect _dragArea;

		public Draggable(Rect dragArea, Action<Event> onDrag, IInputProcessor owner)
		{
			_onDrag = onDrag;
			InputPriority = owner.InputPriority;
			_dragArea = dragArea;
		}

		public event Action DragStarted;
		public event Action DragEnded;
		public int InputPriority { get; }

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
					DragStarted?.Invoke();
				}
			}

			if (current.type == EventType.MouseUp)
			{
				_isDragging = false;
				DragEnded?.Invoke();
			}

			if (_isDragging && current.type == EventType.MouseDrag)
			{
				_onDrag?.Invoke(current);
			}
		}

		public void UpdateDragArea(Rect dragArea)
		{
			_dragArea = dragArea;
		}

		public void UpdateDragArea(Vector2 dragPosition)
		{
			_dragArea.position = dragPosition;
		}
	}
}