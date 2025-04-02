using UnityEngine;

public class Credits : MonoBehaviour
{

    public float scrollSpeed = 120f;

    private RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }

    public void resetCredit()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
