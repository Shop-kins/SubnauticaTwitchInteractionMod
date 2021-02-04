# SubnauticaTwitchInteractionMod

A mod for Subnautica that allows Twitch Chat to interact with the game. Works either via direct connection to Twitch or using [Crowd Control](https://crowdcontrol.live/).

## Dev Environment

### Publicise dlls

in order to use this (if you're building the solution yourself), you'll be required to first publicise the following .dll files

- Assembly-CSharp
- Assembly-CSharp-firstpass

to do so you can use [This](https://github.com/MrPurple6411/AssemblyPublicizer) 
but its very possible this just doesn't work for you in which case just ask someone for the resultant .dlls

### Remaining dlls

you will need to also include

- 0Harmony
- QModInstaller 
- UnityEngine.CoreModule
- UnityEngine.InputLegacyModule

## Packaging

When packaging the mod, include the following files:
- TwitchInteraction.dll
- Config.txt.template
- mod.json
- SubCrowdControl.cs 
- dependencies/

Where the dependencies folder contains all the referenced dlls that are not already included in Subnautica and QMod (TwitchLib.Unity, Microsoft.Bcl.AsyncInterfaces and the System dlls)

## Usage

### Twitch 

You will need to generate tokens https://twitchtokengenerator.com/
with appropriate permissions. These can then be dumped in a Config.txt file in the qmod folder, an example of what that would look like is in Config.txt.template

### Crowd Control

To run the mod with Crowd Control:
1. Setup Crowd Control as per their [Setup Guide](https://crowdcontrol.live/setup)
2. Modify the Config.txt to include { "client": "crowdcontrol" }
3. Select `Subnautica` from the `Game Selection` portion of Crowd Control and start the server
4. Run Subnautica. It should connect to the mod automatically.

If you start the server after starting Subnautica it should still connect properly as the mod retries every 30 seconds. 

#### Testing
Use the [Crowd Control SDK](https://forum.warp.world/t/how-to-setup-and-use-the-crowd-control-sdk/5121) to test it which mimics the the local Crowd Control server.

1. Open `Crowd Control EffectPack SDK`
2. Click on `Load Pack Source`
3. Select `SubCrowdControl.cs` at the root of this repository. This is also bundled as a part of the mod.
4. Select `Subnautica` from the list
5. Click on `Connect` at the bottom of the left hand side
6. Run Subnautica, and it should connect automatically
7. Once it is running, you can use the Play button to create an effect (with an optional delay)
