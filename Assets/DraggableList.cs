using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DraggableList : MonoBehaviour
{
    [SerializeField]
    private bool isAutoPack;    //間を詰めるかどうか

    private List<Dropable> _dropables;

    // Use this for initialization
    void Start()
    {
        _dropables = GetComponentsInChildren<Dropable>().OrderBy(dropable => dropable.transform.GetSiblingIndex()).ToList();

        //このリストで詰めたり、間をあけさせたりすればいい（はず）

        foreach (var dropable in _dropables)
        {
            dropable.OnInsert = dragObj =>
            {
                //既に子がいる場所にDropされた
                if (dropable.Child != null)
                {
                    if (isAutoPack)
                    {
                        //全部下にずらす
                        Draggable insertObj = null;
                        foreach (var t in _dropables)
                        {
                            if (dropable == t)
                            {
                                insertObj = dragObj;
                            }

                            if (insertObj != null)
                            {
                                var temp = t.Child;
                                t.Child = insertObj;
                                insertObj = temp;
                            }
                        }
                    }
                    else
                    {
                        //ずらすとはみ出る可能性があるので、キャンセルする
                        dragObj.Parent.OnInsert(dragObj);
                    }
                }
                else
                {
                    dropable.Child = dragObj;
                }
                AutoSort();
            };

            dropable.OnRemove = dragObj =>
            {
                dragObj.Parent.Child = null;
                AutoSort();
            };
        }
    }

    private void AutoSort()
    {
        if (isAutoPack == false) return;
        while (true)
        {
            var isChange = false;
            for (var i = 0; i < _dropables.Count - 1; i++)
            {
                if (_dropables[i].Child == null && _dropables[i + 1].Child != null)
                {
                    _dropables[i].Child = _dropables[i + 1].Child;
                    _dropables[i + 1].Child = null;
                    isChange = true;
                }
            }

            if (isChange == false) break;   //もう詰めるnullがある可能性がないので抜ける
        }
    }
}