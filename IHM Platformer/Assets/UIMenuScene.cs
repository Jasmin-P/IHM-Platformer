using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuScene : MonoBehaviour
{

    [SerializeField]
    GameObject instructions;
    [SerializeField]
    GameObject menuButton;
    [SerializeField]
    GameObject instructionsButton;
    [SerializeField]
    GameObject quitButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxis("Dash")) > 0.5f || Input.GetButtonDown("X"))
        {
            instructions.SetActive(!instructions.activeSelf);
            menuButton.SetActive(!menuButton.activeSelf);
            instructionsButton.SetActive(!instructionsButton.activeSelf);
            quitButton.SetActive(!quitButton.activeSelf);
        }

        if (Input.GetButtonDown("A"))
        {
            startGame();
        }

        if (Input.GetButtonDown("B"))
        {
            Application.Quit();
        }
    }

    void startGame()
    {
        SceneManager.LoadScene("SampleScene 1");
    }
}
