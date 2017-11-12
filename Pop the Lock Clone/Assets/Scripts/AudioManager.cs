using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    public AudioClip pop;
    private AudioSource source;

    public void Pop() {
        source.PlayOneShot(pop);
    }

    private void Awake() {
        instance = this;
        source = GetComponent<AudioSource>();
    }

}
