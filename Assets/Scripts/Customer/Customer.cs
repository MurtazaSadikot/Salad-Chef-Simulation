////////////////////////////////////////////////////////////////////////////
//
//	Create by: Murtaza Sadikot
//	Date : *Today's date*
//  
////////////////////////////////////////////////////////////////////////////
/// The customer script deals with the customer salad requests, the negative
/// scoring if the customer is not fed properly or the customer is not fed
/// at all.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    #region PUBLIC_VARIABLES
    [HideInInspector]
    public List<string> m_RequestedSalad;  // salad requested by the customer
    [HideInInspector]
    public int m_WaitTime;  // the time before the customer leaves
    [HideInInspector]
    public int m_RemainingTime; //ticking timer
    [HideInInspector]
    public int servedBy = -1;    // -1 = not served, 0 = served by chef1, 1 = served by chef2

    [HideInInspector]
    public float decreasemulti = 1f; // a multiplier who speeds up the time if the customer is fed wrong order
    #endregion

    #region PRIVATE_VARIABLES
    [SerializeField]
    private TextMesh TimeText;  // text to show time in UI
    private string[] vegetableOptions = new string[] { "Tomato", "Lettuce", "Cabbage", "Cucumber", "Corn", "Carrot"};  // vegetable options
    [SerializeField]
    private TextMesh m_CustomerHud; // hud showing the cutomer 

    private CustomerSpawner customerSpawner; //script that spawns customer when anyone of them leaves
    #endregion

    #region UNITY_METHODS

    /// <summary>
    /// the start will find the CustomerSpawner script and select a random combination of 2 or 3 vegetables
    /// and it will display on the customer HUD so that the chef know what to serve whom.
    /// </summary>
    /// <returns></returns>
    void Start()
    {
        customerSpawner = FindObjectOfType<CustomerSpawner>();

        for (int i = 0; i < vegetableOptions.Length/Random.Range(2,4); i++)
        {
            int rand = Random.Range(0, 6);
            m_RequestedSalad.Add(vegetableOptions[rand]);
            int randTime = Random.Range(30,36);  // 10-16
            m_WaitTime += randTime;
        }
        m_CustomerHud.text = string.Join(",\n", m_RequestedSalad);
        StartCoroutine(CountDownTimer());
    }

    #endregion

    #region PRIVATE_METHODS
    /// <summary>
    /// count down timer that shows the amount of time before the customer leaves
    /// </summary>
    /// <returns></returns>
    IEnumerator CountDownTimer()
    {
        m_RemainingTime = m_WaitTime;
        while(m_RemainingTime > 0)
        {
            yield return new WaitForSeconds(decreasemulti);
            m_RemainingTime--;
            TimeText.text = "Time : " + m_RemainingTime;
        }

        ScoreCheckBeforeLeaving();
    }

    /// <summary>
    /// here the check is done that which customer is fed by whom.
    /// if the value is -1 then the customer is not fed at all.
    /// if the value is 0, chef 1 has fed the customer.
    /// if the value is 1, chef 2 has fed the customer.
    /// </summary>
    private void ScoreCheckBeforeLeaving()
    {
        ChefManager[] chefs = FindObjectsOfType<ChefManager>();
        switch(servedBy)
        {
            case -1:
                for (int i = 0; i < chefs.Length; i++)
                {
                    chefs[i].m_ChefScore -= CalculateScore.m_Instance.CalculateNegativeScore();
                }
                break;
            case 0:
                for (int i = 0; i < chefs.Length; i++)
                {
                    if(chefs[i].gameObject.name == "Player_1")
                    {
                        chefs[i].m_ChefScore -= chefs[i].m_ChefScore -= CalculateScore.m_Instance.CalculatePenaltyScore();
                    }
                }
                break;
            case 1:
                for (int i = 0; i < chefs.Length; i++)
                {
                    if (chefs[i].gameObject.name == "Player_2")
                    {
                        chefs[i].m_ChefScore -= chefs[i].m_ChefScore -= CalculateScore.m_Instance.CalculatePenaltyScore();
                    }
                }
                break;
        }

        Destroy(this.gameObject);
    }

    /// <summary>
    /// Before destroying the customer prefab it will send out a request
    /// to customer spawner to create a new customer as replacment.
    /// </summary>
    private void OnDestroy()
    {
        Debug.Log("Destroyed called");
        customerSpawner.FillTheLeftCustomer();
    }

    #endregion

    #region PUBLIC_METHODS

    #endregion
}
