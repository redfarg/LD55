using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonsSpawner : MonoBehaviour
{

    [SerializeField] GameManager manager;
    [SerializeField] Sprite spriteImp;
    [SerializeField] Sprite spriteFiend;
    [SerializeField] Sprite spriteLord;
    [SerializeField] Sprite spriteShrimp;
    [SerializeField] Sprite spriteFriend;
    [SerializeField] Sprite spritePotato;
 
    private List<int> summoningSuccessFullThreshholds;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        manager.OnEndOfRitual += DisplaySummon;
        manager.OnRitualStart += HideSummon;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        summoningSuccessFullThreshholds = manager.GetSummoningSuccessFullThreshholds();
    }

    private void DisplaySummon(float totalPercentage, int ritualCount)
    {
        bool ritualSuccess = totalPercentage / 100f >= summoningSuccessFullThreshholds[ritualCount] / 500f;

        if (ritualCount == 0) {
            if(ritualSuccess) spriteRenderer.sprite = spriteImp;
            else spriteRenderer.sprite = spriteShrimp;
        }
        else if (ritualCount == 1) {
            if(ritualSuccess) spriteRenderer.sprite = spriteFiend;
            else spriteRenderer.sprite = spriteFriend;
        }
        else if (ritualCount == 2) {
            if(ritualSuccess) spriteRenderer.sprite = spriteLord;
            else spriteRenderer.sprite = spritePotato;
        }
    }

    private void HideSummon(int ritualCount)
    {
        spriteRenderer.sprite = null;
    }
}
