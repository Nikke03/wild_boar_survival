using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;
    public AudioClip death;
    public AudioClip camminata;
    public AudioClip corsa;
    public AudioClip playerAttack; //quando il poliziotto prende danno
    public AudioClip dannoSubito;
    public AudioClip healing;
    public AudioClip special; //quando raccogli l'oggeto speciale
    public AudioClip jump;
    public AudioClip attaccoPoliziotto;
    public AudioClip attaccoGabbiano;
    public AudioClip attaccoApe;
    public AudioClip mortePoliziotto;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
