using UnityEngine;
using DG.Tweening;
public class Loot : MonoBehaviour
{
    public enum LootType
    {
        money,
        gold,
        diamond
    }

    public LootType lootType;
    [SerializeField] Loot[] loots;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private AnimationCurve animationCurve;
    [HideInInspector] public int lastGateID = 0;
    [HideInInspector] public LootController lootController;
    Sequence sequence;
    Vector3 startScale;
    private void Start()
    {
        sequence = DOTween.Sequence();
        startScale = transform.localScale;
    }

    public void Move(float posX,float time)
    {
        sequence.Append(transform.DOMoveX(posX, time).SetEase(animationCurve));
    }
    public void CollectAnimation()
    {
        transform.DOScale(1.5f, 0.2f).OnComplete(() =>
        {
            transform.DOScale(startScale, 0.2f);
        });
    }

    [ContextMenu("Convert")]
    public void ConvertLoot()
    {
        switch (lootType)
        {
            case LootType.money:
                ConvertToGold();
                break;
            case LootType.gold:
                ConvertToDiamond();
                break;
            case LootType.diamond:
                break;
            default:
                break;
        }
    }

    private void ConvertToDiamond()
    {
        lootType = LootType.diamond;
        meshFilter.mesh = loots[2].meshFilter.sharedMesh;
        meshRenderer.material = loots[2].meshRenderer.sharedMaterial;
        transform.rotation = loots[2].transform.rotation;
        transform.position = new Vector3(transform.position.x, loots[2].transform.position.y, transform.position.z);
        transform.localScale = Vector3.one;
        startScale = transform.localScale;
        CollectAnimation();
    }

    private void ConvertToGold()
    {
        lootType = LootType.gold;
        meshFilter.mesh = loots[1].meshFilter.sharedMesh;
        meshRenderer.material = loots[1].meshRenderer.sharedMaterial;
        transform.rotation = loots[1].transform.rotation;
        transform.position = new Vector3(transform.position.x, loots[1].transform.position.y, transform.position.z);
        transform.localScale = Vector3.one;
        startScale = transform.localScale;
        CollectAnimation();
        //particle
    }
    public void OnBeforeDestroy()
    {
        if(lootController != null)
        {
            lootController.loots.Remove(this);
        }
    }
}
