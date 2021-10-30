using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(returnToMenu);
    }

    void returnToMenu()
    {
        
        SceneManager.LoadScene("Menu");

    }
}
