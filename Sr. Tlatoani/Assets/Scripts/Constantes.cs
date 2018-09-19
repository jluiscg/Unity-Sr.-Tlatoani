using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constantes {
	//MODOS DE JUEGO
	public const int SINGLE = 1;
	public const int MULTI_LOCAL = 2;
	public const int MULTI_RED_HOST = 3;
	public const int MULTI_RED_CLI = 4;
	//PERSONAJES
	public const int AMLO = 1;
	public const int MEADE = 2;
	public const int ANAYA = 3;
	public const int AI = 4;
	//CLASES DE INSULTO
	public const int NEUTRO = 0;
	public const int FAMILIA = 1;
	public const int CARRERA_PROF = 2;
	public const int CONDUCTA = 3;
	public const int ASPECTO = 4;
	public const int EDAD = 5;
	public const int PREPARACION = 6;
	public const int IMAGEN_PUBLICA = 7;
	//TIPO DE COMPONENTE
	public const int SUJETO = 0;
	public const int INSULTO_CON = 1;
	public const int INSULTO_SIM = 2;
	public const int CONJUNCION = 3;
	public const int SER_ESTAR = 4;
	//PARA JUEGO
	public const int MAX_REPUTACION = 100;
	//cantidades de dano correspondientes a cada tipo
	public const int DANO_S = 3;
	public const int DANO_IC = 2;
	public const int DANO_IS = 6;
	public const int DANO_C = 1;
	public const int DANO_SE = 1;
	//tipo victoria
	public const int VICTORIA_J1 = 1;
	public const int VICTORIA_J2 = 2;
	public const int EMPATE = 3;
	public const int DESCONEXION = 4;
	//IA
	public const int GENERACIONES = 30;
	//porcentajes en partida
	public const float BONUS_DEBILIDAD = 50;
	public const float PORCENTAJE_DANO_PROPIO = 20;

	//datos de red
	public const int PUERTO = 33120;
	public const int MAX_TAMANO_PAQUETE = 254;
	///tipo de dato esperado
	public const int NINGUNO = 0;
	public const int SELECCION_JUGADOR = 1;
	public const int ID_COMPONENTE = 2;
	public const int ID_BOTON_COMPONENTE_ELEGIDO = 3;
	public const int CLIENTE_LISTO = 4;

}