using SimpleDialogSystem.Runtime.Settings;
using UnityEngine;

namespace SimpleDialogSystem.Runtime.Data.ScriptableObjects
{
	[CreateAssetMenu(menuName = Names.DialogDataPath)]
	public class DialogData : ScriptableObject
	{
		public Line OpeningLine;
	}
}