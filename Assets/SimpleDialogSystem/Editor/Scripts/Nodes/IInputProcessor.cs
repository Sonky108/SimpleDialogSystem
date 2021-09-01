using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Nodes
{
	public interface IInputProcessor
	{
		void ProcessInput(Event current);
	}
}