# SubnauticaTwitchInteractionMod

A mod for Subnautica that allows Twitch Chat to interact with the game. Works either via direct connection to Twitch
or using [Crowd Control](https://crowdcontrol.live/).

## Dev Environment

### Publicise dlls

in order to use this (if you're building the solution yourself), you'll be required to first publicise the following .dll files

- Assembly-CSharp
- Assembly-CSharp-firstpass

to do so you can use [This](https://github.com/MrPurple6411/AssemblyPublicizer) 
but its very possible this just doesn't work for you in which case just ask someone for the resultant .dlls

### Remaining dlls

you will need to also include

- 0Harmony12
- UnityEngine.CoreModule
- UnityEngine.InputLegacyModule

## Usage

### Twitch 

You will need to generate tokens https://twitchtokengenerator.com/
with appropriate permissions. These can then be dumped in a Config.txt file in the qmod folder, an example of what that would look like is in Config.txt.template

### Crowd Control

Use the [Crowd Control SDK](https://forum.warp.world/t/how-to-setup-and-use-the-crowd-control-sdk/5121) to test it which mimics the the local Crowd Control server.

1. Open `Crowd Control EffectPack SDK`
2. Click on `Load Pack Source`
3. Select `SubCrowdControl.cs` at the root of this repository
4. Select `Subnautica` from the list
5. Click on `Connect` at the bottom of the left hand side
6. Run Subnautica, and it should connect automatically
7. Once it is running, you can use the Play button to create an effect (with an optional delay)
