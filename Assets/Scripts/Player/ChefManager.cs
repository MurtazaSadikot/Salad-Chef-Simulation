////////////////////////////////////////////////////////////////////////////
//
//	Create by: Murtaza Sadikot
//	Date : *Today's date*
//
////////////////////////////////////////////////////////////////////////////
/// <summary>
/// The chefmanager handles all the chef interactions that the chef does.
/// It all depends on the type of tag that the chef is interacting with.
/// It can be a chopping board, plate, customer, dustbin etc.
/// Methods are called according to the tags. We have used here the normal 2D physics methods
/// oncolliderstay and oncolliderexit.
/// 
/// left shift = pick up objects for chef 1
/// left ctrl =  drop objects for chef 1
/// 
/// right shift = pick up objects for chef 2
/// right ctrl = drop objects for chef 2
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefManager : MonoBehaviour
{
    #region PUBLIC_VARIABLES
    public List<string> saladToServe = new List<string>();      // the scombination of vegetable that chef picks up from chopping board
    public delegate void  OnVegetableChopped();     //delegate for chopping vegeatable
    public event OnVegetableChopped OnChopped;      // event which is called when the vegetables are kept for chopping

    public delegate void OnSaladServed();       //delegate for serving the salad
    public event OnSaladServed OnServed;        //event called when the salad is served to the customer.

    public int m_ChefScore;     //score accumulated by the chef by serving the customers
    public PlayerController m_PlayerController;     //controller script reference which deals with player movement.
    #endregion

    #region PRIVATE_VARIABLES

    [SerializeField]
    private string m_Pickup = "Pickup_P1";      // button for chef to pick up objects
    [SerializeField]
    private string m_Drop = "Drop_P1";      //button for chef to drop the objects
    [SerializeField]
    private TextMesh m_HudText;     //the UI display of chef either carrying the vegetables or the salad.

  
    private bool canPickorDrop;     //bool which restricts the chefs ability to pickup or drop objects
    private Collider2D currentCollider;     //the current object that the chef is interacting with.


    private Queue<string> pickedVeggies = new Queue<string>();      // queue of the vegetable that the chef picks up (max 2 objects)
    private string vegetableOnPlate;        //vegetable plate beside the chopping board to store vegetable for later usage
    private List<string> choppingboard = new List<string>();        //list of vegetables chopped by the chef
    #endregion

    #region UNITY_METHODS

    /// <summary>
    /// adding the event callbacks which will be called when the event occurs
    /// </summary>
    void Start()
    {
        OnChopped += StartChopping;     
        OnServed += CheckContents;
    }
    /// <summary>
    /// removing the callbacks from the events
    /// </summary>
    private void OnApplicationQuit()
    {
        OnChopped -= StartChopping;
        OnServed -= CheckContents;
    }

    /// <summary>
    /// Check the input button and perform necessary action depend upon if
    /// its pickup or drop action
    /// </summary>
    void Update()
    {

        if (Input.GetButtonDown(m_Pickup) && canPickorDrop)
        {
            PickUpCases();
        }

        if(Input.GetButtonDown(m_Drop) && canPickorDrop)
        {
            DropCases();
        }
    }

#endregion

#region PRIVATE_METHODS
    /// <summary>
    /// When the chef enters the collider of the object
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerStay2D(Collider2D collider)
    {
        canPickorDrop = true;
        currentCollider = collider;
    }

    /// <summary>
    /// when the chef leaves the collider of the object
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        canPickorDrop = false;
        currentCollider = null;
    }


    /// <summary>
    /// Adds the vegetable to the queue. Max at a time the chef can carry
    /// only 2 vegetable, if the 3rd is picked up the 1st is discarded.
    /// </summary>
    /// <param name="veggie"></param>
    private void AddVegetableToQ(string veggie)
    {
        pickedVeggies.Enqueue(veggie);
        if (pickedVeggies.Count > 2)
        {
            pickedVeggies.Dequeue();
        }

        DisplayInHandVeggies();
    }


    /// <summary>
    /// Remove the vegetable from the chef queue and add it to either
    /// the plate, or chopping board or the dustbin. Depends upon the current collider that
    /// the chef is interacting with.
    /// </summary>
    private void RemoveVegetableFromQ()
    {
        if(pickedVeggies.Count > 0)
        {
            pickedVeggies.Dequeue();
        }

        DisplayInHandVeggies();
    }

    /// <summary>
    /// This switch case decides the further action that needs to be done when
    /// the chef picks up vegetables or salad from particular colliders.
    /// </summary>
    private void PickUpCases()
    {
        switch (currentCollider.tag)
        {
            case "vegetable":
                AddVegetableToQ(currentCollider.name);
                break;
            case "choppingboard":
                if(choppingboard.Count > 0)
                {
                    saladToServe = choppingboard;
                    DisplaySalad();
                }
                break;
            case "plate":
                if(!string.IsNullOrEmpty(vegetableOnPlate))
                {
                    pickedVeggies.Enqueue(vegetableOnPlate);
                    DisplayInHandVeggies();
                    vegetableOnPlate = null;
                }
                break;
        }
    }

    /// <summary>
    /// This switch case decides the further action that needs to be done when
    /// the chef drops the vegetables or salad inside different colliders.
    /// </summary>
    private void DropCases()
    {
        switch (currentCollider.tag)
        {
            case "dustbin":
                m_ChefScore -= 10;
                ClearEverthing();
                DisplayInHandVeggies();
                break;
            case "choppingboard":
                StartChopping();
                RemoveVegetableFromQ();
                break;
            case "plate":
                vegetableOnPlate = pickedVeggies.Peek();
                RemoveVegetableFromQ();
                break;
            case "customer":
                if(saladToServe.Count > 1)
                {
                    Debug.Log("serving customer");
                    CheckContents();
                }
                break;
        }
    }

    /// <summary>
    /// It displays the current vegetables holding by the chef in the HUD.
    /// </summary>
    private void DisplayInHandVeggies()
    {
        string[] currentveggies = pickedVeggies.ToArray();
        m_HudText.text = string.Join(",\n", currentveggies);
    }

    /// <summary>
    /// This displays the salad that the chef is carrying to the customer in the HUD.
    /// </summary>
    private void DisplaySalad()
    {
        string[] currentveggies = saladToServe.ToArray();
        m_HudText.text = string.Join(",\n", currentveggies);
    }

    /// <summary>
    /// This method is called when the chef throw everything in the dustbin.
    /// Note : when this method is called everything is cleared out. whatever is on the chopping board,
    /// plate and in the hand of chef, everything is cleared out.
    /// </summary>
    private void ClearEverthing()
    {
        pickedVeggies.Clear();
        saladToServe.Clear();
        choppingboard.Clear();
        DisplayInHandVeggies();
        DisplaySalad();
    }

    /// <summary>
    /// A routine which will remove the vegetable from the queue and add it to the chopping board
    /// </summary>
    void StartChopping()
    {
        StartCoroutine(AddingToChopboard(pickedVeggies.Peek()));
    }

    /// <summary>
    /// While the vegetable is chopped the chef cannot move for 3 seconds.
    /// </summary>
    /// <param name="chop"></param>
    /// <returns></returns>
    IEnumerator AddingToChopboard(string chop)
    {
        m_PlayerController.m_CanMove = false;
        canPickorDrop = false;
        choppingboard.Add(chop);
        Debug.Log("currently chopping : " + chop);
        yield return new WaitForSeconds(3f);
        m_PlayerController.m_CanMove = true;
        canPickorDrop = true;
    }

    /// <summary>
    /// When the customer is served it sets a int which determines which
    /// customer is served by which chef.
    /// After that it checks if the order is correct if it is, necessary score is calculated.
    /// if the order is not correct the customer starts losing time faster and if the correct 
    /// order is not fed to the customer.
    /// </summary>
    private void CheckContents()
    {
        Customer currentcustomer = currentCollider.GetComponent<Customer>();

        if (gameObject.name == "Player_1")
        {
            currentcustomer.servedBy = 0;
        }
        else
        {
            currentcustomer.servedBy = 1;
        }

        bool isordercorrect = CheckOrder.CompareLists(saladToServe, currentcustomer.m_RequestedSalad);

        if (isordercorrect)
        {
            m_ChefScore += CalculateScore.m_Instance.CalculatePositiveScore(currentcustomer.m_RemainingTime, currentcustomer.m_WaitTime);
            ClearEverthing();
            Destroy(currentcustomer.gameObject);
        }
        else
        {
            currentcustomer.decreasemulti = 0.5f;
            ClearEverthing();
        }
    }

#endregion

#region PUBLIC_METHODS

#endregion
}
