# SubnauticaTwitchInteractionMod

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

You will need to generate tokens https://twitchtokengenerator.com/
with appropriate permissions. These can then be dumped in a Config.txt file in the qmod folder, an example of what that would look like is in Config.txt.template
