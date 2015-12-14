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

    public Button btnComecar;
    public Button btnOpcoes;
    public Button btnCreditos;
    public Button btnSair;
    public Button btnSairConfirma;
    public Button btnSairDesiste;

	// Use this for initialization
	void Start () {
       
        menuPrincipal = menuPrincipal.GetComponent<Canvas>();
        menuJogos = menuJogos.GetComponent<Canvas>();
        menuOpcoes = menuOpcoes.GetComponent<Canvas>();
        menuCreditos = menuCreditos.GetComponent<Canvas>();
        menuSaida = menuSaida.GetComponent<Canvas>();

        /* Desabilita os 4 primeiros menus para interações com o jogador */
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = false;

        btnComecar = btnComecar.GetComponent<Button>();
        btnOpcoes = btnOpcoes.GetComponent<Button>();
        btnCreditos = btnCreditos.GetComponent<Button>();
        btnSair = btnSair.GetComponent<Button>();
        btnSairConfirma = btnSairConfirma.GetComponent<Button>();
        btnSairDesiste = btnSairDesiste.GetComponent<Button>();
    }

    public void pressionaSair() {
        menuPrincipal.enabled = false;
        menuJogos.enabled = false;
        menuOpcoes.enabled = false;
        menuCreditos.enabled = false;
        menuSaida.enabled = true;
    }

    public void pressionaSairConfirma() {
    }

    public void pressionaSairDesiste() {
        menuPrincipal.enabled = true;
        menuJogos.enabled = true;
        menuOpcoes.enabled = true;
        menuCreditos.enabled = true;
        menuSaida.enabled = false;
    }
}
