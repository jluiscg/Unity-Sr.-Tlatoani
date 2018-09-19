using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using System;
public class Cliente : MonoBehaviour{
	public int socketId;
	public int myReliableChannelId;
	public int connectionId;
	public byte error;
	PreferenciasPartida prefs;
	// Use this for initialization
	void Start(){
		
	}
	public void IniCliente (string direccionIP) {
		if (direccionIP == "localhost")
			direccionIP = "127.0.0.1";
		//inicializacion de socketId
		NetworkTransport.Init();
		ConnectionConfig config = new ConnectionConfig();
		myReliableChannelId  = config.AddChannel(QosType.Reliable);

		HostTopology topology = new HostTopology(config, 1);
		socketId = NetworkTransport.AddHost(topology, 0);//puerto arbitrario
		//conexion
		connectionId = NetworkTransport.Connect(socketId, direccionIP, Constantes.PUERTO, 0, out error);
		//ConnectionSimulatorConfig sim = new ConnectionSimulatorConfig(30,45,31,44,0.02F);
		//connectionId = NetworkTransport.ConnectWithSimulator(socketId, direccionIP, Constantes.PUERTO, 0, out error,sim);
		GameObject tmp = GameObject.FindGameObjectWithTag ("Prefs");
		prefs = tmp.GetComponent<PreferenciasPartida>();
		//Debug.Log("Connected, error:" + error.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		int recHostId; 
		int recConnectionId; 
		int channelId; 
		byte[] recBuffer = new byte[Constantes.MAX_TAMANO_PAQUETE]; 
		int bufferSize = Constantes.MAX_TAMANO_PAQUETE;
		int dataSize;
		NetworkEventType recData = NetworkTransport.Receive(out recHostId, out recConnectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
		switch (recData)
		{
		case NetworkEventType.Nothing:
			break;
		case NetworkEventType.ConnectEvent:

			if (socketId == recHostId)
			{
				Debug.Log("Cliente conectado a server");
				prefs.esperado = Constantes.SELECCION_JUGADOR;
				UnityEngine.SceneManagement.SceneManager.LoadScene ("Seleccion_Personaje");
			} else {
				//se recibe solicitud de conexion pero no se hace nada pues esto jamas deberia pasar
				Debug.Log("Cliente recibio solicitud de conexion");
			}
			break;
		case NetworkEventType.DataEvent:
			StartCoroutine (ProcesarDatos (recBuffer));
			break;
		case NetworkEventType.DisconnectEvent:
				Debug.Log ("Server desconectado");
			if (prefs.victoria == 0) {
				prefs.victoria = Constantes.DESCONEXION;
				UnityEngine.SceneManagement.SceneManager.LoadScene ("PantallaVictoria");
			}
			break;
		}
	}
	IEnumerator ProcesarDatos(byte[] recBuffer){
		Stream stream = new MemoryStream(recBuffer);
		BinaryFormatter formatter = new BinaryFormatter();
		string message = formatter.Deserialize(stream) as string;
		Debug.Log("mensaje recibido: " + message);
		if (message.Length > 14) {
			StartCoroutine (GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Partida> ().RecibirComponenteV2 (message));
			yield return null;
		}
		switch (prefs.esperado) {
		case Constantes.NINGUNO:
			Debug.Log ("ignorado");
			break;
		case Constantes.SELECCION_JUGADOR:
			SeleccionPersonaje sel = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SeleccionPersonaje>();
			sel.RegistrarSeleccionPersonaje(int.Parse(message));
			break;
		case Constantes.ID_COMPONENTE:
			StartCoroutine (GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Partida> ().RecibirComponenteV2 (message));
			break;
		case Constantes.ID_BOTON_COMPONENTE_ELEGIDO:
			if (int.Parse (message) > 0) {
				GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Partida> ().RecibirSeleccionRed (int.Parse (message));
			} else {
				//es senal de termino de insulto
				GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Partida> ().BotonTerminarRed();
			}
			break;
		}
		yield return null;
	}
	public void Enviar(string msg){
		byte[] buffer = new byte[Constantes.MAX_TAMANO_PAQUETE];
		Stream stream = new MemoryStream(buffer);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, msg);

		int bufferSize = Constantes.MAX_TAMANO_PAQUETE;

		NetworkTransport.Send(socketId, connectionId, myReliableChannelId, buffer, bufferSize, out error);
	}
	public void closeClient(){
		NetworkTransport.RemoveHost (socketId);
		NetworkTransport.Shutdown ();
	}
}
