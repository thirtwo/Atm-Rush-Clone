using UnityEngine;
using DG.Tweening;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private Transform conveyorATM;
    private int moneyCount;
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
            loot.OnBeforeDestroy();
            switch (loot.lootType)
            {
                case Loot.LootType.money:
                    moneyCount++;
                    break;
                case Loot.LootType.gold:
                    moneyCount += 2;
                    break;
                case Loot.LootType.diamond:
                    moneyCount += 3;
                    break;
                default:
                    break;
            }
            loot.transform.SetParent(null);
            loot.transform.DOMoveX(conveyorATM.position.x, 0.5f);
            loot.transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                Destroy(loot.gameObject);
            });
            if (loot.lootController.loots.Count <= 0)
            {
                PlayerPrefs.SetInt("Money", moneyCount);
                GameManager.FinishGame(true);
            }
        }
    }
}
