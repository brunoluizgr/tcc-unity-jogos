/// <sumary>
/// Menu Principal do Jogo
/// </sumary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScriptMenuPrincipal : MonoBehaviour {

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

        /* Desabilita os menus para interações com o jogador */
        menuPrincipal.enabled = true;
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

    public void pressionaBtnVoltar() {
        menuPrincipal.enabled = true;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
    }
    public void pressionaBtnComecar() {
        menuPrincipal.enabled = false;
        menuJogos.enabled = true;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
    }
    public void pressionaBtnAvancandoComOResto() {
        Application.LoadLevel (1);    
    }
    public void pressionaBtnCorridaDeMenos() {
        Application.LoadLevel (2);
    }
    public void pressionaBtnJogoDaTartaruga() {
        Application.LoadLevel (3);
    }
    public void pressionaBtnOpcoes() {
        menuPrincipal.enabled = false;
        menuJogos.enabled = false;
        menuOpcoes.enabled = true;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
    }
    public void pressionaBtnCreditos() {
        menuPrincipal.enabled = false;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = true;
        menuSaida.enabled = false;
    }
    public void pressionaBtnSair() {
        menuPrincipal.enabled = false;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = true;
    }
    public void pressionaSairConfirma() {
        Application.Quit();
    }
    public void pressionaSairDesiste() {
        menuPrincipal.enabled = true;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;
    }
}
