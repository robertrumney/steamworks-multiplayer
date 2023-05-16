# FriendList

The "FriendList" script provides functionality for managing friends on Steam in Unity. It allows you to retrieve the list of online friends, invite friends to play co-op games, and get information about their current game status. Here's a brief overview of the script:

## FriendList.cs

- Description: Handles the management of friends on Steam and provides methods for retrieving online friends and inviting them to play co-op games.
- Dependencies: Steamworks SDK, Unity.

### Usage

1. Ensure that the Steamworks SDK is properly integrated into your Unity project.

2. Attach the `FriendList.cs` script to a game object in your scene.

3. Call the `GetOnlineFriends()` method to retrieve the list of online friends.

4. Use the `InviteFriendToCoop(CSteamID friendID)` method to invite a friend to play co-op games. Pass the friend's Steam ID as the parameter.

### Notes

- Make sure to have the Steam client running and logged in for the friend list functionality to work.

- The script relies on the SteamManager script to ensure proper initialization of the Steam API.

- The list of online friends is stored in the `onlineFriends` list, which is of type `List<FriendInfo>`. Each `FriendInfo` struct contains the friend's Steam ID and their display name.

Please refer to the script file for more detailed explanations and implementation specifics.

For more information on integrating Steamworks into your Unity project, refer to the Steamworks documentation.
