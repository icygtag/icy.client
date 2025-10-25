using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx.Logging;
using dark.efijiPOIWikjek;
using ExitGames.Client.Photon;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaTagScripts;
using GTAG_NotificationLib;
using Malachis_Temp;
using MalachiTemp.Backend;
using MalachiTemp.UI;
using MalachiTemp.Utilities;
using Oculus.Interaction;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static Oculus.Interaction.Context;
using static Photon.Voice.Unity.Recorder;
using Text = UnityEngine.UI.Text;




namespace MalachiTemp.Backend
{
    
    internal class Mods : MonoBehaviour
    {
        // Double click a grey square to open it, click the - in the box to the left of "#region" to close it
        
        #region Shit
        public static void DisableButton(string name)
        {
            GetButton(name).enabled = new bool?(false);
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void PLACEHOLDER()
        {
            // DONT PUT ANYTHING IN HERE
        }
        public static void DrawHandOrbs()
        {
            orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            orb2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(orb.GetComponent<Rigidbody>());
            Destroy(orb.GetComponent<SphereCollider>());
            Destroy(orb2.GetComponent<Rigidbody>());
            Destroy(orb2.GetComponent<SphereCollider>());
            orb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            orb.transform.position = GorillaTagger.Instance.leftHandTransform.position;
            orb2.transform.position = GorillaTagger.Instance.rightHandTransform.position;
            orb.GetComponent<Renderer>().material.color = CurrentGunColor;
            orb2.GetComponent<Renderer>().material.color = CurrentGunColor;
            Destroy(orb, Time.deltaTime);
            Destroy(orb2, Time.deltaTime);
        }

        public static void NoFinger()
        {
            ControllerInputPoller.instance.leftControllerGripFloat = 0f;
            ControllerInputPoller.instance.rightControllerGripFloat = 0f;
            ControllerInputPoller.instance.leftControllerIndexFloat = 0f;
            ControllerInputPoller.instance.rightControllerIndexFloat = 0f;
            ControllerInputPoller.instance.leftControllerPrimaryButton = false;
            ControllerInputPoller.instance.leftControllerSecondaryButton = false;
            ControllerInputPoller.instance.rightControllerPrimaryButton = false;
            ControllerInputPoller.instance.rightControllerSecondaryButton = false;
            ControllerInputPoller.instance.leftControllerPrimaryButtonTouch = false;
            ControllerInputPoller.instance.leftControllerSecondaryButtonTouch = false;
            ControllerInputPoller.instance.rightControllerPrimaryButtonTouch = false;
            ControllerInputPoller.instance.rightControllerSecondaryButtonTouch = false;
        }
        #endregion
        #region Movement
        public static void FlyMeth(float speed)
        {
            if (WristMenu.abuttonDown)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * speed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        public static void Platforms()
        {
            PlatformsThing(invisplat, stickyplatforms);
        }
        public static void Invisableplatforms()
        {
            PlatformsThing(true, false);
        }
        public static void Noclip()
        {
            foreach (MeshCollider m in Resources.FindObjectsOfTypeAll<MeshCollider>())
            {
                if (WristMenu.triggerDownR)
                {
                    m.enabled = false;
                }
                else
                {
                    m.enabled = true;
                }
            }
        }
        public static void AntiReport(Action<VRRig, Vector3> onReport)
        {
            if (!NetworkSystem.Instance.InRoom) return;

            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
            {
                if (line.linePlayer != NetworkSystem.Instance.LocalPlayer) continue;
                Transform report = line.reportButton.gameObject.transform;

                foreach (var vrrig in from vrrig in GorillaParent.instance.vrrigs where !vrrig.isLocal let D1 = Vector3.Distance(vrrig.rightHandTransform.position, report.position) let D2 = Vector3.Distance(vrrig.leftHandTransform.position, report.position) where D1 < threshold || D2 < threshold where !smartarp || SmartAntiReport(line.linePlayer) select vrrig)
                    onReport?.Invoke(vrrig, report.transform.position);
            }
        }

        public static float antiReportDelay;
        public static void AntiReportDisconnect()
        {
            AntiReport((vrrig, position) =>
            {
                NetworkSystem.Instance.ReturnToSinglePlayer();
                RPCProtection();

                if (!(Time.time > antiReportDelay)) return;
                antiReportDelay = Time.time + 1f;
                NotifiLib.SendNotification("<color=grey>[</color><color=purple>ANTI-REPORT</color><color=grey>]</color> " + GetPlayerFromVRRig(vrrig).NickName + " attempted to report you, you have been disconnected.");
            });
        }


        public static void AntiReportJoinRandom()
        {
            AntiReport((vrrig, position) =>
            {
                if (!(Time.time > antiReportDelay)) return;

                Mods.JoinRandom();
                RPCProtection();

                antiReportDelay = Time.time + 1f;
                NotifiLib.SendNotification("<color=grey>[</color><color=purple>ANTI-REPORT</color><color=grey>]</color> " + GetPlayerFromVRRig(vrrig).NickName + " attempted to report you, you have been disconnected and will be reconnected shortly.");
            });
        }
        public static NetPlayer GetPlayerFromVRRig(VRRig p) =>
            p.Creator;

        public static float threshold = 0.35f;
        public static void RPCProtection()
        {
            try
            {
                if (!hasRemovedThisFrame)
                {
                    if (NoOverlapRPCs)
                        hasRemovedThisFrame = true;

                    GorillaNot.instance.rpcErrorMax = int.MaxValue;
                    GorillaNot.instance.rpcCallLimit = int.MaxValue;
                    GorillaNot.instance.logErrorMax = int.MaxValue;

                    PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
                    PhotonNetwork.QuickResends = int.MaxValue;

                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
            catch { Debug.LogWarning("$RPC protection failed, {ex.Message}"); }
        }
        public static bool HasLoaded;
        public static bool hasLoadedPreferences;
        public static bool hasRemovedThisFrame;
        public static bool NoOverlapRPCs = true;
        public static float loadPreferencesTime;
        public static float playTime;
        public static float badAppleTime;
        public static bool smartarp;
        public static int buttonClickTime;
        public static string buttonClickPlayer;
        public static IEnumerator JoinRandomDelay()
        {
            yield return new WaitForSeconds(1.5f);
            JoinRandom();
        }
        public static bool SmartAntiReport(NetPlayer linePlayer) =>
            smartarp && linePlayer.UserId == buttonClickPlayer && Time.frameCount == buttonClickTime && PhotonNetwork.CurrentRoom.IsVisible && !PhotonNetwork.CurrentRoom.CustomProperties.ToString().Contains("MODDED");
        public static void JoinRandom()
        {
            if (PhotonNetwork.InRoom)
            {
                NetworkSystem.Instance.ReturnToSinglePlayer();
                CoroutineManager.RunCoroutine(JoinRandomDelay());
                return;
            }
        }
        public class CoroutineManager : MonoBehaviour // Thanks to ShibaGT for helping with the coroutines
        {
            public static CoroutineManager instance;

            private void Awake() =>
                instance = this;

            public static Coroutine RunCoroutine(IEnumerator enumerator) =>
                instance.StartCoroutine(enumerator);

            public static void EndCoroutine(Coroutine enumerator) =>
                instance.StopCoroutine(enumerator);
        }

        public static void RainbowBracelet()
        {
            BraceletPatch.enabled = true;
            if (!VRRig.LocalRig.nonCosmeticRightHandItem.IsEnabled)
            {
                SetBraceletState(true, false);
                RPCProtection();

                VRRig.LocalRig.nonCosmeticRightHandItem.EnableItem(true);
            }
            List<Color> rgbColors = new List<Color>();
            for (int i = 0; i < 10; i++)
                rgbColors.Add(Color.HSVToRGB((Time.frameCount / 180f + i / 10f) % 1f, 1f, 1f));

            VRRig.LocalRig.reliableState.isBraceletLeftHanded = false;
            VRRig.LocalRig.reliableState.braceletSelfIndex = 99;
            VRRig.LocalRig.reliableState.braceletBeadColors = rgbColors;
            VRRig.LocalRig.friendshipBraceletRightHand.UpdateBeads(rgbColors, 99);

            if (Time.time > isDirtyDelay)
            {
                isDirtyDelay = Time.time + 0.1f;
                VRRig.LocalRig.reliableState.SetIsDirty();
            }
        }
        public static float isDirtyDelay;
        public static void SetBraceletState(bool enable, bool isLeftHand) =>
            GorillaTagger.Instance.myVRRig.SendRPC("EnableNonCosmeticHandItemRPC", RpcTarget.All, enable, isLeftHand);

        public static void RemoveRainbowBracelet()
        {
            BraceletPatch.enabled = false;
            if (!VRRig.LocalRig.nonCosmeticRightHandItem.IsEnabled)
            {
                SetBraceletState(false, false);
                RPCProtection();

                VRRig.LocalRig.nonCosmeticRightHandItem.EnableItem(false);
            }

            VRRig.LocalRig.reliableState.isBraceletLeftHanded = false;
            VRRig.LocalRig.reliableState.braceletSelfIndex = 0;
            VRRig.LocalRig.reliableState.braceletBeadColors.Clear();
            VRRig.LocalRig.UpdateFriendshipBracelet();

            VRRig.LocalRig.reliableState.SetIsDirty();
        }
        public static void GiveBuilderWatch()
        {
            VRRig.LocalRig.EnableBuilderResizeWatch(true);
            RPCProtection();
        }
        public static void unGiveBuilderWatch()
        {
            VRRig.LocalRig.EnableBuilderResizeWatch(false);
            RPCProtection();
        }


        public static void StumpTeleporterEffectSpam()
        {
            if (Time.time > spamDelay)
            {
                spamDelay = Time.time + 0.1f;
                returnOrTeleport = !returnOrTeleport;

                GetObject("Environment Objects/LocalObjects_Prefab/TreeRoom/StumpVRHeadset/VirtualStump_StumpTeleporter/NetObject_VRTeleporter").GetComponent<PhotonView>().RPC("ActivateTeleportVFX", RpcTarget.All, returnOrTeleport, (short)0);
                RPCProtection();
            }
        }
        private static float spamDelay;
        private static bool returnOrTeleport;
        public static GameObject GetObject(string find)
        {
            if (objectPool.TryGetValue(find, out GameObject go))
                return go;

            GameObject tgo = GameObject.Find(find);
            if (tgo != null)
                objectPool.Add(find, tgo);

            return tgo;
        }

        private static readonly Dictionary<string, GameObject> objectPool = new Dictionary<string, GameObject>();

        public static readonly Vector3[] lastLeft = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        public static readonly Vector3[] lastRight = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

        public static void PunchMod()
        {
            int index = -1;
            foreach (var vrrig in GorillaParent.instance.vrrigs.Where(vrrig => !vrrig.isLocal))
            {
                index++;

                Vector3 they = vrrig.rightHandTransform.position;
                Vector3 notthem = VRRig.LocalRig.head.rigTarget.position;
                float distance = Vector3.Distance(they, notthem);

                if (distance < 0.25f)
                    GorillaTagger.Instance.rigidbody.linearVelocity += Vector3.Normalize(vrrig.rightHandTransform.position - lastRight[index]) * 10f;

                lastRight[index] = vrrig.rightHandTransform.position;

                they = vrrig.leftHandTransform.position;
                distance = Vector3.Distance(they, notthem);

                if (distance < 0.25f)
                    GorillaTagger.Instance.rigidbody.linearVelocity += Vector3.Normalize(vrrig.leftHandTransform.position - lastLeft[index]) * 10f;

                lastLeft[index] = vrrig.leftHandTransform.position;
            }
        }

        private static VRRig sithlord;
        private static bool sithright;
        private static float sithdist = 1f;
        public static void Telekinesis()
        {
            if (sithlord == null)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    try
                    {
                        if (!vrrig.isLocal)
                        {
                            if (vrrig.rightIndex.calcT < 0.5f && vrrig.rightMiddle.calcT > 0.5f)
                            {
                                Vector3 dir = vrrig.transform.Find("GorillaPlayerNetworkedRigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").up;
                                Physics.SphereCast(vrrig.rightHandTransform.position + dir * 0.1f, 0.3f, dir, out var Ray, 512f, NoInvisLayerMask());
                                {
                                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                                    if (gunTarget && gunTarget.isLocal)
                                    {
                                        sithlord = vrrig;
                                        sithright = true;
                                        sithdist = Ray.distance;
                                    }
                                }
                            }
                            if (vrrig.leftIndex.calcT < 0.5f && vrrig.leftMiddle.calcT > 0.5f)
                            {
                                Vector3 dir = vrrig.transform.Find("GorillaPlayerNetworkedRigAnchor/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L").up;
                                Physics.SphereCast(vrrig.leftHandTransform.position + dir * 0.1f, 0.3f, dir, out var Ray, 512f, NoInvisLayerMask());
                                {
                                    VRRig gunTarget = Ray.collider.GetComponentInParent<VRRig>();
                                    if (gunTarget && gunTarget.isLocal)
                                    {
                                        sithlord = vrrig;
                                        sithright = false;
                                        sithdist = Ray.distance;
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
            else
            {
                if (sithright ? sithlord.rightIndex.calcT < 0.5f && sithlord.rightMiddle.calcT > 0.5f : sithlord.leftMiddle.calcT < 0.5f && sithlord.leftMiddle.calcT > 0.5f)
                {
                    Transform hand = sithright ? sithlord.rightHandTransform : sithlord.leftHandTransform;
                    Vector3 dir = sithright ? sithlord.transform.Find("GorillaPlayerNetworkedRigAnchor/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R").up : sithlord.transform.Find("GorillaPlayerNetworkedRigAnchor/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L").up;
                    TeleportPlayer(Vector3.Lerp(GorillaTagger.Instance.bodyCollider.transform.position, hand.position + dir * sithdist, 0.1f));
                    GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
                    ZeroGravity();
                }
                else
                    sithlord = null;
            }
        }
        private static int? noInvisLayerMask;
        public static int NoInvisLayerMask()
        {
            noInvisLayerMask = ~(
                1 << LayerMask.NameToLayer("TransparentFX") |
                1 << LayerMask.NameToLayer("Ignore Raycast") |
                1 << LayerMask.NameToLayer("Zone") |
                1 << LayerMask.NameToLayer("Gorilla Trigger") |
                1 << LayerMask.NameToLayer("Gorilla Boundary") |
                1 << LayerMask.NameToLayer("GorillaCosmetics") |
                1 << LayerMask.NameToLayer("GorillaParticle"));

            return noInvisLayerMask ?? GTPlayer.Instance.locomotionEnabledLayers;
        }

        public static void TeleportPlayer(Vector3 pos) // Prevents your hands from getting stuck on trees
        {
            GTPlayer.Instance.TeleportTo(World2Player(pos), GTPlayer.Instance.transform.rotation);
            closePosition = Vector3.zero;
            Mods.lastPosition = Vector3.zero;
            if (VRKeyboard != null)
            {
                VRKeyboard.transform.position = GorillaTagger.Instance.bodyCollider.transform.position;
                VRKeyboard.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            }
        }
        public static GameObject VRKeyboard;
        public static Vector3 closePosition;
        public static Vector3 lastPosition = Vector3.zero;
        public static Vector3 World2Player(Vector3 world) =>
            world - GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.transform.position;
        public static void ZeroGravity() =>
          GorillaTagger.Instance.rigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);

        private static float pullPower = 0.05f;
        private static bool lasttouchleft;
        private static bool lasttouchright;
        public static void PullMod()
        {
            if (((!GTPlayer.Instance.IsHandTouching(true) && lasttouchleft) || (!GTPlayer.Instance.IsHandTouching(false) && lasttouchright)) && ControllerInputPoller.instance.rightGrab)
            {
                Vector3 vel = GorillaTagger.Instance.rigidbody.linearVelocity;
                GTPlayer.Instance.transform.position += new Vector3(vel.x * pullPower, 0f, vel.z * pullPower);
            }
            lasttouchleft = GTPlayer.Instance.IsHandTouching(true);
            lasttouchright = GTPlayer.Instance.IsHandTouching(false);
        }

        



    



    
        public static void AutoWalk()
        {
            // Get the left hand joystick input
            Vector2 leftJoystick = Vector2.zero;
            bool leftJoystickClick = false;

            UnityEngine.XR.InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (leftHandDevice.isValid)
            {
                leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out leftJoystick);
                leftHandDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out leftJoystickClick);
            }

            float armLength = 0.45f;
            float animSpeed = 9f;

            if (leftJoystickClick)
                animSpeed *= 1.5f;

            if (Mathf.Abs(leftJoystick.y) > 0.05f || Mathf.Abs(leftJoystick.x) > 0.05f)
            {
                Transform body = GorillaTagger.Instance.bodyCollider.transform;

                GorillaTagger.Instance.leftHandTransform.position =
                    body.position +
                    body.forward * (Mathf.Sin(Time.time * animSpeed) * (leftJoystick.y * armLength)) +
                    body.right * (Mathf.Sin(Time.time * animSpeed) * (leftJoystick.x * armLength) - 0.2f) +
                    new Vector3(0f, -0.3f + Mathf.Cos(Time.time * animSpeed) * 0.2f, 0f);

                GorillaTagger.Instance.rightHandTransform.position =
                    body.position +
                    body.forward * (-Mathf.Sin(Time.time * animSpeed) * (leftJoystick.y * armLength)) +
                    body.right * (-Mathf.Sin(Time.time * animSpeed) * (leftJoystick.x * armLength) + 0.2f) +
                    new Vector3(0f, -0.3f + Mathf.Cos(Time.time * animSpeed) * -0.2f, 0f);
            }
        }

        public static void FakeOculusMenu()
        {
            if (ControllerInputPoller.instance.leftControllerPrimaryButton)
            {
                NoFinger();
                GTPlayer.Instance.inOverlay = true;
                GTPlayer.Instance.leftControllerTransform.localPosition = new Vector3(238f, -90f, 0f);
                GTPlayer.Instance.rightControllerTransform.localPosition = new Vector3(-190f, 90f, 0f);
                GTPlayer.Instance.leftControllerTransform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-55f, 90f, 0f);
                GTPlayer.Instance.rightControllerTransform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-55f, -49f, 0f);
            }

        }









        #endregion
        #region Rig Mods
        public static void Ghostmonke()
        {
            if (right)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = !ghostMonke;
                if (ghostMonke)
                {
                    DrawHandOrbs();                 
                }
                if (WristMenu.ybuttonDown && !lastHit)
                {
                    ghostMonke = !ghostMonke;
                }
                lastHit = WristMenu.ybuttonDown;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = !ghostMonke;
                if (ghostMonke)
                {
                    DrawHandOrbs();
                }
                if (WristMenu.bbuttonDown && !lastHit)
                {
                    ghostMonke = !ghostMonke;
                }
                lastHit = WristMenu.bbuttonDown;
            }
        }
        public static void Invis()
        {
            if (right)
            {
                if (invisMonke)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(9999f, 9999f, 9999f);
                    DrawHandOrbs();
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
                if (WristMenu.ybuttonDown && !lastHit2)
                {
                    invisMonke = !invisMonke;
                }
                lastHit2 = WristMenu.ybuttonDown;
            }
            else
            {
                if (invisMonke)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(9999f, 9999f, 9999f);
                    DrawHandOrbs();
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
                if (WristMenu.bbuttonDown && !lastHit2)
                {
                    invisMonke = !invisMonke;
                }
                lastHit2 = WristMenu.bbuttonDown;
            }
        }
        #endregion
        #region Visual
        public static void Tracers()
        {
            foreach (Player p in PhotonNetwork.PlayerListOthers)
            {
                VRRig rig = RigShit.GetVRRigFromPlayer(p);
                GameObject g = new GameObject("Line");
                LineRenderer l = g.AddComponent<LineRenderer>();
                l.startWidth = 0.01f;
                l.endWidth = 0.01f;
                l.positionCount = 2;
                l.useWorldSpace = true;
                l.SetPosition(0, GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position);
                l.SetPosition(1, rig.transform.position);
                l.material.shader = Shader.Find("GUI/Text Shader");
                l.startColor = CurrentESPColor;
                l.endColor = CurrentESPColor;
                Destroy(l, Time.deltaTime);
            }
        }
        [Obsolete]
        public static void FPSboost()
        {
            fps = true;
            if (fps)
            {
                QualitySettings.masterTextureLimit = 999999999;
                QualitySettings.masterTextureLimit = 999999999;
                QualitySettings.globalTextureMipmapLimit = 999999999;
                QualitySettings.maxQueuedFrames = 60;
            }
        }

        [Obsolete]
        public static void fixFPS()
        {
            if (fps)
            {
                QualitySettings.masterTextureLimit = default;
                QualitySettings.masterTextureLimit = default;
                QualitySettings.globalTextureMipmapLimit = default;
                QualitySettings.maxQueuedFrames = default;
                fps = false;
            }
        }
        private static readonly Dictionary<VRRig, List<LineRenderer>> boneESP = new Dictionary<VRRig, List<LineRenderer>>();
        public static readonly int[] bones = {
            4, 3, 5, 4, 19, 18, 20, 19, 3, 18, 21, 20, 22, 21, 25, 21, 29, 21, 31, 29, 27, 25, 24, 22, 6, 5, 7, 6, 10, 6, 14, 6, 16, 14, 12, 10, 9, 7
        };
        public static void CasualBoneESP()
        {
            try
            {
                // 🛠️ Local settings (edit these manually or connect them to your menu later)
                bool followTheme = false;      // replaces GetIndex("Follow Menu Theme")
                bool hiddenOnCamera = false;   // replaces GetIndex("Hidden on Camera")
                bool transparentTheme = false; // replaces GetIndex("Transparent Theme")
                bool thinTracers = false;      // replaces GetIndex("Thin Tracers")


                if (GorillaParent.instance?.vrrigs == null)
                    return;

                List<VRRig> removeKeys = new List<VRRig>();

                // 🧹 Clean up missing rigs
                foreach (var kvp in boneESP)
                {
                    if (kvp.Key == null || !GorillaParent.instance.vrrigs.Contains(kvp.Key))
                    {
                        foreach (var lr in kvp.Value)
                        {
                            if (lr != null)
                                UnityEngine.Object.Destroy(lr);
                        }
                        removeKeys.Add(kvp.Key);
                    }
                }
                foreach (var rk in removeKeys)
                    boneESP.Remove(rk);

                // 🦴 Main ESP loop
                foreach (var rig in GorillaParent.instance.vrrigs)
                {
                    if (rig == null || rig.isLocal) continue;
                    if (rig.head?.rigTarget == null || rig.mainSkin?.bones == null) continue;

                    // ✅ Ensure boneESP entry exists
                    if (!boneESP.TryGetValue(rig, out List<LineRenderer> lines))
                    {
                        lines = new List<LineRenderer>();

                        // Head line
                        LineRenderer headLine = rig.head.rigTarget.gameObject.GetComponent<LineRenderer>();
                        if (headLine == null)
                            headLine = rig.head.rigTarget.gameObject.AddComponent<LineRenderer>();

                        headLine.positionCount = 2;
                        headLine.material = new Material(Shader.Find("GUI/Text Shader"));
                        lines.Add(headLine);

                        // Bone lines
                        for (int i = 0; i < bones.Length; i += 2)
                        {
                            int idx0 = bones[i];
                            int idx1 = bones[i + 1];
                            if (idx0 >= rig.mainSkin.bones.Length || idx1 >= rig.mainSkin.bones.Length) continue;
                            if (rig.mainSkin.bones[idx0] == null || rig.mainSkin.bones[idx1] == null) continue;

                            LineRenderer lr = rig.mainSkin.bones[idx0].gameObject.GetComponent<LineRenderer>();
                            if (lr == null)
                                lr = rig.mainSkin.bones[idx0].gameObject.AddComponent<LineRenderer>();

                            lr.positionCount = 2;
                            lr.material = new Material(Shader.Find("GUI/Text Shader"));
                            lines.Add(lr);
                        }

                        boneESP[rig] = lines;
                    }

                    // 🎨 Update all line renderers
                    for (int i = 0; i < lines.Count; i++)
                    {
                        LineRenderer lr = lines[i];
                        if (lr == null) continue;

                        Color color = rig.playerColor;
                        if (followTheme) color = Color.white;
                        if (transparentTheme) color.a = 0.5f;

                        lr.startColor = color;
                        lr.endColor = color;
                        lr.startWidth = thinTracers ? 0.0075f : 0.025f;
                        lr.endWidth = thinTracers ? 0.0075f : 0.025f;
                        if (hiddenOnCamera) lr.gameObject.layer = 19;

                        if (i == 0)
                        {
                            // Head line
                            lr.SetPosition(0, rig.head.rigTarget.position + new Vector3(0f, 0.16f, 0f));
                            lr.SetPosition(1, rig.head.rigTarget.position - new Vector3(0f, 0.4f, 0f));
                        }
                        else
                        {
                            // Bone lines
                            int boneIndex = (i - 1) * 2;
                            if (boneIndex + 1 >= bones.Length) continue;

                            int idx0 = bones[boneIndex];
                            int idx1 = bones[boneIndex + 1];
                            if (idx0 >= rig.mainSkin.bones.Length || idx1 >= rig.mainSkin.bones.Length) continue;
                            if (rig.mainSkin.bones[idx0] == null || rig.mainSkin.bones[idx1] == null) continue;

                            lr.SetPosition(0, rig.mainSkin.bones[idx0].position);
                            lr.SetPosition(1, rig.mainSkin.bones[idx1].position);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Failsafe logger
                UnityEngine.Debug.LogWarning("CasualBoneESP() failed: " + ex);
            }
        }


        #endregion
        #region Save-Load Buttons & Settings
        public static void Save1()
        {
            List<string> list = new List<string>();
            foreach (ButtonInfo buttonInfo in WristMenu.CatButtons1)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null)
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            foreach (ButtonInfo buttonInfo in WristMenu.CatButtons2)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null)
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            foreach (ButtonInfo buttonInfo in WristMenu.CatButtons3)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null)
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            foreach (ButtonInfo buttonInfo in WristMenu.CatButtons4)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null)
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            foreach (ButtonInfo buttonInfo in WristMenu.CatButtons5)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null)
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            foreach (ButtonInfo buttonInfo in WristMenu.CatButtons6)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null)
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            foreach (ButtonInfo buttonInfo in WristMenu.CatButtons7)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null)
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            foreach (ButtonInfo buttonInfo in WristMenu.CatButtons8)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null)
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            foreach (ButtonInfo buttonInfo in WristMenu.CatButtons9)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null)
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            foreach (ButtonInfo buttonInfo in WristMenu.CatButtons10)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null)
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            File.WriteAllLines(WristMenu.FolderName + "\\Saved_Buttons.txt", list);
            NotifiLib.SendNotification("<color=white>[</color><color=blue>SAVE</color><color=white>]</color> <color=white>Saved Buttons Successfully!</color>");
        }
        public static void Load1()
        {
            string[] array = File.ReadAllLines(WristMenu.FolderName + "\\Saved_Buttons.txt");
            foreach (string b in array)
            {
                foreach (ButtonInfo buttonInfo in WristMenu.CatButtons1)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
                foreach (ButtonInfo buttonInfo in WristMenu.CatButtons2)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
                foreach (ButtonInfo buttonInfo in WristMenu.CatButtons3)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
                foreach (ButtonInfo buttonInfo in WristMenu.CatButtons4)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
                foreach (ButtonInfo buttonInfo in WristMenu.CatButtons5)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
                foreach (ButtonInfo buttonInfo in WristMenu.CatButtons6)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
                foreach (ButtonInfo buttonInfo in WristMenu.CatButtons7)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
                foreach (ButtonInfo buttonInfo in WristMenu.CatButtons8)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
                foreach (ButtonInfo buttonInfo in WristMenu.CatButtons9)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
                foreach (ButtonInfo buttonInfo in WristMenu.CatButtons10)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
            }
            NotifiLib.SendNotification("<color=white>[</color><color=blue>LOAD</color><color=white>]</color> <color=white>Loaded Buttons Successfully!</color>");
        }
        public static void Save()
        {
            List<string> list = new List<string>();
            foreach (ButtonInfo buttonInfo in WristMenu.settingsbuttons)
            {
                bool? enabled = buttonInfo.enabled;
                bool flag = true;
                if (enabled.GetValueOrDefault() == flag & enabled != null && buttonInfo.buttonText != "Save Settings")
                {
                    list.Add(buttonInfo.buttonText);
                }
            }
            File.WriteAllLines(WristMenu.FolderName + "\\Saved_Settings.txt", list);
            string text4 = string.Concat(new string[]
            {
               change1.ToString(),
               "\n",
               change2.ToString(),
               "\n",
               change3.ToString(),
               "\n",
               change4.ToString(),
               "\n",
               change6.ToString(),
               "\n",
               change7.ToString(),
               "\n",
               change8.ToString(),
               "\n",
               change9.ToString(),
               "\n",
               change10.ToString(),
               "\n",
               change11.ToString(),
               "\n",
               change12.ToString(),
               "\n",
               change13.ToString(),
               "\n",
               change14.ToString(),
               "\n",
               change15.ToString(),
               "\n",
               change16.ToString()
            });
            File.WriteAllText(WristMenu.FolderName + "/Saved_Settings2.txt", text4.ToString());
            NotifiLib.SendNotification("<color=white>[</color><color=blue>SAVE</color><color=white>]</color> <color=white>Saved Settings Successfully!</color>");
        }
        public static void Load()
        {
            string[] array = File.ReadAllLines(WristMenu.FolderName + "\\Saved_Settings.txt");
            foreach (string b in array)
            {
                foreach (ButtonInfo buttonInfo in WristMenu.settingsbuttons)
                {
                    if (buttonInfo.buttonText == b)
                    {
                        buttonInfo.enabled = new bool?(true);
                    }
                }
            }
            try
            {
                string text3 = File.ReadAllText(WristMenu.FolderName + "/Saved_Settings2.txt");
                string[] array4 = text3.Split(new string[] { "\n" }, StringSplitOptions.None);
                change1 = int.Parse(array4[0]) - 1;
                Changeplat();
                change2 = int.Parse(array4[1]) - 1;
                Changenoti();
                change3 = int.Parse(array4[2]) - 1;
                ChangeFPS();
                change4 = int.Parse(array4[3]) - 1;
                Changedisconnect();
                change6 = int.Parse(array4[4]) - 1;
                Changemenu();
                change7 = int.Parse(array4[5]) - 1;
                Changepagebutton();
                change8 = int.Parse(array4[6]) - 1;
                ChangeOrbColor();
                change9 = int.Parse(array4[7]) - 1;
                ChangeVisualColor();
                change10 = int.Parse(array4[8]) - 1;
                ThemeChangerV1();
                change11 = int.Parse(array4[9]) - 1;
                ThemeChangerV2();
                change12 = int.Parse(array4[10]) - 1;
                ThemeChangerV3();
                change13 = int.Parse(array4[11]) - 1;
                ThemeChangerV4();
                change14 = int.Parse(array4[12]) - 1;
                ThemeChangerV5();
                change15 = int.Parse(array4[13]) - 1;
                ThemeChangerV6();
                change16 = int.Parse(array4[14]) - 1;
                ThemeChangerV7();
            }
            catch
            {
            }
            NotifiLib.SendNotification("<color=white>[</color><color=blue>LOAD</color><color=white>]</color> <color=white>Loaded settings successfully!</color>");
        }
        #endregion
        #region Platform Shit
        // sticky plats r broke still srry
        private static void PlatformsThing(bool invis, bool sticky)
        {
            if (TriggerPlats)
            {
                RPlat = WristMenu.triggerDownR;
                LPlat = WristMenu.triggerDownL;
            }
            else
            {
                RPlat = WristMenu.gripDownR;
                LPlat = WristMenu.gripDownL;
            }
            if (RPlat)
            {
                if (!once_right && jump_right_local == null)
                {
                    if (sticky)
                    {
                        jump_right_local = GameObject.CreatePrimitive(0);
                    }
                    else
                    {
                        jump_right_local = GameObject.CreatePrimitive((PrimitiveType)3);
                    }
                    if (invis)
                    {
                        Destroy(jump_right_local.GetComponent<Renderer>());
                    }
                    jump_right_local.transform.localScale = scale;
                    jump_right_local.transform.position = new Vector3(0f, -0.01f, 0f) + GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.position;
                    jump_right_local.transform.rotation = GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation;
                    jump_right_local.AddComponent<GorillaSurfaceOverride>().overrideIndex = jump_right_local.GetComponent<GorillaSurfaceOverride>().overrideIndex;
                    once_right = true;
                    once_right_false = false;
                    ColorChanger colorChanger1 = jump_right_local.AddComponent<ColorChanger>();
                    colorChanger1.colors = new Gradient
                    {
                        colorKeys = colorKeysPlatformMonke
                    };
                    colorChanger1.Start();
                }
            }
            else
            {
                if (!once_right_false && jump_right_local != null)
                {
                    Destroy(jump_right_local.GetComponent<Collider>());
                    Rigidbody platr = jump_right_local.AddComponent(typeof(Rigidbody)) as Rigidbody;
                    platr.velocity = GorillaLocomotion.GTPlayer.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 5);
                    Destroy(jump_right_local, 2.0f);
                    jump_right_local = null;
                    once_right = false;
                    once_right_false = true;
                }
            }
            if (LPlat)
            {
                if (!once_left && jump_left_local == null)
                {
                    if (sticky)
                    {
                        jump_left_local = GameObject.CreatePrimitive(0);
                    }
                    else
                    {
                        jump_left_local = GameObject.CreatePrimitive((PrimitiveType)3);
                    }
                    if (invis)
                    {
                        Destroy(jump_left_local.GetComponent<Renderer>());
                    }
                    jump_left_local.transform.localScale = scale;
                    jump_left_local.transform.position = new Vector3(0f, -0.01f, 0f) + GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position;
                    jump_left_local.transform.rotation = GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation;
                    jump_left_local.AddComponent<GorillaSurfaceOverride>().overrideIndex = jump_left_local.GetComponent<GorillaSurfaceOverride>().overrideIndex;
                    once_left = true;
                    once_left_false = false;
                    ColorChanger colorChanger2 = jump_left_local.AddComponent<ColorChanger>();
                    colorChanger2.colors = new Gradient
                    {
                        colorKeys = colorKeysPlatformMonke
                    };
                    colorChanger2.Start();
                }
            }
            else
            {
                if (!once_left_false && jump_left_local != null)
                {
                    Destroy(jump_left_local.GetComponent<Collider>());
                    Rigidbody comp = jump_left_local.AddComponent(typeof(Rigidbody)) as Rigidbody;
                    comp.velocity = GorillaLocomotion.GTPlayer.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 5);
                    Destroy(jump_left_local, 2.0f);
                    jump_left_local = null;
                    once_left = false;
                    once_left_false = true;
                }
            }
            if (!PhotonNetwork.InRoom)
            {
                for (int i = 0; i < jump_right_network.Length; i++)
                {
                    Destroy(jump_right_network[i]);
                }
                for (int j = 0; j < jump_left_network.Length; j++)
                {
                    Destroy(jump_left_network[j]);
                }
            }
        }
#endregion
        #region GetButton
        public static ButtonInfo GetButton(string name)
        {
            foreach (ButtonInfo b in WristMenu.buttons)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.settingsbuttons)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.CatButtons1)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.CatButtons2)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.CatButtons3)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.CatButtons4)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.CatButtons5)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.CatButtons6)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.CatButtons7)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.CatButtons8)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.CatButtons9)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            foreach (ButtonInfo b in WristMenu.CatButtons10)
            {
                if (b.buttonText == name)
                {
                    return b;
                }
            }
            return null;
        }
        #endregion
        #region Category shit
        public static void Settings()
        {
            WristMenu.settingsbuttons[0].enabled = new bool?(false);
            WristMenu.buttons[2].enabled = new bool?(false);
            inSettings = !inSettings;
            if (inSettings)
            {
                WristMenu.pageNumber = 0;
            }
            if (!inSettings)
            {
                WristMenu.pageNumber = 0;
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void Cat1()
        {
            WristMenu.CatButtons1[0].enabled = new bool?(false);
            WristMenu.buttons[3].enabled = new bool?(false);
            inCat1 = !inCat1;
            if (inCat1)
            {
                WristMenu.pageNumber = 0;
            }
            if (change7 == 1)
            {
                if (!inCat1)
                {
                    WristMenu.pageNumber = 1;
                }
            }
            if (change7 == 2 | change7 == 3 | change7 == 4 | change7 == 5)
            {
                if (!inCat1)
                {
                    WristMenu.pageNumber = 0;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void Cat2()
        {
            WristMenu.CatButtons2[0].enabled = new bool?(false);
            WristMenu.buttons[4].enabled = new bool?(false);
            inCat2 = !inCat2;
            if (inCat2)
            {
                WristMenu.pageNumber = 0;
            }
            if (change7 == 1)
            {
                if (!inCat2)
                {
                    WristMenu.pageNumber = 1;
                }
            }
            if (change7 == 2 | change7 == 3 | change7 == 4 | change7 == 5)
            {
                if (!inCat2)
                {
                    WristMenu.pageNumber = 0;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void Cat3()
        {
            WristMenu.CatButtons3[0].enabled = new bool?(false);
            WristMenu.buttons[5].enabled = new bool?(false);
            inCat3 = !inCat3;
            if (inCat3)
            {
                WristMenu.pageNumber = 0;
            }
            if (change7 == 1)
            {
                if (!inCat3)
                {
                    WristMenu.pageNumber = 1;
                }
            }
            if (change7 == 2 | change7 == 3 | change7 == 4 | change7 == 5)
            {
                if (!inCat3)
                {
                    WristMenu.pageNumber = 1;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void Cat4()
        {
            WristMenu.CatButtons4[0].enabled = new bool?(false);
            WristMenu.buttons[6].enabled = new bool?(false);
            inCat4 = !inCat4;
            if (inCat4)
            {
                WristMenu.pageNumber = 0;
            }
            if (change7 == 1)
            {
                if (!inCat4)
                {
                    WristMenu.pageNumber = 1;
                }
            }
            if (change7 == 2 | change7 == 3 | change7 == 4 | change7 == 5)
            {
                if (!inCat4)
                {
                    WristMenu.pageNumber = 1;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void Cat5()
        {
            WristMenu.CatButtons5[0].enabled = new bool?(false);
            WristMenu.buttons[7].enabled = new bool?(false);
            inCat5 = !inCat5;
            if (inCat5)
            {
                WristMenu.pageNumber = 0;
            }
            if (change7 == 1)
            {
                if (!inCat5)
                {
                    WristMenu.pageNumber = 2;
                }
            }
            if (change7 == 2 | change7 == 3 | change7 == 4 | change7 == 5)
            {
                if (!inCat5)
                {
                    WristMenu.pageNumber = 1;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void Cat6()
        {
            WristMenu.CatButtons6[0].enabled = new bool?(false);
            WristMenu.buttons[8].enabled = new bool?(false);
            inCat6 = !inCat6;
            if (inCat6)
            {
                WristMenu.pageNumber = 0;
            }
            if (change7 == 1)
            {
                if (!inCat6)
                {
                    WristMenu.pageNumber = 2;
                }
            }
            if (change7 == 2 | change7 == 3 | change7 == 4 | change7 == 5)
            {
                if (!inCat6)
                {
                    WristMenu.pageNumber = 1;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void Cat7()
        {
            WristMenu.CatButtons7[0].enabled = new bool?(false);
            WristMenu.buttons[9].enabled = new bool?(false);
            inCat7 = !inCat7;
            if (inCat7)
            {
                WristMenu.pageNumber = 0;
            }
            if (change7 == 1)
            {
                if (!inCat7)
                {
                    WristMenu.pageNumber = 2;
                }
            }
            if (change7 == 2 | change7 == 3 | change7 == 4 | change7 == 5)
            {
                if (!inCat7)
                {
                    WristMenu.pageNumber = 1;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void Cat8()
        {
            WristMenu.CatButtons8[0].enabled = new bool?(false);
            WristMenu.buttons[10].enabled = new bool?(false);
            inCat8 = !inCat8;
            if (inCat8)
            {
                WristMenu.pageNumber = 0;
            }
            if (change7 == 1)
            {
                if (!inCat8)
                {
                    WristMenu.pageNumber = 2;
                }
            }
            if (change7 == 2 | change7 == 3 | change7 == 4 | change7 == 5)
            {
                if (!inCat8)
                {
                    WristMenu.pageNumber = 1;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void Cat9()
        {
            WristMenu.CatButtons9[0].enabled = new bool?(false);
            WristMenu.buttons[11].enabled = new bool?(false);
            inCat9 = !inCat9;
            if (inCat9)
            {
                WristMenu.pageNumber = 0;
            }
            if (change7 == 1)
            {
                if (!inCat9)
                {
                    WristMenu.pageNumber = 2;
                }
            }
            if (change7 == 2 | change7 == 3 | change7 == 4 | change7 == 5)
            {
                if (!inCat9)
                {
                    WristMenu.pageNumber = 2;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        public static void Cat10()
        {
            WristMenu.CatButtons10[0].enabled = new bool?(false);
            WristMenu.buttons[12].enabled = new bool?(false);
            inCat10 = !inCat10;
            if (inCat10)
            {
                WristMenu.pageNumber = 0;
            }
            if (change7 == 1)
            {
                if (!inCat10)
                {
                    WristMenu.pageNumber = 3;
                }
            }
            if (change7 == 2 | change7 == 3 | change7 == 4 | change7 == 5)
            {
                if (!inCat10)
                {
                    WristMenu.pageNumber = 2;
                }
            }
            WristMenu.DestroyMenu();
            WristMenu.instance.Draw();
        }
        #endregion
        #region Changers
        // DO NOT MESS WITH ANY OF THE THEME CHANGERS OR CHANGERS
        public static void Changeplat()
        {
            change1++;
            if (change1 > 2)
            {
                change1 = 1;
            }
            if (change1 == 1)
            {
                TriggerPlats = false;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>PLATFORMS</color><color=white>] Enable Platforms: Grips</color>");
            }
            if (change1 == 2)
            {
                TriggerPlats = true;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>PLATFORMS</color><color=white>] Enable Platforms: Triggers</color>");
            }
        }
        public static Font MenuFont = WristMenu.MenuFont;
        public static void Changenoti()
        {
            change2++;
            if (change2 > 2)
            {
                change2 = 1;
            }
            if (change2 == 1)
            {
                NotifiLib.IsEnabled = true;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>NOTIS</color><color=white>] Notis Enabled: Yes</color>");
            }
            if (change2 == 2)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>NOTIS</color><color=white>] Notis Enabled: No</color>");
                NotifiLib.IsEnabled = false;
            }
        }
        public static void ChangeFPS()
        {
            change3++;
            if (change3 > 2)
            {
                change3 = 1;
            }
            if (change3 == 1)
            {
                FPSPage = false;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>FPS & PAGE COUNTER</color><color=white>] Is Enabled: No</color>");
            }
            if (change3 == 2)
            {
                FPSPage = true;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>FPS & PAGE COUNTER</color><color=white>] Is Enabled: Yes</color>");
            }
        }
        public static void Changedisconnect()
        {
            change4++;
            if (change4 > 4)
            {
                change4 = 1;
            }
            if (change4 == 1)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>DISCONNECT BUTTON</color><color=white>] Disconnect Location: Right Side</color>");
            }
            if (change4 == 2)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>DISCONNECT BUTTON</color><color=white>] Disconnect Location: Left Side</color>");
            }
            if (change4 == 3)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>DISCONNECT BUTTON</color><color=white>] Disconnect Location: Top</color>");
            }
            if (change4 == 4)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>DISCONNECT BUTTON</color><color=white>] Disconnect Location: Bottom</color>");
            }
        }
        public static void Changemenu()
        {
            change6++;
            if (change6 > 2)
            {
                change6 = 1;
            }
            if (change6 == 1)
            {
                right = false;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>MENU LOCATION</color><color=white>] Current Location: Left Hand</color>");
            }
            if (change6 == 2)
            {
                right = true;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>MENU LOCATION</color><color=white>] Current Location: Right Hand</color>");
            }
        }
        public static void Changepagebutton()
        {
            change7++;
            if (change7 > 5)
            {
                change7 = 1;
            }
            if (change7 == 1)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>NEXT & PREV</color><color=white>] Page Change Button Location: On Menu</color>");
            }
            if (change7 == 2)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>NEXT & PREV</color><color=white>] Page Change Button Location: Top</color>");
            }
            if (change7 == 3)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>NEXT & PREV</color><color=white>] Page Change Button Location: Sides</color>");
            }
            if (change7 == 4)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>NEXT & PREV</color><color=white>] Page Change Button Location: Bottom</color>");
            }
            if (change7 == 5)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>NEXT & PREV</color><color=white>] Page Change Button Location: Triggers</color>");
            }
        }
        public static void ChangeOrbColor()
        {
            change8++;
            if (change8 > 9)
            {
                change8 = 1;
            }
            if (change8 == 1)
            {
                CurrentGunColor = Color.blue;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>GUN & HAND ORB COLOR</color><color=white>] Current Color: Blue</color>");
            }
            if (change8 == 2)
            {
                CurrentGunColor = Color.red;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>GUN & HAND ORB COLOR</color><color=white>] Current Color: Red</color>");
            }
            if (change8 == 3)
            {
                CurrentGunColor = Color.white;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>GUN & HAND ORB COLOR</color><color=white>] Current Color: White</color>");
            }
            if (change8 == 4)
            {
                CurrentGunColor = Color.green;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>GUN & HAND ORB COLOR</color><color=white>] Current Color: Green</color>");
            }
            if (change8 == 5)
            {
                CurrentGunColor = Color.magenta;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>GUN & HAND ORB COLOR</color><color=white>] Current Color: Magenta</color>");
            }
            if (change8 == 6)
            {
                CurrentGunColor = Color.cyan;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>GUN & HAND ORB COLOR</color><color=white>] Current Color: Cyan</color>");
            }
            if (change8 == 7)
            {
                CurrentGunColor = Color.yellow;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>GUN & HAND ORB COLOR</color><color=white>] Current Color: Yellow</color>");
            }
            if (change8 == 8)
            {
                CurrentGunColor = Color.black;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>GUN & HAND ORB COLOR</color><color=white>] Current Color: Black</color>");
            }
            if (change8 == 9)
            {
                CurrentGunColor = Color.grey;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>GUN & HAND ORB COLOR</color><color=white>] Current Color: Grey</color>");
            }
        }
        public static void ChangeVisualColor()
        {
            change9++;
            if (change9 > 9)
            {
                change9 = 1;
            }
            if (change9 == 1)
            {
                CurrentESPColor = Color.blue;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>ESP COLOR</color><color=white>] Current Color: Blue</color>");
            }
            if (change9 == 2)
            {
                CurrentESPColor = Color.red;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>ESP COLOR</color><color=white>] Current Color: Red</color>");
            }
            if (change9 == 3)
            {
                CurrentESPColor = Color.white;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>ESP COLOR</color><color=white>] Current Color: White</color>");
            }
            if (change9 == 4)
            {
                CurrentESPColor = Color.green;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>ESP COLOR</color><color=white>] Current Color: Green</color>");
            }
            if (change9 == 5)
            {
                CurrentESPColor = Color.magenta;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>ESP COLOR</color><color=white>] Current Color: Magenta</color>");
            }
            if (change9 == 6)
            {
                CurrentESPColor = Color.cyan;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>ESP COLOR</color><color=white>] Current Color: Cyan</color>");
            }
            if (change9 == 7)
            {
                CurrentESPColor = Color.yellow;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>ESP COLOR</color><color=white>] Current Color: Yellow</color>");
            }
            if (change9 == 8)
            {
                CurrentESPColor = Color.black;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>ESP COLOR</color><color=white>] Current Color: Black</color>");
            }
            if (change9 == 9)
            {
                CurrentESPColor = Color.grey;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>ESP COLOR</color><color=white>] Current Color: Grey</color>");
            }
        }
        public static void ThemeChangerV1()
        {
            change10++;
            if (change10 > 11)
            {
                change10 = 1;
            }
            if (change10 == 1)
            {
                if (WristMenu.ChangingColors)
                {
                    RGBMenu = false;
                    WristMenu.FirstColor = Color.blue;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] First Color: Blue</color>");
                }
                else
                {
                    RGBMenu = false;
                    WristMenu.NormalColor = Color.blue;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: Blue</color>");
                }
            }
            if (change10 == 2)
            {
                if (WristMenu.ChangingColors)
                {
                    WristMenu.FirstColor = Color.red;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] First Color: Red</color>");
                }
                else
                {
                    WristMenu.NormalColor = Color.red;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: Red</color>");
                }
            }
            if (change10 == 3)
            {
                if (WristMenu.ChangingColors)
                {
                    WristMenu.FirstColor = Color.white;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] First Color: White</color>");
                }
                else
                {
                    WristMenu.NormalColor = Color.white;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: White</color>");
                }
            }
            if (change10 == 4)
            {
                if (WristMenu.ChangingColors)
                {
                    WristMenu.FirstColor = Color.green;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] First Color: Green</color>");
                }
                else
                {
                    WristMenu.NormalColor = Color.green;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: Green</color>");
                }
            }
            if (change10 == 5)
            {
                if (WristMenu.ChangingColors)
                {
                    WristMenu.FirstColor = Color.magenta;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] First Color: Magenta</color>");
                }
                else
                {
                    WristMenu.NormalColor = Color.magenta;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: Magenta</color>");
                }
            }
            if (change10 == 6)
            {
                if (WristMenu.ChangingColors)
                {
                    WristMenu.FirstColor = Color.cyan;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] First Color: Cyan</color>");
                }
                else
                {
                    WristMenu.NormalColor = Color.cyan;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: Cyan</color>");
                }
            }
            if (change10 == 7)
            {
                if (WristMenu.ChangingColors)
                {
                    WristMenu.FirstColor = Color.yellow;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] First Color: Yellow</color>");
                }
                else
                {
                    WristMenu.NormalColor = Color.yellow;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: Yellow</color>");
                }
            }
            if (change10 == 8)
            {
                if (WristMenu.ChangingColors)
                {
                    WristMenu.FirstColor = Color.black;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] First Color: Black</color>");
                }
                else
                {
                    WristMenu.NormalColor = Color.black;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: Black</color>");
                }
            }
            if (change10 == 9)
            {
                if (WristMenu.ChangingColors)
                {
                    WristMenu.FirstColor = Color.grey;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] First Color: Grey</color>");
                }
                else
                {
                    WristMenu.NormalColor = Color.grey;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: Grey</color>");
                }
            }
            if (change10 == 10)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: Clear</color>");
            }
            if (change10 == 11)
            {
                if (WristMenu.ChangingColors)
                {
                    RGBMenu = true;
                    NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Menu Color: RGB</color>");
                }
                else
                {
                    NotifiLib.SendNotification("<color=white>[</color><color=red>ERROR</color><color=white>] Cannot Change The Menu To RGB Due To WristMenu.ChangingColors Being false</color>");
                }
            }
        }
        public static void ThemeChangerV2()
        {
            change11++;
            if (change11 > 9)
            {
                change11 = 1;
            }
            if (change11 == 1)
            {
                WristMenu.SecondColor = Color.black;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Second Color: Black</color>");
            }
            if (change11 == 2)
            {
                WristMenu.SecondColor = Color.red;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Second Color: Red</color>");
            }
            if (change11 == 3)
            {
                WristMenu.SecondColor = Color.white;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Second Color: White</color>");
            }
            if (change11 == 4)
            {
                WristMenu.SecondColor = Color.green;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Second Color: Green</color>");
            }
            if (change11 == 5)
            {
                WristMenu.SecondColor = Color.magenta;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Second Color: Magenta</color>");
            }
            if (change11 == 6)
            {
                WristMenu.SecondColor = Color.cyan;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Second Color: Cyan</color>");
            }
            if (change11 == 7)
            {
                WristMenu.SecondColor = Color.yellow;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Second Color: Yellow</color>");
            }
            if (change11 == 8)
            {
                WristMenu.SecondColor = Color.blue;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Second Color: Blue</color>");
            }
            if (change11 == 9)
            {
                WristMenu.SecondColor = Color.grey;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Second Color: Grey</color>");
            }
        }
        public static void ThemeChangerV3()
        {
            change12++;
            if (change12 > 10)
            {
                change12 = 1;
            }
            if (change12 == 1)
            {
                WristMenu.ButtonColorDisable = Color.yellow;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disable Button Color: Yellow</color>");
            }
            if (change12 == 2)
            {
                WristMenu.ButtonColorDisable = Color.red;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disable Button Color: Red</color>");
            }
            if (change12 == 3)
            {
                WristMenu.ButtonColorDisable = Color.white;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disable Button Color: White</color>");
            }
            if (change12 == 4)
            {
                WristMenu.ButtonColorDisable = Color.green;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disable Button Color: Green</color>");
            }
            if (change12 == 5)
            {
                WristMenu.ButtonColorDisable = Color.magenta;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disable Button Color: Magenta</color>");
            }
            if (change12 == 6)
            {
                WristMenu.ButtonColorDisable = Color.cyan;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disable Button Color: Cyan</color>");
            }
            if (change12 == 7)
            {
                WristMenu.ButtonColorDisable = Color.black;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disable Button Color: Black</color>");
            }
            if (change12 == 8)
            {
                WristMenu.ButtonColorDisable = Color.blue;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disable Button Color: Blue</color>");
            }
            if (change12 == 9)
            {
                WristMenu.ButtonColorDisable = Color.grey;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disable Button Color: Grey</color>");
            }
            if (change12 == 10)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disable Button Color: Clear</color>");
            }
        }
        public static void ThemeChangerV4()
        {
            change13++;
            if (change13 > 10)
            {
                change13 = 1;
            }
            if (change13 == 1)
            {
                WristMenu.ButtonColorEnabled = Color.magenta;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enable Button Color: Magenta</color>");
            }
            if (change13 == 2)
            {
                WristMenu.ButtonColorEnabled = Color.red;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enable Button Color: Red</color>");
            }
            if (change13 == 3)
            {
                WristMenu.ButtonColorEnabled = Color.white;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enable Button Color: White</color>");
            }
            if (change13 == 4)
            {
                WristMenu.ButtonColorEnabled = Color.green;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enable Button Color: Green</color>");
            }
            if (change13 == 5)
            {
                WristMenu.ButtonColorEnabled = Color.yellow;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enable Button Color: Yellow</color>");
            }
            if (change13 == 6)
            {
                WristMenu.ButtonColorEnabled = Color.cyan;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enable Button Color: Cyan</color>");
            }
            if (change13 == 7)
            {
                WristMenu.ButtonColorEnabled = Color.black;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enable Button Color: Black</color>");
            }
            if (change13 == 8)
            {
                WristMenu.ButtonColorEnabled = Color.blue;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enable Button Color: Blue</color>");
            }
            if (change13 == 9)
            {
                WristMenu.ButtonColorEnabled = Color.grey;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enable Button Color: Grey</color>");
            }
            if (change13 == 10)
            {
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enable Button Color: Clear</color>");
            }
        }
        public static void ThemeChangerV5()
        {
            change14++;
            if (change14 > 9)
            {
                change14 = 1;
            }
            if (change14 == 1)
            {
                WristMenu.EnableTextColor = Color.black;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enabled Text Color: Black</color>");
            }
            if (change14 == 2)
            {
                WristMenu.EnableTextColor = Color.red;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enabled Text Color: Red</color>");
            }
            if (change14 == 3)
            {
                WristMenu.EnableTextColor = Color.white;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enabled Text Color: White</color>");
            }
            if (change14 == 4)
            {
                WristMenu.EnableTextColor = Color.green;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enabled Text Color: Green</color>");
            }
            if (change14 == 5)
            {
                WristMenu.EnableTextColor = Color.yellow;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enabled Text Color: Yellow</color>");
            }
            if (change14 == 6)
            {
                WristMenu.EnableTextColor = Color.cyan;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enabled Text Color: Cyan</color>");
            }
            if (change14 == 7)
            {
                WristMenu.EnableTextColor = Color.magenta;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enabled Text Color: Magenta</color>");
            }
            if (change14 == 8)
            {
                WristMenu.EnableTextColor = Color.blue;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enabled Text Color: Blue</color>");
            }
            if (change14 == 9)
            {
                WristMenu.EnableTextColor = Color.grey;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Enabled Text Color: Grey</color>");
            }
        }
        public static void ThemeChangerV6()
        {
            change15++;
            if (change15 > 9)
            {
                change15 = 1;
            }
            if (change15 == 1)
            {
                WristMenu.DIsableTextColor = Color.black;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disabled Text Color: Black</color>");
            }
            if (change15 == 2)
            {
                WristMenu.DIsableTextColor = Color.red;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disabled Text Color: Red</color>");
            }
            if (change15 == 3)
            {
                WristMenu.DIsableTextColor = Color.white;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disabled Text Color: White</color>");
            }
            if (change15 == 4)
            {
                WristMenu.DIsableTextColor = Color.green;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disabled Text Color: Green</color>");
            }
            if (change15 == 5)
            {
                WristMenu.DIsableTextColor = Color.yellow;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disabled Text Color: Yellow</color>");
            }
            if (change15 == 6)
            {
                WristMenu.DIsableTextColor = Color.cyan;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disabled Text Color: Cyan</color>");
            }
            if (change15 == 7)
            {
                WristMenu.DIsableTextColor = Color.magenta;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disabled Text Color: Magenta</color>");
            }
            if (change15 == 8)
            {
                WristMenu.DIsableTextColor = Color.blue;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disabled Text Color: Blue</color>");
            }
            if (change15 == 9)
            {
                WristMenu.DIsableTextColor = Color.grey;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Disabled Text Color: Grey</color>");
            }
        }
        public static void ThemeChangerV7()
        {
            change16++;
            if (change16 > 6)
            {
                change16 = 1;
            }
            if (change16 == 1)
            {
                ButtonSound = 67;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Button Sound: Normal</color>");
            }
            if (change16 == 2)
            {
                ButtonSound = 8;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Button Sound: Stump</color>");
            }
            if (change16 == 3)
            {
                ButtonSound = 203;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Button Sound: AK47</color>");
            }
            if (change16 == 4)
            {
                ButtonSound = 50;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Button Sound: Glass</color>");
            }
            if (change16 == 5)
            {
                ButtonSound = 66;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Button Sound: KeyBoard</color>");
            }
            if (change16 == 6)
            {
                ButtonSound = 114;
                NotifiLib.SendNotification("<color=white>[</color><color=blue>THEME CHANGER</color><color=white>] Button Sound: Cayon Bridge</color>"); // this sounds the best tbh
            }
        }
        public static void FonyChangeV1()
        {
            change10++;
            if (change10 > 6) // number of fonts to cycle through
            {
                change10 = 1;
            }

            // Cycle through built-in Unity fonts or ones loaded from Resources
            if (change10 == 1)
            {
                MenuFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
                NotifiLib.SendNotification("<color=white>[</color><color=blue>FONT CHANGER</color><color=white>] Menu Font: Arial</color>");
            }
            if (change10 == 2)
            {
                MenuFont = Resources.Load<Font>("Fonts/ComicSansMS"); // example custom font in Resources/Fonts/
                NotifiLib.SendNotification("<color=white>[</color><color=blue>FONT CHANGER</color><color=white>] Menu Font: Comic Sans</color>");
            }
            if (change10 == 3)
            {
                MenuFont = Resources.Load<Font>("Fonts/Impact");
                NotifiLib.SendNotification("<color=white>[</color><color=blue>FONT CHANGER</color><color=white>] Menu Font: Impact</color>");
            }
            if (change10 == 4)
            {
                MenuFont = Resources.Load<Font>("Fonts/CourierNew");
                NotifiLib.SendNotification("<color=white>[</color><color=blue>FONT CHANGER</color><color=white>] Menu Font: Courier New</color>");
            }
            if (change10 == 5)
            {
                MenuFont = Resources.Load<Font>("Fonts/TimesNewRoman");
                NotifiLib.SendNotification("<color=white>[</color><color=blue>FONT CHANGER</color><color=white>] Menu Font: Times New Roman</color>");
            }
            if (change10 == 6)
            {
                MenuFont = Resources.Load<Font>("Fonts/PixelFont");
                NotifiLib.SendNotification("<color=white>[</color><color=blue>FONT CHANGER</color><color=white>] Menu Font: PixelFont</color>");
            }

            // Apply to your UI system if needed (for example):
            // WristMenu.UpdateFont(MenuFont);
        }

        #endregion
        #region GunLib
        public static void MakeGun(Color color, Vector3 pointersize, float linesize, PrimitiveType pointershape, Transform arm, bool liner, Action shit, Action shit1)
        {
            if (arm == GorillaLocomotion.GTPlayer.Instance.rightControllerTransform)
            {
                hand = WristMenu.gripDownR;
                hand1 = WristMenu.triggerDownR;
            }
            else if (arm == GorillaLocomotion.GTPlayer.Instance.leftControllerTransform)
            {
                hand = WristMenu.gripDownL;
                hand1 = WristMenu.triggerDownL;
            }
            if (hand)
            {
                Physics.Raycast(arm.position, -arm.up, out raycastHit);
                if (pointer == null) { pointer = GameObject.CreatePrimitive(pointershape); }
                pointer.transform.localScale = pointersize;
                pointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                pointer.transform.position = raycastHit.point;
                pointer.GetComponent<Renderer>().material.color = color;
                if (liner)
                {
                    GameObject g = new GameObject("Line");
                    Line = g.AddComponent<LineRenderer>();
                    Line.material.shader = Shader.Find("GUI/Text Shader");
                    Line.startWidth = linesize;
                    Line.endWidth = linesize;
                    Line.startColor = color;
                    Line.endColor = color;
                    Line.positionCount = 2;
                    Line.useWorldSpace = true;
                    Line.SetPosition(0, arm.position);
                    Line.SetPosition(1, pointer.transform.position);
                    Destroy(g, Time.deltaTime);
                }
                Destroy(pointer.GetComponent<BoxCollider>());
                Destroy(pointer.GetComponent<Rigidbody>());
                Destroy(pointer.GetComponent<Collider>());
                if (hand1)
                {
                    shit.Invoke();
                }
                else
                {
                    shit1.Invoke();
                }
            }
            else
            {
                if (pointer != null)
                {
                    Destroy(pointer, Time.deltaTime);
                }
            }
        }
        // here are some examples of how to use the gunlib
        // this example is for when u only want to execute code when holding ur trigger
        public static void ExampleOnHowToUseGunLib() // this mod is a bug gun
        {
            MakeGun(CurrentGunColor, new Vector3(0.15f, 0.15f, 0.15f), 0.025f, PrimitiveType.Sphere, GorillaLocomotion.GTPlayer.Instance.rightControllerTransform, true, delegate
            {
                GameObject.Find("Floating Bug Holdable").transform.position = pointer.transform.position + new Vector3(0f, 0.25f, 0f);
            }, delegate { });
        }
        // this example is for when u want to execute code when holding ur trigger and when ur not holding trigger
        public static void ExampleOnHowToUseGunLibV2() // this mod is a rig gun
        {
            MakeGun(CurrentGunColor, new Vector3(0.15f, 0.15f, 0.15f), 0.025f, PrimitiveType.Sphere, GorillaLocomotion.GTPlayer.Instance.rightControllerTransform, true, delegate
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = pointer.transform.position + new Vector3(0f, 0.3f, 0f);
            }, delegate { GorillaTagger.Instance.offlineVRRig.enabled = true; }); // this code makes ur rig go back to normal if ur not holding ur trigger
        }
        public static void TeleportGun() // this mod is a bug gun
        {
            MakeGun(CurrentGunColor, new Vector3(0.15f, 0.15f, 0.15f), 0.025f, PrimitiveType.Sphere, GorillaLocomotion.GTPlayer.Instance.rightControllerTransform, true, delegate
            {
                GorillaTagger.Instance.transform.position = pointer.transform.position + new Vector3(0f, 0.25f, 0f);
            }, delegate { });
        }
        public static void HoverBoardGun() // this mod is a bug gun
        {
            MakeGun(CurrentGunColor, new Vector3(0.15f, 0.15f, 0.15f), 0.025f, PrimitiveType.Sphere, GorillaLocomotion.GTPlayer.Instance.rightControllerTransform, true, delegate
            {
                BetaDropBoard(pointer.transform.position + Vector3.up, RandomQuaternion(), Vector3.zero, Vector3.zero, RandomColor());
            }, delegate { });
        }
        public static void WaterSplashGun() // this mod is a bug gun
        {
            MakeGun(CurrentGunColor, new Vector3(0.15f, 0.15f, 0.15f), 0.025f, PrimitiveType.Sphere, GorillaLocomotion.GTPlayer.Instance.rightControllerTransform, true, delegate
            {
                BetaWaterSplash(pointer.transform.position, RandomQuaternion(), 4f, 100f, true, false);
            }, delegate { });
        }


        public static void BetaWaterSplash(Vector3 splashPosition, Quaternion splashRotation, float splashScale, float boundingRadius, bool bigSplash, bool enteringWater, object general = null)
        {
            general = RpcTarget.All;

            splashScale = Mathf.Clamp(splashScale, 1E-05f, 1f);
            boundingRadius = Mathf.Clamp(boundingRadius, 0.0001f, 0.5f);

            if ((GorillaTagger.Instance.bodyCollider.transform.position - splashPosition).sqrMagnitude >= 8.5f)
            {
                VRRig.LocalRig.enabled = false;
                VRRig.LocalRig.transform.position = splashPosition + Vector3.down * 2f;

                if (waterSplashCoroutine != null)
                    CoroutineManager.instance.StopCoroutine(waterSplashCoroutine);

                waterSplashCoroutine = CoroutineManager.instance.StartCoroutine(EnableRig());
            }
        }

        public static Coroutine waterSplashCoroutine;
        public static float splashDel;


        public static void WaterSplashHands()
        {
            if (Time.time > splashDel && (ControllerInputPoller.instance.rightGrab || ControllerInputPoller.instance.leftGrab))
            {
                BetaWaterSplash(ControllerInputPoller.instance.rightGrab ? GorillaTagger.Instance.rightHandTransform.position : GorillaTagger.Instance.leftHandTransform.position, ControllerInputPoller.instance.rightGrab ? GorillaTagger.Instance.rightHandTransform.rotation : GorillaTagger.Instance.leftHandTransform.rotation, 4f, 100f, true, false);
                splashDel = Time.time + 0.1f;
            }
        }



        public static Quaternion RandomQuaternion(float range = 360f) =>
            Quaternion.Euler(UnityEngine.Random.Range(0f, range),
                        UnityEngine.Random.Range(0f, range),
                        UnityEngine.Random.Range(0f, range));

        public static Color RandomColor(byte range = 255, byte alpha = 255) =>
            new Color32((byte)UnityEngine.Random.Range(0, range),
                        (byte)UnityEngine.Random.Range(0, range),
                        (byte)UnityEngine.Random.Range(0, range),
                        alpha);

        private static bool hasGrabbedHoverboard;
        public static void GlobalHoverboard()
        {
            if (!hasGrabbedHoverboard)
            {
                GTPlayer.Instance.GrabPersonalHoverboard(false, Vector3.zero, Quaternion.identity, Color.black);
                hasGrabbedHoverboard = true;
            }

            GTPlayer.Instance.SetHoverAllowed(true);
            GTPlayer.Instance.SetHoverActive(true);
            VRRig.LocalRig.hoverboardVisual.gameObject.SetActive(true);
        }






        HalloweenGhostChaser hgc = lucy;

        public static HalloweenGhostChaser lucy
        {
            get
            {
                if (_lucy == null)
                    lucy = GetObject("Environment Objects/05Maze_PersistentObjects/2025_Halloween1_PersistentObjects/Halloween Ghosts/Lucy/Halloween Ghost/FloatingChaseSkeleton").GetComponent<HalloweenGhostChaser>();

                return _lucy;
            }
            set => _lucy = value;
        }
        public static HalloweenGhostChaser _lucy;

        public static void lucygun() // this mod is a bug gun
        {
            MakeGun(CurrentGunColor, new Vector3(0.15f, 0.15f, 0.15f), 0.025f, PrimitiveType.Sphere, GorillaLocomotion.GTPlayer.Instance.rightControllerTransform, true, delegate
            {
                lucy.transform.position = pointer.transform.position + new Vector3(0f, 0.25f, 0f);
            }, delegate { });
        }

        public static void SpawnBlueLucy()
        {
            HalloweenGhostChaser hgc = lucy;
            if (hgc.IsMine)
            {
                hgc.timeGongStarted = Time.time;
                hgc.currentState = HalloweenGhostChaser.ChaseState.Gong;
                hgc.isSummoned = false;
                NotifiLib.SendNotification("Blue Lucy Has Been Spawned!");
            }
            else NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not master client.");
        }

        public static void LucyAttackSelf()
        {
            HalloweenGhostChaser hgc = lucy;
            if (hgc.IsMine)
            {
                hgc.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
                hgc.grabTime = Time.time;
                hgc.targetPlayer = NetworkSystem.Instance.LocalPlayer;
            }
            else
                NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not master client.");
        }

        public static void BanAllInParty()
        {
            if (FriendshipGroupDetection.Instance.IsInParty)
            {
                partyLastCode = PhotonNetwork.CurrentRoom.Name;
                waitForPlayerJoin = true;
                PhotonNetworkController.Instance.AttemptToJoinSpecificRoom("KKK", JoinType.ForceJoinWithParty);
                partyTime = Time.time + 0.25f;
                phaseTwo = false;
                amountPartying = FriendshipGroupDetection.Instance.PartyMemberIDs.Count - 2;
                NotifiLib.SendNotification("<color=grey>[</color><color=purple>PARTY</color><color=grey>]</color> Banning " + amountPartying + " party members, please be patient..");
            }
            else
                NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not in a party.");
        }

        public static void SpawnRedLucy()
        {
            HalloweenGhostChaser hgc = lucy;
            if (hgc.IsMine)
            {
                hgc.timeGongStarted = Time.time;
                hgc.currentState = HalloweenGhostChaser.ChaseState.Gong;
                hgc.isSummoned = true;
            }
            else NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not master client.");
        }
        public static void DespawnLucy()
        {
            HalloweenGhostChaser hgc = lucy;
            if (hgc.IsMine)
            {
                hgc.currentState = HalloweenGhostChaser.ChaseState.Dormant;
                hgc.isSummoned = false;
            }
            else NotifiLib.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not master client.");
        }



        public static bool lastMasterClient;
        public static string lastRoom = "";

        public static string partyLastCode;
        public static float partyTime;
        public static bool phaseTwo;
        public static int? fullModAmount;
        public static int amountPartying;
        public static bool waitForPlayerJoin;
        public static bool scaleWithPlayer;
        public static float menuScale = 1f;
        public static int notificationScale = 30;
        public static int overlayScale = 30;
        public static int arraylistScale = 20;

        public static bool dynamicSounds;
        public static bool exclusivePageSounds;
        public static bool dynamicAnimations;
        public static bool dynamicGradients;
        public static bool horizontalGradients;
        public static bool animatedTitle;
        public static bool gradientTitle;
        public static string lastClickedName = "";

        public static void Helicopter()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position += new Vector3(0f, 0.05f, 0f);
                VRRig.LocalRig.transform.rotation = Quaternion.Euler(VRRig.LocalRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));

                VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void FixRigHandRotation()
        {
            VRRig.LocalRig.leftHand.rigTarget.transform.rotation *= Quaternion.Euler(VRRig.LocalRig.leftHand.trackingRotationOffset);
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation *= Quaternion.Euler(VRRig.LocalRig.rightHand.trackingRotationOffset);
        }

        public static void Beyblade()
        {
            if (ControllerInputPoller.instance.leftControllerPrimaryButton)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                VRRig.LocalRig.transform.rotation = Quaternion.Euler(VRRig.LocalRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));

                VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }










        public static float delaybetweenscore;
        public static void MaxQuestScore()
        {
            if (Time.time > delaybetweenscore)
            {
                delaybetweenscore = Time.time + 1f;
                VRRig.LocalRig.SetQuestScore(int.MaxValue);
            }
        }

        private static Quaternion grabHeadRot;

        private static Vector3 grabLeftHandPos;
        private static Quaternion grabLeftHandRot;

        private static Vector3 grabRightHandPos;
        private static Quaternion grabRightHandRot;

        public static void GrabRig()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                if (grabHeadRot == Quaternion.identity)
                    grabHeadRot = VRRig.LocalRig.transform.InverseTransformRotation(VRRig.LocalRig.head.rigTarget.transform.rotation);

                if (grabLeftHandPos == Vector3.zero)
                    grabLeftHandPos = VRRig.LocalRig.transform.InverseTransformPoint(VRRig.LocalRig.leftHand.rigTarget.transform.position);

                if (grabLeftHandRot == Quaternion.identity)
                    grabLeftHandRot = VRRig.LocalRig.transform.InverseTransformRotation(VRRig.LocalRig.leftHand.rigTarget.transform.rotation);

                if (grabRightHandPos == Vector3.zero)
                    grabRightHandPos = VRRig.LocalRig.transform.InverseTransformPoint(VRRig.LocalRig.rightHand.rigTarget.transform.position);

                if (grabRightHandRot == Quaternion.identity)
                    grabRightHandRot = VRRig.LocalRig.transform.InverseTransformRotation(VRRig.LocalRig.rightHand.rigTarget.transform.rotation);

                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                VRRig.LocalRig.transform.rotation = Quaternion.Euler(new Vector3(0f, GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles.y, 0f));

                VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.rightHandTransform.TransformRotation(grabHeadRot);

                VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.rightHandTransform.TransformPoint(grabLeftHandPos);
                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.rightHandTransform.TransformRotation(grabLeftHandRot);

                VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.rightHandTransform.TransformPoint(grabRightHandPos);
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.rightHandTransform.TransformRotation(grabRightHandRot);
            }
            else
            {
                VRRig.LocalRig.enabled = true;

                grabHeadRot = Quaternion.identity;

                grabLeftHandPos = Vector3.zero;
                grabRightHandPos = Vector3.zero;

                grabLeftHandRot = Quaternion.identity;
                grabRightHandRot = Quaternion.identity;
            }
        }

        public static Vector3 offsetLH = Vector3.zero;
        public static Vector3 offsetRH = Vector3.zero;
        public static Vector3 offsetH = Vector3.zero;
        public static void EnableSpazRig()
        {
            ghostException = true;
            offsetLH = VRRig.LocalRig.leftHand.trackingPositionOffset;
            offsetRH = VRRig.LocalRig.rightHand.trackingPositionOffset;
            offsetH = VRRig.LocalRig.head.trackingPositionOffset;
        }
        public static bool ghostException;

        public static void SpazRig()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                float spazAmount = 0.1f;
                ghostException = true;
                VRRig.LocalRig.leftHand.trackingPositionOffset = offsetLH + RandomVector3(spazAmount);
                VRRig.LocalRig.rightHand.trackingPositionOffset = offsetRH + RandomVector3(spazAmount);
                VRRig.LocalRig.head.trackingPositionOffset = offsetH + RandomVector3(spazAmount);
            }
            else
            {
                ghostException = false;
                VRRig.LocalRig.leftHand.trackingPositionOffset = offsetLH;
                VRRig.LocalRig.rightHand.trackingPositionOffset = offsetRH;
                VRRig.LocalRig.head.trackingPositionOffset = offsetH;
            }
        }

        public static Vector3 RandomVector3(float range = 1f) =>
            new Vector3(UnityEngine.Random.Range(-range, range),
                        UnityEngine.Random.Range(-range, range),
                        UnityEngine.Random.Range(-range, range));

        public static void BetterRigLerping(VRRig rig)
        {
            if (rigLerpCoroutines.TryGetValue(rig, out Coroutine coroutine))
                CoroutineManager.instance.StopCoroutine(coroutine);

            rigLerpCoroutines[rig] = CoroutineManager.instance.StartCoroutine(LerpRig(rig));
        }

        private static readonly Dictionary<VRRig, GameObject> cosmeticIndicators = new Dictionary<VRRig, GameObject>();
        private static readonly Dictionary<string, Texture2D> cosmeticTextures = new Dictionary<string, Texture2D>();
        public static readonly Dictionary<VRRig, Coroutine> rigLerpCoroutines = new Dictionary<VRRig, Coroutine>();

        public static IEnumerator LerpRig(VRRig rig)
        {
            Quaternion headStartRot = rig.head.rigTarget.localRotation;

            Vector3 syncStartPos = rig.transform.position;
            Quaternion syncStartRot = rig.transform.rotation;

            Vector3 leftHandStartPos = rig.leftHand.rigTarget.localPosition;
            Quaternion leftHandStartRot = rig.leftHand.rigTarget.localRotation;

            Vector3 rightHandStartPos = rig.rightHand.rigTarget.localPosition;
            Quaternion rightHandStartRot = rig.rightHand.rigTarget.localRotation;

            float startTime = Time.time;
            while (Time.time < startTime + 0.1f)
            {
                float t = (Time.time - startTime) / 0.1f;

                rig.head.rigTarget.localRotation = Quaternion.Lerp(headStartRot, rig.head.syncRotation, t);

                rig.transform.position = Vector3.Lerp(syncStartPos, rig.syncPos, t);
                rig.transform.rotation = Quaternion.Lerp(syncStartRot, rig.syncRotation, t);

                rig.leftHand.rigTarget.localPosition = Vector3.Lerp(leftHandStartPos, rig.leftHand.syncPos, t);
                rig.leftHand.rigTarget.localRotation = Quaternion.Lerp(leftHandStartRot, rig.leftHand.syncRotation, t);

                rig.rightHand.rigTarget.localPosition = Vector3.Lerp(rightHandStartPos, rig.rightHand.syncPos, t);
                rig.rightHand.rigTarget.localRotation = Quaternion.Lerp(rightHandStartRot, rig.rightHand.syncRotation, t);

                yield return null;
            }

            rigLerpCoroutines.Remove(rig);
        }

        

        public static void Toggle(string buttonText)
        {
            UnityEngine.Debug.Log($"Toggled: {buttonText}");
        }

        public static void CrashOnTouch()
        {
            if (Time.time < crashDelay)
                return;
            foreach (var Player in from rig in GorillaParent.instance.vrrigs where !rig.isLocal && (Vector3.Distance(GorillaTagger.Instance.leftHandTransform.position, rig.headMesh.transform.position) < 0.25f || Vector3.Distance(GorillaTagger.Instance.rightHandTransform.position, rig.headMesh.transform.position) < 0.25f) select GetPlayerFromVRRig(rig))
            {
                CrashPlayer(Player.ActorNumber);
                crashDelay = Time.time + 0.2f;
            }
        }

        public static float crashDelay;

        public static void CrashPlayer(int ActorNumber)
        {
            PhotonNetwork.RaiseEvent(180, new object[] { "leaveGame", (double)ActorNumber, false, (double)ActorNumber }, new RaiseEventOptions
            {
                TargetActors = new[]
                {
                    ActorNumber
                }
            }, SendOptions.SendReliable);
            RPCProtection();
        }

        public static void BetaDropBoard(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 avelocity, Color boardColor)
        {
            if (Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, position) > 5f)
            {
                VRRig.LocalRig.enabled = false;
                VRRig.LocalRig.transform.position = position + Vector3.down * 4f;

                if (dropBoard != null)
                    CoroutineManager.instance.StopCoroutine(dropBoard);

                dropBoard = CoroutineManager.instance.StartCoroutine(EnableRig());
            }

            FreeHoverboardManager.instance.SendDropBoardRPC(position, rotation, velocity, avelocity, boardColor);
            RPCProtection();
        }

        public static Coroutine dropBoard;
        public static IEnumerator EnableRig()
        {
            yield return new WaitForSeconds(0.3f);
            VRRig.LocalRig.enabled = true;
        }











        #endregion
        #region Vars
        // category vars
        public static bool inSettings = false;
        public static bool inCat1 = false;
        public static bool inCat2 = false;
        public static bool inCat3 = false;
        public static bool inCat4 = false;
        public static bool inCat5 = false;
        public static bool inCat6 = false;
        public static bool inCat7 = false;
        public static bool inCat8 = false;
        public static bool inCat9 = false;
        public static bool inCat10 = false;
        // color vars
        public static Color CurrentGunColor = Color.blue;
        public static Color CurrentESPColor = Color.blue;
        // changers
        public static int change1 = 1;
        public static int change2 = 1;
        public static int change3 = 1;
        public static int change4 = 1;
        public static int change6 = 1;
        public static int change7 = 1;
        public static int change8 = 1;
        public static int change9 = 1;
        public static int change10 = 1;
        public static int change11 = 1;
        public static int change12 = 1;
        public static int change13 = 1;
        public static int change14 = 1;
        public static int change15 = 1;
        public static int change16 = 1;
        // rig vars
        public static bool ghostMonke = false;
        public static bool rightHand = false;
        public static bool lastHit;
        public static bool lastHit2;
        public static GameObject orb;
        public static GameObject orb2;
        // random vars
        public static bool FPSPage;
        public static bool RGBMenu;
        public static bool right;
        public static bool fps;
        public static int ButtonSound = 67;
        public static float balll435342111;
        // gun vars
        public static GameObject pointer = null;
        public static LineRenderer Line;
        public static RaycastHit raycastHit;
        public static bool hand = false;
        public static bool hand1 = false;
        // platform vars
        public static bool invisplat = false;
        public static bool invisMonke = false;
        public static bool stickyplatforms = false;
        private static Vector3 scale = new Vector3(0.0125f, 0.28f, 0.3825f);
        private static bool once_left;
        private static bool once_right;
        private static bool once_left_false;
        private static bool once_right_false;
        private static GameObject[] jump_left_network = new GameObject[9999];
        private static GameObject[] jump_right_network = new GameObject[9999];
        private static GameObject jump_left_local = null;
        private static GameObject jump_right_local = null;
        private static GradientColorKey[] colorKeysPlatformMonke = new GradientColorKey[4];
        public static bool TriggerPlats;
        public static bool RPlat;
        public static bool LPlat;
        // put these near the top of Movement (class-level)
        private static readonly Dictionary<VRRig, List<LineRenderer>> _casualBoneESP = new Dictionary<VRRig, List<LineRenderer>>();
        private static readonly int[] _casualBones = {
    4, 3, 5, 4, 19, 18, 20, 19, 3, 18, 21, 20, 22, 21, 25, 21, 29, 21, 31, 29, 27, 25, 24, 22, 6, 5, 7, 6, 10, 6, 14, 6, 16, 14, 12, 10, 9, 7
};
        private static Material _casualLineMaterial;

        #endregion
        #region Soundboard
        private static bool isEnabled = false;
        private static AudioClip clip = null;
        private static string audioFilePath = "Assets/Audio/song.wav"; // Adjust path

        public static void ToggleMicMusic()
        {
            var recorder = GameObject.FindObjectOfType<Recorder>();
            if (recorder == null)
            {
                Debug.LogWarning("No Photon Recorder found in scene.");
                return;
            }

            if (isEnabled)
            {
                // Switch back to microphone
                isEnabled = false;
                recorder.SourceType = InputSourceType.Microphone;
                recorder.AudioClip = null;
                recorder.RestartRecording();
                Debug.Log("🎙️ Switched to microphone input.");
            }
            else
            {
                // Switch to audio file
                isEnabled = true;

                if (clip == null)
                    clip = LoadWav(audioFilePath);

                if (clip == null)
                {
                    Debug.LogError($"Failed to load WAV from {audioFilePath}");
                    isEnabled = false;
                    return;
                }

                recorder.SourceType = InputSourceType.AudioClip;
                recorder.AudioClip = clip;
                recorder.LoopAudioClip = true;
                recorder.RestartRecording();
                Debug.Log("🎵 Music streaming through mic enabled.");
            }
        }

        private static AudioClip LoadWav(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"File not found: {path}");
                return null;
            }

            using (WWW www = new WWW("file://" + path))
            {
                while (!www.isDone) { } // Simple synchronous wait (you could async this)
                return www.GetAudioClip(false, false, AudioType.WAV);
            }
        }
    }
    #endregion
}
