using Steamworks;

using UnityEngine;
using UnityEngine.UI;

public class SteamLeaderBoards : MonoBehaviour
{
    public static SteamLeaderBoards instance;

    private const string s_leaderboardName = "Top Scores";
    private const ELeaderboardUploadScoreMethod s_leaderboardMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;

    private static SteamLeaderboard_t s_currentLeaderboard;
    private static bool s_initialized = false;

    private static readonly CallResult<LeaderboardFindResult_t> m_findResult = new();
    private static readonly CallResult<LeaderboardScoreUploaded_t> m_uploadResult = new();
    private static readonly CallResult<LeaderboardScoresDownloaded_t> OnLeaderboardScoresDownloadedCallResult = new();

    public bool getScores;
    public int testScore = 0;
    public Text scores;
    public Text[] actualScores;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }


        if (scores)
        {
            scores.text = "LOADING SCORES...";
        }
    }

    private void OnEnable()
    {
        Init();
    }

    public static void UpdateScore(int score)
    {
        if (!SteamManager.Initialized)
            return;
		
        if (instance.testScore > 0)
        {
            score += instance.testScore;
            instance.testScore = 0;
        }

        if (!s_initialized)
        {
            Init();
            Debug.Log("Can't upload to the leaderboard because it isn't loaded yet");
        }
        else
        {
            //Debug.Log("uploading score(" + score + ") to steam leaderboard(" + s_leaderboardName + ")");
            SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(s_currentLeaderboard, s_leaderboardMethod, score, null, 0);
            m_uploadResult.Set(hSteamAPICall, OnLeaderboardUploadResult);
            SteamAPI.RunCallbacks();

            if(score>40000)
            {
                Game.instance.Achieve("jackpot");
            }
        }
    }

    public static void Init()
    {
        if (!SteamManager.Initialized)
            return;

        SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(s_leaderboardName);
        m_findResult.Set(hSteamAPICall, OnLeaderboardFindResult);
    }

    static private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
    {
        s_currentLeaderboard = pCallback.m_hSteamLeaderboard;
        s_initialized = true;

        if (instance.getScores)
            GetScores();
    }

    static private void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        Debug.Log("STEAM LEADERBOARDS: failure - " + failure + " Completed - " + pCallback.m_bSuccess + " NewScore: " + pCallback.m_nGlobalRankNew + " Score " + pCallback.m_nScore + " HasChanged - " + pCallback.m_bScoreChanged);
    }

    public static void GetScores()
    {
        if (!s_initialized)
        {
            Debug.Log("Can't fetch leaderboard because isn't loaded yet");
        }
        else
        {
            SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(s_currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 100); //40
            OnLeaderboardScoresDownloadedCallResult.Set(handle, OnLeaderboardScoresDownloaded);
        }
    }

    static private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        instance.scores.gameObject.SetActive(false);

        instance.actualScores[0].text = "";
        instance.actualScores[1].text = "";
        instance.actualScores[2].text = "";
        instance.actualScores[3].text = "";

        instance.actualScores[0].gameObject.SetActive(true);
        instance.actualScores[1].gameObject.SetActive(true);
        instance.actualScores[2].gameObject.SetActive(true);
        instance.actualScores[3].gameObject.SetActive(true);

        int Num_Entries = pCallback.m_cEntryCount;

        instance.m_SteamLeaderboardEntries = pCallback.m_hSteamLeaderboardEntries;

        int rank = 1;

        for (int index = 0; index < Num_Entries; index++)
        {
            LeaderboardEntry_t LeaderboardEntry;
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, index, out LeaderboardEntry, null, 0);
            string username = SteamFriends.GetFriendPersonaName(LeaderboardEntry.m_steamIDUser);

            if(username.ToUpper() == "[UNKNOWN]")
            {
                if (instance.scores)
                {
                    instance.scores.gameObject.SetActive(true);
                    instance.scores.text = "LOADING SCORES...";
                    instance.actualScores[0].gameObject.SetActive(false);
                    instance.actualScores[1].gameObject.SetActive(false);
                    instance.actualScores[2].gameObject.SetActive(false);
                    instance.actualScores[3].gameObject.SetActive(false);

                }

                Init();
                return;
            }

            if(rank==1)
                instance.actualScores[0].text += "#" + rank.ToString() + ". " + username.ToUpper() + "  : <color=#E50000> " + LeaderboardEntry.m_nScore.ToString("n2") + "</color>\n";

            if (rank > 1 && rank < 35)
                instance.actualScores[1].text += "#" + rank.ToString() + ". " + username.ToUpper() + "  : <color=#E50000> " + LeaderboardEntry.m_nScore.ToString("n2") + "</color>\n";

            if (rank > 34 && rank < 68)
                instance.actualScores[2].text += "#" + rank.ToString() + ". " + username.ToUpper() + "  : <color=#E50000> " + LeaderboardEntry.m_nScore.ToString("n2") + "</color>\n";

            if (rank > 67)
                instance.actualScores[3].text += "#" + rank.ToString() + ". " + username.ToUpper() + "  : <color=#E50000> " + LeaderboardEntry.m_nScore.ToString("n2") + "</color>\n";

            rank++;
        }

        instance.scores.text += "\n\nPRESS ANY KEY TO RETURN";  
    }
}
