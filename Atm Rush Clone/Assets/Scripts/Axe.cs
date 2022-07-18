using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Axe : MonoBehaviour
{
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetZ;
    private void Start()
    {
        transform.DOLocalRotate(new Vector3(0,0,89),5,RotateMode.Fast).SetLoops(-1, LoopType.Yoyo).Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (other.TryGetComponent<Loot>(out var loot))
            {
                var loots = loot.lootController.loots;
                var index = loots.IndexOf(loot);
                var removed = new List<Loot>();
                for (int i = index; i < loots.Count; i++)
                {
                    removed.Add(loots[i]);
                }
                loots = loots.Except(removed).ToList();
                loot.lootController.loots = loots;
                for (int i = 0; i < removed.Count; i++)
                {
                    var money = removed[i];
                    var seq = DOTween.Sequence();
                    money.transform.SetParent(null);
                    seq.Append(money.transform.DOMove(new Vector3(transform.position.x + Random.Range(-offsetX, offsetX),
                         money.transform.position.y, transform.position.z + Random.Range(2.5f, offsetZ)), 0.25f));
                    seq.Append(money.transform.DOMoveY(0, 0.1f).SetEase(Ease.InBounce).SetLoops(5, LoopType.Yoyo));
                }
            }
        }
    }
}
