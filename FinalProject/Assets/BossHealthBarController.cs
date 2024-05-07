using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarController : MonoBehaviour
{
    public FighterStats bossStats;

    public Image healthBarFull;

    private float initialHeight;
    private Vector3 initialPosition;

    void Start()
    {
        initialHeight = healthBarFull.rectTransform.sizeDelta.y;
        initialPosition = healthBarFull.rectTransform.localPosition;
        bossStats.onHealthChangedCallback += UpdateHealthBar;
    }
    public void UpdateHealthBar()
    {
        float healthPercent = (float)bossStats.CurrentHealth / bossStats.MaxHealth;

        float newHeight = initialHeight * healthPercent;

        Vector2 size = healthBarFull.rectTransform.sizeDelta;
        size.y = newHeight;
        healthBarFull.rectTransform.sizeDelta = size;


        Vector3 pos = initialPosition;
        pos.y -= (initialHeight - newHeight) / 2; // Adjust position to center
        healthBarFull.rectTransform.localPosition = pos;
    }
}
