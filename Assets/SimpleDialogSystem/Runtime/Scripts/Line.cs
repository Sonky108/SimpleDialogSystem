using System;
using System.Collections.Generic;

namespace SimpleDialogSystem.Runtime.Scripts
{
	public class Line : IMessageProvider
	{
		
		private Message _message;
		public List<Response> Responses = new List<Response>();

		public Line(Data.Line line)
		{
			_message = new Message(line.Message);
		}

		public string GetMessage()
		{
			return _message.GetMessage();
		}

		public Response GetResponse(int i)
		{
			if (i < 0 || i > Responses.Count)
			{
				throw new ArgumentOutOfRangeException($"Trying to get {i} response in line {_message.GetMessage()} while there is {Responses.Count} response(s) available!");
			}

			return Responses[i];
		}
	}
}