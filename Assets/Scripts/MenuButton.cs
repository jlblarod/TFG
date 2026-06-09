using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerDown(PointerEventData e) {
        Button button = GetComponent<Button>();
        if (button != null && !button.interactable) return;
        text.rectTransform.offsetMin = new Vector2(text.rectTransform.offsetMin.x, 2);
    }

    public void OnPointerUp(PointerEventData e)
    {
        text.rectTransform.offsetMin = new Vector2(text.rectTransform.offsetMin.x, 7);
    }
}