using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    GameObject pauseMenu;

    bool paused = false;

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    GameObject buttonToSelect;

    [SerializeField]
    Character character;

    //GameObject previouslySelectedButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {           
            if (character.alive == true)
            {
                TogglePause(!paused);
                
            }
        }

        /*/ Not exactly sure why, but whatever I've done makes this obsolete
        if (paused)
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                previouslySelectedButton = eventSystem.currentSelectedGameObject;
            }
            else
            {
                    eventSystem.SetSelectedGameObject(previouslySelectedButton);
            }
        }/*/
    }
    public void TogglePause(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
        paused = isPaused;
      
        if (isPaused)
        {
            eventSystem.SetSelectedGameObject(buttonToSelect);
        }
        else
        {
            eventSystem.SetSelectedGameObject(null);
            
        }        
        Time.timeScale = isPaused ? 0 : 1;       
    }

    public void LoadScene(int sceneNum)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneNum);
    }

    public void QuitGame()
    {
        //unity editor scripts cannot be in final game because it won't compile when you build
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
        Debug.Log("Application Quit");
    }
}
