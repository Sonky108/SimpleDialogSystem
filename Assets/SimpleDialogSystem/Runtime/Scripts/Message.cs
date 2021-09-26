namespace SimpleDialogSystem.Runtime.Scripts
{
	public class Message
	{
		private string _message;
		public Message(string message)
		{
			_message = message;
		}

		public string GetMessage()
		{
			return _message;
		}
	}
}