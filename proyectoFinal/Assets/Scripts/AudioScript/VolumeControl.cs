using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField]
    AudioMixer mixer;
    [SerializeField]
    Slider volumenSlider;

    private void Awake()
    {
        volumenSlider.onValueChanged.AddListener(ControlMusicaVolumen);
    }

    void Start()
    {
        Cargar();
    }

    void ControlMusicaVolumen(float valor)
    {
        mixer.SetFloat("GeneralVolumen", Mathf.Log10(valor) * 20);
        PlayerPrefs.SetFloat("GeneralVolumen", volumenSlider.value);
    }

    void Cargar()
    {
        volumenSlider.value = PlayerPrefs.GetFloat("GeneralVolumen", 0.25f);
        ControlMusicaVolumen(volumenSlider.value);
    }
}
