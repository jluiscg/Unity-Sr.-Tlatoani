using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PreferenciasPartida : MonoBehaviour {
	public int tipoPartida;
	public int[] personajes = new int[2];
	public int victoria;
	public Server server;
	public Cliente client;
	public static PreferenciasPartida instancia = null;
	public int esperado=0;
	void Awake () {
		//revisa si ya existe una instancia
		if (instancia == null)
		{
			//si no existe referencia a esta instancia al ser creada
			instancia = this;
		}
		//si ya existe y no es esta la destruye
		else if (instancia != this)
		{
			Destroy(gameObject);   
		}


		//configura que esta instancia no se destruya al cargar otra escena
		DontDestroyOnLoad(gameObject);
	}
	// Use this for initialization
	void Start () {
		victoria = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
