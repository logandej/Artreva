using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource ambianceSource;


    [Header("Audio Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float ambianceVolume = 0.5f;


    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Garde l'AudioManager entre les scènes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialise les volumes
        InitializeVolumes();
    }

    private void InitializeVolumes()
    {
        if (musicSource) musicSource.volume = musicVolume;
        if (ambianceSource) ambianceSource.volume = ambianceVolume;
    }

    #region Music

    public void PlayMusicLooped(AudioClip clip)
    {
        PlayMusic(clip, true);
    }
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource && clip)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource) musicSource.Stop();
    }


    #endregion

    #region Ambiance
    public void PlayAmbiance(AudioClip clip, bool loop = true)
    {
        if (ambianceSource && clip)
        {
            ambianceSource.clip = clip;
            ambianceSource.loop = loop;
            ambianceSource.Play();
        }
    }

    public void StopAmbiance()
    {
        if (ambianceSource) ambianceSource.Stop();
    }
    #endregion




    #region Sound Effects

    public void PlaySoundEffect(AudioSource audio, AudioClip clip)
    {
        if (audio && clip)
        {
            audio.PlayOneShot(clip);
        }
    }

    #endregion
}