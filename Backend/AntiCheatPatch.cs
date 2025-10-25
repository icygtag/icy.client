using GTAG_NotificationLib;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace MalachiTemp.Backend
{
    
    [HarmonyPatch(typeof(GorillaNot), "SendReport")]
    internal class anticheatnotif : MonoBehaviour
    {
        private static bool Prefix(string susReason, string susId, string susNick)
        {
            if (susReason != "empty rig" && susId == PhotonNetwork.LocalPlayer.UserId)
            {
                NotifiLib.SendNotification("[<color=red>ANTICHEAT</color>] REPORTED FOR: " + susReason);
            }
            return false;
        }
    }
}
