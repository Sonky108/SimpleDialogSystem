namespace SimpleDialogSystem.Runtime.Scripts
{
	public class Response : IMessageProvider
	{
		private Line _line;
		private Message _message;

		public string GetMessage()
		{
			return _message.GetMessage();
		}

		public Line GetLine()
		{
			return _line;
		}

		public bool IsResponseAvailable()
		{
			return true;
		}
	}
}