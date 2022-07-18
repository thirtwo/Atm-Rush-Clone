using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ATM : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            CollectLoot(other);
        }
    }

    private void CollectLoot(Collider other)
    {
        if (other.TryGetComponent<Loot>(out var loot))
        {
            //particle
            loot.OnBeforeDestroy();
            loot.transform.SetParent(null);
            loot.transform.DOMove(transform.position + Vector3.up, 0.3f);
            loot.transform.DOScale(0, 0.3f).OnComplete(() =>
            {
                Destroy(loot.gameObject);
            });
        }
    }
}
