using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireHandler : MonoBehaviour
{
    [SerializeField] private GameObject porte;
    [SerializeField] private SerialHandler serialHandler;

    private bool wireStarted = false;

    [SerializeField] private GameObject firstWire;
    [SerializeField] private GameObject secondWire;
    [SerializeField] private GameObject thirdWire;

    int first;
    int second;
    int third;


    // Start is called before the first frame update
    void Start()
    {
        first = Random.Range(1,4);
        second = Random.Range(1, 4);
        third = Random.Range(1, 4);
        while (second == first)
        {
            second = Random.Range(1, 4);
        }
        while (third == first || third == second)
        {
           third = Random.Range(1, 4);
        }

        firstWire.GetComponent<RectTransform>().anchoredPosition = returnPosition(first);
        secondWire.GetComponent<RectTransform>().anchoredPosition = returnPosition(second);
        thirdWire.GetComponent<RectTransform>().anchoredPosition = returnPosition(third);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!wireStarted)
            {
                Debug.Log("Wire start");
                serialHandler.WireTouched();
                wireStarted = true;
            }
        }
        
    }

    public void WireFinished()
    {
        porte.SetActive(false);
        Debug.Log("Wire end");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.CompareTag("Player"))
        {
            if (!wireStarted)
            {
                Debug.Log("Wire start");
                serialHandler.WireTouched();
                wireStarted = true;
            }
        }
    }

    private Vector2 returnPosition(int value)
    {
        switch (value)
        {
            case 1:
                return new Vector2(203, 154);
            break;
            case 2:
                return new Vector2(203, -4);
            break;
            case 3:
                return new Vector2(203, -175);
            break;
        }
        return new Vector2(0,0);
    } 
}


