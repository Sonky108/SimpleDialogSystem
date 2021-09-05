using SimpleDialogSystem.Editor.Scripts.NodeEditor.Interfaces;
using SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils;
using UnityEditor;
using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Base
{
	public class Curve : IDrawable
	{
		private Vector3 _start;
		private Vector3 _end;

		public Curve(Vector2 start, Vector2 end)
		{
			_start = start;
			_end = end;
		}
		
		public void Draw()
		{
			if (Vector2.Distance(_end, _start) < Settings.MinimalCurveDrawDistance)
			{
				return;
			}

			Handles.DrawBezier(_start, _end, _start + Vector3.right * 50, _end + Vector3.up * 50, Color.red, null, 5);
		}

		public void SetStart(Vector2 start)
		{
			_start = start;
		}

		public void MoveStart(Vector3 delta)
		{
			_start += delta;
		}
		
		public void MoveEnd(Vector3 delta)
		{
			_end += delta;
		}
		
		public void SetEnd(Vector3 end)
		{
			_end = end;
		}

		public void Clear()
		{
			_end = _start;
		}
	}
}