using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryController : MonoBehaviour
{
    private static VictoryController instance;
    private VictoryController() { }
    public static VictoryController Instance { get; private set; }
    public bool onVictory = false;
    [SerializeField]
    GameObject victoryMenu;
    [SerializeField]
    GameObject menuButton;

    private void Awake()
    {
        if (Instance != null && instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }
    public void Start()
    {
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    public void Pause()
    {
        if (!onVictory)
        {
            Time.timeScale = 0;
            victoryMenu.SetActive(true);
            onVictory = true;
        }
        else
        {
            Time.timeScale = 1;
            victoryMenu.SetActive(false);
            onVictory = false;
        }

    }

    public void returnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
