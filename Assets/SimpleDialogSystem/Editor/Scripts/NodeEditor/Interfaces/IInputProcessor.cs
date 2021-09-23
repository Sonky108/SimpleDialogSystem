using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces
{
	public interface IInputProcessor
	{
		int InputPriority { get; }
		void ProcessInput(Event current);
	}
}