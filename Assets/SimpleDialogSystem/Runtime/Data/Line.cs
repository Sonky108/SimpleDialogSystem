using System.Collections.Generic;
using UnityEngine;

namespace SimpleDialogSystem.Runtime.Data
{
	[System.Serializable]
	public class Line
	{
		public string Message;
		public List<Response> Response;
		public Vector2 Position;
	}
}