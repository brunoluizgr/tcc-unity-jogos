using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scriptJogoDaTartaruga : MonoBehaviour {
    
    // Texturas relacionadas com o Mouse
    public Texture2D spriteCursor, spriteCursorOnClick;
    public Button btnAbreRegras, btnFechaRegras;
    public Canvas menuRegras;
    private AudioSource sfxCursorOnClick;

    // Use isso para a inicialização
    void Start () {
        menuRegras.enabled = false;
        btnAbreRegras.onClick.AddListener(abreRegras);
        btnFechaRegras.onClick.AddListener(fechaRegras);
    }

    // Update é chamado uma vez por frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            sfxCursorOnClick = GetComponent<AudioSource>();
            sfxCursorOnClick.Play();
            Cursor.SetCursor(spriteCursorOnClick, Vector2.zero, CursorMode.Auto);

        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(spriteCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    public void abreRegras()
    {
        Debug.Log("Abri o Menu de Regras.");
        menuRegras.enabled = true;
    }

    public void fechaRegras()
    {
        Debug.Log("Fechei o Menu de Regras.");
        menuRegras.enabled = false;
    }
}
