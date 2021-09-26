using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleDialogSystem.Runtime.Data
{
	[Serializable]
	public class Line : NodeContent
	{
		public string Guid;
		public string Message;
		public List<string> ResponseIDs = new List<string>();
		public bool IsStartingLine;
		
		public Line(Vector2 position) : base(position)
		{
			Guid = System.Guid.NewGuid().ToString();
		}
	}
}