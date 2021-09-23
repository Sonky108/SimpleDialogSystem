using System;
using UnityEngine;

namespace SimpleDialogSystem.Runtime.Data
{
	[Serializable]
	public class NodeContent
	{
		public Vector2 Position;

		public NodeContent(Vector2 position)
		{
			Position = position;
		}
	}
}