/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private GameObject[] heartContainers;
    private Image[] heartFills;

    public Transform playerHeartsParent;
    public GameObject playerHeartContainerPrefab;

    public FighterStats playerFighterStats;
    
    private void Start()
    {
        heartContainers = new GameObject[playerFighterStats.TotalMaxHealth];
        heartFills = new Image[playerFighterStats.TotalMaxHealth];

        playerFighterStats.onHealthChangedCallback += UpdateHeartsHUD;
        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    public void UpdateHeartsHUD()
    {
        SetHeartContainers();
        SetFilledHearts();
    }

    void SetHeartContainers()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < playerFighterStats.MaxHealth)
            {
                heartContainers[i].SetActive(true);
            }
            else
            {
                heartContainers[i].SetActive(false);
            }
        }
    }

    void SetFilledHearts()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < playerFighterStats.CurrentHealth)
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }

        if (playerFighterStats.CurrentHealth % 1 != 0)
        {
            int lastPos = Mathf.FloorToInt(playerFighterStats.CurrentHealth);
            heartFills[lastPos].fillAmount = playerFighterStats.CurrentHealth % 1;
        }
    }

    void InstantiateHeartContainers()
    {
        for (int i = 0; i < playerFighterStats.TotalMaxHealth; i++)
        {
            GameObject temp = Instantiate(playerHeartContainerPrefab);
            temp.transform.SetParent(playerHeartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
}
