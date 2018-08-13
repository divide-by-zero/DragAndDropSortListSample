using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 tapRefPosition;
    private bool isDrag = false;
    private CanvasGroup _canvasGroup;
    public Dropable Parent { set; get; }

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Parent = transform.parent.GetComponent<Dropable>();
        if (Parent != null) Parent.Child = this;    //親子関係結びつけ
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition + tapRefPosition;
        isDrag = true;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        tapRefPosition = (Vector2) transform.position - eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        _canvasGroup.blocksRaycasts = true;
    }

    void Update()
    {
        if (isDrag == false)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 10);
        }
    }
}