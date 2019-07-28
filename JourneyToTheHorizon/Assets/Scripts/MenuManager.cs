using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private GameObject escMenu;
	[SerializeField] private GameObject credits;
	[SerializeField] private GameObject mainMenu;

	private bool wasEscPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if ( escMenu != null )
		{
			if ( Input.GetAxis( "Cancel" ) > 0 )
			{
				if ( !wasEscPressed )
				{
					escMenu.SetActive( !escMenu.activeSelf );
				}
				wasEscPressed = true;
			}
			else
			{
				wasEscPressed = false;
			}
		}
    }

	public void StartGame()
	{
		SceneManager.LoadSceneAsync( "Dana's Test Scene" );
	}

	public void ExitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void ToggleCredits( bool enable )
	{
		if ( credits != null  && mainMenu != null )
		{
			mainMenu.SetActive( !enable );
			credits.SetActive( enable );
		}
	}
}
