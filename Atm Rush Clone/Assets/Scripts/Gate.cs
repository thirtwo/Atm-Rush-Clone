using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (other.TryGetComponent<Loot>(out var loot))
            {
                if (loot.lastGateID != GetInstanceID())
                {
                    loot.lastGateID = GetInstanceID();
                    loot.ConvertLoot();
                }
            }
        }
    }
}
