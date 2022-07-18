using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LootController : MonoBehaviour
{
    [SerializeField] private Transform lootParent;
    [SerializeField] private Transform player;
    //[HideInInspector]
    public List<Loot> loots = new List<Loot>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Collect(other.transform);
        }
    }

    private void Collect(Transform loot)
    {
        if (lootParent.childCount <= 0)
        {
            loot.SetParent(lootParent);
            loot.localPosition += Vector3.forward / 2;
        }
        else
        {
            var lastLoot = lootParent.GetChild(lootParent.childCount - 1);
            loot.SetParent(lootParent);
            loot.localPosition = lastLoot.localPosition + Vector3.forward/2;
        }
        if (loot.TryGetComponent<Loot>(out var money))
        {
            loots.Add(money);
            money.lootController = this;
        }
        StartCoroutine(AnimateLoots());
    }
    private IEnumerator AnimateLoots()
    {
        var tempLoots = loots;
        for (int i = tempLoots.Count - 1; i >= 0; i--)
        {
            try
            {
                if (tempLoots[i] == null) continue;
                tempLoots[i].CollectAnimation();
            }
            catch
            {
                //
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void MoveLootOneByOne()
    {
        StartCoroutine(MoveSyncronous());
    }
    private IEnumerator MoveSyncronous()
    {
        var pos = player.position.x;
        foreach (Transform child in lootParent)
        {
            if (child.TryGetComponent<Loot>(out var loot))
            {
                var distance = Mathf.Abs(child.position.x - pos);
                loot.Move(pos, distance / 2);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
