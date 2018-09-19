using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regresar_Menu_Red : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}void OnMouseDown() {
		Debug.Log ("regresando al menu de red");
		GameObject tmp = GameObject.FindGameObjectWithTag ("Prefs");
		PreferenciasPartida prefs = tmp.GetComponent<PreferenciasPartida>();
		if(prefs.tipoPartida == Constantes.MULTI_RED_CLI && prefs.client !=null){
			prefs.client.closeClient();
			Destroy(prefs.client);
		}
		else if(prefs.tipoPartida == Constantes.MULTI_RED_HOST && prefs.server !=null){
			prefs.server.closeServer();
			Destroy(prefs.server);
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Menu_Red");
	}
}
