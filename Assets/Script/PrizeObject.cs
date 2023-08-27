using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeObject : MonoBehaviour
{
    public bool IsHeld { get; private set; } = false;
    public Transform clawPos;
    public int Size;
    public Sprite rewardSprite;

    void Start()
    {
        clawPos = GameObject.FindWithTag("PrizeHolder").transform;
    }

    private void Update()
    {
        if (IsHeld)
        {
            //transform.position = clawPos.position; // Move the prize to the claw's position
        }
    }

    public void Hold(Vector3 clawPosition)
    {
        if (!IsHeld)
        {
            IsHeld = true;
            //transform.position = clawPosition; // Move the prize to the claw's position
            transform.parent = clawPos;
            GetComponent<Rigidbody>().isKinematic = true; // Disable physics while held
        }
    }

    public void Release()
    {
        if (IsHeld)
        {
            IsHeld = false;
            transform.parent = null;
            GetComponent<Rigidbody>().isKinematic = false; // Enable physics
        }
    }

    public Sprite GetRewardSprite()
    {
        return rewardSprite; // Return the reward sprite of the captured object
    }
}

