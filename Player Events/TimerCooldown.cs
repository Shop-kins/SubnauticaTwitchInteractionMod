using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TwitchInteraction.Player_Events;
using TwitchInteraction.Player_Events.Models;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

namespace TwitchInteraction
{

    class CustomText
    {

        private GameObject textObject;
        private TextMeshProUGUI textText;
        private uGUI_TextFade textFade;
        private ContentSizeFitter textFitter;

        private GameObject progressObject;
        private uGUI_ItemIcon progressIcon;
        private Mesh progressMesh;
        private Texture2D progressTexture;

        private float duration;
        private float startTime;

        private float yOffset;

        private bool isFadingOut;
        private bool useGlobalTextWidth;

        private Tuple<string, string, TimedEventInfo> timedEvent;
        private Tuple<string, string, EventInfo> normalEvent;
        private bool hasTimedEvent = false;


        public CustomText(string text, float duration, int yOffset = 0, bool showProgress = false, bool useGlobalTextWidth = true)
        {
            this.duration = duration;
            this.yOffset = yOffset;
            startTime = Time.time;
            isFadingOut = false;
            this.useGlobalTextWidth = useGlobalTextWidth;

            textObject = new GameObject("TwitchInteractionTimerCooldown");
            textText = textObject.AddComponent<TextMeshProUGUI>();
            textFade = textObject.AddComponent<uGUI_TextFade>();
            textFitter = textObject.AddComponent<ContentSizeFitter>();

            textFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            textFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            textText.font = uGUI.main.intro.mainText.text.font;
            textText.fontSize = 16;
            textText.fontStyle = uGUI.main.intro.mainText.text.fontStyle;
            textText.alignment = TextAlignmentOptions.Left;
            textText.color = uGUI.main.intro.mainText.text.color;
            textText.material = uGUI.main.intro.mainText.text.material;

            if (showProgress)
            {
                progressObject = new GameObject("TwitchInteractionTimerCooldownIcon");
                progressIcon = progressObject.AddComponent<uGUI_ItemIcon>();

                progressTexture = new Texture2D(TimerCooldown.ActualTimerTextHeight(), TimerCooldown.ActualTimerTextHeight());

                for (int x = 0; x < progressTexture.width; x++)
                {
                    for (int y = 0; y < progressTexture.height; y++)
                    {
                        float distanceFromCenter = (float)Math.Sqrt((x - progressTexture.width / 2) * (x - progressTexture.width / 2) + (y - progressTexture.height / 2) * (y - progressTexture.height / 2));

                        Color c = new Color(0, 0, 0, 0);

                        if (distanceFromCenter <= progressTexture.height)
                        {
                            c = Color.white;
                        }

                        progressTexture.SetPixel(x, y, c);
                    }
                }

                /*Atlas.Sprite progressSprite = new Atlas.Sprite(progressTexture);
                progressIcon.SetForegroundSprite(progressSprite);
                progressIcon.SetForegroundAlpha(1);
                progressIcon.SetForegroundColors(Color.white, Color.white, Color.white);
                progressIcon.SetForegroundAlpha(1, 1, 1);
                progressIcon.SetForegroundChroma(1f);
                progressIcon.SetBackgroundAlpha(0);
                progressIcon.SetBackgroundColors(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0), new Color(0, 0, 0, 0));
                progressIcon.SetForegroundAlpha(0);
                progressIcon.SetForegroundColors(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0), new Color(0, 0, 0, 0));
                progressIcon.SetAlpha(0, 0, 0);
                progressIcon.foreground.canvasRenderer.SetColor(Color.white);
                progressIcon.foreground.color = Color.white;*/
            }

            // Do this so it also shows over black screens
            Graphic g = uGUI.main.overlays.overlays[0].graphic;
            textObject.transform.SetParent(g.transform, false);
            textText.canvas.overrideSorting = true;
            textObject.layer = 1;

            if (showProgress)
            {
                progressObject.transform.SetParent(g.transform, false);
                progressIcon.canvas.overrideSorting = true;
                progressObject.layer = 1;
                progressIcon.SetActive(true);
            }


            SetText(text);

            Update();
        }

        public void SetSize(int textSize)
        {
            textText.fontSize = textSize;
        }

        public void Update(int yOffset = 0)
        {
            this.yOffset = yOffset;
            AlignText();

            float elapsedTime = Time.time - startTime;
            float remainingSeconds = duration - elapsedTime;
            float percentage = elapsedTime / duration;

            if (remainingSeconds <= 1 && !isFadingOut)
            {
                textFade.FadeOut(remainingSeconds, null);
                isFadingOut = true;
            }

            // TODO progress circle (as in fabricator) => thats what percentage is for
            if (progressIcon != null)
            {
                progressIcon.SetProgress(percentage);

                // Make a circular mesh
                progressMesh = new Mesh();

                // Verticies
                List<Vector3> verticiesList = new List<Vector3> { };
                float circleX;
                float circleY;
                int n = 64;
                int maxN = (int)(n * percentage);

                verticiesList.Add(new Vector3(0, 0, 0));
                for (int i = 0; i < n; i++)
                {
                    circleX = progressTexture.width / 2 * Mathf.Sin((2 * Mathf.PI * i) / n);
                    circleY = progressTexture.height / 2 * Mathf.Cos((2 * Mathf.PI * i) / n);
                    verticiesList.Add(new Vector3(circleX, circleY, 0f));
                    if (i > maxN + 1)
                    {
                        break;
                    }
                }
                Vector3[] verticies = verticiesList.ToArray();

                // Triangles
                List<int> trianglesList = new List<int> { };
                for (int i = 0; i < (maxN - 2); i++)
                {
                    trianglesList.Add(0);
                    trianglesList.Add(i + 1);
                    trianglesList.Add(i + 2);
                }
                int[] triangles = trianglesList.ToArray();

                // Normals
                List<Vector3> normalsList = new List<Vector3> { };
                for (int i = 0; i < verticies.Length; i++)
                {
                    normalsList.Add(-Vector3.forward);
                }
                Vector3[] normals = normalsList.ToArray();

                // Set mesh
                progressMesh.vertices = verticies;
                progressMesh.triangles = triangles;
                progressMesh.normals = normals;
                progressIcon.canvasRenderer.SetMesh(progressMesh);

                progressIcon.canvasRenderer.SetColor(textFade.text.color);
            }
        }

        public void Destroy()
        {
            GameObject.Destroy(textObject);
            GameObject.Destroy(progressObject);
        }

        public bool IsFinished()
        {
            return Time.time - startTime >= duration;
        }

        public void SetText(string text)
        {
            SetText(text, 0);
        }

        public void SetText(string text, float seconds)
        {
            textFade.SetText(text.Replace(" [Integration]", ""), false);
            //AlignText();
            textFade.SetState(true);
            textObject.SetActive(true);
            if (seconds > 0f && !isFadingOut)
            {
                textFade.FadeOut(seconds, null);
                isFadingOut = true;
            }
        }

        public float getTextWidth()
        {
            return textText.preferredWidth;
        }

        public void SetTimedEvent(Tuple<string, string, TimedEventInfo> timedEvent)
        {
            this.timedEvent = timedEvent;
            hasTimedEvent = true;
        }

        public Tuple<string, string, TimedEventInfo> GetTimedEvent()
        {
            return timedEvent;
        }

        public void SetEvent(Tuple<string, string, EventInfo> normalEvent)
        {
            this.normalEvent = normalEvent;
        }

        public Tuple<string, string, EventInfo> GetEvent()
        {
            return normalEvent;
        }

        public bool HasTimedEvent()
        {
            return hasTimedEvent;
        }

        private void AlignText()
        {
            float scaleX = (1920f / Screen.width);
            float scaleY = (1920f / Screen.width);

            //float width = textText.preferredWidth;
            float width = getTextWidth();

            float widestText = useGlobalTextWidth ? TimerCooldown.widestText : getTextWidth();

            float x = Screen.width / 2 - (Screen.width / 1920f * widestText) - TimerCooldown.ActualTimerHeadingHeight();
            float y = -Screen.height / 2 - yOffset + TimerCooldown.ActualTimerHeadingHeight();

            x *= scaleX;
            y *= scaleY;

            float displayX;
            switch (textText.alignment)
            {
                case TextAlignmentOptions.TopLeft:
                case TextAlignmentOptions.Left:
                case TextAlignmentOptions.BottomLeft:
                    displayX = x + width / 2f;
                    goto IL_9A;
                case TextAlignmentOptions.TopRight:
                case TextAlignmentOptions.Right:
                case TextAlignmentOptions.BottomRight:
                    displayX = x - width / 2f;
                    goto IL_9A;
            }
            displayX = x;
        IL_9A:
            float displayY = y;
            textObject.transform.localPosition = new Vector3(displayX, displayY, 0f);

            if (progressIcon != null)
            {
                progressIcon.rectTransform.localPosition = new Vector3(displayX - TimerCooldown.ActualTimerHeadingHeight() / 2 - width / 2f, displayY, 0f);
            }
        }

    }

    public class TimerCooldown
    {

        private static CustomText activeEffectsText, queueText, cooldownText;

        private static List<CustomText> customTimerTexts;

        public static int pixelTimerTextHeight = 20;
        public static int pixelTimerHeadingHeight = 32;

        private static List<KeyValuePair<string, CustomText>> actionQueueTexts;
        private static List<KeyValuePair<string, CustomText>> cooldownTexts;
        // Key: Event name, Value: Pair<User, BitCost>
        private static ConcurrentQueue<KeyValuePair<string, KeyValuePair<string, int>>> newEventsList; // Allow for events from threads other than the main Unity UI thread.

        private static ConcurrentQueue<string> redemptionTexts;
        private static CustomText currentRedemptionText;

        private static bool initialised = false;

        public static float widestText = 0;

        public static int ActualTimerTextHeight()
        {
            return (int)(pixelTimerTextHeight * (Screen.width / 1920f));
        }

        public static int ActualTimerHeadingHeight()
        {
            return (int)(pixelTimerHeadingHeight * (Screen.width / 1920f));
        }

        public static void AddCooldown(string text, float duration, Tuple<string, string, EventInfo> eventInfo)
        {
            if (!initialised)
            {
                Initialise();
            }

            CustomText cooldownText = new CustomText(text, duration, 0, true);
            cooldownText.SetEvent(eventInfo);
            customTimerTexts.Add(cooldownText);
        }

        public static void AddCooldown(string text, Tuple<string, string, TimedEventInfo> eventInfo)
        {
            if (!initialised)
            {
                Initialise();
            }

            CustomText cooldownText = new CustomText(text, eventInfo.Item3.TimerLength, 0, true);
            cooldownText.SetTimedEvent(eventInfo);
            EventLookup.RunningEventIDs.Add(text);
            customTimerTexts.Add(cooldownText);
        }

        public static void Update()
        {
            float newWidestText = 0;
            if (!initialised)
            {
                Initialise();
            }

            // First step is to add actions to the queue based on newly added events.
            CreateNewText();

            try
            {
                activeEffectsText.Update(-((actionQueueTexts.Count + cooldownTexts.Count + customTimerTexts.Count - 1) * ActualTimerTextHeight()) - 5 * ActualTimerHeadingHeight());
                cooldownText.Update(-((actionQueueTexts.Count + cooldownTexts.Count - 1) * ActualTimerTextHeight()) - 3 * ActualTimerHeadingHeight());
                queueText.Update(-((actionQueueTexts.Count - 1) * ActualTimerTextHeight() + ActualTimerHeadingHeight()));

                if (activeEffectsText.getTextWidth() > newWidestText)
                {
                    newWidestText = activeEffectsText.getTextWidth();
                }
                if (cooldownText.getTextWidth() > newWidestText)
                {
                    newWidestText = cooldownText.getTextWidth();
                }
                if (queueText.getTextWidth() > newWidestText)
                {
                    newWidestText = queueText.getTextWidth();
                }
            }
            catch (Exception)
            {
                Initialise();
                activeEffectsText.Update(-((actionQueueTexts.Count + cooldownTexts.Count + customTimerTexts.Count - 1) * ActualTimerTextHeight()) - 5 * ActualTimerHeadingHeight());
                cooldownText.Update(-((actionQueueTexts.Count + cooldownTexts.Count - 1) * ActualTimerTextHeight()) - 3 * ActualTimerHeadingHeight());
                queueText.Update(-((actionQueueTexts.Count - 1) * ActualTimerTextHeight() + ActualTimerHeadingHeight()));
            }

            // Redemption messages
            if (currentRedemptionText != null)
            {
                currentRedemptionText.Update(-((actionQueueTexts.Count + cooldownTexts.Count + customTimerTexts.Count) * ActualTimerTextHeight()) - 6 * ActualTimerHeadingHeight());

                if (currentRedemptionText.IsFinished())
                {
                    currentRedemptionText.Destroy();
                    currentRedemptionText = null;
                    
                }
            }
            if (currentRedemptionText == null)
            {
                string newRedemptionText;
                if (redemptionTexts.TryDequeue(out newRedemptionText))
                {
                    currentRedemptionText = new CustomText(newRedemptionText, 5, 0, false, false);

                    // Only do this in the main thread, to hopefully prevent errors
                    if (MainPatcher.secrets.saveRedemptionMessages)
                    {
                        string filePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "redemptions.txt");
                        using (StreamWriter sw = File.AppendText(filePath))
                        {
                            sw.WriteLine(newRedemptionText);
                        }
                    }
                }
            }

            // Active effects
            for (int i = 0; i < customTimerTexts.Count; i++)
            {
                customTimerTexts[i].Update(-((actionQueueTexts.Count + cooldownTexts.Count + i) * ActualTimerTextHeight()) - 4 * ActualTimerHeadingHeight());

                if (customTimerTexts[i].getTextWidth() > newWidestText)
                {
                    newWidestText = customTimerTexts[i].getTextWidth();
                }

                if (customTimerTexts[i].IsFinished())
                {
                    if (customTimerTexts[i].HasTimedEvent())
                    {
                        Tuple<string, string, TimedEventInfo> timedEvent = customTimerTexts[i].GetTimedEvent();
                        EventLookup.RunningEventIDs.Remove(timedEvent.Item1);
                        EventLookup.TimedActionsQueue.Add(timedEvent);
                        EventLookup.Cooldowns.Add(timedEvent.Item1, Time.time);
                        AddCooldownText(timedEvent.Item1, timedEvent.Item3.CooldownSeconds, new Tuple<string, string, EventInfo>(timedEvent.Item1, timedEvent.Item2, timedEvent.Item3));
                    }

                    customTimerTexts[i].Destroy();
                    customTimerTexts.RemoveAt(i);
                    i--;
                }
            }

            // Queue
            int j = 0;
            foreach (KeyValuePair<string, CustomText> actionQueueText in actionQueueTexts)
            {
                actionQueueText.Value.Update(-(j * ActualTimerTextHeight()));
                j++;

                if (actionQueueText.Value.getTextWidth() > newWidestText)
                {
                    newWidestText = actionQueueText.Value.getTextWidth();
                }
            }

            // Cooldown
            List<string> finishedCooldowns = new List<string>();

            int k = 0;
            foreach (KeyValuePair<string, CustomText> cooldownText in cooldownTexts)
            {
                cooldownText.Value.Update(-((actionQueueTexts.Count + k) * ActualTimerTextHeight()) - 2 * ActualTimerHeadingHeight());
                k++;

                Tuple<string, string, EventInfo> eventInfo = cooldownText.Value.GetEvent();
                float currentCooldownDuration = Time.time - EventLookup.Cooldowns[eventInfo.Item1];
                if (currentCooldownDuration >= eventInfo.Item3.CooldownSeconds)
                {
                    finishedCooldowns.Add(eventInfo.Item1);
                }

                if (cooldownText.Value.getTextWidth() > newWidestText)
                {
                    newWidestText = cooldownText.Value.getTextWidth();
                }
            }

            foreach (string finishedCooldown in finishedCooldowns)
            {
                RemoveCooldownText(finishedCooldown);
                EventLookup.Cooldowns.Remove(finishedCooldown);
            }

            widestText = newWidestText;
        }

        private static void CreateNewText()
        {
            KeyValuePair<string, KeyValuePair<string, int>> eventInfo;
            while(newEventsList.TryDequeue(out eventInfo))
            {
                AddQueueText(eventInfo.Key, eventInfo.Value.Key, eventInfo.Value.Value);
            }
        }

        /**
         * This is necessary with CC because events come in off of the main thread, and we cannot create UI elements.
         */
        public static void AddNewEventText(string text, string user, int bits = -1)
        {
            KeyValuePair<string, int> userBitPair = new KeyValuePair<string, int>(user, bits);
            newEventsList.Enqueue(new KeyValuePair<string, KeyValuePair<string, int>>(text, userBitPair));
        }

        public static void AddCooldownText(string text, float duration, Tuple<string, string, EventInfo> eventInfo)
        {
            CustomText cooldownText = new CustomText(text, duration, 0, true);
            cooldownText.SetEvent(eventInfo);
            cooldownTexts.Add(new KeyValuePair<string, CustomText>(text, cooldownText));
        }

        public static void AddQueueText(string text, string User, int Bits = -1)
        {
            CustomText cooldownText = new CustomText(text, float.MaxValue);
            actionQueueTexts.Add(new KeyValuePair<string, CustomText>(text, cooldownText));

            if (MainPatcher.secrets.showRedemptionMessages)
            {
                string redeptionMessage = "";
                if (Bits == -1)
                {
                    redeptionMessage = User + " has redeemed " + text + "!";
                }
                else
                {
                    redeptionMessage = User + " has redeemed " + text + " using " + Bits + " bits!";
                }
                redemptionTexts.Enqueue(redeptionMessage);
            }
        }

        public static void RemoveCooldownText(string text)
        {

            for (int i = 0; i < cooldownTexts.Count; i++)
            {
                KeyValuePair<string, CustomText> keyValuePair = cooldownTexts[i];
                if (text.Equals(keyValuePair.Key))
                {
                    // Only remove once, because it should never be in here multiple times
                    keyValuePair.Value.Destroy();
                    cooldownTexts.RemoveAt(i);
                    break;
                }
            }
        }

        public static void RemoveQueueText(string text)
        {

            for (int i = 0; i < actionQueueTexts.Count; i++)
            {
                KeyValuePair<string, CustomText> keyValuePair = actionQueueTexts[i];
                if (text.Equals(keyValuePair.Key))
                {
                    // Only remove once, because if its in here multiple times, then multiple people have submitted it
                    keyValuePair.Value.Destroy();
                    actionQueueTexts.RemoveAt(i);
                    break;
                }
            }
        }

        public static Boolean IsInitialised()
        {
            return initialised;
        }

        public static void Initialise()
        {
            customTimerTexts = new List<CustomText>();
            actionQueueTexts = new List<KeyValuePair<string, CustomText>>();
            cooldownTexts = new List<KeyValuePair<string, CustomText>>();
            activeEffectsText = new CustomText("Active", float.MaxValue);
            activeEffectsText.SetSize(24);
            queueText = new CustomText("Queue", float.MaxValue);
            queueText.SetSize(24);
            cooldownText = new CustomText("Cooldowns", float.MaxValue);
            cooldownText.SetSize(24);

            newEventsList = new ConcurrentQueue<KeyValuePair<string, KeyValuePair<string, int>>>();
            redemptionTexts = new ConcurrentQueue<string>();

            initialised = true;

            EventLookup.ActionQueue = new List<Tuple<string, string, EventInfo>>();
            EventLookup.Cooldowns = new Dictionary<string, float>();
            EventLookup.RunningEventIDs = new List<string>();
            EventLookup.TimedActionsQueue = new List<Tuple<string, string, TimedEventInfo>>();
        }

    }
}
