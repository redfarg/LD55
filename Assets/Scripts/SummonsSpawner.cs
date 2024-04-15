using System;
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
    [SerializeField] List<AudioSource> impSummonSounds;
    [SerializeField] List<AudioSource> fiendSummonSounds;
    [SerializeField] List<AudioSource> lordSummonSounds;
    [SerializeField] AudioSource shrimpSummonSound;
    [SerializeField] AudioSource friendSummonSound;
    [SerializeField] AudioSource potatoSummonSound;


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

        switch (ritualCount)
        {
            case 0:
                if (ritualSuccess)
                {
                    spriteRenderer.sprite = spriteImp;
                    PlayImpSummonSound();
                }
                else
                {
                    spriteRenderer.sprite = spriteShrimp;
                    shrimpSummonSound.Play();
                }
                break;
            case 1:
                if (ritualSuccess)
                {
                    spriteRenderer.sprite = spriteFiend;
                    PlayFiendSummonSound();
                }
                else
                {
                    spriteRenderer.sprite = spriteFriend;
                    friendSummonSound.Play();
                }
                break;
            case 2:
                if (ritualSuccess)
                {
                    spriteRenderer.sprite = spriteLord;
                    PlayLordSummonSound();
                }
                else
                {
                    spriteRenderer.sprite = spritePotato;
                    potatoSummonSound.Play();
                }
                break;
        }
    }

    private void PlayLordSummonSound()
    {
        var index = UnityEngine.Random.Range(0, lordSummonSounds.Count - 1);
        lordSummonSounds[index].Play();
    }

    private void PlayFiendSummonSound()
    {
        var index = UnityEngine.Random.Range(0, fiendSummonSounds.Count - 1);
        fiendSummonSounds[index].Play();
    }

    private void PlayImpSummonSound()
    {
        var index = UnityEngine.Random.Range(0, impSummonSounds.Count - 1);
        impSummonSounds[index].Play();
    }

    private void HideSummon(int ritualCount)
    {
        spriteRenderer.sprite = null;
    }
}
