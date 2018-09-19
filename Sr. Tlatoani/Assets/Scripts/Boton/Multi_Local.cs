using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_Local : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown() {
		Debug.Log ("se pusho el boton local");
		GameObject tmp = GameObject.FindGameObjectWithTag ("Prefs");
		PreferenciasPartida prefs = tmp.GetComponent<PreferenciasPartida>();
		prefs.tipoPartida = Constantes.MULTI_LOCAL;
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Seleccion_Personaje");
	}
}
