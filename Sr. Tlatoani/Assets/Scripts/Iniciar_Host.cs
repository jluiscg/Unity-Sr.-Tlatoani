using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iniciar_Host : MonoBehaviour {
	public UnityEngine.UI.Text texto;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void OnClick(){
		GameObject tmp = GameObject.FindGameObjectWithTag ("Prefs");
		PreferenciasPartida prefs = tmp.GetComponent<PreferenciasPartida>();
		Destroy (prefs.server);
		Destroy (prefs.client);
		prefs.server = prefs.gameObject.AddComponent<Server> ();
		if (prefs.server.IniServer ()>=0) {
			texto.text = "server iniciado,esperando conexion\nIP: " + Network.player.ipAddress;
		}
		else 
			texto.text = "no se pudo iniciar el server";

		//pasar a la siguiente escena
	}
}
