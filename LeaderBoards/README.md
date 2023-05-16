# Steam Leader Boards

This script provides functionality for interacting with Steam leaderboards in Unity. It allows you to upload scores, retrieve leaderboard data, and handle callbacks from Steam's leaderboard system. Here's a brief overview of the script:

## SteamLeaderBoards.cs

- Description: Handles the communication with Steam leaderboards and provides methods for uploading and retrieving scores.
- Dependencies: Steamworks SDK, Unity.

### Usage

1. Ensure that the Steamworks SDK is properly integrated into your Unity project.

2. Attach the `SteamLeaderBoards.cs` script to a game object in your scene.

3. Call the `Init()` method to initialize the leaderboard.

4. Use the `UpdateScore(int score)` method to upload a player's score to the leaderboard.

5. Call the `GetScores()` method to retrieve the leaderboard data.

6. Handle the callbacks in the respective callback methods (`OnLeaderboardFindResult`, `OnLeaderboardUploadResult`, `OnLeaderboardScoresDownloaded`) to process the results.

### Notes

- Make sure to have the Steam client running and logged in for the leaderboard functionality to work.

- The script relies on the SteamManager script to ensure proper initialization of the Steam API.

Please refer to the script file for more detailed explanations and implementation specifics.

For more information on integrating Steamworks into your Unity project, refer to the Steamworks documentation.
