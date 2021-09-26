namespace SimpleDialogSystem.Runtime.Scripts
{
	public class Response : IMessageProvider
	{
		public Line Line;
		private Message _message;

		public Response(Data.Response response)
		{
			_message = new Message(response.Message);
		}
		
		public string GetMessage()
		{
			return _message.GetMessage();
		}

		public bool IsResponseAvailable()
		{
			return true;
		}

		public Line GetLine()
		{
			return Line;
		}
	}
}