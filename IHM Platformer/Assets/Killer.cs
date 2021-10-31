using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    private Vector3 startingPosition;
    public ParticleSystem deathParticle;

    private void Start()
    {
        startingPosition = PlayerController.instance.transform.position;    
    }

    public void KillPlayer(Vector3 position)
    {
        transform.position = position;
        deathParticle.Play();
        Debug.Log(deathParticle.transform.position);

        PlayerController.instance.transform.position = startingPosition;
    }
}
