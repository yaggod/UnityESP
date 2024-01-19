using UnityEngine;

namespace UnityESP
{
	public class Render : MonoBehaviour
	{
		public static GUIStyle StringStyle { get; set; } = new GUIStyle(GUI.skin.label);

		public static Color Color
		{
			get { return GUI.color; }
			set { GUI.color = value; }
		}

		public static void DrawString(Vector2 position, string label, bool centered = true)
		{
			var content = new GUIContent(label);
			var size = StringStyle.CalcSize(content);
			var upperLeft = centered ? position - size / 2f : position;
			GUI.Label(new Rect(upperLeft, size), content);
		}

		public static Texture2D lineTex;
		public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
		{
			Matrix4x4 matrix = GUI.matrix;
			if (!lineTex)
				lineTex = new Texture2D(1, 1);

			Color color2 = GUI.color;
			GUI.color = color;
			float num = Vector3.Angle(pointB - pointA, Vector2.right);

			if (pointA.y > pointB.y)
				num = -num;

			GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
			GUIUtility.RotateAroundPivot(num, pointA);
			GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), lineTex);
			GUI.matrix = matrix;
			GUI.color = color2;
		}

		public static void CastToLocalAndDraw(Vector3 pointA, Vector3 pointB, Color color, float width)
		{
			Vector2 from = Camera.main.WorldToScreenPoint(pointA);
			Vector2 to = Camera.main.WorldToScreenPoint(pointB);

			from.y = Screen.height - from.y;
			to.y = Screen.height - to.y;


			DrawLine(from, to, color, width);
		}


		// Is there better way?
		public static void DrawBounds(Bounds? boundsNullable, Color color, float width)
		{
			Bounds bounds = boundsNullable.Value;
			if (boundsNullable == null)
				return;

			Vector3[] points = new Vector3[8];
			points[0] = bounds.max;
			points[4] = bounds.min;

			points[1] = new Vector3(points[0].x, points[0].y, points[4].z);
			points[2] = new Vector3(points[0].x, points[4].y, points[0].z);
			points[3] = new Vector3(points[0].x, points[4].y, points[4].z);

			points[5] = new Vector3(points[4].x, points[4].y, points[0].z);
			points[6] = new Vector3(points[4].x, points[0].y, points[4].z);
			points[7] = new Vector3(points[4].x, points[0].y, points[0].z);

			CastToLocalAndDraw(points[0], points[1], color, width);
			CastToLocalAndDraw(points[0], points[2], color, width);
			CastToLocalAndDraw(points[1], points[3], color, width);
			CastToLocalAndDraw(points[2], points[3], color, width);

			CastToLocalAndDraw(points[4], points[5], color, width);
			CastToLocalAndDraw(points[4], points[6], color, width);
			CastToLocalAndDraw(points[5], points[7], color, width);
			CastToLocalAndDraw(points[6], points[7], color, width);

			CastToLocalAndDraw(points[0], points[7], color, width);
			CastToLocalAndDraw(points[1], points[6], color, width);
			CastToLocalAndDraw(points[3], points[4], color, width);
			CastToLocalAndDraw(points[2], points[5], color, width);
		}
	}
}
