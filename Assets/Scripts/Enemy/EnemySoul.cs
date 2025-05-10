using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoul : MonoBehaviour
{
    [SerializeField]
    private GameObject gfx;
    [SerializeField]
    private float travelSpeed = 5f;    

    public void SetTarget(EnemyStateMachineContext enemyContext)
    {
        StartCoroutine(ReachTarget(enemyContext));
    }
    IEnumerator ReachTarget(EnemyStateMachineContext target)
    {
        while(Vector3.Distance(transform.position, target.SoulTargetTransform.position) > 0.5f)
        {
            transform.position += Time.deltaTime * travelSpeed * (target.SoulTargetTransform.position - transform.position).normalized;
            yield return null;
        }        
        target.Restore();
        Destroy(gameObject);
    }
}
