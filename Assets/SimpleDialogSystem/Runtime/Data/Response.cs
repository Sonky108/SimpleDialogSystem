using System;
using UnityEngine;

namespace SimpleDialogSystem.Runtime.Data
{
	[Serializable]
	public class Response : NodeContent
	{
		public string Message;
		public Line Line;
	}
}