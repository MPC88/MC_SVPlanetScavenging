﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MC_SVPlanetScavenging
{
    internal class ProbeDroneEquipment
    {
        internal const int id_mkI = 30001;        
        internal const int id_mkII = 30002;
        internal const string equipmentName_mkI = "Probe Drone Bay";
        internal const string equipmentName_mkII = "Probe Drone Bay Mk.II";

        [HarmonyPatch(typeof(EquipmentDB), "LoadDatabaseForce")]
        [HarmonyPostfix]
        private static void EquipmentDBLoadDBForce_Post()
        {
            List<Equipment> eqs = AccessTools.StaticFieldRefAccess<List<Equipment>>(typeof(EquipmentDB), "equipments");
            eqs.Add(ProbeDroneEquipment.CreateMkIEquipment());
            eqs.Add(ProbeDroneEquipment.CreateMkIIEquipment());
        }

        private static Equipment CreateMkIEquipment()
        {
            Equipment refEq = EquipmentDB.GetEquipment(66 / 2);

            Equipment equipment = ScriptableObject.CreateInstance<Equipment>();
            equipment.name = id_mkI + "." + equipmentName_mkI;
            equipment.id = id_mkI;
            equipment.refName = equipmentName_mkI;
            equipment.minShipClass = refEq.minShipClass;
            equipment.activated = true;
            equipment.enableChangeKey = true;
            equipment.space = 4;
            equipment.energyCost = 8;
            equipment.energyCostPerShipClass = refEq.energyCostPerShipClass;
            equipment.rarityCostMod = refEq.rarityCostMod;
            equipment.techLevel = refEq.techLevel;
            equipment.sortPower = refEq.sortPower;
            equipment.massChange = refEq.massChange;
            equipment.type = refEq.type;
            equipment.effects = new List<Effect>() { 
                new Effect() { type = 18, description = "", mod = 1f, value = 1f, uniqueLevel = 0 },
                new Effect() { type = 32, description = "", mod = 1f, value = 3f, uniqueLevel = 0 } 
            };
            equipment.uniqueReplacement = refEq.uniqueReplacement;
            equipment.rarityMod = 1.2f;
            equipment.sellChance = refEq.sellChance;
            equipment.repReq = refEq.repReq;
            equipment.dropLevel = refEq.dropLevel;
            equipment.lootChance = refEq.lootChance;
            equipment.spawnInArena = false;
            equipment.sprite = refEq.sprite;
            equipment.activeEquipmentIndex = id_mkI;
            equipment.defaultKey = KeyCode.Alpha1;
            equipment.requiredItemID = refEq.requiredItemID;
            equipment.requiredQnt = 5;
            equipment.equipName = Language.equipmentName_mkI;
            equipment.description = Language.equipmentDescription;
            equipment.craftingMaterials = refEq.craftingMaterials;
            equipment.buff = null;

            return equipment;
        }

        private static Equipment CreateMkIIEquipment()
        {
            Equipment refEq = EquipmentDB.GetEquipment(234 / 2);

            Equipment equipment = ScriptableObject.CreateInstance<Equipment>();
            equipment.name = id_mkII + "." + equipmentName_mkII;
            equipment.id = id_mkII;
            equipment.refName = equipmentName_mkII;
            equipment.minShipClass = refEq.minShipClass;
            equipment.activated = true;
            equipment.enableChangeKey = true;
            equipment.space = 4;
            equipment.energyCost = 8;
            equipment.energyCostPerShipClass = refEq.energyCostPerShipClass;
            equipment.rarityCostMod = refEq.rarityCostMod;
            equipment.techLevel = refEq.techLevel;
            equipment.sortPower = refEq.sortPower;
            equipment.massChange = refEq.massChange;
            equipment.type = refEq.type;
            equipment.effects = new List<Effect>() {
                new Effect() { type = 18, description = "", mod = 1f, value = 1f, uniqueLevel = 0 },
                new Effect() { type = 32, description = "", mod = 1f, value = 6f, uniqueLevel = 0 } 
            };
            equipment.uniqueReplacement = refEq.uniqueReplacement;
            equipment.rarityMod = 1.2f;
            equipment.sellChance = refEq.sellChance;
            equipment.repReq = refEq.repReq;
            equipment.dropLevel = refEq.dropLevel;
            equipment.lootChance = refEq.lootChance;
            equipment.spawnInArena = false;
            equipment.sprite = refEq.sprite;
            equipment.activeEquipmentIndex = id_mkII;
            equipment.defaultKey = KeyCode.Alpha1;
            equipment.requiredItemID = refEq.requiredItemID;
            equipment.requiredQnt = 5;
            equipment.equipName = Language.equipmentName_mkII;
            equipment.description = Language.equipmentDescription;
            equipment.craftingMaterials = refEq.craftingMaterials;
            equipment.buff = null;

            return equipment;
        }

        private static AE_ProbeDroneBay MakeActiveEquip(Equipment equipment, SpaceShip ss, KeyCode key, int rarity, int qnt)
        {
            AE_ProbeDroneBay probeDroneBay = new AE_ProbeDroneBay
            {
                id = equipment.id,
                rarity = rarity,
                key = key,
                ss = ss,
                isPlayer = (ss != null && ss.CompareTag("Player")),
                equipment = equipment,
                qnt = qnt,
                active = false
            };
            return probeDroneBay;
        }

        [HarmonyPatch(typeof(ActiveEquipment), nameof(ActiveEquipment.AddActivatedEquipment))]
        [HarmonyPrefix]
        internal static bool ActiveEquipmentAdd_Pre(Equipment equipment, SpaceShip ss, KeyCode key, int rarity, int qnt, ref ActiveEquipment __result)
        {
            if (equipment.id != id_mkI && equipment.id != id_mkII)
                return true;

            AE_ProbeDroneBay ae = MakeActiveEquip(equipment, ss, key, rarity, qnt);
            ss.activeEquips.Add(ae);
            ae.AfterConstructor();
            __result = ae;
            return false;
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.CreateDefaultChar))]
        [HarmonyPostfix]
        private static void GameDataCreateDefaultChar_Post()
        {
            if (PChar.Char != null && PChar.Char.HasPerk(324))
            {
                PChar.Char.AddBlueprint(2, id_mkI, 1f);
                PChar.Char.SortBlueprints();
            }
        }

        [HarmonyPatch(typeof(SpaceShip), nameof(SpaceShip.CalculateEnergy))]
        [HarmonyPostfix]
        private static void SpaceShipCalculateEnergy_Post(SpaceShip __instance)
        {
            if (!__instance.IsPlayer)
                return;

            foreach(ActiveEquipment ae in __instance.activeEquips)
            {
                if(ae.active && (ae.equipment.id == id_mkI || ae.equipment.id == id_mkII))
                {
                    if (ae is AE_ProbeDroneBay)
                    {
                        AE_ProbeDroneBay aepdb = ae as AE_ProbeDroneBay;
                        if(aepdb.activePDCs != null)
                            __instance.stats.energyExpend += ae.equipment.energyCost * aepdb.activePDCs.Count;
                    }                    
                }
            }
        }
    }

    public class AE_ProbeDroneBay : ActiveEquipment
    {
        internal PersistentData.PlanetData pd;
        internal List<ProbeDroneController> activePDCs;

        public override void ActivateDeactivate(bool shiftPressed, Transform target)
        {
            if (!this.isPlayer)
                return;

            if (!this.active)
            {
                PlanetControl planetControl = null;
                if (ScanningControl.CheckForPlanet(PlayerControl.inst, out planetControl))
                {                    
                    PersistentData.PlanetData planetData = new PersistentData.PlanetData()
                    {
                        sector = GameData.data.currentSectorIndex,
                        x = planetControl.planetData.x,
                        y = planetControl.planetData.y
                    };

                    if (Main.data == null)
                        Main.data = new PersistentData();

                    PersistentData.PlanetData pdExisting = Main.data.GetData(planetData);
                    if (pdExisting == null)
                    {
                        InfoPanelControl.inst.ShowWarning(Language.planetNotScannedWarning, 1, playAudio: false);
                        return;
                    }

                    if (pdExisting.scavenged)
                    {
                        InfoPanelControl.inst.ShowWarning(Language.planetAlreadyScavengedWarning, 1, playAudio: false);
                        return;
                    }

                    this.pd = pdExisting;
                    this.activePDCs = new List<ProbeDroneController>();

                    for (int i = 0; i < this.qnt; i++)
                    {
                        if (ss.cs.ConsumeItem(3, this.equipment.requiredItemID, this.equipment.requiredQnt, -1) > -1)
                            InstantiateDrone();
                        else
                            InfoPanelControl.inst.ShowWarning(Language.insufficientDronePartsWarning, 1, false);
                    }

                    this.active = true;
                    ss.CalculateEnergy();                    
                }                
            }
            else
            {
                foreach(ProbeDroneController pdc in activePDCs)
                    SideInfo.AddMsg(Language.drone + " " + ss.activeEquips.IndexOf(this) + "-" + pdc.id + ": " + pdc.Report());
            }            
        }

        private void InstantiateDrone()
        {
            GameObject obj = UnityEngine.Object.Instantiate(GameManager.instance.repairDroneBaseObj[(int)ss.stats.modelData.manufacturer], ss.transform.position, ss.transform.rotation);
            obj.SetActive(false);
            obj.transform.SetParent(GameManager.instance.GetDronesGroup());
            obj.name = ss.transform.name + "'s Probe Drone";            
            ProbeDroneController pdc = obj.AddComponent<ProbeDroneController>();
            this.activePDCs.Add(pdc);
            pdc.pd = this.pd;
            pdc.owner = this.ss;
            pdc.ae = this;
            pdc.id = activePDCs.Count;
            UnityEngine.Object.Destroy(obj.GetComponent<Drone>());
            UnityEngine.Object.Destroy(obj.GetComponentInChildren<Collider>());
            pdc.dockingAudio = obj.GetComponent<Drone>().dockingAudio;            
            obj.SetActive(true);
        }

        internal void NotifyDone(ProbeDroneController pdc, bool lost)
        {
            if (lost)
                SideInfo.AddMsg("<color=red>" + Language.drone + " " + ss.activeEquips.IndexOf(this) + "-" + pdc.id + ": " + Language.lostContact + ".</color>");

            if (this.activePDCs.Contains(pdc))
                this.activePDCs.Remove(pdc);

            if (this.activePDCs.Count < 1)
            {                
                this.active = false;
                this.AfterDeactivate();
            }
        }
    }


    public class ProbeDroneController : MonoBehaviour
    {
        internal PersistentData.PlanetData pd;
        internal SpaceShip owner;
        internal AE_ProbeDroneBay ae;
        internal AudioClip dockingAudio;
        internal int id;

        private Rigidbody rb;
        private Vector3 target;
        private bool returning = false;
        private bool special = false;

        private float baseSpeed;
        private float hp;
        
        public void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
        }

        public void Start()
        {
            GetStats();            
        }
        
        public void Update()
        {
            if (returning)
            {
                target = owner.gameObject.transform.position;

                if (Vector3.Distance(gameObject.transform.position, target) < 2f)
                    DockOrDie(true, false);
            }
            else
            {
                if (target == null || target == Vector3.zero)
                {
                    target = new Vector3(pd.wpX, pd.wpY, pd.wpZ);
                    target.x += UnityEngine.Random.Range(-60, 60);
                    target.z += UnityEngine.Random.Range(-60, 60);
                }

                if (Vector3.Distance(gameObject.transform.position, target) < 2f)
                {
                    bool dead = UnityEngine.Random.Range(0f, 1f) <= pd.risk ? true : false;
                    bool save = UnityEngine.Random.Range(0f, 1f) <= (hp / 200);
                    if (dead && !save)
                    {
                        DockOrDie(false, true);
                    }
                    else
                    {
                        if (UnityEngine.Random.Range(0f, 1f) <= pd.quality)
                        {
                            PChar.ExplorerUp(GameData.data.sectors[pd.sector].level - 4, 0, 0f);
                            special = true;
                            SideInfo.AddMsg(Language.drone + " " + ae.ss.activeEquips.IndexOf(ae) + "-" + this.id + " " + Language.valuableFind + ".");
                        }
                        pd.scavenged = true;
                        returning = true;
                    }
                }
            }

            base.transform.LookAt(target);
            rb.velocity = base.transform.forward * baseSpeed;
        }

        private void FixedUpdate()
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 400f);
        }
        
        private void GetStats()
        {
            DroneModifiers droneMods = owner.stats.droneMods;
            float speedBouns = 0;
            float hpBonus = 0;
            if (droneMods != null)
            {
                speedBouns = droneMods.speedBonus;
                hpBonus = droneMods.HPbonus;
            }

            hp = hpBonus;
            baseSpeed = (60 + ae.rarity * 5) + speedBouns;
        }


        public void DockOrDie(bool giveFeedback, bool dead)
        {
            if (!dead)
            {
                DebrisField df = new DebrisField(new Coordenates(99999, 99999), GameData.data.sectors[pd.sector].level, GameData.data.sectors[pd.sector]);
                df.type = special ? 1 : 0;
                LootSystem.instance.DropScavengeLoot(ae.ss.transform.position, GameData.data.sectors[pd.sector].level, df);
                owner.cs.StoreItem(3, ae.equipment.requiredItemID, 1, ae.equipment.requiredQnt, 0f, -1, -1);
            }

            base.gameObject.SetActive(value: false);
            owner.CalculateEnergy();            
            if (giveFeedback && !dead)
            {
                if (SoundSys.SFXvolume > 0f)
                {
                    owner.GetComponent<AudioSource>().PlayOneShot(dockingAudio, SoundSys.SFXvolume);
                }
                if (owner.CompareTag("Player"))
                {
                    owner.hpBarControl.ShowFloatingText(Language.droneParts, 3, 0f);
                }
            }
            if (ae.active)
                ae.NotifyDone(this, dead);
        }

        public string Report()
        {
            float dist = Vector3.Distance(this.gameObject.transform.position, target);
            string inoutMsg = returning ? Language.returning : Language.approaching;
            string distMsg = " | " + Language.distance + ": " + Math.Round(dist, 2).ToString();
            string etaMsg = " | " + Language.eta + ": " + Math.Round(dist/baseSpeed, 2).ToString() + " " + Language.seconds;

            return inoutMsg + distMsg + etaMsg;
        }
    }
}