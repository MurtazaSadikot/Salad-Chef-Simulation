////////////////////////////////////////////////////////////////////////////
//
//	Create by: Murtaza Sadikot
//	Date : *Today's date*
//
////////////////////////////////////////////////////////////////////////////
////// <summary>
/// This class spawns customer at specific intervals.
/// There are 4 static place where the customer will spawn.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    #region PUBLIC_VARIABLES

    #endregion

    #region PRIVATE_VARIABLES
    [SerializeField]
    private Transform[] m_CustomerPositions;        //static position to spawn the customer
    [SerializeField]
    private GameObject m_CustomerPrefab;        //customer prefab
    [SerializeField]
    private Transform m_CustomerParent;     //parent object under which all the customer prefabs will be instantiated
    [SerializeField]
    private Color[] m_CustomerColor;        //four specific customer color to distinguish

    private List<Transform> activeCustomer = new List<Transform>();     //list of presently alive customers
    
    private int leftindex;      //index of the customer who has lef
    #endregion

    #region UNITY_METHODS

   /// <summary>
   /// Call to generate the first batch of customers when the game starts.
   /// </summary>
    void Start()
    {
        GenerateCustomers();
    }

    #endregion

    #region PRIVATE_METHODS
    /// <summary>
    /// The first time all the customers are spawned at the same time.
    /// Here we are keeping max customer count to 4.
    /// </summary>
    void GenerateCustomers()
    {
        for (int i = 0; i < m_CustomerPositions.Length; i++)
        {
            GameObject customer = Instantiate(m_CustomerPrefab, m_CustomerParent);
            customer.GetComponent<Renderer>().material.color = m_CustomerColor[i];
            customer.transform.position = m_CustomerPositions[i].position;
            activeCustomer.Add(customer.transform);
        }
    }

    /// <summary>
    /// The routine will check for the empty position by checking the active customer list.
    /// Whichever index is found empty a new customer is spawned at that place.
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckForEmptyRoutine()
    {
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < activeCustomer.Count; i++)
        {
            if (activeCustomer[i] == null)
            {
                leftindex = i;
                break;
            }
        }

       // yield return new WaitForSeconds(3.5f);
        GameObject customer = Instantiate(m_CustomerPrefab, m_CustomerParent);
        customer.GetComponent<Renderer>().material.color = m_CustomerColor[leftindex];
        customer.transform.position = m_CustomerPositions[leftindex].position;
        activeCustomer[leftindex] = customer.transform;
    }
    #endregion

    #region PUBLIC_METHODS
    /// <summary>
    /// The method will fill the void when the customer leaves once the correct order is fed
    /// or the customer wait time is up.
    /// </summary>
    public void FillTheLeftCustomer()
    {
        // coroutine to overcome race condition
        if(this != null)
            StartCoroutine(CheckForEmptyRoutine());
    }
    #endregion
}
