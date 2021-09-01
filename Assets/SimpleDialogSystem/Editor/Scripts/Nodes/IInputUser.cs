using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.Nodes
{
	public interface IInputUser
	{
		bool CanUseInput(Event current);
	}
}