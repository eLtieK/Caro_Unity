using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    public AudioSource tracks;
    public AudioSource click;
    public AudioSource correct;
    public AudioSource wrong;
    public AudioSource select;
    public AudioSource over;
    public Slider volumeSlider;
    void Start()
    {
        if (gameObject.tag == "Audio")
        {
            tracks.Play();
            GameObject[] musicObject = GameObject.FindGameObjectsWithTag("Audio");
            if (musicObject.Length > 1) { Destroy(this.gameObject); }
            DontDestroyOnLoad(this.gameObject);
        }
        if (SceneManager.GetActiveScene().name == "PlayScreen" || SceneManager.GetActiveScene().name == "BotScreen")
        {
            if (!PlayerPrefs.HasKey("volume")) {
                PlayerPrefs.SetFloat("volume", 1);
                Load();
            } else {
                Load();
            }
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }
    public void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }
    public void Save()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
    public void Click() { click.Play(); }
    public void Correct() { correct.Play(); }
    public void Wrong() { wrong.Play(); }
    public void Select() { select.Play(); }
    public void Over() { over.Play(); }
}
