using UnityEngine;
using UnityEngine.Pool;

public class PoolableAudioSource : MonoBehaviour
{
    public AudioSource AudioSource { get; set; }

    private IObjectPool<PoolableAudioSource> _objectPool;
    private bool _isPaused = false;

    public IObjectPool<PoolableAudioSource> ObjectPool { set => _objectPool = value; }
    public bool IsPaused => _isPaused;
    public bool IsPlaying => AudioSource.isPlaying;

    private void Awake()
    {
        this.AudioSource = GetComponent<AudioSource>();
    }

    public void Play(SoundAsset sound, float volumeMultiplier = 1f, Transform transform = null, Vector3? position = null)
    {
        _isPaused = false;

        AudioSource.volume = sound.Volume * volumeMultiplier;
        AudioSource.pitch = sound.Pitch;
        AudioSource.spatialBlend = sound.Is3D ? 1 : 0;
        AudioSource.loop = sound.Loop;
        AudioSource.clip = sound.Clip;
        AudioSource.time = sound.StartTime;

        if (sound.Is3D)
        {
            if (transform != null)
            {
                this.transform.SetParent(transform);
                this.transform.localPosition = Vector3.zero;
            }
            else if (position != null)
                this.transform.position = position.Value;
        }

        AudioSource.Play();

        if (!sound.Loop)
            StartCoroutine(Helper.PerformActionInSeconds(sound.EndTime != -1 ? sound.EndTime : sound.Clip.length, Deactivate));
        else
        {
            if (sound.StartTime != 0)
                StartCoroutine(Helper.PerformActionInSeconds(sound.EndTime != -1 ? sound.EndTime : sound.Clip.length, () =>
                {
                    Play(sound, volumeMultiplier, transform, position);
                }));
        }
    }

    public void Pause()
    {
        AudioSource.Pause();
        _isPaused = true;
    }

    public void Resume()
    {
        if (!_isPaused)
            return;

        AudioSource.UnPause();
        _isPaused = false;
    }

    /// <summary>
    /// Places the instance into the pool
    /// </summary>
    public void Deactivate()
    {
        AudioSource.Stop();
        StopAllCoroutines();
        this.transform.SetParent(null);
        _objectPool.Release(this);
    }
}
