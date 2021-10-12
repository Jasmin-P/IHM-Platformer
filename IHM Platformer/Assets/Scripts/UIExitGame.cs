using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIExitGame : MonoBehaviour
{
    [SerializeField]
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(exitGame);
    }

    void exitGame()
    {
        Application.Quit();
        Debug.Log("Game is closed");
    }
}
