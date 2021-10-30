using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private static PauseController instance;
    private PauseController(){}
    public static PauseController Instance { get; private set; }
    public bool onPause = false;
    [SerializeField]
    GameObject PauseMenu;
    [SerializeField]
    GameObject instructions;
    [SerializeField]
    GameObject menuButton;
    [SerializeField]
    GameObject instructionsButton;
    
    private void Awake()
    {
        if(Instance != null && instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }
    // Start is called before the first frame update
    public void Pause()
    {
        if (!onPause)
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
            onPause = true;
        }
        else
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
            onPause = false;
        }
        
    }

    public void returnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void showInstructions()
    {
        instructions.SetActive(!instructions.activeSelf);
        menuButton.SetActive(!menuButton.activeSelf);
        instructionsButton.SetActive(!instructionsButton.activeSelf);
    }
}
