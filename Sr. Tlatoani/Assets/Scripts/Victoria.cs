using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Victoria : MonoBehaviour {
	public Text uitext_victoria;
	// Use this for initialization
	void Start () {
		PreferenciasPartida prefs = GameObject.FindGameObjectWithTag ("Prefs").GetComponent<PreferenciasPartida> ();
		string texto_victoria;
		if (prefs.victoria == Constantes.VICTORIA_J1) {
			texto_victoria = "JUGADOR 1 ES EL NUEVO TLATOANI";
		} else if (prefs.victoria == Constantes.VICTORIA_J2) {
			if(prefs.tipoPartida == Constantes.SINGLE)
				texto_victoria = "SERA UNA INTELIGENCIA ARTIFICIAL\nQUIEN GOBIERNE";
			else
				texto_victoria = "JUGADOR 1 ES EL NUEVO TLATOANI";
		} else if (prefs.victoria == Constantes.EMPATE) {
			texto_victoria = "\\[T]/ EMPATE \\[T]/";
		} else {
			texto_victoria = "ERROR DE CONEXION DE RED";
		}
		uitext_victoria.text = texto_victoria;
		prefs.personajes [0] = 0;
		prefs.personajes [1] = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
