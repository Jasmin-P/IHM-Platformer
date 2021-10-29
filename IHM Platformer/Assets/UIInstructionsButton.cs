using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInstructionsButton : MonoBehaviour
{
    [SerializeField]
    Button instructionsButton;
    [SerializeField]
    GameObject instructions;
    // Start is called before the first frame update
    void Start()
    {
        instructionsButton.onClick.AddListener(Instructions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Instructions()
    {
        Debug.Log("instructions!");
        instructions.SetActive(!instructions.activeSelf);
    } 
}
