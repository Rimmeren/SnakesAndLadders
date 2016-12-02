using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;

public class TcpThreadServer : MonoBehaviour
{


	int port = 13456;

	IPAddress myIp = "172.20.10.3";

	//Server and Client connection
	TcpListener listener;
	TcpClient client;

	//Players
	public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;

	bool server = false;

	// Use this for initialization
	void Start ()
	{
		player1 = GameObject.Find ("Player1");
		player2 = GameObject.Find ("Player2");
		player3 = GameObject.Find ("Player3");
		player4 = GameObject.Find ("Player4");


	
	}

	// Update is called once per frame
	void Update ()
	{

		if (server == true) {
			
			if (listener.Pending () == true) {
				client = listener.AcceptTcpClient ();

				HandleClients client1 = new HandleClients (client);
				client1.startClient ();

			}

		}

	}


	public void startServer ()
	{

		listener = new TcpListener (myIp, port);

		listener.Start ();

		server = true;

		print ("Server has been created and your friends can join with the IP: " + myIp);

	}

	public void stopServer ()
	{

		listener.Stop ();

		print ("Server has been stopped");

	}


	class HandleClients
	{

		TcpClient randomClient;
	
		int playerCount;

		private NetworkStream stream;

		StreamWriter writer;
		StreamReader reader;

		Thread clientThread;

		public HandleClients (TcpClient client)
		{
			this.randomClient = client;

			stream = client.GetStream ();

			writer = new StreamWriter (stream, Encoding.ASCII) { AutoFlush = true };
			reader = new StreamReader (stream, Encoding.ASCII);

		}

		public void startClient ()
		{
			clientThread = new Thread (new ThreadStart (playGame));
			playerCount++;

			clientThread.Name = "player" + playerCount;
			print (clientThread.Name);

			clientThread.Start ();

			print ("User with IP: " + ((IPEndPoint)randomClient.Client.RemoteEndPoint).Address.ToString () + " has joined the game");
		}

		void playGame ()
		{

			if (clientThread.Name == "player1") {
				//player1.GetComponent<SpriteRenderer> ().enabled = true;
				print ("Player 1 has connected");
				writer.WriteLine ("Welcome Messi");
			}
			if (clientThread.Name == "player2") {
				//player2.GetComponent<SpriteRenderer> ().enabled = true;
				print ("Player 2 has connected");
				writer.WriteLine ("Welcome Messi");
			}


		}

	}
}
