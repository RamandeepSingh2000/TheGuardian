using UnityEngine;

public enum SFXCategory
{
    Music,
    Player,
    Enemy
}

[CreateAssetMenu(fileName = "SoundAsset", menuName = "SFX/Sound", order = 1)]
public class SoundAsset : ScriptableObject
{
    [SerializeField] private SFXCategory _category;
    [SerializeField] private string _name;

    [SerializeField] private AudioClip _clip;
    [SerializeField, Range(-3, 3)] private float _pitch;
    [SerializeField, Range(0, 1)] private float _volume;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _is3D;
    [SerializeField] private float _startTime = 0;
    [SerializeField] private float _endTime = -1;

    public SFXCategory Category => _category;
    public string Name => _name;

    public AudioClip Clip => _clip;
    public float Pitch => _pitch;
    public float Volume => _volume;
    public bool Loop => _loop;
    public bool Is3D => _is3D;

    public float StartTime => _startTime;
    public float EndTime => _endTime;
}
