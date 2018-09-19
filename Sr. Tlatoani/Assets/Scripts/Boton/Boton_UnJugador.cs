using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton_UnJugador : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown() {
		GameObject tmp = GameObject.FindGameObjectWithTag ("Prefs");
		PreferenciasPartida prefs = tmp.GetComponent<PreferenciasPartida>();
		prefs.tipoPartida = Constantes.SINGLE;
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Seleccion_Personaje");
	}
}
