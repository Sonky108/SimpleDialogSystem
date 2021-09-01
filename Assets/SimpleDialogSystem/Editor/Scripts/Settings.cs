using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts
{
	public static class Settings
	{
		public static Color BackgroundColor = new Color(0.38f, 0.38f, 0.38f);

		static Settings()
		{
			Texture2D backgroundTexture = new Texture2D(1, 1) {wrapMode = TextureWrapMode.Repeat};
			backgroundTexture.SetPixel(0, 0, BackgroundColor);
			backgroundTexture.Apply();
			BackgroundStyle = new GUIStyle {normal = new GUIStyleState {background = backgroundTexture}};

			Texture2D blackTexture = new Texture2D(1, 1) {wrapMode = TextureWrapMode.Repeat};
			blackTexture.SetPixel(0, 0, TitleColor);
			blackTexture.Apply();

			TitleStyle = new GUIStyle {normal = new GUIStyleState {background = blackTexture}};
		}

		public static Color TitleColor = Color.grey;
		public static float TitleHeight => 20;
		public static GUIStyle BackgroundStyle { get; }
		public static GUIStyle TitleStyle { get; }
		public static int LeftMouseButton = 0;
		public static int RightMouseButton = 1;

		public static class LineNode
		{
			public static int Width = 100;
			public static int Height = 100;
		}
		
		public static class ResponseNode
		{
			public static int Width = 200;
			public static int Height = 100;
		}
	}
}