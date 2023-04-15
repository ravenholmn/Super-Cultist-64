using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _images;

    public void UpdateUI(int remainingHealth)
    {
        foreach (var image in _images)
        {
            image.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < remainingHealth; i++)
        {
            _images[i].gameObject.SetActive(true);
        }
    }
}
