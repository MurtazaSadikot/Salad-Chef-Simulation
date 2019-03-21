////////////////////////////////////////////////////////////////////////////
//
//	Create by: Murtaza Sadikot
//	Date : *Today's date*
//
////////////////////////////////////////////////////////////////////////////
/// <summary>
/// The Class calculates the postive and negative score for the chef.
/// </summary>

using UnityEngine;

public class CalculateScore : MonoBehaviour
{
    #region PUBLIC_VARIABLES
    public static CalculateScore m_Instance;        //singleton instance of the script
    #endregion

    #region PRIVATE_VARIABLES
    [SerializeField]
    private int m_PositiveScore;        //positive score when the chef serves correct order
    [SerializeField]
    private int m_NegativeScore;        //negative score when the customer is not served
    [SerializeField]
    private int m_Penalty;      //negative score when the customer is served incorrect order and leaves

    #endregion

    #region UNITY_METHODS
    /// <summary>
    /// Assigning the instance for singelton.
    /// </summary>
    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
    }
    #endregion

    #region PRIVATE_METHODS
    
    #endregion

    #region PUBLIC_METHODS
    /// <summary>
    /// Calculating the positive score. Along with the bonus condition.
    /// </summary>
    /// <param name="remainingtime"></param>
    /// <param name="totaltime"></param>
    /// <returns></returns>
    public int CalculatePositiveScore(float remainingtime, float totaltime)
    {
        int score = 0;

        if(remainingtime > totaltime * 0.70f)
        {
            Debug.Log("Spawn a bonus object");
        }

        score = m_PositiveScore + (int)remainingtime;
        return score;
    }

    /// <summary>
    /// Negative score for not serving the customer.
    /// </summary>
    /// <returns></returns>
    public int CalculateNegativeScore()
    {
        return m_NegativeScore;
    }

    /// <summary>
    /// Negative score penalty when the customer is served incorrect
    /// order. This is solely given to the chef who served the incorrect
    /// order.
    /// </summary>
    /// <returns></returns>
    public int CalculatePenaltyScore()
    {
        return m_Penalty;
    }

	#endregion
}
