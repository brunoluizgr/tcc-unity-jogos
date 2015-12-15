/// <sumary>
/// Menu Principal do Jogo
/// </sumary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScriptMenuPrincipal : MonoBehaviour {

    public Texture2D spriteCursor;

    public Canvas menuPrincipal;
    public Canvas menuJogos;
    public Canvas menuOpcoes;
    public Canvas menuCreditos;
    public Canvas menuSaida;


    // Botões do Menu Principal
    public Button btnComecar;
    public Button btnOpcoes;
    public Button btnCreditos;
    public Button btnSair;

    // Botões dos Menu dos Jogos
    public Button btnAvancandoComOResto;
    public Button btnCorridaDeMenos;
    public Button btnJogoDaTartaruga;

    // Botões do Menu de Opções
    public Button btnAumentarVolume;
    public Button btnDiminuirVolume;

    // Botões do Menu de Saída
    public Button btnSairConfirma;
    public Button btnSairDesiste;


    // Use isso na inicialização
    void Start () {
       

   
        menuPrincipal = menuPrincipal.GetComponent<Canvas>();
        menuJogos = menuJogos.GetComponent<Canvas>();
        menuOpcoes = menuOpcoes.GetComponent<Canvas>();
        menuCreditos = menuCreditos.GetComponent<Canvas>();
        menuSaida = menuSaida.GetComponent<Canvas>();

        menuPrincipal.enabled = true;
        /* Desabilita os menus-secundarios para interações com o jogador */
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;

        btnComecar = btnComecar.GetComponent<Button>();
            btnAvancandoComOResto = btnAvancandoComOResto.GetComponent<Button>();
            btnCorridaDeMenos = btnCorridaDeMenos.GetComponent<Button>();
            btnJogoDaTartaruga = btnJogoDaTartaruga.GetComponent<Button>();

        btnOpcoes = btnOpcoes.GetComponent<Button>();
            btnAumentarVolume = btnAumentarVolume.GetComponent<Button>();
            btnDiminuirVolume = btnDiminuirVolume.GetComponent<Button>();

        btnCreditos = btnCreditos.GetComponent<Button>();
        btnSair = btnSair.GetComponent<Button>();
            btnSairConfirma = btnSairConfirma.GetComponent<Button>();
            btnSairDesiste = btnSairDesiste.GetComponent<Button>();
    }
    void Update() {}

    // Voltar
    public void pressionaBtnVoltar() {
        menuPrincipal.enabled = true;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
    }

    // Começar - abre o menu dos Jogos
    public void pressionaBtnComecar() {
        menuPrincipal.enabled = false;
        menuJogos.enabled = true;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
    }

    // Seleção de Leveis - Botão Avançando com o Resto
    public void pressionaBtnAvancandoComOResto() {
        Application.LoadLevel (1);    
    }

    // Seleção de Leveis - Botão Corrida de Menos
    public void pressionaBtnCorridaDeMenos() {
        Application.LoadLevel (2);
    }

    // Seleção de Leveis - Botão Jogo da Tartaruga
    public void pressionaBtnJogoDaTartaruga() {
        Application.LoadLevel (3);
    }

    // Botão Opções
    public void pressionaBtnOpcoes() {
        menuPrincipal.enabled = false;
        menuJogos.enabled = false;
        menuOpcoes.enabled = true;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
    }

    // Botão Créditos
    public void pressionaBtnCreditos() {
        menuPrincipal.enabled = false;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = true;
        menuSaida.enabled = false;
    }

    // Botão Sair
    public void pressionaBtnSair() {
        menuPrincipal.enabled = false;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = true;
    }

    // Botão Sair - Sim
    public void pressionaSairConfirma() {
        Application.Quit();
        Application.OpenURL("http://tcc.brunoluizgr.com");
    }

    // Botão Sair - Não
    public void pressionaSairDesiste() {
        menuPrincipal.enabled = true;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
    }
}
