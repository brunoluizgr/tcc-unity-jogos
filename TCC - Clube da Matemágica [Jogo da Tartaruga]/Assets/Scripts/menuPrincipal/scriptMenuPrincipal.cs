/// <sumary>
/// Menu Principal do Jogo
/// </sumary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scriptMenuPrincipal : MonoBehaviour {

    // Texturas relacionadas com o Mouse
    public Texture2D spriteCursor, spriteCursorOnClick;
    // Som do Click do Mouse
    private AudioSource sfxCursorOnClick;
    // Variável Global de Som
    public float volumeGlobal = 1.0F;
    // Canvas dos Internos da Cena
    public Canvas menuPrincipal, menuJogos, menuOpcoes, menuCreditos, menuSaida, menuVoltar;
    // Botão do Logotipo do Projeto (vai pro Catálogo de Jogos)
    public Button btnLogotipoProjeto;
    // Botões do Menu Principal
    public Button btnIniciarJogo, btnOpcoesJogo, btnCreditosJogo, btnSairJogo, btnVoltar;
    // Botões dos Menu dos Jogos
    public Button btnJogo1, btnJogo2, btnJogo3;
    // Botões do Menu de Opções
    public Button btnAumentarVolume, btnDiminuirVolume;
    // Botões do Menu de Saída
    public Button btnSairConfirma, btnSairCancela;

    // Use isso para a inicialização
    void Start () {
        pressionaSairCancela();
    }
	
	// Update é chamado uma vez por frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            sfxCursorOnClick = GetComponent<AudioSource>();
            sfxCursorOnClick.Play();
            Cursor.SetCursor(spriteCursorOnClick, Vector2.zero, CursorMode.Auto);
            
        }
        if (Input.GetMouseButtonUp(0)) {
            Cursor.SetCursor(spriteCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // FixedUpdate
    void FixedUpdate () {}

    // Botão Pressiona Logotipo Projeto
    public void pressionaBtnLogotipoProjeto () {
        Debug.Log("Você clicou no botão: LOGO DO PROJETO");
        Application.Quit();
        Application.OpenURL("http://tcc.brunoluizgr.com/faculdade/catalogo");
    }
    public void aumentaVolumeGlobal () {
        volumeGlobal = Mathf.Min(volumeGlobal + 0.15F, 1.0F);
        AudioListener.volume = volumeGlobal;
    }
    public void diminuiVolumeGlobal () {
        volumeGlobal = Mathf.Min(volumeGlobal - 0.15F, 1.0F);
        AudioListener.volume = volumeGlobal;
    }
    // Entrada do Jogo / Botão Sair -> Cancela
    public void pressionaSairCancela() {
        Debug.Log("Jogo iniciou-se ou você clicou no botão: VOLTAR");
        menuPrincipal.enabled = true;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
        menuVoltar.enabled = false;
        btnIniciarJogo.onClick.AddListener(pressionaBtnIniciaJogos);
        btnOpcoesJogo.onClick.AddListener(pressionaBtnOpcoes);
        btnCreditosJogo.onClick.AddListener(pressionaBtnCreditos);
        btnSairJogo.onClick.AddListener(pressionaBtnSair);
    }
    // Botão Jogos
    public void pressionaBtnIniciaJogos(){
        Debug.Log("Você clicou no botão: JOGOS");
        menuPrincipal.enabled = false;
        menuJogos.enabled = true;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
        menuVoltar.enabled = true;
        btnVoltar.onClick.AddListener(pressionaSairCancela);
        btnJogo1.onClick.AddListener(iniciaJogo1);
        btnJogo2.onClick.AddListener(iniciaJogo2);
        btnJogo3.onClick.AddListener(iniciaJogo3);
    }
    // Botão Opções
    public void pressionaBtnOpcoes () {
        Debug.Log("Você clicou no botão: OPÇÕES");
        menuPrincipal.enabled = false;
        menuJogos.enabled = false;
        menuOpcoes.enabled = true;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
        menuVoltar.enabled = true;
        btnVoltar.onClick.AddListener(pressionaSairCancela);
        btnAumentarVolume.onClick.AddListener(aumentaVolumeGlobal);
        btnDiminuirVolume.onClick.AddListener(diminuiVolumeGlobal);
    }
    // Botão Créditos
    public void pressionaBtnCreditos () {
        Debug.Log("Você clicou no botão: CRÉDITOS");
        menuPrincipal.enabled = false;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = true;
        menuSaida.enabled = false;
        menuVoltar.enabled = true;
        btnVoltar.onClick.AddListener(pressionaSairCancela);
    }
    // Botão Sair
    public void pressionaBtnSair () {
        Debug.Log("Você clicou no botão: SAIR");
        menuPrincipal.enabled = false;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = true;
        menuVoltar.enabled = false;
        btnSairConfirma.onClick.AddListener(pressionaSairConfirma);
        btnSairCancela.onClick.AddListener(pressionaSairCancela);
    }
    // Botão Sair - Confirma
    public void pressionaSairConfirma () {
        Debug.Log("Você clicou no botão: SAIR - CONFIRMA");
        Application.OpenURL("http://tcc.brunoluizgr.com");
        Application.Quit(); 
    }
    // Seleção de Leveis - Botão Jogo 1
    public void iniciaJogo1() {
        Debug.Log("Você iniciou o módulo: JogoDaTartaruga");
        SceneManager.LoadScene("JogoDaTartaruga");
    }
    // Seleção de Leveis - Botão Jogo 2
    public void iniciaJogo2() {
        Debug.Log("Você iniciou o módulo: Jogo2");
    }
    // Seleção de Leveis - Botão Jogo 2
    public void iniciaJogo3 () {
        Debug.Log("Você iniciou o módulo: Jogo3");
        SceneManager.LoadScene("JogoMancala");
    }
}
