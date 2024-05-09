# MC_SVPlanetScavenging  
  
Backup your save before using any mods.  
  
Uninstall any mods and attempt to replicate issues before reporting any suspected base game bugs on official channels.  
    
Function  
========  
Adds probe drone bays and planet scanning for scavenging.  
  
Hover over a planet and scan it using the scavenge keybind.  Quality indicates the change of hidden debris field level loot.  Risk indicates the chance of loosing a drone to dangers.  
  
Activate probe drone bays to send drones to a scanned planet.  Each drone that makes it back counts as a debris field of level equal to the sector level.  If a drone dies due to failed risk roll, you lose the parts and get nothing (launching a probe drone costs 5 parts).  
  
There's a flat 1 hour cooldown on scavenging a planet.  

Probe drones cannot be recalled, but will return to your ship as long as you stay in sector.  Pressing the activate key while drones are deployed will provide a status report in the bottom-right of the screen.  The same message area is used to highlight any lost drones or special finds.  

The probe drone bays provide scavenging loot bonuses.  
  
- Explorer level buffs quality.  You know where to focus your scans.  
- Tech level reduces risk.  
- Tech level applies to accelerate the scanning phase, same debris field salvaging.  It is compared against sector level.  
- Drone speed buffs apply to accelerate the retrieval phase.  They go faster.  
- Drone hp bonus buffs provide a chance to save if the risk roll fails.  The save chance is BonusHP/2 percent chance, so the T3 ship enhancement of +60hp = 30% save.  
- As scavenging loot drops, all scavenging loot bonuses and skills apply (e.g. engineering skill tree).  
  
Install  
=======  
1. Install BepInEx - https://docs.bepinex.dev/articles/user_guide/installation/index.html Stable version 5.4.21 x86.  
2. Run the game at least once to initialise BepInEx and quit.  
3. Download latest mod release.  
4. Place MC_SVPlanetScavenging.dll in .\SteamLibrary\steamapps\common\Star Valor\BepInEx\plugins\  
  
Mod Info
======
Probe Drone Bay ID = 30001
Probe Drone Bay Mk.II ID = 30002
  
