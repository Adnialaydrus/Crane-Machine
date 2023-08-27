using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;  // Array of cameras to switch between
    private int currentCameraIndex = 0;  // Index of the current active camera

    void Start()
    {
        // Disable all cameras except the initial one
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
        }

        // Attach the button click event to the function
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangeCamera);
    }

    void ChangeCamera()
    {
        // Disable the current camera
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Move to the next camera index
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // Enable the new camera
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}
