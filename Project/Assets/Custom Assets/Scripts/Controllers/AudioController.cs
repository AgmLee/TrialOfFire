using UnityEngine;     
using UnityEngine.Audio;

public class AudioController : MonoBehaviour {
    public AudioMixer mixer;

    public void SetSFXVol(float value)
    {
        mixer.SetFloat("soundVol", value);
    }
    public void SetMusicVol(float value)
    {
        mixer.SetFloat("musicVol", value);
    }
    public void SetMasterVol(float value)
    {
        mixer.SetFloat("masterVol", value);
    }
    public void ClearVolume(string name)
    {
        mixer.ClearFloat(name);
    }
}
