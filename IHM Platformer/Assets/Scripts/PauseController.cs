using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    private static PauseController instance;
    private PauseController(){}
    public static PauseController Instance { get; private set; }
    bool onPause = false;
    GameObject PauseMenu;
    
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
        }
        else
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }
        
    }
}
