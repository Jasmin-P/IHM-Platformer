using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireHandler : MonoBehaviour
{
    [SerializeField] private GameObject porte;
    [SerializeField] private SerialHandler serialHandler;

    private bool wireStarted = false;

    [SerializeField] private GameObject wiresCanvas;

    [SerializeField] private GameObject firstWire;
    [SerializeField] private GameObject secondWire;
    [SerializeField] private GameObject thirdWire;

    [SerializeField] private LineRenderer greenWire;
    [SerializeField] private LineRenderer yellowWire;
    [SerializeField] private LineRenderer redWire;

    private List<Vector2> positionsCanvas = new List<Vector2>(3);
    private List<Vector3> positions = new List<Vector3>(3);

    int first;
    int second;
    int third;

    int currentFirst;
    int currentSecond;
    int currentThird;


    // Start is called before the first frame update
    void Start()
    {
        positionsCanvas.Add(new Vector2(203, 154));
        positionsCanvas.Add(new Vector2(203, -4));
        positionsCanvas.Add(new Vector2(203, -175));

        positions.Add(new Vector3(10, 6.7f, -1));
        positions.Add(new Vector3(10, -1.2f, -1));
        positions.Add(new Vector3(10, -9.7f, -1));

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

        greenWire.gameObject.SetActive(false);
        yellowWire.gameObject.SetActive(false);
        redWire.gameObject.SetActive(false);
        wiresCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!wireStarted)
            {
                Debug.Log("Wire start");
                serialHandler.WireStart();
                wireStarted = true;

                wiresCanvas.gameObject.SetActive(true);
                greenWire.gameObject.SetActive(false);
                yellowWire.gameObject.SetActive(false);
                redWire.gameObject.SetActive(false);
            }
        }


        if (wireStarted)
        {
            TestWireComplete();
        }
        
    }

    private void TestWireComplete()
    {
        if (first == currentFirst && second == currentSecond && third == currentThird)
        {
            WireFinished();
        }
    }


    public void WireFinished()
    {
        greenWire.gameObject.SetActive(false);
        yellowWire.gameObject.SetActive(false);
        redWire.gameObject.SetActive(false);
        wiresCanvas.gameObject.SetActive(false);
        
        wireStarted = false;
        porte.SetActive(false);
        serialHandler.WireEnd();
    }
    /*
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
    */
    private Vector2 returnPosition(int value)
    {
        switch (value)
        {
            case 1:
                return positionsCanvas[0];
            break;
            case 2:
                return positionsCanvas[1];
            break;
            case 3:
                return positionsCanvas[2];
            break;
        }
        return new Vector2(0,0);
    } 

    public void connectWire(int wireNumber, int positionNumber)
    {
        switch (wireNumber)
        {
            case 0:
                greenWire.gameObject.SetActive(true);
                greenWire.SetPosition(1, positions[positionNumber]);
                currentFirst = positionNumber + 1;
                break;
            case 1:
                yellowWire.gameObject.SetActive(true);
                yellowWire.SetPosition(1, positions[positionNumber]);
                currentSecond = positionNumber + 1;
                break;
            case 2:
                redWire.gameObject.SetActive(true);
                redWire.SetPosition(1, positions[positionNumber]);
                currentThird = positionNumber + 1;
                break;
        }

    }

    public void unconnectWire(int wireNumber)
    {
        switch (wireNumber)
        {
            case 0:
                greenWire.gameObject.SetActive(false);
                break;
            case 1:
                yellowWire.gameObject.SetActive(false);
                break;
            case 2:
                redWire.gameObject.SetActive(false);
                break;
        }

    }
}


