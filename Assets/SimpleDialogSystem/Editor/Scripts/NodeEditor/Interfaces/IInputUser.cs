using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces
{
	public interface IInputUser
	{
		bool CanUseInput(Event current);
	}
}