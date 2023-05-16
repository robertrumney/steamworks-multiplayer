using UnityEngine;
using Steamworks;
using System.Collections.Generic;

public class FriendList : MonoBehaviour
{
    public List<FriendInfo> onlineFriends = new List<FriendInfo>();

    private void Start()
    {
        GetOnlineFriends();
    }

    public void InviteFriendToCoop(CSteamID friendID)
    {
        // Check if the friend is online
        if (SteamFriends.GetFriendPersonaState(friendID) == EPersonaState.k_EPersonaStateOnline)
        {
            // Send the invite
            SteamFriends.InviteUserToGame(friendID, "Let's play co-op!");
        }
        else
        {
            Debug.LogError("Friend is not online!");
        }
    }

    public void GetOnlineFriends()
    {
        // Initialize Steamworks
        if (!SteamAPI.Init())
        {
            Debug.LogError("SteamAPI initialization failed!");
            return;
        }

        // Get the number of friends
        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);

        // Iterate through all friends
        for (int i = 0; i < friendCount; i++)
        {
            CSteamID friendID = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            
            // Check if friend is online or away & owns the game
            if (
                (
                SteamFriends.GetFriendPersonaState(friendID) == EPersonaState.k_EPersonaStateOnline
                || SteamFriends.GetFriendPersonaState(friendID) == EPersonaState.k_EPersonaStateAway
                || SteamFriends.GetFriendPersonaState(friendID) == EPersonaState.k_EPersonaStateSnooze
                )
                && SteamApps.BIsSubscribedApp(SteamUtils.GetAppID()))
            {
                // Get the game the friend is currently playing
                FriendGameInfo_t gameInfo;
                if (SteamFriends.GetFriendGamePlayed(friendID, out gameInfo))
                {
                    // Check if the friend is currently playing the game
                    if (gameInfo.m_gameID.AppID() == SteamUtils.GetAppID())
                    {
                        // Add friend to onlineFriends list
                        onlineFriends.Add(new FriendInfo(friendID, SteamFriends.GetFriendPersonaName(friendID)));
                    }
                }
            }
        }
    }
}

[System.Serializable]
public struct FriendInfo
{
    public CSteamID ID;
    public string Name;

    public FriendInfo(CSteamID id, string name)
    {
        ID = id;
        Name = name;
    }
}
