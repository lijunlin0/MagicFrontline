using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager sCurrent;
    private AudioSource mMusicAudioSource;
    private AudioSource mSoundAudioSource;
    public static AudioManager GetCurrent()
    {
        return sCurrent;
    }
    public static void AudioManagerInit(AudioManager audioManager)
    {
        sCurrent=audioManager;
    }
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        AudioSource[] audioSources=GetComponents<AudioSource>();
        mMusicAudioSource=audioSources[0];
        mSoundAudioSource=audioSources[1];
        mMusicAudioSource.loop=true;
        mSoundAudioSource.loop=false;
    }
    public void PlayFightMusic(int level)
    {
        mMusicAudioSource.clip=Resources.Load<AudioClip>("Sound/FightMusic"+level.ToString());
        mMusicAudioSource.Play();
    }
    public void PlaySettingButtonkSound()
    {
        AudioClip clip=Resources.Load<AudioClip>("Sound/SettingClick");
        mSoundAudioSource.PlayOneShot(clip);
    }
    public void PlayUIButtonkSound()
    {
        AudioClip clip=Resources.Load<AudioClip>("Sound/Click");
        mSoundAudioSource.PlayOneShot(clip);
    }
    public void PlayCreateTowerSound()
    {
        AudioClip clip=Resources.Load<AudioClip>("Sound/CreateTower");
        mSoundAudioSource.PlayOneShot(clip);
    }
    public void PlayCreateTowerFailureSound()
    {
        AudioClip clip=Resources.Load<AudioClip>("Sound/CreateTowerFailure");
        mSoundAudioSource.PlayOneShot(clip);
    }
    public void PlayRemoveTowerSound()
    {
        AudioClip clip=Resources.Load<AudioClip>("Sound/RemoveTower");
        mSoundAudioSource.PlayOneShot(clip);
    }
    public void PlayCoinSound()
    {
        AudioClip clip=Resources.Load<AudioClip>("Sound/Coin");
        mSoundAudioSource.PlayOneShot(clip);
    }
    public void StopPlay()
    {
        mSoundAudioSource.Stop();
    }
    public void IsPauseFightMusicPlay(bool pause)
    {
        if(pause)
        {
            mMusicAudioSource.Pause();
        }
        else
        {
            mMusicAudioSource.UnPause();
        }
        
    }    
}