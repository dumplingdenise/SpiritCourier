using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    private Slider bgmSlider; // reference to the UI slider for adjusting BGM volume

    private void Start()
    {
        bgmSlider = GetComponent<Slider>(); // get the slider component attached to this gameobject

        if (BGMManager.instance != null) //ensure BGMManager exists
        {
            bgmSlider.value = BGMManager.instance.GetVolume(); // Set slider to current volume
            bgmSlider.onValueChanged.AddListener(BGMManager.instance.SetVolume); //updae bgm volume when slider changes
        }
    }
}
