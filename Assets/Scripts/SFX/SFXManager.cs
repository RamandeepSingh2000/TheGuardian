using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class SFXManager : Singleton<SFXManager>, IPausable
{
    public static string MUSIC_VOLUME_PROPERTY = "MUSIC_VOLUME";
    public static string SFX_VOLUME_PROPERTY = "SFX_VOLUME";

    [SerializeField] protected int _defaultCapacity = 5;
    [SerializeField] protected int _maxSize = 10;

    [SerializeField] private PoolableAudioSource _audioSourcePrefab;
    [SerializeField] private SoundAsset[] _sounds;

    private IObjectPool<PoolableAudioSource> _audioSourcePool;
    private List<PoolableAudioSource> _audioSourcesInstances = new();

    private bool _triggerOnGamePause = true;
    public bool TriggerOnGamePaused { get => true; set => _triggerOnGamePause = value; }

    private float _sfxMultiplier = 1;
    private float _musicMultiplier = 1;

    private void OnEnable()
    {
        GameManager.Instance.OnGameResumed += OnGameResumed;
        GameManager.Instance.OnGamePaused += OnGamePaused;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameResumed -= OnGameResumed;
        GameManager.Instance.OnGamePaused -= OnGamePaused;
    }

    #region POOL

    protected override void Awake()
    {
        base.Awake();
        //A projectile pool is created in order to reuse projectiles instances
        _audioSourcePool = new ObjectPool<PoolableAudioSource>(CreateAudioSource, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck: true, _defaultCapacity, _maxSize);
        if (PlayerPrefs.HasKey(MUSIC_VOLUME_PROPERTY))
            _musicMultiplier = PlayerPrefs.GetFloat(MUSIC_VOLUME_PROPERTY);
        else PlayerPrefs.SetFloat(MUSIC_VOLUME_PROPERTY, _musicMultiplier);

        if (PlayerPrefs.HasKey(SFX_VOLUME_PROPERTY))
            _sfxMultiplier = PlayerPrefs.GetFloat(SFX_VOLUME_PROPERTY);
        else PlayerPrefs.SetFloat(SFX_VOLUME_PROPERTY, _sfxMultiplier);
    }

    /// <summary>
    /// Destroys an instance from the pool in case it is above the maximum amount
    /// </summary>
    /// <param name="audioSource"></param>
    private void OnDestroyPooledObject(PoolableAudioSource audioSource)
    {
        _audioSourcesInstances.Remove(audioSource);
        Destroy(audioSource.gameObject);
    }

    /// <summary>
    /// Deactivates the projectile gameobject once it's put into the pool
    /// </summary>
    /// <param name="audioSource"></param>
    private void OnReleaseToPool(PoolableAudioSource audioSource)
    {
        audioSource.gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates the projectile gameobject once it's released from the pool
    /// </summary>
    /// <param name="audioSource"></param>
    private void OnGetFromPool(PoolableAudioSource audioSource)
    {
        audioSource.gameObject.SetActive(true);
    }

    /// <summary>
    /// Instantiates the projectile and updates its references
    /// </summary>
    /// <returns></returns>
    private PoolableAudioSource GetNextAudioSource()
    {
        return _audioSourcePool.Get();
    }

    private PoolableAudioSource CreateAudioSource()
    {
        var instance = Instantiate(_audioSourcePrefab);
        instance.ObjectPool = _audioSourcePool;
        _audioSourcesInstances.Add(instance);
        return instance;
    }

    #endregion

    public PoolableAudioSource Play2DSound(SFXCategory category, string soundName)
    {
        var soundAsset = GetSoundAsset(category, soundName);
        if (soundAsset == null)
            return null;

        var audioSource = GetNextAudioSource();
        audioSource.Play(soundAsset, volumeMultiplier: (category == SFXCategory.Music) ? _musicMultiplier : _sfxMultiplier);
        return audioSource;
    }

    public PoolableAudioSource Play3DSound(SFXCategory category, string soundName, Transform transform)
    {
        var soundAsset = GetSoundAsset(category, soundName);
        if (soundAsset == null)
            return null;

        var audioSource = GetNextAudioSource();
        audioSource.Play(soundAsset, volumeMultiplier: (category == SFXCategory.Music) ? _musicMultiplier : _sfxMultiplier, transform: transform);
        return audioSource;
    }

    public PoolableAudioSource Play3DSound(SFXCategory category, string soundName, Vector3 position)
    {
        var soundAsset = GetSoundAsset(category, soundName);
        if (soundAsset == null)
            return null;

        var audioSource = GetNextAudioSource();
        audioSource.Play(soundAsset, volumeMultiplier: (category == SFXCategory.Music) ? _musicMultiplier : _sfxMultiplier, position: position);
        return audioSource;
    }

    private SoundAsset GetSoundAsset(SFXCategory category, string soundName) => _sounds.Where(s => s.Category == category)?.FirstOrDefault(s => s.Name.Equals(soundName, System.StringComparison.OrdinalIgnoreCase));

    public void OnGamePaused()
    {
        for (int i = 0; i < _audioSourcesInstances.Count; i++)
        {
            if (_audioSourcesInstances[i].isActiveAndEnabled && _audioSourcesInstances[i].IsPlaying)
                _audioSourcesInstances[i].Pause();
        }
    }

    public void OnGameResumed()
    {
        for (int i = 0; i < _audioSourcesInstances.Count; i++)
        {
            if (_audioSourcesInstances[i].isActiveAndEnabled && _audioSourcesInstances[i].IsPaused)
                _audioSourcesInstances[i].Resume();
        }
    }
}
