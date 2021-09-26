using SimpleDialogSystem.Runtime.Data.ScriptableObjects;
using UnityEngine;

namespace SimpleDialogSystem.Runtime.Scripts
{
	public class Game : MonoBehaviour
	{
		private DialogFactory _factory = new DialogFactory();

		public DialogData DialogData;
		private Dialog dialog;

		private void Awake()
		{
			dialog = _factory.Create(DialogData);
			dialog.DialogStarted += line =>
			                        {
				                        Debug.Log(line.GetMessage());
				                        Debug.Log("Responses:");

				                        for (int i = 0; i < line.Responses.Count; i++)
				                        {
					                        Debug.Log(i + ": " + line.Responses[i].GetMessage());
				                        }
			                        };

			dialog.DialogEnded += () => { Debug.Log("Koniec!"); };
			dialog.LineReady += line =>
			                    {
				                    Debug.Log(line.GetMessage());
				                    Debug.Log("Responses:");

				                    for (int i = 0; i < line.Responses.Count; i++)
				                    {
					                    Debug.Log(i + ": " + line.Responses[i].GetMessage());
				                    }
			                    };
			dialog.Start();
			
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				dialog.SelectResponse(0);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				dialog.SelectResponse(1);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				dialog.SelectResponse(2);
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				dialog.SelectResponse(3);
			}
		}
	}
}