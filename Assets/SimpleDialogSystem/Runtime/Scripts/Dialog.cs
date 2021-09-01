using System;

namespace SimpleDialogSystem.Runtime.Scripts
{
	public class Dialog
	{
		private Line _openingLine;
		private Line _currentLine;
		public event Action DialogEnded;
		public event Action<Line> LineReady;
		public event Action<Line> DialogStarted;

		public void SelectResponse(int i)
		{
			_currentLine = _currentLine.GetResponse(i).GetLine();

			if (_currentLine == null)
			{
				End();
			}

			LineReady?.Invoke(_currentLine);
		}

		public void Start()
		{
			_currentLine = _openingLine;
			DialogStarted?.Invoke(_currentLine);
		}

		public void End()
		{
			DialogEnded?.Invoke();
		}
	}
}