using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;
    public Slider sliderAudio;

    private void Start()
    {
        SystemVariable.audioController = this;
        audioSource = GetComponent<AudioSource>();
        sliderAudio.value = audioSource.volume;
    }

    private void Update()
    {
        audioSource.volume = sliderAudio.value;
    }

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void unmuteBG() => audioSource.clip = null;
}
