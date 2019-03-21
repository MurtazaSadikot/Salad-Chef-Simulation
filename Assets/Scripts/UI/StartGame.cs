////////////////////////////////////////////////////////////////////////////
//
//	Create by: Murtaza Sadikot
//	Date : *Today's date*
//
////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    #region Public_Variables

    #endregion

    #region Private_Variables
    [SerializeField]
    private Button m_PlayButton;
    [SerializeField]
    private Button m_ExitButton;
    #endregion
    
    #region Unity_Methods
    void Start()
    {
        m_PlayButton.onClick.AddListener(delegate {
            OnPlayButtonClicked();
        });

        m_ExitButton.onClick.AddListener(delegate {
            OnExitButtonClicked();
        });
    }

    
    void Update()
    {
        
    }
    #endregion

    #region Private_Methods
    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
    #endregion
    
    #region Public_Methods
    
    #endregion
}
