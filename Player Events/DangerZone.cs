using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchInteraction.Player_Events
{
    class DangerZone
    {
        public static void KillPlayer()
        {
            Player.main.liveMixin.TakeDamage(666);
        }
    }
}
