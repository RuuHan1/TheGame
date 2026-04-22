using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct SoundData
{
    public SfxType Type;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume;
    [Tooltip("Ayný sesin üst üste binmesini engellemek için geçmesi gereken minimum süre (Saniye). Örn: 0.05")]
    public float Cooldown;
}
[Serializable]
public struct MusicData
{
    public MusicType Type;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume;
    public bool Loop;
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField, Range(0f, 1f)] private float _sfxVolume = 1f;
    [SerializeField] private SoundData[] _soundDataArray;
    [Header("Müzik")]
    [SerializeField] private MusicData[] _musicDataArray;
    [SerializeField, Range(0f, 1f)] private float _musicVolume = 1f;
    [SerializeField] private float _fadeDuration = 1f;
    private AudioSource _sfxSource;
    private AudioClip[] _clips;
    private float[] _cooldowns;
    private float[] _lastPlayedTimes;
    private float[] _sfxVolumes;
    // Music
    private AudioSource _musicSource;
    private AudioClip[] _musicClips;
    private float[] _musicVolumes;
    private bool[] _musicLoops;
    private MusicType _currentMusic;
    private Coroutine _fadeCoroutine;
    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = Mathf.Clamp01(value);
            // Fade yoksa anýnda uygula
            if (_fadeCoroutine == null)
                _musicSource.volume = _musicVolume;
        }
    }
    public float SfxVolume
    {
        get => _sfxVolume;
        set => _sfxVolume = Mathf.Clamp01(value);
    }

    private void Awake()
    {
        SetupSfxSource();
        SetupMusicSource();
        InitSfxArrays();
        InitMusicArrays();
        LoadSavedVolumes();
    }


    private void OnEnable()
    {

        GameEvents.PlaySound += OnPlaySound;
        GameEvents.PlayMusic += OnPlayMusic;
        GameEvents.StopMusic += OnStopMusic;
        GameEvents.SfxSliderChanged += OnSfxSliderChanged;
        GameEvents.MusicSliderChanged += OnMusicSliderChanged;
    }

    private void OnDisable()
    {
        GameEvents.PlaySound -= OnPlaySound;
        GameEvents.PlayMusic -= OnPlayMusic;
        GameEvents.StopMusic -= OnStopMusic;
        GameEvents.SfxSliderChanged -= OnSfxSliderChanged;
        GameEvents.MusicSliderChanged -= OnMusicSliderChanged;

    }

    private void OnPlaySound(SfxType soundType)
    {
        int i = (int)soundType;

        if (_clips[i] == null)
        {
            Debug.LogWarning($"Eksik ses referansý: {soundType}");
            return;
        }

        if (Time.time - _lastPlayedTimes[i] < _cooldowns[i])
            return;

        _lastPlayedTimes[i] = Time.time;
        _sfxSource.PlayOneShot(_clips[i], _sfxVolumes[i] * _sfxVolume);
    }

    private void SetupSfxSource()
    {
        _sfxSource = GetComponent<AudioSource>();
        _sfxSource.spatialBlend = 0f;
        _sfxSource.playOnAwake = false;
    }

    private void SetupMusicSource()
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.spatialBlend = 0f;
        _musicSource.playOnAwake = false;
        _musicSource.loop = true;
    }

    private void InitSfxArrays()
    {
        int count = (int)SfxType.COUNT;
        _clips = new AudioClip[count];
        _cooldowns = new float[count];
        _lastPlayedTimes = new float[count];
        _sfxVolumes = new float[count];

        foreach (var data in _soundDataArray)
        {
            int i = (int)data.Type;
            _clips[i] = data.Clip;
            _cooldowns[i] = data.Cooldown;
            _lastPlayedTimes[i] = -100f;
            _sfxVolumes[i] = data.Volume > 0f ? data.Volume : 1f;
        }
    }

    private void InitMusicArrays()
    {
        int count = (int)MusicType.COUNT;
        _musicClips = new AudioClip[count];
        _musicVolumes = new float[count];
        _musicLoops = new bool[count];

        foreach (var data in _musicDataArray)
        {
            int i = (int)data.Type;
            _musicClips[i] = data.Clip;
            _musicVolumes[i] = data.Volume > 0f ? data.Volume : 1f;
            _musicLoops[i] = data.Loop;
        }
    }

    private void LoadSavedVolumes()
    {
        _sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }
    private void OnPlayMusic(MusicType musicType)
    {
        // Zaten çalýyorsa tekrar baþlatma
        if (_currentMusic == musicType && _musicSource.isPlaying)
            return;

        int i = (int)musicType;

        if (_musicClips[i] == null)
        {
            Debug.LogWarning($"Eksik müzik referansý: {musicType}");
            return;
        }

        _currentMusic = musicType;
        _musicSource.loop = _musicLoops[i];

        if (_fadeDuration > 0f)
            FadeTo(_musicClips[i], _musicVolumes[i]);
        else
            PlayImmediate(_musicClips[i], _musicVolumes[i]);
    }

    private void OnStopMusic(bool fade)
    {
        if (fade && _fadeDuration > 0f)
            FadeOut();
        else
            _musicSource.Stop();
    }

    private void PlayImmediate(AudioClip clip, float baseVolume)
    {
        _musicSource.clip = clip;
        _musicSource.volume = baseVolume * _musicVolume;
        _musicSource.Play();
    }
    private void FadeTo(AudioClip newClip, float targetBaseVolume)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeRoutine(newClip, targetBaseVolume));
    }

    private void FadeOut()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeRoutine(AudioClip newClip, float targetBaseVolume)
    {
        // Mevcut sesi kýs
        if (_musicSource.isPlaying)
        {
            float startVolume = _musicSource.volume;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / _fadeDuration;
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, t);
                yield return null;
            }
        }

        // Yeni sesi baþlat ve aç
        _musicSource.clip = newClip;
        _musicSource.volume = 0f;
        _musicSource.Play();

        float targetVolume = targetBaseVolume * _musicVolume;
        float t2 = 0f;

        while (t2 < 1f)
        {
            t2 += Time.deltaTime / _fadeDuration;
            _musicSource.volume = Mathf.Lerp(0f, targetVolume, t2);
            yield return null;
        }

        _musicSource.volume = targetVolume;
        _fadeCoroutine = null;
    }

    private IEnumerator FadeOutRoutine()
    {
        float startVolume = _musicSource.volume;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / _fadeDuration;
            _musicSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        _musicSource.Stop();
        _fadeCoroutine = null;
    }
    //-----------settings
    public void OnSfxSliderChanged(float value)
    {
        SfxVolume = value;
        PlayerPrefs.SetFloat("SfxVolume", value);
    }
    public void OnMusicSliderChanged(float value)
    {
        MusicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    //public void SetSfxVolume(float value)
    //{
    //    SfxVolume = value;
    //    PlayerPrefs.SetFloat("SfxVolume", value);
    //}

    //public void SetMusicVolume(float value)
    //{
    //    MusicVolume = value;
    //    PlayerPrefs.SetFloat("MusicVolume", value);
    //}
}
public enum SfxType
{
    PlayerShoot,
    Sfx_redExplosion,
    Sfx_blueExplosion,
    Sfx_yellowExplosion,
    Sfx_enemyExplosion,
    Sfx_xpCollected,
    COUNT
}
public enum MusicType { MainMenu, Gameplay, Boss, Victory, COUNT }