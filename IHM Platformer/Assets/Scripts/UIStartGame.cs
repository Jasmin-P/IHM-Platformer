using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStartGame : MonoBehaviour
{
    [SerializeField]
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(startGame);
    }

    void startGame()
    {
        SceneManager.LoadScene("SampleScene 1");
    }
}
