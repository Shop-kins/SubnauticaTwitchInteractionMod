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

        public static void TeleportPlayerOOB()
        {
            // First, pick a depth at random
            var depth = Random.Range(-20, -1500);
            var xPos = 0f;
            var zPos = 0f;

            // Our goal here it to be close to being in bounds if possible
            if (depth > -200f)
            {
                // Close to the surface, place in the shallows and surrounding
                xPos = Random.Range(-900f, 540f); // Avoid the aurora
                zPos = Random.Range(-550f, 750f); // Avoid Mountain Island
            }
            else if (depth > -400f)
            {
                // Slightly deeper, include the edges of the map
                // Likely to be OOB if under the shallows
                xPos = Random.Range(-1500, 1500);
                zPos = Random.Range(-1500, 1500);
            }
            else if (depth > -1000)
            {
                // Lost River
                xPos = Random.Range(-1200, 0);
                zPos = Random.Range(-700, 900);
            }
            else if (depth > -1300)
            {
                // Inactive Lava Zone
                xPos = Random.Range(-600, 300);
                zPos = Random.Range(-315, 550);
            }
            else
            {
                // Lava Zone
                xPos = Random.Range(-66, 400);
                zPos = Random.Range(-190, 166);
            }

            var newPosition = new Vector3(xPos, depth, zPos);
            Player.main.SetPosition(newPosition);
            Player.main.OnPlayerPositionCheat();
        }

        public static void TeleportPlayer()
        {
            var biomeTeleportData = BiomeConsoleCommand.main.data;
            var locationTeleportData = GotoConsoleCommand.main.data;

            var totalCount = biomeTeleportData.locations.Length + locationTeleportData.locations.Length;
            var newPositionNum = Random.Range(0, totalCount);

            TeleportPosition newPositionData;
            if (newPositionNum < biomeTeleportData.locations.Length)
            {
                newPositionData = biomeTeleportData.locations[newPositionNum];
            }
            else
            {
                newPositionData = locationTeleportData.locations[newPositionNum - biomeTeleportData.locations.Length];
            }

            Player.main.SetPosition(newPositionData.position);
            Player.main.OnPlayerPositionCheat();
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

        public static void ActivateOHKO()
        {
            OneHitKnockout.active = true;
        }

        public static void DeactiveOHKO()
        {
            OneHitKnockout.active = false;
        }
    }
}
