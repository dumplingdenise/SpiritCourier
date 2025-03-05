using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public Slider loadingSlider; // UI Slider (loading bar)
    public RectTransform playerImage; // Player RectTransform
    public RectTransform parcelImage; // Parcel RectTransform
    public float loadingTime = 3f; // Time to complete loading
    public float parcelOffset = 50f; // Distance between player & parcel
    public float parcelYOffset = -30f; // Y offset for parcel (adjustable)
    public string nextSceneName = "NextScene"; // Target scene name

    private Vector2 startPos;
    private Vector2 endPos;

    void OnEnable() // Use OnEnable to ensure it runs every time the scene is loaded
    {
        Time.timeScale = 1f; // Ensure time is not paused
        StartCoroutine(AnimateLoadingBar());
    }

    IEnumerator AnimateLoadingBar()
    {
        float elapsedTime = 0f;
        RectTransform barRect = loadingSlider.GetComponent<RectTransform>();

        // **Calculate Start & End Positions Based on Bar**
        float barWidth = barRect.rect.width * barRect.lossyScale.x;
        startPos = new Vector2(barRect.position.x - barWidth / 2, playerImage.position.y);
        endPos = new Vector2(barRect.position.x + barWidth / 2 - playerImage.rect.width, playerImage.position.y);

        // **Initialize Player & Parcel Positions**
        playerImage.position = startPos;
        parcelImage.position = new Vector2(startPos.x - parcelOffset, startPos.y + parcelYOffset);
        loadingSlider.value = 0f; // Ensure loading starts from 0

        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / loadingTime;

            // **Move Player & Parcel Based on Slider Value**
            loadingSlider.value = progress;
            float newX = Mathf.Lerp(startPos.x, endPos.x, progress);
            playerImage.position = new Vector2(newX, startPos.y);
            parcelImage.position = new Vector2(newX - parcelOffset, startPos.y + parcelYOffset);

            yield return null;
        }

        // **Ensure Everything Stops Exactly at End Position**
        loadingSlider.value = 1f;
        playerImage.position = endPos;
        parcelImage.position = new Vector2(endPos.x - parcelOffset, endPos.y + parcelYOffset);

        // **Load the next scene**
        SceneManager.LoadScene("Opening Scene"); // Uses variable instead of hardcoded name
    }
}
