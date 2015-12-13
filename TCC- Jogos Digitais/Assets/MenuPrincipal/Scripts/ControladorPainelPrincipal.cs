using UnityEngine;
using System.Collections;

public class ControladorPainelPrincipal : MonoBehaviour {

    public Texture btnSair;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        if (btnSair || Input.GetKeyDown(KeyCode.Escape)) 
            Application.Quit();
    }
}
