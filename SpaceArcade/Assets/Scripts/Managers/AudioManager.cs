using UnityEngine;

namespace SpaceArcade.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] AudioSource sfxSource;
        [SerializeField] AudioSource musicSource;

        [Header("Volume Settings")]
        [Range(0f, 1f)] public float masterVolume = 1f;
        [Range(0f, 1f)] public float sfxVolume = 1f;
        [Range(0f, 1f)] public float musicVolume = 0.5f;
        
        [Header("Music Clips")]
        [SerializeField] AudioClip[] musicClips;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            //PlayMusic(musicClips[Random.Range(0, musicClips.Length)]);
        }

        public void PlaySfx(AudioClip clip, float volume = 1f)
        {
            if (clip == null || sfxSource == null) return;

            sfxSource.PlayOneShot(clip, volume * sfxVolume * masterVolume);
        }

        public void PlaySfxAtPoint(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (clip == null) return;

            AudioSource.PlayClipAtPoint(clip, position, volume * sfxVolume * masterVolume);
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null || musicSource == null) return;

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = musicVolume * masterVolume;
            musicSource.Play();
        }

        public void StopMusic()
        {
            if (musicSource != null)
                musicSource.Stop();
        }
    }
}