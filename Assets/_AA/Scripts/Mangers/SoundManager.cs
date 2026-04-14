using System;
using UnityEngine;

[Serializable]
public struct SoundData
{
    public SoundType Type;
    public AudioClip Clip;
    [Tooltip("Ayný sesin üst üste binmesini engellemek için geçmesi gereken minimum süre (Saniye). Örn: 0.05")]
    public float Cooldown;
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundData[] _soundDataArray;
    private AudioSource _sfxSource;
    private AudioClip[] _clips;
    private float[] _cooldowns;
    private float[] _lastPlayedTimes;

    private void Awake()
    {
        _sfxSource = GetComponent<AudioSource>();

        _sfxSource.spatialBlend = 0f;

        int enumCount = (int)SoundType.COUNT;

        _clips = new AudioClip[enumCount];
        _cooldowns = new float[enumCount];
        _lastPlayedTimes = new float[enumCount];

        foreach (var data in _soundDataArray)
        {
            int index = (int)data.Type;
            _clips[index] = data.Clip;
            _cooldowns[index] = data.Cooldown;
            _lastPlayedTimes[index] = -100f;
        }
    }

    private void OnEnable()
    {
        GameEvents.PlaySound += OnPlaySound;
    }

    private void OnDisable()
    {
        GameEvents.PlaySound -= OnPlaySound;
    }

    private void OnPlaySound(SoundType soundType)
    {
        int index = (int)soundType;

        if (_clips[index] == null)
        {
            Debug.LogWarning($"Eksik ses referansý: {soundType}");
            return;
        }
        if (Time.time - _lastPlayedTimes[index] < _cooldowns[index])
        {
            return;
        }
        _lastPlayedTimes[index] = Time.time;
        _sfxSource.PlayOneShot(_clips[index]);
    }
}
public enum SoundType
{
    EnemyHit,
    PlayerShoot,
    Explosion,
    LevelUp,
    ButtonHover,
    COUNT
}