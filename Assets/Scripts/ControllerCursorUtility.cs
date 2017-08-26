using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerCursorUtility : MonoBehaviour, ISelectHandler
{

    public RectTransform ControllerCursorTransform;
    private Vector3 ButtonPosition;

    private void Start()
    {
        ButtonPosition = GetComponentInParent<RectTransform>().position;
    }

    public void OnSelect(BaseEventData eventData)
    {
        ControllerCursorTransform.position = ButtonPosition;
        ControllerCursorTransform.position += new Vector3(-294, 0, 0);
    }
}
