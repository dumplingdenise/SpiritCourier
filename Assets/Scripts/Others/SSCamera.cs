using UnityEngine;
using System.IO;

public class SSCamera : MonoBehaviour
{
    public Camera captureCamera;
    public string fileName = "TilemapScreenshot.png";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Press 'P' to capture
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        int width = Screen.width;
        int height = Screen.height;
        RenderTexture rt = new RenderTexture(width, height, 24);
        captureCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + fileName, bytes);
        Debug.Log("Screenshot Saved at: " + Application.dataPath + "/" + fileName);
    }
}

