using UnityEngine;
using UnityEngine.EventSystems;

public class HelpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverImage;
    public GameObject startButton;
    public GameObject artImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverImage != null)
            hoverImage.SetActive(true);
        if (startButton != null)
            startButton.SetActive(false);
        if (artImage != null)
            artImage.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverImage != null)
            hoverImage.SetActive(false);
        if (startButton != null)
            startButton.SetActive(true);
        if (artImage != null)
            artImage.SetActive(true);
    }
}
