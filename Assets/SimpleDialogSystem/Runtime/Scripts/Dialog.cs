using System;
using System.Collections.Generic;
using SimpleDialogSystem.Runtime.Data.ScriptableObjects;

namespace SimpleDialogSystem.Runtime.Scripts
{
	public class Dialog
	{
		private Line _openingLine;
		private Line[] _lines;
		private Response[] _responses;
		private Line _currentLine;

		public Dialog(DialogData data)
		{
			_lines = new Line[data.Lines.Count];
			_responses = new Response[data.Responses.Count];

			var missingResponsesLines = new Dictionary<string, List<Line>>();
			var lines = new Dictionary<string, Line>();
			
			for (int i = 0; i < data.Lines.Count; i++)
			{
				_lines[i] = new Line(data.Lines[i]);

				if (data.Lines[i].IsStartingLine)
				{
					_openingLine = _lines[i];
				}

				foreach (var x in data.Lines[i].ResponseIDs)
				{
					if (missingResponsesLines.TryGetValue(x, out var list))
					{
						list.Add(_lines[i]);
					}
					else
					{
						missingResponsesLines.Add(x, new List<Line>(){_lines[i]});		
					}
				}
				
				lines.Add(data.Lines[i].Guid, _lines[i]);
			}

			for (int i = 0; i < data.Responses.Count; i++)
			{
				_responses[i] = new Response(data.Responses[i]);

				if (missingResponsesLines.ContainsKey(data.Responses[i].Guid))
				{
					foreach (var x in missingResponsesLines[data.Responses[i].Guid])
					{
						x.Responses.Add(_responses[i]);
					}
				}

				_responses[i].Line = lines[data.Responses[i].LineGuid];
			}
		}

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