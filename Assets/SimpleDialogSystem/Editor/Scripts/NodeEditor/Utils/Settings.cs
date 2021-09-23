using UnityEngine;

namespace SimpleDialogSystem.Editor.Scripts.NodeEditor.Utils
{
	public class Settings
	{
		public static Color NodeBackgroundColor = new Color(0.38f, 0.38f, 0.38f);
		public static Color BackgroundColor = new Color(0.25f, 0.25f, 0.25f);
		public static Color TitleColor = Color.grey;
		public static Color PortColor = new Color(1f, 0.66f, 0.27f);
		public static float MinimalCurveDrawDistance = 5f;
		public static int LeftMouseButton = 0;
		public static int RightMouseButton = 1;

		static Settings()
		{
			Texture2D background = new Texture2D(1, 1) {wrapMode = TextureWrapMode.Repeat};
			background.SetPixel(0, 0, BackgroundColor);
			background.Apply();
			BackgroundStyle = new GUIStyle {normal = new GUIStyleState {background = background}};

			Texture2D backgroundTexture = new Texture2D(1, 1) {wrapMode = TextureWrapMode.Repeat};
			backgroundTexture.SetPixel(0, 0, NodeBackgroundColor);
			backgroundTexture.Apply();
			NodeBackgroundStyle = new GUIStyle {normal = new GUIStyleState {background = backgroundTexture}};

			Texture2D blackTexture = new Texture2D(1, 1) {wrapMode = TextureWrapMode.Repeat};
			blackTexture.SetPixel(0, 0, TitleColor);
			blackTexture.Apply();
			TitleStyle = new GUIStyle {normal = new GUIStyleState {background = blackTexture}, alignment = TextAnchor.MiddleCenter};

			Texture2D portTexture = new Texture2D(1, 1) {wrapMode = TextureWrapMode.Repeat};
			portTexture.SetPixel(0, 0, PortColor);
			portTexture.Apply();

			PortStyle = new GUIStyle {normal = new GUIStyleState {background = portTexture}};
		}

		public static float TitleHeight => 20;
		public static float PortHeight => 14;
		public static GUIStyle NodeBackgroundStyle { get; }
		public static GUIStyle BackgroundStyle { get; }
		public static GUIStyle TitleStyle { get; }
		public static GUIStyle PortStyle { get; }
	}
}