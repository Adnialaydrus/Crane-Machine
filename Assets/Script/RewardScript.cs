using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardScript : MonoBehaviour
{
    public Image rewardImagePrefab; // Reference to the UI Image prefab for rewards
    public Transform rewardImageContainer; // Parent transform for reward images
    public GameObject rewardPanel;

    private List<Image> rewardImages = new List<Image>();

    private void Start()
    {
        // Hide the reward images initially
        foreach (Transform child in rewardImageContainer)
        {
            child.gameObject.SetActive(false);
            rewardImages.Add(child.GetComponent<Image>());
        }
    }

    public void ShowReward(Sprite rewardSprite)
    {
        foreach (Image rewardImage in rewardImages)
        {
            if (!rewardImage.gameObject.activeSelf)
            {
                rewardImage.sprite = rewardSprite;
                rewardPanel.SetActive(true);
                rewardImage.gameObject.SetActive(true);
                StartCoroutine(HideRewardImage(rewardImage));
                break;
            }
        }
    }

    IEnumerator HideRewardImage(Image image)
    {
        yield return new WaitForSeconds(3.0f); // Adjust the duration as needed
        rewardPanel.SetActive(false);
        image.gameObject.SetActive(false);
    }
}
