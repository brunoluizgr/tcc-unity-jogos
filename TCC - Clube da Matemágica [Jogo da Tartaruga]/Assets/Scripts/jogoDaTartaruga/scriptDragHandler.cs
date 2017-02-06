using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scriptDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemSendoPego;
    public Texture2D spriteCursorOnGrab;
    Vector3 posicaoInicialObjeto;
    Transform posicaoInicialPai;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Cursor.SetCursor(spriteCursorOnGrab, Vector2.zero, CursorMode.Auto);
        itemSendoPego = gameObject;
        posicaoInicialObjeto = transform.position;
        posicaoInicialPai = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemSendoPego = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent != posicaoInicialPai)
        {
            transform.position = posicaoInicialObjeto;
        }
    }
}
