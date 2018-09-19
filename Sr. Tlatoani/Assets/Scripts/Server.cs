using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using System;

public class Server : MonoBehaviour{
	public int socketId;
	public int myReliableChannelId;
	public int connectionId;
	public byte error;
	PreferenciasPartida prefs;
	// Use this for initialization
	void Start(){
		
	}
	public int IniServer () {
		// Initializing the Transport Layer with no arguments (default settings)
		NetworkTransport.Init();
		ConnectionConfig config = new ConnectionConfig();
		myReliableChannelId  = config.AddChannel(QosType.Reliable);
		HostTopology topology = new HostTopology(config, 1);
		socketId = NetworkTransport.AddHost(topology, Constantes.PUERTO);
		Debug.Log("Socket abierto. SocketId es: " + socketId);
		GameObject tmp = GameObject.FindGameObjectWithTag ("Prefs");
		prefs = tmp.GetComponent<PreferenciasPartida>();
		return socketId;
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
			connectionId = recConnectionId;
			Debug.Log ("Servidor recibe solicitud de conexion");
			prefs.esperado = Constantes.NINGUNO;
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Seleccion_Personaje");
			break;
		case NetworkEventType.DataEvent:
			ProcesarDatos (recBuffer);
			break;
		case NetworkEventType.DisconnectEvent:
			Debug.Log ("Cliente desconectado");
			if (prefs.victoria == 0) {
				prefs.victoria = Constantes.DESCONEXION;
				UnityEngine.SceneManagement.SceneManager.LoadScene ("PantallaVictoria");
			}
			break;
		}

	}
	void ProcesarDatos(byte[] recBuffer){
		Stream stream = new MemoryStream(recBuffer);
		BinaryFormatter formatter = new BinaryFormatter();
		string message = formatter.Deserialize(stream) as string;
		Debug.Log("mensaje recibido: " + message);
		switch (prefs.esperado) {
		case Constantes.NINGUNO:
			Debug.Log ("ignorado");
			break;
		case Constantes.SELECCION_JUGADOR:
			SeleccionPersonaje sel = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SeleccionPersonaje>();
			sel.RegistrarSeleccionPersonaje(int.Parse(message));
			break;
		case Constantes.ID_COMPONENTE:
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

	}
	public void Enviar(string msg){
		byte[] buffer = new byte[Constantes.MAX_TAMANO_PAQUETE];
		Stream stream = new MemoryStream(buffer);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, msg);

		int bufferSize = Constantes.MAX_TAMANO_PAQUETE;

		NetworkTransport.Send(socketId, connectionId, myReliableChannelId, buffer, bufferSize, out error);
	}
	public void closeServer(){
		NetworkTransport.RemoveHost (socketId);
		NetworkTransport.Shutdown ();
	}
}
