////////////////////////////////////////////////////////////////////////////
//
//	Create by: Murtaza Sadikot
//	Date : *Today's date*
//
////////////////////////////////////////////////////////////////////////////
/// <summary>
/// This class is used to manage and display the total score and the time 
/// allocated to both the chefs.
/// Once the time runs out the game stop and the winner is displayed.
/// </summary>

using System.Collections;
using UnityEngine;

public class TimeandScore : MonoBehaviour
{
    #region Public_Variables
    public int m_TimeforP1;     //Total time for chef 1
    public int m_TimeforP2;     //Total time for chef 2
    #endregion

    #region Private_Variables
    [SerializeField]
    private TextMesh m_Player1_Time;        //UI Text for displaying chef 1 time
    [SerializeField]
    private TextMesh m_Player1_Score;       //UI Text for displaying chef 1 score

    [SerializeField]
    private TextMesh m_Player2_Time;        //UI Text for displaying chef 2 time
    [SerializeField]
    private TextMesh m_Player2_Score;       //UI Text for displaying chef 2 score
    [SerializeField]
    private ChefManager m_ChefManagerP1;        //script refernce of chefmanager for chef 1 
    [SerializeField]
    private ChefManager m_ChefManagerP2;        //script refernce of chefmanager for chef 2

    [SerializeField]
    private Canvas m_GameOverCanvas;     //UI Canvas for leaderboard.

    [SerializeField]
    private GameOver gameOver;
    #endregion

    #region Unity_Methods
    /// <summary>
    /// countdown timer for the total game time.
    /// After which the game over screen is shown and the leaderboard.
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        m_GameOverCanvas.enabled = false;
        while (m_TimeforP1 > 0 || m_TimeforP2 > 0)
        {
            yield return new WaitForSeconds(1f);
            if(m_TimeforP1 > 0)
            {
                m_TimeforP1 -= 1;
                m_Player1_Time.text = "Time : " + m_TimeforP1.ToString() + " s";
            }

            if(m_TimeforP2 > 0)
            {
                m_TimeforP2 -= 1;
                m_Player2_Time.text = "Time : "  + m_TimeforP2.ToString() + " s";
            }
        }

        m_ChefManagerP1.m_PlayerController.m_CanMove = false;
        m_ChefManagerP2.m_PlayerController.m_CanMove = false;
        if(m_ChefManagerP1.m_ChefScore > m_ChefManagerP2.m_ChefScore)
        {
            gameOver.AddToLeaderBoard(m_ChefManagerP1.m_ChefScore, "Chef 1");
        }
        else
        {
            gameOver.AddToLeaderBoard(m_ChefManagerP2.m_ChefScore, "Chef 2");
        }
        m_GameOverCanvas.enabled = true;
    }

    
    void Update()
    {
        m_Player1_Score.text = "Score : " + m_ChefManagerP1.m_ChefScore.ToString();
        m_Player2_Score.text = "Score : " + m_ChefManagerP2.m_ChefScore.ToString();
    }
    #endregion

    #region Private_Methods

    #endregion
    
    #region Public_Methods
  
    #endregion
}
