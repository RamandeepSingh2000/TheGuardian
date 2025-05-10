using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEnergyOrb : MonoBehaviour
{
    [SerializeField]
    private GameObject gfx;
    [SerializeField]
    private float travelSpeed = 5f;

    public void Init(IPlayer player, float deathEnergyAmount)
    {
        StartCoroutine(ReachTarget(player, deathEnergyAmount));
    }
    IEnumerator ReachTarget(IPlayer player, float deathEnergyAmount)
    {
        var target = player.GetDeathEnergyTargetTransform();
        while (Vector3.Distance(transform.position, target.position) > 0.5f)
        {
            transform.position += Time.deltaTime * travelSpeed * (target.position - transform.position).normalized;
            yield return null;
        }
        player.AddDeathEnergy(deathEnergyAmount);
        SFXManager.Instance.Play2DSound(SFXCategory.Player, "DeathEnergyAbsorbed");
        Destroy(gameObject);
    }
}
