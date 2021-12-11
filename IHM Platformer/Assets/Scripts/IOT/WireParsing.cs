using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireParsing : MonoBehaviour
{
    private int leftConnexion = 0;
    private int rightConnexion = 0;
    [SerializeField] WireHandler wireHandler;



    public bool ParseMessage(string str)
    {

        if (str.Split(' ')[0] == "c")
        {
            leftConnexion = int.Parse(str.Split(' ')[1]);
            rightConnexion = int.Parse(str.Split(' ')[2]);
            wireHandler.connectWire(leftConnexion, rightConnexion);
        }
        
        else if (str.Split(' ')[0] == "u")
        {
            leftConnexion = int.Parse(str.Split(' ')[1]);
            wireHandler.unconnectWire(leftConnexion);
        }

        return true;

    }
}
