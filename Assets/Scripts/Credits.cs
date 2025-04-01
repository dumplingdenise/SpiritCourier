using UnityEngine;

public class Credits : MonoBehaviour
{

    public float scrollSpeed = 100f;

    private RectTransform rectTrasnform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTrasnform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTrasnform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }
}
