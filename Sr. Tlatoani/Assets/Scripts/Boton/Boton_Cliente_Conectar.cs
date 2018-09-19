using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Boton_Cliente_Conectar : MonoBehaviour {
	public Text ip;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnClick(){
		GameObject tmp = GameObject.FindGameObjectWithTag ("Prefs");
		PreferenciasPartida prefs = tmp.GetComponent<PreferenciasPartida>();
		prefs.client = prefs.gameObject.AddComponent<Cliente> ();
		prefs.client.IniCliente (ip.text);
	}
}
