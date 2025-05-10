using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private HealthComponent _healthComponent;
    private Renderer[] _renderers;
    // Start is called before the first frame update
    void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnEnable()
    {
        _healthComponent.OnDamageReceived += OnDamageReceived;
        _healthComponent.OnDie += OnDamageReceived;
    }

    private void OnDisable()
    {
        _healthComponent.OnDamageReceived -= OnDamageReceived;
        _healthComponent.OnDie -= OnDamageReceived;
    }

    private void OnDamageReceived()
    {
        ChangeColors(true);
        StartCoroutine(Helper.PerformActionInSeconds(0.2f, () =>
        {
            StopAllCoroutines();
            ChangeColors(false);
        }
        ));
    }

    private void ChangeColors(bool enableEmission)
    {        
        for (int i = 0; i < _renderers.Length; i++)
        {
            var materials = _renderers[i].materials;
            for (int j = 0; j < materials.Length; j++)
            {
                if (enableEmission)
                    materials[j].EnableKeyword("_EMISSION");
                else
                    materials[j].DisableKeyword("_EMISSION");
            }

        }
    }
}
