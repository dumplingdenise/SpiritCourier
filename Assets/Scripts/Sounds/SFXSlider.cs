using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour
{
    private Slider sfxSlider;  // Reference to the UI slider for adjusting SFX volume

    private void Start()
    {
        sfxSlider = GetComponent<Slider>();  // Get the Slider component attached to this GameObject

        // Ensure SoundEffectManager is initialized and exists
        if (SoundEffectManager.instance != null)
        {
            sfxSlider.value = SoundEffectManager.instance.GetVolume();  // Set the slider to the current volume
            sfxSlider.onValueChanged.AddListener(SoundEffectManager.instance.SetVolume);  // Update volume when slider changes
        }
    }
}
