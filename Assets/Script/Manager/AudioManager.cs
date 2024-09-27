using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance {get; private set;}
    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion
    [SerializeField] private Sound[] sounds;

    ///summary
    ///     Play Audio
    ///summary
    private void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);

        if(s == null) return;

        s.source.Play();
    }
    ///summary
    ///     Stop Audio
    ///summary
    private void Stop(string name)
    { 
        Sound s = System.Array.Find(sounds, sound => sound.name == name);

        if(s == null) return;

        s.source.Stop();
    }


    void Start() 
    {
        ///summary
        ///     Create AudioSource for each Audio
        ///summary
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixer;
        }

        Play("BGM");
    } 
    
    ///summary
    ///     Set Volume, Maybe ganti cr set lsg d audio mixer drpd foreach? ||Ganti Ga Ya||
    ///summary
    public void SetSFXVolume(float volume)
    {
        foreach(Sound s in sounds) 
        {
            if(s.name == "BGM") continue;
            if(s.name != "BGM") s.source.volume = volume;
        }
    }

    public void SetBGMVolume(float volume)
    {
        foreach(Sound s in sounds) 
        {
            if(s.name == "BGM")
            {
                s.source.volume = volume;
                break;
            }         
        }
    }

    public void StopAllSFX()
    {
        foreach(Sound s in sounds) 
        {
            if(s.name != "BGM") s.source.Stop();
        }
    }

///summary
///     Maybe Change to Enum ? ||Ganti ga ya||
///summary
    public void PlayDialogueActionSFX(string name) => Play(name);
    public void PlaySippingTeaSFX() => Play("Sipping");
    public void StopSippingTeaSFX() => Stop("Sipping");
    public void StopKidsPlaySFX() => Stop("KidsPlay");
    public void PlayWritingSFX() => Play("Writing");
    public void StopWritingSFX() => Stop("Writing");
    public void PlayBoxOpenSFX() => Play("BoxOpen");
    public void PlayBoxMoveSFX() => Play("BoxMove");
    public void PlayBoxMoveBackSFX() => Play("BoxMoveBack");
    public void PlayOpenVaselineSFX() => Play("OpenVaseline");
    public void PlayCloseVaselineSFX() => Play("CloseVaseline");
    public void PlayOpenCloseSFX() => Play("OpenClose");
    public void PlayPeelSFX() => Play("Peel");
    public void PlayButtonClickSFX() => Play("ButtonClick");
    public void PlayTearPaperSFX() => Play("TearPaper");
    public void PlayLevelCompleteSFX() => Play("LevelComplete");
    public void PlayWrongProcedureSFX() => Play("Error");
    public void PlayKidsPlaySFX() => Play("KidsPlay");
    public void PlayFallSFX() => Play("Fall");
    public void PlayWatchTVSFX() => Play("WatchTV");
    public void PlayKnockDoorSFX() => Play("KnockDoor");
    public void PlayBellSFX() => Play("Bell");
    public void PlayKidsCrySFX() => Play("KidsCry");
    public void PlaySweepingSFx() => Play("Sweeping");
    public void PlayItchScratchSFX() => Play("ItchScratch");
} 

