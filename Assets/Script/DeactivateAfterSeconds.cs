using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAfterSeconds : MonoBehaviour
{
    public float deactivationTime = 5f; // Time in seconds after which the object will be deactivated

    private float timer;

    private void Start()
    {
        timer = deactivationTime;
    }

    private void Update()
    {
        // Countdown the timer
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
