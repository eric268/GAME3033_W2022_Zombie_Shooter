using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public static AudioClip AKGunshot, BettyDead, BettyHit, DryShot, LeonDead, LeonHit, M4Gunshot, PickUp, Pistol, Reload, SMGGunshot;
    static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        AKGunshot = Resources.Load<AudioClip>("AKGunshot");
        BettyDead = Resources.Load<AudioClip>("BettyDead");
        BettyHit = Resources.Load<AudioClip>("BettyHit");
        DryShot = Resources.Load<AudioClip>("DryShot");
        LeonDead = Resources.Load<AudioClip>("LeonDead");
        LeonHit = Resources.Load<AudioClip>("LeonHit");
        M4Gunshot = Resources.Load<AudioClip>("M4Gunshot");
        PickUp = Resources.Load<AudioClip>("PickUp");
        Pistol = Resources.Load<AudioClip>("Pistol");
        Reload = Resources.Load<AudioClip>("Reload");
        SMGGunshot = Resources.Load<AudioClip>("SMGGunshot");

        audioSource = GetComponent<AudioSource>();

    }
    //Static function that can play background sounds
    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "AKGunshot":
                audioSource.PlayOneShot(AKGunshot, 0.05f);
                break;
            case "BettyDead":
                audioSource.PlayOneShot(BettyDead, 0.2f);
                break;
            case "BettyHit":
                audioSource.PlayOneShot(BettyHit, 0.1f);
                break;
            case "DryShot":
                audioSource.PlayOneShot(DryShot, 0.15f);
                break;
            case "LeonDead":
                audioSource.PlayOneShot(LeonDead, 0.05f);
                break;
            case "LeonHit":
                audioSource.PlayOneShot(LeonHit, 0.05f);
                break;
            case "M4Gunshot":
                audioSource.PlayOneShot(M4Gunshot, 0.05f);
                break;
            case "PickUp":
                audioSource.PlayOneShot(PickUp, 0.05f);
                break;
            case "Pistol":
                audioSource.PlayOneShot(Pistol, 0.02f);
                break;
            case "Reload":
                audioSource.PlayOneShot(Reload, 0.05f);
                break;
            case "SMGGunshot":
                audioSource.PlayOneShot(SMGGunshot, 0.05f);
                break;
        }
    }
}
