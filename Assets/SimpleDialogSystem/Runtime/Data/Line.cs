using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleDialogSystem.Runtime.Data
{
	[Serializable]
	public class Line : NodeContent
	{
		public string Guid;
		public string Message;
		public List<string> ResponseIDs;

		public Line(Vector2 position) : base(position)
		{
			Guid = GUID.Generate().ToString();
		}
	}
}