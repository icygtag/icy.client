using Photon.Realtime;
using HarmonyLib;
using GTAG_NotificationLib;
using Photon.Pun;

namespace MalachiTemp.Backend
{
    

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
    internal class OnJoin : HarmonyPatch
    {
        private static void Prefix(Player newPlayer)
        {
            NotifiLib.SendNotification("[<color=blue>ROOM</color>] Player: " + newPlayer.NickName + " Joined Lobby");
        }
    }

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
    internal class OnLeave : HarmonyPatch
    {
        private static void Prefix(Player otherPlayer)
        {
            if (otherPlayer != PhotonNetwork.LocalPlayer)
            {
                NotifiLib.SendNotification("[<color=blue>ROOM</color>] Player: " + otherPlayer.NickName + " Left Lobby");
            }
        }
    }
}