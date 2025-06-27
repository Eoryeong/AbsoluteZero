using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CampfireArea : MonoBehaviour
{
    public float detectionRadius = 3f;
    public float playerIncCold = 0.1f;
    public float duration = 30f;
    public LayerMask detectionLayer;

    private HashSet<PickupItem> grillTargets = new(); // 현재 영역 내 고기
    private bool playerIn;
    private bool soundOn;

    private void Start()
    {
        playerIn = false;
        soundOn = false;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if(duration <= 0)
        {
            SoundManager.Instance.StopFireSound();
            Destroy(gameObject);
        }

        if (playerIn)
        {
            PlayerStatusManager.Instance.AddCurrentCold(playerIncCold * Time.deltaTime);
        }    

        foreach (var item in grillTargets)
        {
            if (item == null) continue;

            // itemCode 6 = 날고기
            if (item.data.itemCode == 6)
            {
                item.grillTime -= Time.deltaTime;

                if (item.grillTime <= 0)
                {
                    // 익은 고기 생성 itemCode 7 = 익힌 고기
                    Instantiate(ItemDatabase.instance.GetItemPrefabByCode(7), item.transform.position, Quaternion.identity);
                    Destroy(item.gameObject);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // 주기적으로 주변 오브젝트 확인 (최소화된 검사)
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        HashSet<PickupItem> currentFrame = new();
        bool playerHot = false;
        GameObject playerObj = null;

        foreach (var col in hits)
        {
            if (col.CompareTag("Item"))
            {
                var item = col.GetComponent<PickupItem>();
                if (item != null && item.data.itemCode == 6)
                {
                    currentFrame.Add(item);
                    grillTargets.Add(item); // 새로 들어온 항목 포함
                }
            }
            else if (col.CompareTag("Player"))
            {
                playerHot = true;
            }
        }

        // 영역을 벗어난 오브젝트 제거
        grillTargets.RemoveWhere(item => !currentFrame.Contains(item));

        if(playerHot)
        {
            playerIn = true;
            if (!soundOn)
            {
                soundOn = true;
                SoundManager.Instance.PlaySurvivalSound(SoundManager.SurvivalSoundType.Campfire);
            }
        }
        else
        {
            playerIn = false;
            if (soundOn)
            {
                soundOn = false;
                SoundManager.Instance.StopFireSound();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}