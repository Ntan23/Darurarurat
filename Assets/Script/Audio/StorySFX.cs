using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySFX : MonoBehaviour
{
    private enum SFX{
        SippingTea, OpenBox, KidsPlay, Fall, WatchTV, Knock, Bell, KidsCry, Sweeping, ItchScratch
    }

    [SerializeField] private SFX soundFX;
    private AudioManager am;

    void Start() => am = AudioManager.instance;

    public void PlaySFX()
    {
        if(soundFX == SFX.SippingTea) am.PlaySippingTeaSFX();
        if(soundFX == SFX.OpenBox) am.PlayBoxOpenSFX();
        if(soundFX == SFX.KidsPlay) am.PlayKidsPlaySFX();
        if(soundFX == SFX.Fall) am.PlayFallSFX();
        if(soundFX == SFX.WatchTV) am.PlayWatchTVSFX();
        if(soundFX == SFX.Knock) am.PlayKnockDoorSFX();
        if(soundFX == SFX.Bell) am.PlayBellSFX();
        if(soundFX == SFX.KidsCry) am.PlayKidsCrySFX();
        if(soundFX == SFX.Sweeping) am.PlaySweepingSFx();
        if(soundFX == SFX.ItchScratch) am.PlayItchScratchSFX();
    }

    public void StopSFX()
    {
        if(soundFX == SFX.SippingTea) am.StopSippingTeaSFX();
    }
}
