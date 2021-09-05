using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces
{
	public interface IUserInputtable
	{
		bool CanUseInput(Event current);
		void OnDragged(IUserInputtable userInputtable);
	}
}