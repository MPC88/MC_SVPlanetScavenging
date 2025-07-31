using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_SVPlanetScavenging
{
    internal class Language
    {
        internal static string loadFailed = "Planet scavenging mod load failed.";
        internal static string equipmentName_mkI = "Probe Drone Bay";
        internal static string equipmentName_mkII = "Probe Drone Bay Mk.II";
        internal static string equipmentDescription = "Used for planetary surface scavenging.  These drones cannot be recalled, but can report on mission progress.\n\nLaunched drones will complete their task and return to mothership as long as it remains in sector.";
        internal static string planetNotScannedWarning = "Planet not scanned.  Use scavenge to perform scanning.";
        internal static string planetAlreadyScavengedWarning = "You have already scavenged all promising locations.";
        internal static string insufficientDronePartsWarning = "Insufficient drone parts.";
        internal static string drone = "Drone";
        internal static string lostContact = "Lost contact";
        internal static string valuableFind = "telemetry indicates a valuable find";
        internal static string droneParts = "Drone Parts";
        internal static string returning = "Returning to mothership";
        internal static string approaching = "Approaching target";
        internal static string distance = "Distance";
        internal static string eta = "ETA";
        internal static string seconds = "seconds";
        internal static string planetAlreadyScavenged = "Planet already scavenged";
        internal static string cooldown = "Cooldown";
        internal static string minutes = "minutes";
        internal static string planetAlreadyScanned = "Planet already scanned";
        internal static string quality = "Quality";
        internal static string risk = "Risk";
        internal static string scanningPlanetAt = "Scanning planet at";
        internal static string inSector = "in sector";
        internal static string scanningAborted = "Scanning aborted";
        internal static string hold = "Hold";
        internal static string toStabaliseShip = "to stabalise ship";
        internal static string scanComplete = "Scan complete";
        internal static string fromExplorer = "from explorer";
        internal static string fromTech = "from tech";

        internal static void Load(string file)
        {
            try
            {
                if (File.Exists(file))
                {
                    StreamReader sr = new StreamReader(file);
                    loadFailed = sr.ReadLine();
                    equipmentName_mkI = sr.ReadLine();
                    equipmentName_mkII = sr.ReadLine();
                    equipmentDescription = sr.ReadLine();
                    planetNotScannedWarning = sr.ReadLine();
                    planetAlreadyScavengedWarning = sr.ReadLine();
                    insufficientDronePartsWarning = sr.ReadLine();
                    drone = sr.ReadLine();
                    lostContact = sr.ReadLine();
                    valuableFind = sr.ReadLine();
                    droneParts = sr.ReadLine();
                    returning = sr.ReadLine();
                    approaching = sr.ReadLine();
                    distance = sr.ReadLine();
                    eta = sr.ReadLine();
                    seconds = sr.ReadLine();
                    planetAlreadyScavenged = sr.ReadLine();
                    cooldown = sr.ReadLine();
                    minutes = sr.ReadLine();
                    planetAlreadyScanned = sr.ReadLine();
                    quality = sr.ReadLine();
                    risk = sr.ReadLine();
                    scanningPlanetAt = sr.ReadLine();
                    inSector = sr.ReadLine();
                    scanningAborted = sr.ReadLine();
                    hold = sr.ReadLine();
                    toStabaliseShip = sr.ReadLine();
                    scanComplete = sr.ReadLine();
                    fromExplorer = sr.ReadLine();
                    fromTech = sr.ReadLine();
                }
            }
            catch
            {
                Main.log.LogError("Language load failed");
            }
        }
    }
}
