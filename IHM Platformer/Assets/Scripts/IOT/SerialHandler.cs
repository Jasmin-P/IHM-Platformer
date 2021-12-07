using System;
using System.IO.Ports;
using UnityEngine;

public class SerialHandler : MonoBehaviour
{

    private SerialPort _serial;

    // Common default serial device on a Windows machine
    [SerializeField] private string serialPort = "COM4";
    [SerializeField] private int baudrate = 115200;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private WireHandler wireHandler;

    private int speed_value_recieved = 0;
    private Vector2 direction = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        _serial = new SerialPort(serialPort, baudrate);
        // Once configured, the serial communication must be opened just like a file : the OS handles the communication.
        _serial.Open();

    }

    // Update is called once per frame
    void Update()
    {
        // Prevent blocking if no message is available as we are not doing anything else
        // Alternative solutions : set a timeout, read messages in another thread, coroutines, futures...
        if (_serial.BytesToRead <= 0) return;

        var message = _serial.ReadLine();

        // Arduino sends "\r\n" with println, ReadLine() removes Environment.NewLine which will not be 
        // enough on Linux/MacOS.
        if (Environment.NewLine == "\n")
        {
            message = message.Trim('\r');
        }

        switch (message)
        {
            case "X +":
                playerController.Dash(new Vector2(1, 0));
                break;
            case "X -":
                playerController.Dash(new Vector2(-1, 0));
                break;
            case "Y +":
                playerController.Dash(new Vector2(0, 1));
                break;
            case "Y -":
                playerController.Dash(new Vector2(0, -1));
                break;
            case "Wire Finished":
                wireHandler.WireFinished();
                break;
        }
        if (message.Split(' ')[0] == "speed")
        {
            int value = Int32.Parse(message.Split(' ')[1]);
        }


    }

    public void SetLed(bool newState)
    {
        _serial.WriteLine(newState ? "LED ON" : "LED OFF");
    }

    private void OnDestroy()
    {
        _serial.Close();
    }

    public void WireTouched()
    {
        _serial.WriteLine("wire");
    }
}

