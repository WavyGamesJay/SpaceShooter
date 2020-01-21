using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] bool cameraShake = false;
    [SerializeField] float shakeDuration = 2f;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraShake == true) {
            transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), startingPos.z);

            shakeDuration -= Time.deltaTime;

            if(shakeDuration <= 0f) {
                cameraShake = false;

                transform.position = startingPos;
            }

            Debug.Log("Camera is Shaking !");
        }
    }

    public void CameraShaking() {
        shakeDuration = 2f;

        cameraShake = true;
    }
}
