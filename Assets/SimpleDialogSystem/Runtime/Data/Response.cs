using System;
using UnityEditor;
using UnityEngine;

namespace SimpleDialogSystem.Runtime.Data
{
	[Serializable]
	public class Response : NodeContent
	{
		public string LineGuid;
		public string Message;
		public string Guid;

		public Response(Vector2 position) : base(position)
		{
			Guid = GUID.Generate().ToString();
		}
	}
}