using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundManager : MonoBehaviour
{
    public static AudioClip mainSceneBackgroundSound;
    static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        mainSceneBackgroundSound = Resources.Load<AudioClip>("MainSceneBackgroundMusic");
        audioSource = GetComponent<AudioSource>();
        PlaySound("MainSceneBackgroundMusic");
    }

    // Update is called once per frame
    void Update()
    {

    }
    //A static function that can play sound clips
    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "MainSceneBackgroundMusic":
                audioSource.PlayOneShot(mainSceneBackgroundSound, 0.07f);
                break;
        }
    }
}
