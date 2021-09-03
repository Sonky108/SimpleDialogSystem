using System;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Controllers
{
	public class InputLocker : IInputProcessor, IInputHolder
	{
		private readonly Action<Event> _onProcessingInput;

		public InputLocker(Action<Event> onProcessingInput)
		{
			_onProcessingInput = onProcessingInput;
		}

		public int InputPriority => -1;

		public bool IsHoldingInput()
		{
			return true;
		}

		public void ProcessInput(Event current)
		{
			Debug.Log("procesing");
			_onProcessingInput?.Invoke(current);
		}

		public void Clear() { }
	}
}