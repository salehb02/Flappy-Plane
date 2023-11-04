using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singlton
    public static AudioManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip scoreUpSFX;
    [SerializeField] private AudioClip winSFX;

    [SerializeField] private AudioSource sfxSource;

    public void JumpSFX()
    {
        PlayCustomSFX(jumpSFX);
    }

    public void ScoreUpSFX()
    {
        PlayCustomSFX(scoreUpSFX);
    }

    public void WinSFX()
    {
        PlayCustomSFX(winSFX);
    }

    public void PlayCustomSFX(AudioClip clip)
    {
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }
}
