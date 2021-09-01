using SimpleDialogSystem.Runtime.Data.ScriptableObjects;

namespace SimpleDialogSystem.Runtime.Scripts
{
	public class DialogFactory
	{
		public Dialog Create(DialogData data)
		{
			return new Dialog();
		}
	}
}