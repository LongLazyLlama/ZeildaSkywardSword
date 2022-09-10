using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer Mixer;
    private Text _percentage { get{ return GetComponent<Text>(); } }

    public void UpdateSlider(float sliderValue)
    {
        Mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void UpdateText(float value)
    {
        _percentage.text = Mathf.RoundToInt(value * 100) + "%";
    }
}
