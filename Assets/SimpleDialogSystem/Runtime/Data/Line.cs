using System;
using System.Collections.Generic;

namespace SimpleDialogSystem.Runtime.Data
{
	[Serializable]
	public class Line : NodeContent
	{
		public string Message;
		public List<Response> Response;
	}
}