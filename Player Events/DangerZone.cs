using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TwitchInteraction.Player_Events
{
    //Add functions to the EventLookup.cs class
    class DangerZone //This contains events that I do not wish to happen frequently so will be behind paywalls or high channel point walls
    {
        public static void KillPlayer()
        {
            Player.main.liveMixin.TakeDamage(666);
        }

        public static void EnableGun()
        {
            StoryGoalCustomEventHandler.main.gunDisabled = false;

        }

        public static void summonReaper()
        {
            DevConsole.SendConsoleCommand("spawn reaperleviathan");
        }
        public static void TeleportPlayer()
        {
            bool isBad;
            Vector3 spawnPosition;

            do
            {
                spawnPosition = new Vector3(Random.Range(-1250f, 1250f), Random.Range(-10, -1300), Random.Range(-1250f, 1250f));

                if (spawnPosition.x > 543 && spawnPosition.x < 1724 && spawnPosition.z > -574 && spawnPosition.z < 400) // near the aurora, bad spawn
                    isBad = true;
                else if (spawnPosition.x > 231 && spawnPosition.x < 437 && spawnPosition.y >= -10 && spawnPosition.y < -100 && spawnPosition.z > 750 && spawnPosition.z < 1154) // near the QEP island, bad spawn
                    isBad = true;
                else if (spawnPosition.x > -917 && spawnPosition.x < -606 && spawnPosition.y >= -10 && spawnPosition.y < -100 && spawnPosition.z > -1224 && spawnPosition.z < -897) // near the floating island, bad spawn
                    isBad = true;
                else
                    isBad = false;
            } while (isBad);

            Player.main.transform.position = spawnPosition;
        }

<<<<<<< Updated upstream
        public static void TeleportLifepod()
        {
            bool isBad;
            Vector3 spawnPosition;

            do
            {
                spawnPosition = new Vector3(Random.Range(-1250f, 1250f), 0, Random.Range(-1250f, 1250f));

                if (spawnPosition.x > 543 && spawnPosition.x < 1724 && spawnPosition.z > -574 && spawnPosition.z < 400) // near the aurora, bad spawn
                    isBad = true;
                else if (spawnPosition.x > 231 && spawnPosition.x < 437 && spawnPosition.y >= -10 && spawnPosition.y < -100 && spawnPosition.z > 750 && spawnPosition.z < 1154) // near the QEP island, bad spawn
                    isBad = true;
                else if (spawnPosition.x > -917 && spawnPosition.x < -606 && spawnPosition.y >= -10 && spawnPosition.y < -100 && spawnPosition.z > -1224 && spawnPosition.z < -897) // near the floating island, bad spawn
                    isBad = true;
                else if (RandomStart.main.IsStartPointValid(spawnPosition, false))
                    isBad = true;
                else
                    isBad = false;
            } while (isBad);

            EscapePod.main.transform.position = spawnPosition;
            EscapePod.main.anchorPosition = spawnPosition;
=======
        public static void LifePodWarpToTheDeepEnd()
        {
            Vector3 newHome = GetRandomLifePodPosition();
            EscapePod.main.transform.position = newHome;
            EscapePod.main.anchorPosition = newHome;
        }

        private static Vector3 GetRandomLifePodPosition()
        {
            for (int index = 0; index < 1000; ++index)
            {
                Vector3 point = new Vector3(Random.Range(-2048f, 2048f), 0.0f, Random.Range(-2048f, 2048f));
                if (IsInTheDeepEnd(point))
                    return point;
            }
            Debug.LogWarning((object)"Could not find valid position. Using (0,0,0) instead.");
            return Vector3.zero;
        }

        private static Boolean IsInTheDeepEnd(Vector3 point)
        {
            //Texture2D validPosition = Resources.Load<Texture2D>("ValidConstructorPlacementMask");
            var validPosition = FindMap();
            float num1 = Mathf.Clamp01((float)(((double)point.x + 2048.0) / 4096.0));
            double num2 = (double)Mathf.Clamp01((float)(((double)point.z + 2048.0) / 4096.0));
            int x = (int)((double)num1 * (double)validPosition.width);
            double height = (double)validPosition.height;
            int y = (int)(num2 * height);
            return (double)validPosition.GetPixel(x, y).g > 0.5;
        }

        private static Texture2D FindMap()
        {
            AssetBundleManager.
            var assets = AssetBundleManager.FindObjectsOfType<Texture2D>();
            foreach (Texture2D asset in assets)
            {
                Debug.LogWarning((object)"Found asset: " + asset.name);
                if (asset.name == "ValidConstructorPlacementMask")
                {
                    return asset;
                }
            }
            Debug.LogWarning((object)"Unable to find the texture map in the asset bundle manager :(");
            return null;
>>>>>>> Stashed changes
        }
    }
}
