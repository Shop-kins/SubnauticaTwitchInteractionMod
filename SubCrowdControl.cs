using System;
using System.Collections.Generic;
using CrowdControl.Common;
using CrowdControl.Games.Packs;
using ConnectorType = CrowdControl.Common.ConnectorType;

public class Subnautica : SimpleTCPPack
{
    public override string Host => "127.0.0.1";

    public override ushort Port => 2679;

    public Subnautica(IPlayer player, Func<CrowdControlBlock, bool> responseHandler, Action<object> statusUpdateHandler) : base(player, responseHandler, statusUpdateHandler) { }

<<<<<<< HEAD
    public override Game Game => new Game(60, "Subnautica", "subnautica", "PC", ConnectorType.SimpleTCPConnector);
=======
    public override Game Game => new Game(91, "Subnautica", "Subnautica", "PC", ConnectorType.SimpleTCPConnector);
>>>>>>> c53970f94c55dd9b9c3eb81ccde974343ff1ff01

    public override List<Effect> Effects => new List<Effect>
    {
        new Effect("Rip Riley", "kill"),
        new Effect("Heal Riley", "heal"),
        new Effect("Toggle Day/Night", "toggle_day_night"),
        new Effect("Open PDA", "open_pda"),
        new Effect("Turn on the big gun", "gun"),
        new Effect("Fill Oxygen", "fill_o2"),
        new Effect("Find a new home", "teleport_lifepod_shallows"),
        new Effect("Player Teleport", "teleport_player"),
        new Effect("Give that beautiful tooth", "tooth"),
        new Effect("Cow or Reaper? Yes.", "spawn_random_creature"),
        new Effect("Fill him up with junk", "junk"),
        new Effect("Get your pet reaper to hang out", "reaper"),
        new Effect("Resource Roulette", "resource_roulette"),
        new Effect("Blueprint Roulette", "blueprint_roulette"),
        new Effect("An early breakfast", "eat"),
        new Effect("Find a really new home", "teleport_lifepod_deep"),
        new Effect("Play tooth drop sound", "tooth_sound"),
        new Effect("Clear a hotbar slot", "clear_hotbar"),
        new Effect("Shuffle the hotbar", "hotbar_shuffle"),
        new Effect("Steal a battery", "take_battery"),
        new Effect("Steal some equipment", "take_equipment"),
        new Effect("Kill bad things", "kill_enemies"),
        new Effect("Go back home", "teleport_player_shallows"),
        new Effect("Crafted Roulette", "advanced_roulette"),
<<<<<<< HEAD
        new Effect("Random Mouse Sensitivity", "random_mouse"),
        new Effect("Hide HUD", "hide_hud"),
        new Effect("Invert Controls", "invert_controls"),
        new Effect("Disable Controls", "disable_controls"),
        new Effect("Light? What is light?", "filmic_mode"),
        new Effect("Random FOV", "random_fov"),
        new Effect("What explosion?", "restore_ship")
=======
        new Effect("What explosion?", "restore_ship"),
        new Effect("Random Mouse Sensitivity (15 seconds)", "random_mouse"),
        new Effect("Hide HUD (60 seconds)", "hide_hud"),
        new Effect("Invert Controls (60 seconds)", "invert_controls"),
        new Effect("Disable Controls (10 seconds)", "disable_controls"),
        new Effect("Light? What is light? (60 seconds)", "filmic_mode"),
        new Effect("Random FOV (60 seconds)", "random_fov")
>>>>>>> c53970f94c55dd9b9c3eb81ccde974343ff1ff01
    };
}
