using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces
{
	public interface IInputProcessor
	{
		void ProcessInput(Event current);
		int InputPriority { get; }
	}
}