using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using InfinityScript;
using System.Runtime.InteropServices;

namespace IS_SlvrTest
{
    public class Class1 : BaseScript
    {
        Random rng = new Random();
        //HudElem testH;
        Entity testB;
        Entity testH;
        Entity level;
        public static Entity _airdropCollision;
        public static int fx_eyes;
        public static List<Entity> triggerEnts = new List<Entity>();

        public Class1()
        {
            Utilities.PrintToConsole(string.Format("Plugin loaded at {0}", GSCFunctions.GetTime()));
            //GSCFunctions.PreCacheString("Test String");
            //GSCFunctions.PreCacheStatusIcon("cardicon_iwlogo");
            //GSCFunctions.PreCacheMenu("kickplayer");
            //GSCFunctions.PreCacheMenu("elevator_floor_selector");
            //GSCFunctions.PreCacheShader("faction_128_gign");

            //Marshal.WriteInt32(new IntPtr(0x0585AE0C), 24);
            //Marshal.WriteInt32(new IntPtr(0x0585AE1C), 24);

            //working guns
            GSCFunctions.PreCacheItem("at4_mp");
            GSCFunctions.PreCacheItem("airdrop_mega_marker_mp");
            GSCFunctions.PreCacheItem("throwingknife_rhand_mp");
            GSCFunctions.PreCacheItem("iw5_mk12spr_mp");
            GSCFunctions.PreCacheItem("lightstick_mp");
            GSCFunctions.PreCacheItem("killstreak_double_uav_mp");
            GSCFunctions.PreCacheItem("strike_marker_mp");
            GSCFunctions.PreCacheItem("killstreak_helicopter_minigun_mp");
            GSCFunctions.PreCacheItem("airdrop_juggernaut_def_mp");
            GSCFunctions.PreCacheItem("uav_strike_missile_mp");
            GSCFunctions.PreCacheItem("uav_strike_projectile_mp");
            GSCFunctions.PreCacheItem("iw5_xm25_mp");
            GSCFunctions.PreCacheItem("iw5_riotshield_mp");
            GSCFunctions.PreCacheItem("harrier_missile_mp");

            //turret-only
            GSCFunctions.PreCacheTurret("manned_minigun_turret_mp");
            GSCFunctions.PreCacheTurret("manned_gl_turret_mp");
            GSCFunctions.PreCacheItem("remote_uav_weapon_mp");
            GSCFunctions.PreCacheItem("aamissile_projectile_mp");

            //Hacking in all fx
            //smoke
            /*
            string[] smoke_fx;
            smoke_fx = Directory.GetFiles(@"H:\USBFORMAT\MW3 GSCs\devraw\fx\smoke");
            for (int i = 0; i < smoke_fx.Length; i++)
            {
                GSCFunctions.LoadFX("smoke/" + smoke_fx[i].Split('\\')[6].Replace(".FXE", ""));
            }
            string[] prop_fx;
            prop_fx = Directory.GetFiles(@"H:\USBFORMAT\MW3 GSCs\devraw\fx\props");
            for (int i = 0; i < prop_fx.Length; i++)
            {
                GSCFunctions.LoadFX("props/" + prop_fx[i].Split('\\')[6].Replace(".FXE", ""));
            }
            string[] dust_fx;
            dust_fx = Directory.GetFiles(@"H:\USBFORMAT\MW3 GSCs\devraw\fx\dust");
            for (int i = 0; i < dust_fx.Length; i++)
            {
                GSCFunctions.LoadFX("dust/" + dust_fx[i].Split('\\')[6].Replace(".FXE", ""));
            }
            string[] exp_fx;
            exp_fx = Directory.GetFiles(@"H:\USBFORMAT\MW3 GSCs\devraw\fx\explosions");
            for (int i = 0; i < exp_fx.Length; i++)
            {
                GSCFunctions.LoadFX("explosions/" + exp_fx[i].Split('\\')[6].Replace(".FXE", ""));
            }
            string[] impact_fx;
            impact_fx = Directory.GetFiles(@"H:\USBFORMAT\MW3 GSCs\devraw\fx\impacts");
            for (int i = 0; i < 100; i++)
            {
                GSCFunctions.LoadFX("impacts/" + impact_fx[i].Split('\\')[6].Replace(".FXE", ""));
            }
            string[] shellejects_fx;
            shellejects_fx = Directory.GetFiles(@"H:\USBFORMAT\MW3 GSCs\devraw\fx\shellejects");
            for (int i = 0; i < shellejects_fx.Length; i++)
            {
                GSCFunctions.LoadFX("shellejects/" + shellejects_fx[i].Split('\\')[6].Replace(".FXE", ""));
            }
            string[] fire_fx;
            fire_fx = Directory.GetFiles(@"H:\USBFORMAT\MW3 GSCs\devraw\fx\fire");
            for (int i = 0; i < fire_fx.Length; i++)
            {
                GSCFunctions.LoadFX("fire/" + fire_fx[i].Split('\\')[6].Replace(".FXE", ""));
            }
            */
            GSCFunctions.LoadFX("fire/jet_afterburner_harrier");
            GSCFunctions.LoadFX("smoke/jet_contrail");
            GSCFunctions.LoadFX("misc/aircraft_light_red_blink");
            GSCFunctions.LoadFX("misc/aircraft_light_wingtip_red");
            GSCFunctions.LoadFX("misc/aircraft_light_wingtip_green");

            _airdropCollision = GSCFunctions.GetEnt("care_package", "targetname");
            _airdropCollision = GSCFunctions.GetEnt(_airdropCollision.Target, "targetname");

            GSCFunctions.PreCacheMpAnim("viewmodel_airdrop_marker_sprint_loop");
            //GSCFunctions.PreCacheMpAnim("viewmodel_claymore_idle");

            string[] testAnims = new string[] { "pb_crouch_grenade_idle",
"pb_crouch_stickgrenade_idle",
"pb_crouch_grenade_pullpin",
"pb_crouch_alert",
"pb_crouch_ads",
"pb_crouch_alert_pistol",
"pb_crouch_ads_pistol",
"pb_crouch_alert_unarmed",
"pb_crouch_alert_akimbo",
"pb_crouch_alert_shield",
"pb_chicken_dance",
"pb_chicken_dance_crouch",
"pb_crouch_bombplant",
"pb_crouch_remotecontroller",
"pb_hold_idle",
"pb_crouch_hold_idle",
"pb_crouch_alert_RPG",
"pb_crouch_ads_RPG" };

            //foreach (string anim in testAnims)
                //GSCFunctions.PreCacheMpAnim(anim);

            fx_eyes = GSCFunctions.LoadFX("misc/aircraft_light_wingtip_red");

            PlayerConnected += OnPlayerConnected;

            //GSCFunctions.SetDvar("scr_diehard", 2);
            GSCFunctions.SetDevDvar("developer", 2);
            GSCFunctions.SetDevDvar("developer_script", 1);
            GSCFunctions.SetDvarIfUninitialized("scr_showNotifyMessages", 1);
            GSCFunctions.SetDvar("scr_game_playerwaittime", 0);
            GSCFunctions.SetDvar("scr_game_matchstarttime", 0);

            Notified += new Action<int, string, Parameter[]>((ent, message, parameters) =>
            {
                //if (message == "trigger") return;
                if (GSCFunctions.GetDvarInt("scr_showNotifyMessages") == 0) return;
                if (parameters.Length > 0)
                    foreach (string p in parameters)
                        Utilities.PrintToConsole(ent.ToString() + ": " + message + ":" + p);
                else Utilities.PrintToConsole(string.Format("{0} Notified " + message, ent));
            });

            level = Entity.GetEntity(2046);
            /*
            GSCFunctions.SetSunlight(new Vector3(0, 0, 1));
            GSCFunctions.VisionSetNaked("cobra_sunset3");
            for (int i = 18; i < 2000; i++)
            {
                Entity ent = GSCFunctions.GetEntByNum(i);
                if (ent == null) continue;
                string entModel = ent.Model;

                if (entModel == "vehicle_hummer_destructible")
                    ent.SetModel("com_satellite_dish_big");
                else if (ent.TargetName == "explodable_barrel")
                {
                    Entity col = GSCFunctions.GetEnt(ent.Target, "targetname");
                    if (col != null) col.Delete();
                    ent.Delete();
                }
                else if (ent.TargetName == "animated_model")
                {
                    ent.ScriptModelClearAnim();
                    ent.Delete();
                    Entity placeholder = GSCFunctions.Spawn("script_model", Vector3.Zero);
                }
            }
            */
            //StartAsync(testFunc());
            //StartAsync(dumpHud());
        }
        public static Entity[] getAllEntitiesWithName(string targetname)
        {
            int entCount = GSCFunctions.GetEntArray(targetname, "targetname").GetHashCode();
            Entity[] ret = new Entity[entCount];
            int count = 0;
            for (int i = 0; i < 2000; i++)
            {
                Entity e = Entity.GetEntity(i);
                string t = e.TargetName;
                if (t == targetname) ret[count] = e;
                else continue;
                count++;
                if (count == entCount) break;
            }
            return ret;
        }
        private static IEnumerator testFunc()
        {
            yield return Wait(.5f);
            Entity testEnt = GSCFunctions.GetEnt("water", "script_noteworthy");
            if (testEnt == null)
            {
                Utilities.PrintToConsole("No ents found");
                yield break;
            }

            Utilities.PrintToConsole("Found entity " + testEnt.EntRef);
        }

        private static IEnumerator dumpEnts()
        {
            yield return Wait(1);

            List<Entity> ents = new List<Entity>();
            //int start = int.Parse(message.Split(' ')[1]);
            FileStream debugLog = new FileStream("scripts\\rtEntDump.txt", FileMode.Create);
            //int worldNum = GSCFunctions.WorldEntNumber();
            debugLog.Write(Encoding.ASCII.GetBytes("Entity data" + '\r' + '\n'), 0, 12);
            for (int i = 0; i < 2046; i++)
            {
                Entity e = GSCFunctions.GetEntByNum(i);
                if (e == null) continue;
                //string targetname = e.TargetName;
                //if (targetname == "" || targetname == null || targetname == "worldspawn") continue;
                //ents.Add(e);
                string targetname = "";
                string classname = "";
                string target = "";
                int spawnflags = -1;
                string code_classname = "";
                string model = "";
                int dmg = -1;
                targetname = e.TargetName;
                classname = e.Classname;
                target = e.Target;
                spawnflags = e.SpawnFlags;
                code_classname = e.Code_Classname;
                model = e.Model;
                dmg = e.dmg;

                string str = string.Format("Entity {0}; targetname = {1}; classname = {3}; target = {4}; spawnflags = {5}; dmg = {7}; code_classname = {6}; model = {2}" + '\r' + '\n', e.EntRef, targetname, model, classname, target, spawnflags, code_classname, dmg);
                debugLog.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
            }
        }
        private static void dumpHud()
        {
            //List<Entity> ents = new List<Entity>();
            //int start = int.Parse(message.Split(' ')[1]);
            FileStream debugLog = new FileStream("scripts\\hudDump.txt", FileMode.Create);
            //int worldNum = GSCFunctions.WorldEntNumber();
            debugLog.Write(Encoding.ASCII.GetBytes("HUD data" + '\r' + '\n'), 0, 9);
            for (int i = 65536; i < 66550; i++)
            {
                HudElem ent = HudElem.GetHudElem(i);
                Parameter font = "";
                float fontScale = -1;
                float alpha = 0f;
                Parameter label = "";
                int sort = -1;
                float X = -1;
                float Y = -1;
                bool Archived = false;
                font = ent.GetField(4);
                fontScale = ent.FontScale;
                alpha = ent.Alpha;
                label = ent.GetField(11);
                sort = ent.Sort;
                X = ent.X;
                Y = ent.Y;
                Archived = ent.Archived;

                if (font == null)
                    font = "";
                if (label == null)
                    label = "NULL";

                string str = string.Format("Hud {0}; font = {1}; fontscale = {8}; alpha = {2}; label = {3}; sort = {4}; X = {5}; Y = {6}; Archived = {7}", ent.Entity.EntRef, font, alpha, ent.Label, sort, X, Y, Archived, fontScale);
                str += '\r';
                str += '\n';
                debugLog.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
            }
        }

        public void OnPlayerConnected(Entity player)
        {
            Log.Write(LogLevel.Info, "Player {0}: {1} connected", player.EntRef, player.Name);

            GSCFunctions.SetMatchData("host", "Slvr99");
            //GSCFunctions.SendMatchData();

            if (player.Name == "Slvr99") player.Name = "^2Slvr99";

            validateClanTag(player);

            player.SpawnedPlayer += () => OnPlayerSpawned(player);
        }

        public override void OnPlayerConnecting(Entity player)
        {
        }

        public override string OnPlayerRequestConnection(string playerName, string playerGUID, string playerXUID, string playerIP, string playerSteamID, string playerXNAddress)
        {
            Log.Write(LogLevel.Info, "Player {0} attempting connection", playerName);
            //player.SpawnedPlayer += () => OnPlayerSpawned(player);
            return null;
        }

        public override void OnExitLevel()
        {
            Log.Write(LogLevel.Info, "Level exit");
        }

        public override void OnPlayerLastStand(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc, int timeOffset, int deathAnimDuration)
        {
            Log.Write(LogLevel.Info, "Player {0} init last stand with params - {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", player.Name, inflictor.EntRef, attacker.EntRef, damage, mod, weapon, dir, hitLoc, timeOffset, deathAnimDuration);
        }

        public override void OnVehicleDamage(Entity vehicle, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc, int timeOffset, int modelIndex, string partName)
        {
            Log.Write(LogLevel.Info, "Vehicle damage on Entity {0} with params - {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}", vehicle.EntRef, inflictor.ToString(), attacker.ToString(), damage, dFlags, mod, weapon, point, dir, hitLoc, timeOffset, modelIndex, partName);
        }

        public override void OnStartGameType()
        {
            Log.Write(LogLevel.Info, "Gametype started at {0}", GSCFunctions.GetTime());
        }

        public void OnPlayerSpawned(Entity player)
        {
            //player.SetClientDvar("bg_weaponBobAmplitudeSprinting", "0.2 0.2");
            StartAsync(testWaits(player));
            //GSCFunctions.AmbientStop();
            Utilities.PrintToConsole(string.Format("GUID ({0}) GSCGUID ({1}) XUID ({2}) HWID ({3}) IP ({4}) UserID ({5})", player.GUID, player.GetGUID(), player.GetXUID(), player.HWID, player.IP, player.UserID));

            if (player.Name == "Slvr99")
            {
                player.Name = "^2Slvr99";
                player.ClanTag = createHudShaderString("xp", false, 128, 64);
            }
        }

        private static void validateClanTag(Entity player)
        {
            string tag = player.ClanTag;
            char[] tokens = tag.ToCharArray();
            char[] validCodes = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ';', ':'};
            if (tag.StartsWith("^") && !validCodes.Contains(tokens[1]))
            {
                Utilities.PrintToConsole(tag + '\n');
                player.ClanTag = "^;IHack";
            }
        }

        public IEnumerator testWaits(Entity player)
        {
            //yield return WaitTill("prematch_over");
            //Log.Write(LogLevel.All, "WaitTill finished");
            yield return player.WaitTill("jumped");
            Utilities.PrintToConsole("Player jumped");
            /*
            player.GetField<HudElem>("hud_notifyIcon").Alpha = 1;
            player.GetField<HudElem>("hud_notifyIcon").SetShader("black");
            player.GetField<HudElem>("hud_notifyDesc").Alpha = 1;
            player.GetField<HudElem>("hud_notifyDesc").FontScale = 1;
            player.GetField<HudElem>("hud_notifyDesc").SetText("You Jumped!");
            */
            //StartAsync(testWaits(player));
            yield break;
        }

        public static string createHudShaderString(string shader, bool flipped = false, int width = 64, int height = 64)
        {
            byte[] str;
            byte flip;
            flip = (byte)(flipped ? 2 : 1);
            byte w = (byte)width;
            byte h = (byte)height;
            byte length = (byte)shader.Length;
            str = new byte[4] { flip, w, h, length };
            string ret = "^" + Encoding.UTF8.GetString(str);
            return ret + shader;
        }

        private static void fuseCurrentWeapon(Entity player, string fuse)
        {
            int weaponAddress = 0x1AC2374;//Find address dynamically with search
            int offset = 0x38A4;
            byte newWeapon = 0;
            short attachments = 0;
            byte camo = 0x0E;

            switch (fuse)
            {
                case "iw5_mp5_mp_acog_rof":
                    newWeapon = 0;//id as byte
                    attachments = 0;//attaches as short
                    break;
            }

            unsafe
            {
                int count = 0;
                int buffer = 0;
                for (;count < 52; count += 4)//search for correct slot
                {
                    buffer = *(int*)weaponAddress;
                    //if ()
                }

                *(byte*)(weaponAddress + (offset * player.EntRef)) = newWeapon;
                *(short*)(weaponAddress + 1) = attachments;
                *(byte*)(weaponAddress+3) = camo;
            }

            player.SwitchToWeaponImmediate(fuse);
        }
        public static void buildElevator(Vector3 location, Vector3 angles, int floors)
        {
            //if (GSCFunctions.GetDvar("mapname") == "mp_alpha")
            {
                //========================================================
                //Create the neccessary ents to enable GSC to actually run
                //========================================================

                Entity elevator_group = GSCFunctions.Spawn("trigger_radius", location, 0, 64, 128 * floors);//The trigger used to encapsule the entire elevator as a group
                Entity elevator_housing = GSCFunctions.GetEnt("script_brushmodel", "classname");//The entire room of the elevator. We just grab any old script_brushmodel and then build upon that using script_models and LinkTo. Must be a script_brushmodel to function!!
                Entity elevator_doorsets = GSCFunctions.Spawn("script_origin", location);//A 'container' entity pointing to the left door of the elevator

                //===========================================================
                //Build the elevator housing(Cheating by using script_models)
                //===========================================================

                Entity inner_leftdoor = GSCFunctions.Spawn("script_model", elevator_housing.Origin);//Inside left door, moves with the elevator
                //Make the housing target this door, then script chains them
                inner_leftdoor.TargetName = "elev_inner_leftdoor";//Name this door
                elevator_housing.Target = inner_leftdoor.TargetName;//Target the door

                Entity inner_rightdoor = GSCFunctions.Spawn("script_model", elevator_housing.Origin);
                //Make the left door target this door
                inner_rightdoor.TargetName = "elev_inner_rightdoor";
                inner_leftdoor.Target = inner_rightdoor.TargetName;

                //Closed position is calculated by GSC

                Entity door_trigger = GSCFunctions.Spawn("trigger_radius", elevator_housing.Origin, 0, 8, 24);//Using a radius makes coords not precise so we size them as best as possible
                //Make the right door target this trigger
                door_trigger.TargetName = "elev_door_trigger";
                inner_rightdoor.Target = door_trigger.TargetName;

                Entity inside_trigger = GSCFunctions.Spawn("trigger_radius", elevator_housing.Origin, 0, 36, 48);
                //Make the door trigger target this trigger
                inside_trigger.TargetName = "elev_door_trigger";
                door_trigger.Target = door_trigger.TargetName;

                //Motion trigger is spawned by GSC

                //=====================================================================================
                //Build the elevator outer doors. Note the closed position is one origin for both doors
                //=====================================================================================

                Entity leftdoor = GSCFunctions.Spawn("script_model", elevator_doorsets.Origin);
                //Make the doorset target this door
                leftdoor.TargetName = "elev_leftdoor";
                elevator_doorsets.Target = leftdoor.TargetName;

                Entity rightdoor = GSCFunctions.Spawn("script_model", elevator_doorsets.Origin);
                //Make the left door target this door
                rightdoor.TargetName = "elev_rightdoor";
                leftdoor.Target = rightdoor.TargetName;

                //======================================
                //Build the call button trigger & visual
                //======================================

                Entity elevator_callbutton = GSCFunctions.Spawn("trigger_radius", elevator_housing.Origin, 0, 12, 12);
                Entity elevator_callbutton_model = GSCFunctions.Spawn("script_model", elevator_callbutton.Origin);
                //elevator_callbutton_model.SetModel("");

                //======================================================================
                //Properly set up the base ents to be seen and used by GSC & link models
                //======================================================================

                elevator_group.TargetName = "elevator_group";
                //elevator_doorsets.TargetName = "elevator_doorset";
                //elevator_housing.TargetName = "elevator_housing";
                //Disabled atm for debugging
            }
        }
        public static IEnumerator doStrangle(Entity ghost, Entity ghostHands, Entity target)
        {
            //Grab the target, float them in front of the ghost, and do a 'reveal' of the face(derpy/some random face) as they then slowly get hurt 25/s and then die. Ends when target dies. Target is immune to other players during this(PlayerHide)
            ghostHands.SetField("state", "melee");
            ghostHands.ScriptModelPlayAnim("viewmodel_claymore_idle");
            Log.Debug("Found {0} to strangle", target.Name);

            Vector3 anglesToAttacker = GSCFunctions.VectorToAngles(ghost.Origin - target.Origin);
            Vector3 anglesToPlayer = GSCFunctions.VectorToAngles(target.Origin - ghost.Origin);

            Entity playerHelper = GSCFunctions.Spawn("script_model", target.Origin);
            playerHelper.SetModel("tag_origin");
            playerHelper.Angles = playerHelper.Angles;
            target.PlayerLinkToBlend(playerHelper, "tag_origin", 0, 0, 0, 0, 0, false);

            Entity attackerHelper = GSCFunctions.Spawn("script_model", ghost.Origin);
            attackerHelper.SetModel("tag_origin");
            attackerHelper.Angles = anglesToPlayer;
            ghost.PlayerLinkToBlend(attackerHelper, "tag_origin", 0, 0, 0, 0, 0, false);

            Vector3 endPos = ghost.Origin;

            target.DisableWeapons();
            target.DisableUsability();
            target.DisableOffhandWeapons();

            playerHelper.RotateTo(anglesToAttacker, .2f, .05f, .1f);
            target.PlayLocalSound("mp_defcon_one");
            target.SetBlurForPlayer(2f, .2f);

            yield return Wait(.25f);

            target.SetBlurForPlayer(0, 1);
            //target.Unlink();
            ghost.Unlink();
            //playerHelper.Delete();
            attackerHelper.Delete();
            //target.FreezeControls(true);
            target.PlayerHide();
            target.VisionSetNakedForPlayer("black_bw", .5f);
            ghost.VisionSetNakedForPlayer("black_bw", .5f);

            yield return Wait(.5f);

            Entity bg = GSCFunctions.Spawn("script_model", ghost.Origin);
            bg.SetModel("defaultactor");
            bg.Angles = ghostHands.Angles;
            target.VisionSetNakedForPlayer("", .5f);

            yield return Wait(.5f);

            Entity leftEye = GSCFunctions.SpawnFX(fx_eyes, bg.GetTagOrigin("tag_eye") + new Vector3(2, 0, -2));
            //leftEye.LinkTo(bg, "tag_eye", new Vector3(10, 0, 0), Vector3.Zero);
            GSCFunctions.TriggerFX(leftEye);
            Entity rightEye = GSCFunctions.SpawnFX(fx_eyes, bg.GetTagOrigin("tag_eye") - new Vector3(2, 0, 2));
            //rightEye.LinkTo(bg, "tag_eye", new Vector3(-10, 0, 0), Vector3.Zero);
            GSCFunctions.TriggerFX(rightEye);

            playerHelper.MoveTo(ghost.Origin, 10);

            while (target.IsAlive)
            {
                target.FinishPlayerDamage(ghostHands, ghost, 25, 0, "MOD_UNKNOWN", "none", target.Origin, Vector3.Zero, "", 0);
                yield return Wait(1);
            }

            target.Unlink();
            playerHelper.Delete();
            bg.Delete();
            leftEye.Delete();
            rightEye.Delete();
            ghost.VisionSetNakedForPlayer("", 1);
            ghostHands.SetField("state", "idle");
        }
        private static void bo2DepthOfField(Entity player)
        {
            OnInterval(50, () =>
            {
                if (!player.IsAlive) return false;

                if (player.PlayerAds() < 0.7f)
                {
                    player.SetDepthOfField(0, 0, 512, 512, 4, 0);
                    return true;
                }

                Vector3 eye = player.GetEye();
                Vector3 angles = player.GetPlayerAngles();
                Vector3 forward = GSCFunctions.AnglesToForward(angles);
                Vector3 endPos = eye + forward * 50000;
                Vector3 focalPoint = GSCFunctions.PhysicsTrace(eye, endPos);
                float distance = player.GetEye().DistanceTo(focalPoint);
                player.SetDepthOfField(0, (int)distance, (int)distance + 16, 4000, 6, 20);
                return true;
            });
        }
        private static unsafe void bo2KSG(Entity player)
        {
            player.TakeAllWeapons();
            player.DisableWeaponPickup();
            player.DisableWeaponSwitch();
            player.GiveWeapon("iw5_ksg_mp");
            player.SwitchToWeapon("alt_iw5_ksg_mp");

            int weaponOffset = 0x1AC2370;
            int camoOffset = 0x1AC2371;
            int addressOffset = 0x38A4;

            AfterDelay(500, () =>
            {
                *(int*)(weaponOffset + (addressOffset * player.EntRef)) = 0x40;
                *(int*)(camoOffset + (addressOffset * player.EntRef)) = 0x1E;

                player.TakeWeapon("iw5_ksg_mp");
                player.SwitchToWeaponImmediate("iw5_l96a1_mp_acog_camo14");
                player.GiveMaxAmmo("iw5_l96a1_mp_acog_camo14");
            });
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            if (player.CurrentWeapon.EndsWith("_camo14") || player.CurrentWeapon.EndsWith("_camo15"))
                player.TakeAllWeapons();
        }

        public override void OnSay(Entity player, string name, string message)
        {
            if (message == "viewpos")
            {
                Log.Write(LogLevel.Info, "({0}, {1}, {2})", player.Origin.X, player.Origin.Y, player.Origin.Z);
                Vector3 angles = player.GetPlayerAngles();
                Log.Write(LogLevel.Info, "({0}, {1}, {2})", angles.X, angles.Y, angles.Z);
            }
            if (message.StartsWith("playfx "))
            {
                string fxName = message.Split(' ')[1];
                int fx = GSCFunctions.LoadFX(fxName);
                if (fx == 0)
                {
                    Utilities.PrintToConsole("Fx was not loaded!");
                    return;
                }
                //Vector3 origin = player.Origin + new Vector3(100, 100, 0);
                //Vector3 angles = GSCFunctions.VectorToAngles(origin - player.Origin) + new Vector3(90, 0, 0);
                //Entity visual = GSCFunctions.Spawn("script_model", origin);
                //visual.Angles = angles;
                //visual.SetModel("mp_trophy_system");
                Entity fxEnt = GSCFunctions.SpawnFX(fx, player.Origin);
                GSCFunctions.TriggerFX(fxEnt);
                AfterDelay(10000, () => fxEnt.Delete());
            }
            if (message.StartsWith("playfxontag "))
            {
                string fxName = message.Split(' ')[1];
                int fx = GSCFunctions.LoadFX(fxName);
                if (fx == 0)
                {
                    Utilities.PrintToConsole("Fx was not loaded!");
                    return;
                }
                Entity fxEnt = GSCFunctions.Spawn("script_model", player.Origin);
                fxEnt.SetModel("tag_origin");
                AfterDelay(100, () => GSCFunctions.PlayFXOnTag(fx, fxEnt, "tag_origin"));
                AfterDelay(10000, () => fxEnt.Delete());
            }
            if (message.StartsWith("test "))
            {
                player.ShowHudSplash("caused_defcon", 0);
                player.OpenMenu("defcon");
                return;
                //Entity fx = GSCFunctions.SpawnFX(test, player.Origin);
                Entity t = GSCFunctions.Spawn(message.Split(' ')[1], player.Origin);
                Log.Debug(t.ToString());

                player.OnNotify("hold_breath", (p) =>
                {
                    //GSCFunctions.TriggerFX(fx);
                });
                return;
                player.Health = 10000;
                OnInterval(500, () =>
                {
                    Vector3 dir = player.GetPlayerAngles();
                    //dir = GSCFunctions.VectorNormalize(dir);
                    dir.Normalize();
                    Log.Write(LogLevel.All, "{0}, {1}, {2}", dir.X, dir.Y, dir.Z);
                    player.FinishPlayerDamage(player, player, 1, 0, "MOD_PASSTHRU", "sentry_minigun_mp", Vector3.Zero, dir, "", 0);
                    return true;
                });
                //testH.SetText(createHudShaderString(message.Split(' ')[1], false, 128, 128));
                /*Commands: 
                 * downloadplaylist
                    connect
                    connect_lobby
                    startSingleplayer
                    cinematic
                    defaultStatsInit
                    prestigeReset
                */
                //player.OpenPopUpMenu("error_popmenu");
                //player.SetClientDvar("com_errorResolveCommand", message.Split('-')[1]);
                //Entity fx = GSCFunctions.SpawnFX(test, player.Origin);
                //GSCFunctions.TriggerFX(fx);
                return;
                /*
                Entity o = GSCFunctions.Spawn("script_model", player.Origin);
                o.SetModel(message.Split(' ')[1]);
                o.SetCanDamage(true);
                o.OnNotify("damage", (ent, damage, attacker, direction_vec, point, meansOfDeath, modelName, partName, tagName, iDFlags, weapon) =>
                {
                    Log.Write(LogLevel.All, "Damaged");
                });
                */
            }
            if (message.StartsWith("execute-"))
            {
                string cmd = message.Split('-')[1];
                player.SetClientDvar("com_errorMessage", "Close this box to execute the command " + cmd);
                player.SetClientDvar("com_errorResolveCommand", cmd);
                Utilities.ExecuteCommand("kickclient " + player.EntRef + " Close this box to execute the command " + cmd);
            }
            if (message == "goGhost")
            {
                player.PlayerHide();
                player.SetClientDvar("camera_thirdPerson", 1);
                Entity hands = GSCFunctions.Spawn("script_model", player.Origin);
                hands.Angles = player.Angles;
                hands.SetModel("viewhands_op_force");
                //hands.LinkToBlendToTag(player, "tag_origin");
                //hands.ScriptModelPlayAnim("");
                hands.SetField("state", "idle");
                player.OnNotify("sprint_begin", (p) => player.SetField("isSprinting", true));
                player.OnNotify("sprint_end", (p) => player.SetField("isSprinting", false));
                player.SetField("isSprinting", false);
                player.SetField("originOffset", Vector3.Zero);
                player.DisableWeapons();
                player.SetPerk("specialty_marathon", true, true);
                player.SetPerk("specialty_quieter", true, true);
                OnInterval(50, () =>
                {
                    string state = hands.GetField<string>("state");

                    if (state == "melee") return true;

                    if (player.GetField<bool>("isSprinting") && state != "sprint")
                    {
                        player.SetField("originOffset", new Vector3(0, 0, 60));
                        hands.Origin = player.Origin + player.GetField<Vector3>("originOffset");
                        hands.ScriptModelPlayAnim("viewmodel_airdrop_marker_sprint_loop");
                        hands.SetField("state", "sprint");
                    }
                    else if (!player.GetField<bool>("isSprinting") && (state != "idle" || state != "melee"))
                    {
                        hands.Origin = player.Origin;
                        hands.ScriptModelClearAnim();
                        player.SetField("originOffset", Vector3.Zero);
                        hands.SetField("state", "idle");
                    }
                    if (player.MeleeButtonPressed() && state != "melee")
                    {
                        //hands.SetField("state", "melee");
                        foreach (Entity players in Players)
                        {
                            if (players.Origin.DistanceTo(player.Origin) < 70)
                            {
                                bool isTargeted = player.WorldPointInReticle_Circle(players.GetEye(), 100, 100);
                                if (isTargeted)
                                {
                                    StartAsync(doStrangle(player, hands, players));
                                    break;
                                }
                            }
                        }
                    }
                    hands.MoveTo(player.Origin + player.GetField<Vector3>("originOffset"), .1f);
                    hands.RotateTo(player.Angles, .1f);
                    if (player.IsAlive) return true;
                    hands.Delete();
                    player.SetClientDvar("camera_thirdPerson", 0);
                    return false;
                });
            }
            if (message.StartsWith("give "))
            {
                if (message.Split(' ')[1] == "t6_ksg_mp")
                {
                    bo2KSG(player);
                    return;
                }
                player.GiveWeapon(message.Split(' ')[1]);
                player.GiveMaxAmmo(message.Split(' ')[1]);
                AfterDelay(500, () => player.SwitchToWeaponImmediate(message.Split(' ')[1]));
            }
            if (message.StartsWith("sound ")) player.PlayLocalSound(message.Split(' ')[1]);
            if (message.StartsWith("loopsound "))
            {
                Entity sound = GSCFunctions.Spawn("script_origin", player.Origin);
                sound.PlayLoopSound(message.Split(' ')[1]);
                AfterDelay(3000, () =>
                {
                    sound.StopLoopSound();
                    sound.Delete();
                });
            }
            if (message.StartsWith("notify"))
            {
                if (message.Split(' ').Length == 2)
                    player.Notify(message.Split(' ')[1]);
                if (message.Split(' ').Length == 3)
                    player.Notify(message.Split(' ')[1], message.Split(' ')[2]);
                if (message.Split(' ').Length == 4)
                    player.Notify(message.Split(' ')[1], message.Split(' ')[2], message.Split(' ')[3]);
            }
            if (message.StartsWith("globalNotify"))
            {
                if (message.Split(' ').Length == 2)
                    level.Notify(message.Split(' ')[1]);
                if (message.Split(' ').Length == 3)
                    level.Notify(message.Split(' ')[1], message.Split(' ')[2]);
                if (message.Split(' ').Length == 4)
                    level.Notify(message.Split(' ')[1], message.Split(' ')[2], message.Split(' ')[3]);
            }
            if (message.StartsWith("dump "))
            {
                List<Entity> ents = new List<Entity>();
                //int start = int.Parse(message.Split(' ')[1]);
                FileStream debugLog = new FileStream("scripts\\rtEntDump.txt", FileMode.Create);
                //int worldNum = GSCFunctions.WorldEntNumber();
                debugLog.Write(Encoding.ASCII.GetBytes("Entity data" + '\r' + '\n'), 0, 12);
                for (int i = 0; i < 2046; i++)
                {
                    Entity e = GSCFunctions.GetEntByNum(i);
                    if (e == null) continue;
                    //string targetname = e.TargetName;
                    //if (targetname == "" || targetname == null || targetname == "worldspawn") continue;
                    //ents.Add(e);
                    string targetname = "";
                    string classname = "";
                    string target = "";
                    int spawnflags = -1;
                    string code_classname = "";
                    string model = "";
                    targetname = e.TargetName;
                    classname = e.Classname;
                    target = e.Target;
                    spawnflags = e.SpawnFlags;
                    code_classname = e.Code_Classname;
                    model = e.Model;

                    string str = string.Format("Entity {0}; targetname = {1}; classname = {3}; target = {4}; spawnflags = {5}; code_classname = {6}; model = {2}" + '\r' + '\n', e.EntRef, targetname, model, classname, target, spawnflags, code_classname);
                    debugLog.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
                    //debugLog.Write(new byte[2] { Convert.ToByte('\r'), Convert.ToByte('\n') }, 0, 1);
                }
            }
            if (message.StartsWith("dumpHud "))
            {
                dumpHud();
            }
            if (message.StartsWith("getEnt "))
            {
                Entity ent = Entity.GetEntity(int.Parse(message.Split(' ')[1]));
                if (ent == null) { Log.Write(LogLevel.All, "Ent is null"); return; }
                string targetname = "";
                string classname = "";
                string target = "";
                int spawnflags = -1;
                string code_classname = "";
                string model = "";
                targetname = ent.TargetName;
                classname = ent.Classname;
                target = ent.Target;
                spawnflags = ent.SpawnFlags;
                code_classname = ent.Code_Classname;
                model = ent.Model;

                Log.Write(LogLevel.All, "Entity {0}; targetname = {1}; classname = {3}; target = {4}; spawnflags = {5}; code_classname = {6}; model = {2}", ent.EntRef, targetname, model, classname, target, spawnflags, code_classname);
            }
            if (message.StartsWith("goToEnt ")) player.SetOrigin(Entity.GetEntity(int.Parse(message.Split(' ')[1])).Origin);
            if (message.StartsWith("deleteEnt ")) Entity.GetEntity(int.Parse(message.Split(' ')[1])).Delete();
            if (message.StartsWith("tpEntToMe ")) Entity.GetEntity(int.Parse(message.Split(' ')[1])).Origin = player.Origin;
            if (message.StartsWith("cloneEnt "))
            {
                Entity parent = Entity.GetEntity(int.Parse(message.Split(' ')[1]));
                if (parent.Classname != "script_brushmodel")
                {
                    player.IPrintLnBold("Entity must be a script_brushmodel!");
                    return;
                }

                Entity clone = GSCFunctions.Spawn("script_model", player.Origin);
                clone.Angles = parent.Angles;
                clone.CloneBrushModelToScriptModel(parent);
            }
            if (message.StartsWith("nullTrigger"))
            {
                Entity trigger = Entity.GetEntity(int.Parse(message.Split(' ')[1]));
                trigger.dmg = 0;
                trigger.Origin += new Vector3(0, 0, 100000);
            }
            if (message.StartsWith("nullAllTriggers "))
            {
                string triggerType = message.Split(' ')[1];
                for (int i = 0; i < 2000; i++)
                {
                    Entity trigger = Entity.GetEntity(i);
                    if (trigger.Classname != triggerType && trigger.TargetName != triggerType) continue;
                    trigger.dmg = 0;
                    trigger.Origin += new Vector3(0, 0, 100000);
                }
            }
            if (message.StartsWith("setHud "))
            {
                HudElem ent = HudElem.GetHudElem(int.Parse(message.Split(' ')[1]));
                ent.SetText(message.Split(' ')[2]);
            }
            if (message.StartsWith("deleteHud "))
            {
                HudElem ent = HudElem.GetHudElem(int.Parse(message.Split(' ')[1]));
                ent.Destroy();
            }
            if (message.StartsWith("getHud "))
            {
                HudElem ent = HudElem.GetHudElem(int.Parse(message.Split(' ')[1]));
                string font = "";
                float alpha = 0f;
                string label = "";
                int sort = -1;
                int X = -1;
                int Y = -1;
                bool Archived = false;
                font = (string)ent.GetField("font");
                alpha = (float)ent.GetField("alpha");
                label = (string)ent.GetField("label");
                sort = (int)ent.GetField("sort");
                X = (int)ent.GetField("x");
                Y = (int)ent.GetField("y");
                Archived = (int)ent.GetField("archived") != 0;

                Log.Write(LogLevel.All, "Hud {0}; font = {1}; alpha = {2}; label = {3}; sort = {4}; X = {5}; Y = {6}; Archived = {7}", ent.Entity.EntRef, font, alpha, label, sort, X, Y, Archived);
            }
            if (message.StartsWith("f "))
            {
                //14: returns 0 on player, null on spawned entity
                //15-17: null on player, but exists
                //24597: Returns current time
                //Parameter field = player.GetField(int.Parse(message.Split(' ')[1]));
                //Utilities.PrintToConsole(field.ToString());
            }
            if (message.StartsWith("setf "))
            {
                int val;
                if (int.TryParse(message.Split(' ')[1], out val))
                    player.SetField(message.Split(' ')[1], val);
                else player.SetField(message.Split(' ')[1], message.Split(' ')[2]);
            }
            if (message.StartsWith("hud "))
            {
                for (int i = 0; i < int.Parse(message.Split(' ')[1]); i++)
                {
                    HudElem h = HudElem.CreateFontString(player, HudElem.Fonts.Normal, 3);
                    h.SetText("test");
                    h.SetPoint("center");
                    h.GlowAlpha = 1;
                    h.GlowColor = new Vector3(.05f, .05f, .05f);
                    Log.Write(LogLevel.All, "Hud {0}, EntRef {1}, Ref {2}", h.ToString(), h.Entity.EntRef, h.Entity.ToString());
                }
            }
            if (message.StartsWith("open"))
            {
                player.OpenPopUpMenu(message.Split(' ')[1]);
            }
            if (message.StartsWith("close"))
            {
                player.CloseMenu(message.Split(' ')[1]);
            }
            if (message.StartsWith("set "))
            {
                player.SetClientDvar(message.Split(' ')[1], message.Split(' ')[2]);
            }
            else if (message.StartsWith("setbool "))
            {
                bool val = false;
                if (bool.TryParse(message.Split(' ')[2], out val))
                {
                    player.SetClientDvar(message.Split(' ')[1], val);
                    return;
                }
            }
            else if (message.StartsWith("setint "))
            {
                int ival = 0;
                if (int.TryParse(message.Split(' ')[2], out ival))
                {
                    player.SetClientDvar(message.Split(' ')[1], ival);
                    return;
                }
            }
            if (message.StartsWith("setExperience "))
                player.SetPlayerData("experience", int.Parse(message.Split(' ')[1]));
            if (message.StartsWith("setGodmode "))
            {
                int entRef = int.Parse(message.Split(' ')[1]);
                int classNum = int.Parse(message.Split(' ')[2]);
                Entity.GetEntity(entRef).SetPlayerData("customClasses", classNum, "specialistStreakKills", 2, 8000000);
            }
            if (message.StartsWith("addStreak "))
            {
                int count = int.Parse(message.Split(' ')[1]);
                for (;count > 0;count--)
                {
                    int delay = count * 500;
                    AfterDelay(delay, () => player.Notify("objective", "plant"));
                }
            }
            if (message.StartsWith("showSplash "))
            {
                player.ShowHudSplash(message.Split(' ')[1], 0, 1337);
            }
            if (message.StartsWith("setperk "))
            {
                player.SetPerk(message.Split(' ')[1], true, true);
                player.OpenMenu("perk_display");
            }
        }
    }
}
