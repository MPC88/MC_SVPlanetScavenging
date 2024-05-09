using System.Collections.Generic;
using System;
using UnityEngine;

namespace MC_SVPlanetScavenging
{
    [Serializable]
    internal class PersistentData
    {
        internal List<PlanetData> planetDatas;

        internal PersistentData()
        {
            planetDatas = new List<PlanetData>();
        }

        internal PlanetData GetData(PlanetData pd)
        {
            foreach (PlanetData p in planetDatas)
                if(p.SamePlanetAs(pd))
                    return p;

            return null;
        }

        [Serializable]
        internal class PlanetData
        {
            internal int sector = 0;
            internal int x = 0;
            internal int y = 0;
            internal float wpX = 0;
            internal float wpY = 0;
            internal float wpZ = 0;
            internal float quality = 0;
            internal float risk = 0;
            internal float timeScanned = 0;
            internal bool scavenged = false;

            internal PlanetData()
            {
            }

            internal bool SamePlanetAs(PlanetData otherPlanetData)
            {
                return this.sector == otherPlanetData.sector && 
                    this.x == otherPlanetData.x && 
                    this.y == otherPlanetData.y;
            }
        }
    }
}
