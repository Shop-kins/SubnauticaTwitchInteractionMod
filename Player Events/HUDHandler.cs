using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TwitchInteraction.Player_Events
{
    public class HUDHandler
    {

		public static bool recursive = true;

		private static void ProcessComponent(Behaviour b, HideForScreenshots parent, bool enable)
		{
			if (b != null && !enable)
			{
				b.enabled = false;
			} else if (b != null && enable)
            {
				b.enabled = true;
            }
		}

		private static void ProcessComponent(Renderer r, HideForScreenshots parent, bool enable)
		{
			if (r != null && !enable)
			{
				r.enabled = false;
			} else if (r != null && enable)
            {
				r.enabled = true;
            }
		}

		private static void HideInternal(GameObject obj, HideForScreenshots parent)
		{
			obj.BroadcastMessage("HideForScreenshots", SendMessageOptions.DontRequireReceiver);

			// Only disable things for things that are not in the path of our custom objects
			if (!(obj.name.Contains("Twitch") || obj.name.Contains("OverlayCanvas") || obj.name.Contains("Overlays") || obj.name.Contains("OutOfOxygen")))
			{
				ProcessComponent(obj.GetComponent<Text>(), parent, false);
				ProcessComponent(obj.GetComponent<Image>(), parent, false);
				ProcessComponent(obj.GetComponent<Renderer>(), parent, false);
			} else
            {
				// Enable the canvases in the path of our custom objects
				List<Canvas> canvasList = new List<Canvas>(obj.GetComponentsInChildren<Canvas>());
				canvasList.AddRange(obj.GetComponents<Canvas>());
				foreach (Canvas canvas in canvasList)
                {
					if (!(canvas.sortingLayerName == "DepthClear"))
                    {
						canvas.enabled = true;
                    }
                }
            }

			if (recursive)
			{
				Transform transform = obj.transform;
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					HideInternal(child.gameObject, parent);
				}
			}
		}

		private static void UnhideInternal(GameObject obj, HideForScreenshots parent)
		{
			obj.BroadcastMessage("UnhideForScreenshots", SendMessageOptions.DontRequireReceiver);

			ProcessComponent(obj.GetComponent<Text>(), parent, true);
			ProcessComponent(obj.GetComponent<Image>(), parent, true);
			ProcessComponent(obj.GetComponent<Renderer>(), parent, true);

			if (recursive)
			{
				Transform transform = obj.transform;
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					UnhideInternal(child.gameObject, parent);
				}
			}
		}

		public static void Hide(HideForScreenshots.HideType hide)
		{
			foreach (HideForScreenshots hideForScreenshots in UnityEngine.Object.FindObjectsOfType<HideForScreenshots>())
			{
				if ((hideForScreenshots.type & hide) != HideForScreenshots.HideType.None)
				{
					HideInternal(hideForScreenshots.gameObject, hideForScreenshots);
				}
				else
				{
					UnhideInternal(hideForScreenshots.gameObject, hideForScreenshots);
				}
			}
		}

	}
}
