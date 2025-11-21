using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioClipType 
    { e

    }

    [System.Serializable]
    public struct AudioManagerClip
    {
        public AudioClipType Type;
        public AudioClip Clip;
    }

    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private List<AudioManagerClip> _clipList;
    [SerializeField] private float _musicVolume = 0.5f;

    private Dictionary<AudioClipType, AudioClip> _clipDictionary = new();
    private Coroutine _coroutine;
    private AudioClipType? _currentMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var clip in _clipList)
        {
            if (!_clipDictionary.ContainsKey(clip.Type))
                _clipDictionary.Add(clip.Type, clip.Clip);
        }
    }

    
    public static void PlaySoundEffect(AudioClipType type)
    {
        if (!Instance._clipDictionary.ContainsKey(type))
        {
            Debug.LogError($"Audio \"{type}\" not found in clip list.");
            return;
        }

        AudioClip clip = Instance._clipDictionary[type];
        Instance._sfxSource.PlayOneShot(clip);
    }

    public static void PlayMusic(AudioClipType type, float fadeIn, float fadeOut, bool looped)
    {
        if (!Instance._clipDictionary.ContainsKey(type))
        {
            Debug.LogError($"Audio \"{type}\" not found in clip list.");
            return;
        }

        if (Instance._currentMusic == type)
        {
            return;
        }

        Instance._currentMusic = type;
        AudioClip newClip = Instance._clipDictionary[type];

        if (Instance._coroutine != null)
            Instance.StopCoroutine(Instance._coroutine);

        Instance._coroutine = Instance.StartCoroutine(Instance.MusicFade(newClip, fadeIn, fadeOut, looped));
    }

    private IEnumerator MusicFade(AudioClip newClip, float fadeIn, float fadeOut, bool looped)
    {
        if (_musicSource.isPlaying)
        {
            float startVol = _musicSource.volume;
            for (float t = 0; t < fadeOut; t += Time.deltaTime)
            {
                _musicSource.volume = Mathf.Lerp(startVol, 0, t / fadeOut);
                yield return null;
            }
            _musicSource.Stop();
        }

        _musicSource.clip = newClip;
        _musicSource.loop = looped;
        _musicSource.volume = 0;
        _musicSource.Play();

        for (float t = 0; t < fadeIn; t += Time.deltaTime)
        {
            _musicSource.volume = Mathf.Lerp(0, _musicVolume, t / fadeIn);
            yield return null;
        }

        _musicSource.volume = _musicVolume;
    }
}
