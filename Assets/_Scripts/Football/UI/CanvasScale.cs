using UnityEngine;
using UnityEngine.UI;

public class CanvasScale : MonoBehaviour
{
    public static CanvasScale instance;
    public CanvasScaler canvasScaler;
    public float referenceWidth = 1080;
    public float referenceHeight = 1920;

    //[SerializeField] Text screenSize;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        AdjustCanvasScale();
    }

    void AdjustCanvasScale()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float widthRatio = screenWidth / referenceWidth;
        float heightRatio = screenHeight / referenceHeight;

        float match = Mathf.Min(widthRatio, heightRatio);

        canvasScaler.scaleFactor = match;
        canvasScaler.referenceResolution = new Vector2(referenceWidth, referenceHeight);

       // screenSize.text = "width: " + screenWidth + " height: " + screenHeight;
    }
}
