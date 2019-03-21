////////////////////////////////////////////////////////////////////////////
//
//	Create by: Murtaza Sadikot
//	Date : *Today's date*
//
////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The player movement is determined by this class.
/// WASD is used for chef 1
/// IJKL is used for chef 2
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region PUBLIC_VARIABLES
    public bool m_CanMove = true;       //bool to determine if the chef can move or not
    #endregion

    #region PRIVATE_VARIABLES
    [SerializeField]
    private string m_HorizontalAxis = "Horizontal_P1";          //X-axis movement for chef
    [SerializeField]
    private string m_VerticalAxis = "Vertical_P1";      //Y-axis movement for chef
    [SerializeField]
    private int m_Speed;        //the speed by which the chef can move
	#endregion

	#region UNITY_METHODS

 
    /// <summary>
    /// movement of the chef is restricted within the boundry of the table.
    /// </summary>
    void Update()
    {
        if(m_CanMove)
        {
            Vector3 move = new Vector3(Input.GetAxis(m_HorizontalAxis), Input.GetAxis(m_VerticalAxis), 0);
            transform.position += move * m_Speed * Time.deltaTime;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5.15f, 5f), Mathf.Clamp(transform.position.y, -2.5f, 1.25f));
        }
    }

    #endregion

    #region PRIVATE_METHODS

    #endregion

    #region PUBLIC_METHODS

    #endregion
}
