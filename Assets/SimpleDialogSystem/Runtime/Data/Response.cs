﻿using UnityEngine;

namespace SimpleDialogSystem.Runtime.Data
{
	[System.Serializable]
	public class Response : NodeContent
	{
		public string Message;
		public Line Line;
	}
}