using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;

        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;
            transform.position = new Vector3(x, y,originalPos.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
    }
}
