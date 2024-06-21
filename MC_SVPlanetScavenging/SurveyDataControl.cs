using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_SVPlanetScavenging
{
    internal class SurveyDataControl
    {
        internal static int surveyDataID = 30001;
        internal static float baseValue;

        internal static bool HasSurveyData(PersistentData.PlanetData pd, out CargoItem result)
        {
            foreach(CargoItem ci in GameManager.instance.Player.GetComponent<CargoSystem>().cargo)
            {
                if(ci.itemType == 3 && ci.itemID == surveyDataID)
                {
                    CIData_Survey cid = ci.extraData as CIData_Survey;
                    if(cid.planetData.sector == pd.sector && cid.planetData.x == pd.x && cid.planetData.y == pd.y)
                    {
                        result = ci;
                        return true;
                    }
                }
            }

            result = null;
            return false;
        }
    }

    public class CIData_Survey : CI_Data
    {
        public float value;
        internal PersistentData.PlanetData planetData;

        public CIData_Survey(int sector, int x, int y, float quality, float risk)
        {
            planetData = new PersistentData.PlanetData()
            {
                quality = quality,
                risk = risk,
                sector = sector,
                x = x,
                y = y
            };

            value = ;
        }
    }
}
