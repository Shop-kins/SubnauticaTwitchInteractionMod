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
        }
    }
}
