////////////////////////////////////////////////////////////////////////////
//
//	Create by: Murtaza Sadikot
//	Date : *Today's date*
//
////////////////////////////////////////////////////////////////////////////
/// <summary>
/// The class shows the top 10 high scores for the game.
/// </summary>

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    #region Public_Variables

    #endregion

    #region Private_Variables
    private int[] topScores = new int[10];
    [SerializeField]
    private Text[] m_TopscoreTexts;
    [SerializeField]
    private Button m_BackToMainMenuButton;
    [SerializeField]
    private Text m_WinnerName;
    [SerializeField]
    private Text m_WinnerScore;

    #endregion
    
    #region Unity_Methods
    void Start()
    {
        if(PlayerPrefs.HasKey("Leaderboard"))
        {
            topScores = PlayerPrefsX.GetIntArray("Leaderboard");
        }

        m_BackToMainMenuButton.onClick.AddListener(delegate {
            OnBackToMainMenuButtonClicked();
        });
    }

    #endregion

    #region Private_Methods

    private void OnBackToMainMenuButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    private void DisplayLeaderBoard()
    {
        for (int i = 0; i < topScores.Length; i++)
        {
            m_TopscoreTexts[i].text = topScores[i].ToString();
        }
    }
    #endregion
    
    #region Public_Methods
    /// <summary>
    /// - first check if the array has any zero values if yes, save your score there.
    /// - sort the array by descending order.
    /// - if there is no zero values, compare element with your score and replace your score with element
    /// </summary>
    /// <param name="score"></param>
    public void AddToLeaderBoard(int score, string name)
    {
        for (int i = 0; i < topScores.Length; i++)
        {
            if(topScores[i]  == 0)
            {
                topScores[i] = score;
                break;
            }
        }

        Array.Sort(topScores);
        Array.Reverse(topScores);
        PlayerPrefsX.SetIntArray("Leaderboard", topScores);
        m_WinnerName.text = name;
        m_WinnerScore.text = score.ToString();
        DisplayLeaderBoard();
    }
    #endregion
}
