﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton_Cliente : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown() {
		GameObject tmp = GameObject.FindGameObjectWithTag ("Prefs");
		PreferenciasPartida prefs = tmp.GetComponent<PreferenciasPartida>();
		prefs.tipoPartida = Constantes.MULTI_RED_CLI;
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Conexion_Cliente");
	}
}
