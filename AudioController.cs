using UnityEngine;

public enum AudioClipType
{
    COUNTDOWN_321, FIRE_GIRL, FIRE_CANNON, BY_FIRE, DIE_INSECT, SEE_YOU_BURN,
         ROCKET_SOUND, BATTLE_HORN, FIREWORK, HOORAY, UKRAINE_ANTHEM, RUSSIAN_ANTHEM, EXPLOSION
}

public class AudioController : MonoBehaviour {
    static private AudioController instance;

    public AudioSource audioSource;
    public AudioClip[] explosionClips;

    [Tooltip("In enum order")]
    public AudioClip[] otherClips;


    System.Random rand = new System.Random();


    private void Awake()
    {
        instance = this;
    }


    public void PlayAudio(AudioClipType type, float volumeScale = 1)
    {
        AudioClip current;
        switch (type)
        {
            case AudioClipType.EXPLOSION:
                current = explosionClips[rand.Next(0, explosionClips.Length)];
                break;
            default:
                current = otherClips[(int)type];
                break;
        }

        audioSource.PlayOneShot(current, volumeScale);
    }

    public void PlayAudio(AudioClip clip, float volumeScale = 1)
    {
        audioSource.PlayOneShot(clip, volumeScale);
    }

    static public AudioController INSTANCE
    {
        get { return instance; }
    }
}
