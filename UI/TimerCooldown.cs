using UnityEngine;
using UnityEngine.UI;

namespace TwitchInteraction.UI
{
    public class TimerCooldown
    {

        private static GameObject textObject;
        private static Text textText;
        private static uGUI_TextFade textFade;
        private static ContentSizeFitter textFitter;

        private static int x, y;

        public static void ShowMessage(string message)
        {
            ShowMessage(message, 0);
        }

        public static void ShowMessage(string message, float seconds)
        {
            if (textObject == null)
            {
                Initialize();
            }
            textFade.SetText(message, false);
            AlignText();
            textFade.SetState(true);
            textObject.SetActive(true);
            if (seconds > 0f)
            {
                textFade.FadeOut(seconds, null);
            }
        }

        public static void Initialize()
        {
            textObject = new GameObject("TwichInteractionTimerCooldown");
            textText = textObject.AddComponent<Text>();
            textFade = textObject.AddComponent<uGUI_TextFade>();
            textFitter = textObject.AddComponent<ContentSizeFitter>();

            textFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            textFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            textText.font = uGUI.main.intro.mainText.text.font;
            textText.fontSize = uGUI.main.intro.mainText.text.fontSize;
            textText.fontStyle = uGUI.main.intro.mainText.text.fontStyle;
            textText.alignment = uGUI.main.intro.mainText.text.alignment;
            textText.color = uGUI.main.intro.mainText.text.color;
            textText.material = uGUI.main.intro.mainText.text.material;

            // Do this so it also shows over black screens
            Graphic g = uGUI.main.overlays.overlays[0].graphic;
            textObject.transform.SetParent(g.transform, false);
            textText.canvas.overrideSorting = true;
            textObject.layer = 1;

            AlignText();
        }

        private static void AlignText()
        {
            bool flag = textObject == null;
            if (!flag)
            {
                float width = textText.preferredWidth;
                float height = textText.preferredHeight;
                float displayX;
                switch (textText.alignment)
                {
                    case TextAnchor.UpperLeft:
                    case TextAnchor.MiddleLeft:
                    case TextAnchor.LowerLeft:
                        displayX = x + width / 2f;
                        goto IL_9A;
                    case TextAnchor.UpperRight:
                    case TextAnchor.MiddleRight:
                    case TextAnchor.LowerRight:
                        displayX = x - width / 2f;
                        goto IL_9A;
                }
                displayX = x;
            IL_9A:
                TextAnchor alignment = textText.alignment;
                TextAnchor textAnchor = alignment;
                float displayY;
                if (textAnchor > TextAnchor.UpperRight)
                {
                    if (textAnchor - TextAnchor.LowerLeft > 2)
                    {
                        displayY = y;
                    }
                    else
                    {
                        displayY = y + height / 2f;
                    }
                }
                else
                {
                    displayY = y - height / 2f;
                }
                textObject.transform.localPosition = new Vector3(displayX, displayY, 0f);
            }
        }

    }
}
