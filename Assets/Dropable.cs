using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dropable : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    public Action<Draggable> OnInsert;
    public Action<Draggable> OnRemove;

    [SerializeField]
    private Draggable _child;
    public Draggable Child
    {
        set
        {
            _child = value;
            if (_child != null)
            {
                _child.transform.SetParent(transform);
                _child.Parent = this;
            }
        }
        get { return _child; }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragObj = eventData.pointerDrag.GetComponent<Draggable>();
        if (dragObj == null) return;
        dragObj?.Parent?.OnRemove(dragObj); //抜ける
        OnInsert?.Invoke(dragObj);  //挿入する
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerDrag == null)return;
        var dragObj = eventData.pointerDrag.GetComponent<Draggable>();
        if (dragObj == null) return;
        dragObj?.Parent?.OnRemove(dragObj); //抜ける
        OnInsert?.Invoke(dragObj);  //挿入する
    }
}