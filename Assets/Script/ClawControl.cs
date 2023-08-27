using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawControl : MonoBehaviour
{
    public RewardScript rewardScript;
    public float moveSpeed = 5f;
    public float clawSpeed = 2f;
    public Transform craneTransform;
    public Transform clawTransform;

    public Vector3 minBoundary;
    public Vector3 maxBoundary;

    private bool isClosed = false;
    private bool isRising = false;
    private bool canMoveCrane = true;
    private Transform heldPrize;

    private List<Transform> heldPrizes = new List<Transform>();

    Animator clawAnimator;

    private void Start()
    {
        clawAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Handle claw movement
        if (canMoveCrane && !isRising) 
        {
            // Keyboard input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Mouse input
            float mouseXInput = Input.GetAxis("Mouse X");
            float mouseYInput = Input.GetAxis("Mouse Y");

           /* Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;*/

            Vector3 arrowMovement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
            Vector3 mouseMovement = new Vector3(mouseXInput, 0f, mouseYInput) * moveSpeed * Time.deltaTime;

            Vector3 movement = arrowMovement + mouseMovement;

            Vector3 newPosition = transform.position + movement;

            newPosition.x = Mathf.Clamp(newPosition.x, minBoundary.x, maxBoundary.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBoundary.y, maxBoundary.y);
            newPosition.z = Mathf.Clamp(newPosition.z, minBoundary.z, maxBoundary.z);

            transform.position = newPosition;
        }

        // Handle claw opening and closing
        if (Input.GetKeyDown(KeyCode.Space) && !isClosed && !isRising)
        {
            StartCoroutine(CloseClaw());
        }

        // Handle claw rising
        if (isRising)
        {
            // Move the claw upwards
            Vector3 riseMovement = Vector3.up * clawSpeed * Time.deltaTime;
            craneTransform.Translate(riseMovement);

            // Check if claw has reached the top
            if (craneTransform.position.y >= 1.563f)
            {
                isRising = false;
                canMoveCrane = true;
            }
        }

        // Check if claw reach box
        if (craneTransform.position.x <= -0.7f && craneTransform.position.z <= -0.2f)
        {
            StartCoroutine(OpenClaw()); // Transition back to Open State
        }
    }

    IEnumerator CloseClaw()
    {
        Vector3 loweredPosition = craneTransform.position - Vector3.up * 1f; 
        StartCoroutine(MoveClaw(loweredPosition, 2f)); 

        yield return new WaitForSeconds(2.0f);

        // Check for collisions with prize objects using physics layers and colliders
        Collider[] colliders = Physics.OverlapSphere(clawTransform.position, 0.05f);

        List<PrizeObject> eligiblePrizes = new List<PrizeObject>();

        foreach (Collider collider in colliders)
        {
            PrizeObject prize = collider.GetComponent<PrizeObject>();
            if (prize != null && !prize.IsHeld && prize.Size <= 3) // Check size constraint
            {
                eligiblePrizes.Add(prize);
            }
        }

        // Sort eligible prizes by size (smallest to largest)
        eligiblePrizes.Sort((a, b) => a.Size.CompareTo(b.Size));

        int currentSize = 0;
        foreach (PrizeObject prize in eligiblePrizes)
        {
            if (currentSize + prize.Size <= 3) // Check size constraint
            {
                Debug.Log("Closing the claw");
                clawAnimator.SetBool("Open", false);
                clawAnimator.SetBool("Close", true);


                currentSize += prize.Size;
                heldPrize = prize.transform;
                heldPrizes.Add(prize.transform);
                heldPrize.GetComponent<PrizeObject>().Hold(clawTransform.position);

                if (currentSize == 3) // If claw is full, break
                {
                    break;
                }
            }
        }

        if (heldPrizes.Count > 0) // Check if any prizes were collected
        {
            clawAnimator.SetBool("Open", false);
            clawAnimator.SetBool("Close", true);

            GameManager.Instance.IncreaseCoin(heldPrizes.Count - 1);

            // Transition to the Rising State
            isClosed = true;
            StartCoroutine(RiseClaw());
        }
        else
        {
            clawAnimator.SetBool("Open", false);
            clawAnimator.SetBool("Close", true);
            isClosed = true;
            StartCoroutine(RiseClaw()); // If no prizes, just rise the claw

            GameManager.Instance.DecreaseCoin(1);

            yield return new WaitForSeconds(1.5f);

            clawAnimator.SetBool("Open", true);
            clawAnimator.SetBool("Close", false);
            isClosed = false;
        }
    }

    IEnumerator MoveClaw(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = craneTransform.position;

        while (elapsedTime < duration)
        {
            craneTransform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        craneTransform.position = targetPosition; // Ensure the claw reaches the exact target position
    }

    IEnumerator RiseClaw()
    {
        canMoveCrane = false;
        yield return new WaitForSeconds(1.0f);

        // Move the claw upwards
        isRising = true;
    }

    IEnumerator OpenClaw()
    {
        clawAnimator.SetBool("Open", true);
        clawAnimator.SetBool("Close", false);

        yield return new WaitForSeconds(0.5f);

        foreach (Transform prizeTransform in heldPrizes)
        {
            PrizeObject prize = prizeTransform.GetComponent<PrizeObject>();
            if (prize != null)
            {
                prize.Release();
                rewardScript.ShowReward(prize.GetRewardSprite());
            }
        }
        heldPrizes.Clear();

        isClosed = false;
    }
}
