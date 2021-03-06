﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Threading;

public class TcpClient1 : MonoBehaviour
{
	int port = 13456;
	string login;
	int sumbreror1, sumbreror2, sumbreror3, sumbreror4;
	int myPlayerNumber;
	int playerNum;
	int diceNum;
	int playerPos;
	float[] xPoses = {
		17f,
		13.3f,
		9.6f,
		5.9f,
		2.2f,
		-1.5f,
		-5.2f,
		-8.9f,
		-12.6f,
		-16.3f,
		-16.3f,
		-12.6f,
		-8.9f,
		-5.2f,
		-1.5f,
		2.2f,
		5.9f,
		9.6f,
		13.3f,
		17f,
		17f,
		13.3f,
		9.6f,
		5.9f,
		2.2f,
		-1.5f,
		-5.2f,
		-8.9f,
		-12.6f,
		-16.3f,
		-16.3f,
		-12.6f,
		-8.9f,
		-5.2f,
		-1.5f,
		2.2f,
		5.9f,
		9.6f,
		13.3f,
		17f,
		17f,
		13.3f,
		9.6f,
		5.9f,
		2.2f,
		-1.5f,
		-5.2f,
		-8.9f,
		-12.6f,
		-16.3f
	};
	float[] yPoses = {
		-6f,
		-6f,
		-6f,
		-6f,
		-6f,
		-6f,
		-6f,
		-6f,
		-6f,
		-6f,
		-2f,
		-2f,
		-2f,
		-2f,
		-2f,
		-2f,
		-2f,
		-2f,
		-2f,
		-2f,
		1.5f,
		1.5f,
		1.5f,
		1.5f,
		1.5f,
		1.5f,
		1.5f,
		1.5f,
		1.5f,
		1.5f,
		5.3f,
		5.3f,
		5.3f,
		5.3f,
		5.3f,
		5.3f,
		5.3f,
		5.3f,
		5.3f,
		5.3f,
		8.7f,
		8.7f,
		8.7f,
		8.7f,
		8.7f,
		8.7f,
		8.7f,
		8.7f,
		8.7f,
		8.7f
	};
	TcpClient client;
	NetworkStream stream;
	static StreamWriter writer;
	static StreamReader reader;
	Thread readThread;

	//System.Random rnd = new System.Random();

	public GameObject inputField;

	static GameObject player1;
	static GameObject player2;
	static GameObject player3;
	static GameObject player4;

 
	string lineReceived;
	string oldLineReceived;
	bool iHaveJoined;
	bool hasMoved;

	int numOfPlayers;

	// Use this for initialization
	void Start ()
	{

		lineReceived = "";
		oldLineReceived = "OLD";
		iHaveJoined = false;
		DontDestroyOnLoad (this.gameObject);

		sumbreror1 = 0;
		sumbreror2 = 0;
		sumbreror3 = 0;
		sumbreror4 = 0;

		numOfPlayers = 0;

	}

	// Update is called once per frame
	void Update ()
	{
		if (lineReceived != oldLineReceived) {
			print (lineReceived);
			oldLineReceived = lineReceived;
		}


			

		player1 = GameObject.Find ("Player1");
		player2 = GameObject.Find ("Player2");
		player3 = GameObject.Find ("Player3");
		player4 = GameObject.Find ("Player4");
			
		//When server tells us to play, we change the scene to Game
		if (lineReceived.IndexOf ("Your turn") != -1 && SceneManager.GetActiveScene ().name != "Game") {
			SceneManager.LoadScene ("Game");
		}

		//When the scene is Game
		if (SceneManager.GetActiveScene ().name == "Game") {
			if (numOfPlayers == 1) {
				player1.GetComponent<SpriteRenderer> ().enabled = true;
			} else if (numOfPlayers == 2) {
				player1.GetComponent<SpriteRenderer> ().enabled = true;
				player2.GetComponent<SpriteRenderer> ().enabled = true;
			} else if (numOfPlayers == 3) {
				player1.GetComponent<SpriteRenderer> ().enabled = true;
				player2.GetComponent<SpriteRenderer> ().enabled = true;
				player3.GetComponent<SpriteRenderer> ().enabled = true;
			} else if (numOfPlayers == 4) {
				player1.GetComponent<SpriteRenderer> ().enabled = true;
				player2.GetComponent<SpriteRenderer> ().enabled = true;
				player3.GetComponent<SpriteRenderer> ().enabled = true;
				player4.GetComponent<SpriteRenderer> ().enabled = true;
			}

			if (Input.GetKeyUp ("space")) {
				//If the server tells me that it's my turn, then we run the (local) function to roll dice.
				if (Int32.Parse (lineReceived.Split (':') [1]) == myPlayerNumber) {
					print ("Vi mærker det");
					rollDice ();
					writer.WriteLine (myPlayerNumber + "-" + diceNum);

				}
			}
			//Else the server message is about the other players, and we go to this code.
			if (lineReceived.IndexOf ("-") != -1 && hasMoved == false) { 
				playerNum = Int32.Parse (lineReceived.Split ('-') [0]);
				playerPos = Int32.Parse (lineReceived.Split ('-') [1]);

				if (playerNum == 1) {
					sumbreror1 += playerPos;
					if (sumbreror1 == 3) { sumbreror1 = 16; }
					if (sumbreror1 == 11) { sumbreror1 = 32; }
					if (sumbreror1 == 23) { sumbreror1 = 35; }

					if (sumbreror1 == 21) { sumbreror1 = 17; }
					if (sumbreror1 == 43) { sumbreror1 = 37; }
					if (sumbreror1 == 46) { sumbreror1 = 14; }
					if(sumbreror1 == 49){ print ("You won"); }
					if (sumbreror1 > 49) { sumbreror1 -= playerPos;}
					player1.transform.position = new Vector3 (xPoses [sumbreror1], yPoses [sumbreror1]);
				}

				if (playerNum == 2) {
					sumbreror2 += playerPos;
					if (sumbreror2 == 3) { sumbreror2 = 16; }
					if (sumbreror2 == 11) { sumbreror2 = 32; }
					if (sumbreror2 == 23) { sumbreror2 = 35; }
								 					
					if (sumbreror2 == 21) { sumbreror2 = 17; }
					if (sumbreror2 == 43) { sumbreror2 = 37; }
					if (sumbreror2 == 46) { sumbreror2 = 14; }
					if(sumbreror2 == 49){ print ("You won"); }
					if (sumbreror2 > 49) { sumbreror2 -= playerPos;}
					player2.transform.position = new Vector3 (xPoses [sumbreror2], yPoses [sumbreror2]);
				}
					
				if (playerNum == 3) {
					sumbreror3 += playerPos;
					if (sumbreror3 == 3) { sumbreror3 = 16; }
					if (sumbreror3 == 11) { sumbreror3 = 32; }
					if (sumbreror3 == 23) { sumbreror3 = 35; }
								 					
					if (sumbreror3 == 21) { sumbreror3 = 17; }
					if (sumbreror3 == 43) { sumbreror3 = 37; }
					if (sumbreror3 == 46) { sumbreror3 = 14; }
					if(sumbreror3 == 49){ print ("You won"); }
					if (sumbreror3 > 49) { sumbreror3 -= playerPos;}
					player3.transform.position = new Vector3 (xPoses [sumbreror3], yPoses [sumbreror3]);
				}

				if (playerNum == 4) {
					sumbreror4 += playerPos;
					if (sumbreror4 == 3) { sumbreror4 = 16; }
					if (sumbreror4 == 11) { sumbreror4 = 32; }
					if (sumbreror4 == 23) { sumbreror4 = 35; }
								 					
					if (sumbreror4 == 21) { sumbreror4 = 17; }
					if (sumbreror4 == 43) { sumbreror4 = 37; }
					if (sumbreror4 == 46) { sumbreror4 = 14; }
					if(sumbreror4 == 49){ print ("You won"); }
					if (sumbreror4 > 49) { sumbreror4 -= playerPos;}
					player4.transform.position = new Vector3 (xPoses [sumbreror4], yPoses [sumbreror4]);
				}
				hasMoved = true;
			}

		}

		if (SceneManager.GetActiveScene ().name == "Create") {
			player1 = GameObject.Find ("Player1");

			if (lineReceived == "Welcome player 1") {
				player1.GetComponent<SpriteRenderer> ().enabled = true;
				if (iHaveJoined == false) {
					myPlayerNumber = 1;
					print ("I am player: " + myPlayerNumber);
					iHaveJoined = true;
				}
			}

		}

		if (SceneManager.GetActiveScene ().name == "Lobby") {
			player2 = GameObject.Find ("Player2");
			player3 = GameObject.Find ("Player3");
			player4 = GameObject.Find ("Player4");


			if (lineReceived == "Welcome player 2") {
				player2.GetComponent<SpriteRenderer> ().enabled = true;
				if (iHaveJoined == false) {
					myPlayerNumber = 2;
					print ("I am player: " + myPlayerNumber);
					iHaveJoined = true;
				}
			}

			if (lineReceived == "Welcome player 3") {
				player3.GetComponent<SpriteRenderer> ().enabled = true;
				if (iHaveJoined == false) {
					myPlayerNumber = 3;
					print ("I am player: " + myPlayerNumber);
					iHaveJoined = true;
				}
			}

			if (lineReceived == "Welcome player 4") {
				player4.GetComponent<SpriteRenderer> ().enabled = true;
				if (iHaveJoined == false) {
					myPlayerNumber = 4;
					print ("I am player: " + myPlayerNumber);
					iHaveJoined = true;
				}
			}


			if (lineReceived.IndexOf ("status:") != -1) {
				numOfPlayers = Int32.Parse (lineReceived.Split (':') [1]);
				if (numOfPlayers == 1) {
					player1.GetComponent<SpriteRenderer> ().enabled = true;
				} else if (numOfPlayers == 2) {
					player1.GetComponent<SpriteRenderer> ().enabled = true;
					player2.GetComponent<SpriteRenderer> ().enabled = true;
				} else if (numOfPlayers == 3) {
					player1.GetComponent<SpriteRenderer> ().enabled = true;
					player2.GetComponent<SpriteRenderer> ().enabled = true;
					player3.GetComponent<SpriteRenderer> ().enabled = true;
				} else if (numOfPlayers == 4) {
					player1.GetComponent<SpriteRenderer> ().enabled = true;
					player2.GetComponent<SpriteRenderer> ().enabled = true;
					player3.GetComponent<SpriteRenderer> ().enabled = true;
					player4.GetComponent<SpriteRenderer> ().enabled = true;
				}
			}
			
				
		}


	}

	public void rollDice ()
	{
		System.Random rand = new System.Random ();
		diceNum = rand.Next (1, 7);
		print ("Dice num: " + diceNum);
	}

	public void connectToServer ()
	{

		if (SceneManager.GetActiveScene ().name == "Join") {
			login = inputField.GetComponent<InputField> ().text;

			client = new TcpClient (login, port);
			stream = client.GetStream ();
			writer = new StreamWriter (stream, Encoding.ASCII) { AutoFlush = true };
			reader = new StreamReader (stream, Encoding.ASCII);

			readThread = new Thread (new ThreadStart (ReadData));
			readThread.Start ();

			SceneManager.LoadScene ("Lobby");
		}

		if (SceneManager.GetActiveScene ().name == "Create") {

			client = new TcpClient ("127.0.0.1", port);

			stream = client.GetStream ();
			writer = new StreamWriter (stream, Encoding.ASCII) { AutoFlush = true };
			reader = new StreamReader (stream, Encoding.ASCII);

			readThread = new Thread (new ThreadStart (ReadData));
			readThread.Start ();
		}
	}

	void ReadData ()
	{
		while (true) {
			lineReceived = reader.ReadLine ();
			hasMoved = false;
		}
	}

}
