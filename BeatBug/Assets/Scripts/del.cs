using UnityEngine;
using UnityEngine.EventSystems;

public class NewMonoBehaviourScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool ispoint;
    public Transform r;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ispoint = true;
        r.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ispoint = false;
        r.localScale = Vector3.one;
    }
}