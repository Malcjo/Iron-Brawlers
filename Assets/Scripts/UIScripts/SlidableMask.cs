using UnityEngine;
using UnityEngine.UI;

public class SlidableMask : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private RectTransform rectTransform;
    private Vector3 farLeft;
    private Vector3 farRight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        farLeft = rectTransform.position - new Vector3(rectTransform.rect.width, 0f);
        farRight = rectTransform.position;
    }

    private void Update()
    {
        //rectTransform.position = Vector2.Lerp(farLeft, farRight, slider.value);
    }
}