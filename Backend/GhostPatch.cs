using UnityEngine;
using HarmonyLib;

namespace MalachiTemp.Backend
{
    
    [HarmonyPatch(typeof(VRRig), "OnDisable")]
    internal class GhostPatch : MonoBehaviour
    {
        public static bool Prefix(VRRig __instance)
        {
            if (__instance == GorillaTagger.Instance.offlineVRRig)
            {
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(VRRigJobManager), "DeregisterVRRig")]
    public static class Bullshit
    {
        public static bool Prefix(VRRigJobManager __instance, VRRig rig)
        {
            return !(__instance == GorillaTagger.Instance.offlineVRRig);
        }
    }

    [HarmonyPatch(typeof(VRRig), "CheckDistance")]
    public class MoreBullshit
    {
        public static bool enabled = false;
        public static void Postfix(ref bool __result, Vector3 position, float max)
        {
            if (enabled)
                __result = true;
        }
    }
}
