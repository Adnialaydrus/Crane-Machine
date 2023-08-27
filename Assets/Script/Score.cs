using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int scoreValue;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "prize")
        {
            ObjectSpawner spawner = FindObjectOfType<ObjectSpawner>();
            // The object has entered the box trigger area
            ScoreManager.Instance.IncreaseScore(scoreValue);
            spawner.ObjectDestroyed();
            // Destroy the object
            Destroy(gameObject);
        }
    }
}
