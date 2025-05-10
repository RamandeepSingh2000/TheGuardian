using Player;
using UnityEngine;

public class DeathEnergyHolder : MonoBehaviour
{
    [SerializeField] Material deathEnergyHolderMat;
    [Range(0, 1)]
    [SerializeField] float startPercent;    

    public float DeathEnergyPercent { get; private set; }


    private void Awake()
    {
        SetFillPercent(startPercent);
    }

    private void OnAmmunitionChanged(float ammo)
    {
        SetFillPercent((float)ammo);
    }

    public void SetFillPercent(float percent)
    {
        percent = Mathf.Clamp01(percent);
        DeathEnergyPercent = percent;
        if (deathEnergyHolderMat != null)
        {
            deathEnergyHolderMat.SetFloat("_Fill", percent);
        }
    }

    private void OnValidate()
    {
        SetFillPercent(startPercent);
    }    
}
