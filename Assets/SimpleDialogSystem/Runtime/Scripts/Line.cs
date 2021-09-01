using System;

namespace SimpleDialogSystem.Runtime.Scripts
{
	public class Line : IMessageProvider
	{
		private Message _message;
		private Response[] _responses;

		public string GetMessage()
		{
			return _message.GetMessage();
		}

		public Response GetResponse(int i)
		{
			if (i < 0 || i > _responses.Length)
			{
				throw new ArgumentOutOfRangeException($"Trying to get {i} response in line {_message.GetMessage()} while there is {_responses.Length} response(s) available!");
			}

			return _responses[i];
		}
	}
}