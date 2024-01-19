using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityESP
{
	class Hacks : MonoBehaviour
	{
		private readonly Dictionary<Type, bool> TypesToTrack = new Dictionary<Type, bool>();

		public void ChangeState<T>(bool value) where T : MonoBehaviour
		{
			Type type = typeof(T);
			if (TypesToTrack.ContainsKey(type))
				TypesToTrack[typeof(T)] = value;
			else
				TypesToTrack.Add(type, value);
		}


		public void OnGUI()
		{
			foreach (Type type in TypesToTrack.Keys)
			{
				if (!TypesToTrack[type])
					continue;


				foreach (var objectToTrack in FindObjectsOfType(type) as MonoBehaviour[])
				{
					if (objectToTrack == null)
						continue;

					Vector3 enemyPosition = objectToTrack.transform.position;
					Vector3 sceenPosition = Camera.main.WorldToScreenPoint(enemyPosition);

					Bounds bounds = objectToTrack.GetComponent<Collider>().bounds;

					if (sceenPosition.z > 0f)
					{
						DrawBoxESP(sceenPosition, Color.red, bounds);
					}
				}
			}
		}

		public void DrawBoxESP(Vector3 position, Color color, Bounds? bounds)
		{
			Render.DrawLine(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)), new Vector2(position.x, (float)Screen.height - position.y), color, 3f);

			Render.DrawBounds(bounds, Color.green, 2f);
		}
	}
}
