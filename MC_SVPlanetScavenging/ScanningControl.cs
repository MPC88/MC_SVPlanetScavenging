using HarmonyLib;
using Rewired;
using System;
using UnityEngine;

namespace MC_SVPlanetScavenging
{
    internal class ScanningControl
    {
		private const float scavengeCD = 60f;

		private static Player player = null;
		private static Vector3 scanStartPosition;
		private static GameObject scanLight;
		private static float scanTime;
		private static PersistentData.PlanetData scanning = null;

		[HarmonyPatch(typeof(PlayerControl), "Update")]
		[HarmonyPostfix]
		private static void PlayerControlUpdate_Post(PlayerControl __instance)
		{
			if (__instance.blockControls || __instance.blockKeyboard)
				return;

			if (player == null)
				player = (Player)typeof(PlayerControl).GetField("player", AccessTools.all).GetValue(__instance);

			if (player != null && player.GetButtonDown("Interact"))
			{
				PlanetControl planetControl;
				if (CheckForPlanet(__instance, out planetControl))
				{
					if (Main.data == null)
						Main.data = new PersistentData();

					PersistentData.PlanetData pd = new PersistentData.PlanetData()
					{
						sector = GameData.data.currentSectorIndex,
						x = planetControl.planetData.x,
						y = planetControl.planetData.y,
						wpX = planetControl.gameObject.transform.GetChild(0).position.x,
						wpY = planetControl.gameObject.transform.GetChild(0).position.y,
						wpZ = planetControl.gameObject.transform.GetChild(0).position.z,
					};

					PersistentData.PlanetData pdExisting = Main.data.GetData(pd);

					if (pdExisting == null && scanning == null)
					{
						StartScan(planetControl.transform, pd);
					}
					else
					{
						if (pdExisting.timeScanned > 0)
						{
							if (pdExisting.timeScanned + (scavengeCD * 60f) <= GameData.timePlayed && scanning == null)
							{
								Main.data.planetDatas.Remove(pdExisting);
								StartScan(planetControl.transform, pd);
							}
							else if (pdExisting.scavenged)
								InfoPanelControl.inst.ShowWarning("Planet already scavenged.  Cooldown: " +
									Math.Round(((pdExisting.timeScanned + (scavengeCD * 60f)) - GameData.timePlayed) / 60, 2) + " minutes.", 2, false);
							else
								InfoPanelControl.inst.ShowWarning("Planet already scanned.  Quality: " +
								Math.Round(pdExisting.quality * 100, 2) + ", Risk: " + Math.Round(pdExisting.risk * 100, 2),
								2, false);
						}						
					}
				}
			}
		}

		internal static bool CheckForPlanet(PlayerControl pc, out PlanetControl result)
		{
			Ray ray = new Ray(pc.GetSpaceShip.transform.position, Vector3.down);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, (int)Mathf.Pow(2, Main.planetLayer)))
			{
				result = hit.transform.parent.GetComponent<PlanetControl>();
				if (result)
					return true;
			}
			result = null;
			return false;
		}

		internal static void StartScan(Transform planetTransform, PersistentData.PlanetData pd)
		{
			int difficultyRank = PChar.Char.GetDifficultyRank(PChar.TechLevel(), GameData.data.GetCurrentSector().level);
			scanStartPosition = GameManager.instance.Player.transform.position;
			scanLight = GameObject.Instantiate(ObjManager.GetObj("Effects/Scannerlight"), planetTransform);
			scanLight.transform.position = GameManager.instance.Player.transform.position;
			scanLight.GetComponent<AudioSource>().volume = SoundSys.SFXvolume - 0.1f;
			float num = (float)PChar.Char.SK[1] * 0.2f;
			if (difficultyRank >= 3)
			{
				num = 0f;
			}
			scanTime = 2.5f / (1f + num);
			scanLight.GetComponent<DestroyByTime>().lifetime = scanTime;
			scanLight.GetComponent<ModelRotator>().rotationVelocity = new Vector3(0f, 72f * (1f + num), 0f);
			InfoPanelControl.inst.ShowWarning("Scanning planet at x:" + pd.x + " y:" + pd.y + " in sector " + pd.sector, 2, false);
			scanning = pd;
		}

		internal static void Update()
        {
			if (scanning != null)
			{
				scanTime -= Time.deltaTime;
				if (Vector3.Distance(scanStartPosition, GameManager.instance.Player.transform.position) > 6f)
				{
					InfoPanelControl.inst.ShowWarning("Scanning aborted.  Hold " + GameManager.instance.GetKeyBound(3) + " to stabalise ship.", 1, playAudio: false);
					scanning = null;
					GameObject.Destroy(scanLight);
				}
				if (scanTime <= 0f)
				{
					FinishScanning();
				}
			}
		}

		internal static void FinishScanning()
        {
            PChar.TechLevelUp(GameData.data.sectors[scanning.sector].level, 3, 0f);

			float quality = UnityEngine.Random.Range(0.1f, 0.35f);
			float exp = UnityEngine.Random.Range(0f, PChar.Explorer(true) / 100f);
			float risk = UnityEngine.Random.Range(0.1f, 0.9f);
			float tec = UnityEngine.Random.Range(0f, PChar.TechLevel() / 100f);
			scanning.quality = quality + exp;
			scanning.risk = risk - tec >= 0 ? risk - tec : 0;
			scanning.timeScanned = GameData.timePlayed;		

			if (Main.data == null)
				Main.data = new PersistentData();
			
			Main.data.planetDatas.Add(scanning);
			InfoPanelControl.inst.ShowWarning("Scan complete.  " + 
				"Quality: " + Math.Round(scanning.quality * 100, 2) + " (" + Math.Round(quality * 100, 2) + " + " + Math.Round(exp * 100, 2) + " from explorer)\n" + 
				"Risk: " + Math.Round(scanning.risk * 100, 2) + " (" + Math.Round(risk * 100, 2) + " - " + Math.Round(tec * 100, 2) + " from tech)",
								2, false);
			scanning = null;
		}
	}
}
