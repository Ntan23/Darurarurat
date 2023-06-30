using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySFX : MonoBehaviour
{
    private enum SFX{
        SippingTea
    }

    [SerializeField] private SFX soundFX;
    private AudioManager am;

    void Start() => am = AudioManager.instance;

    public void PlaySFX()
    {
        if(soundFX == SFX.SippingTea) am.PlaySippingTeaSFX();
    }
}
