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


	IPAddress myIp = IPAddress.Parse ("172.20.10.3");

	//Server and Client connection
	TcpListener listener;
	TcpClient client;

	//Players
	static GameObject player1;
	static GameObject player2;
	static GameObject player3;
	static GameObject player4;

	static bool player1Image = false;
	static bool player2Image = false;
	static bool player3Image = false;
	static bool player4Image = false;

	bool server = false;
	static bool game = false;
	static bool create = false;
	static bool gameScene = false;


	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (this.gameObject);
		print (myIp);
	}
		
	// Update is called once per frame
	void Update ()
	{
		player1 = GameObject.Find ("Player1");
		player2 = GameObject.Find ("Player2");
		player3 = GameObject.Find ("Player3");
		player4 = GameObject.Find ("Player4");

		if (server == true) {
			if (player1Image)
				player1.GetComponent<SpriteRenderer> ().enabled = true;

			if (player2Image)
				player2.GetComponent<SpriteRenderer> ().enabled = true;

			if (player3Image)
				player3.GetComponent<SpriteRenderer> ().enabled = true;

			if (player4Image)
				player4.GetComponent<SpriteRenderer> ().enabled = true;


			if (listener.Pending () == true) {
				client = listener.AcceptTcpClient ();
				HandleClients client1 = new HandleClients (client);
				client1.startClient ();
			}

		}
		if (SceneManager.GetActiveScene ().name == "Game") {
			game = true;
			create = false;
		}
		if (SceneManager.GetActiveScene ().name == "Create") {
			create = true;
			game = false;
		}
	}

	public void startServer ()
	{

		listener = new TcpListener (myIp, port);

		listener.Start ();

		print ("Server has been created and your friends can join with the IP: " + myIp);

		server = true;

		print ("Server has been created");

	}

	public void stopServer ()
	{

		listener.Stop ();

		print ("Server has been stopped");

	}

	public void gameSceneEnable ()
	{
		gameScene = true;
	}




	class HandleClients
	{
		int sleepTime = 1000;
		TcpClient randomClient;

		static int playerCount;
		static int oldPlayerCount;

		string readLine;

		private NetworkStream stream;

		static ArrayList players = new ArrayList ();


		StreamWriter writer;
		StreamReader reader;
        
		Thread clientThread;

		int turn = 2;

		bool letsPlay = false;

		string recievedFromClient;


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
			oldPlayerCount = playerCount;
			playerCount++;

			clientThread.Name = "player" + playerCount;
		
			clientThread.Start ();

			print ("User with IP: " + ((IPEndPoint)randomClient.Client.RemoteEndPoint).Address.ToString () + " has joined the game as " + clientThread.Name);
		}


		void playGame ()
		{
			bool clientConnected = false;
			bool listening = false;

			string oldMsg = "OLD";
			string msg = "Test";

			while (true) {
				Thread.Sleep (sleepTime);
					
				if (create == true) {
					
					msg = "status: " + playerCount;
					//Thread.Sleep (sleepTime);

					if (clientThread.Name == "player1" && clientConnected == false) {
						player1Image = true;
						writer.WriteLine ("Welcome player " + playerCount);
						Thread.Sleep (sleepTime);
					}
					if (clientThread.Name == "player2" && clientConnected == false) {
						player2Image = true;
						writer.WriteLine ("Welcome player " + playerCount);
						Thread.Sleep (sleepTime);

					}
					if (clientThread.Name == "player3" && clientConnected == false) {
						player3Image = true;
						writer.WriteLine ("Welcome player " + playerCount);
						Thread.Sleep (sleepTime);

					}
					if (clientThread.Name == "player4" && clientConnected == false) {
						player4Image = true;
						writer.WriteLine ("Welcome player " + playerCount);
						Thread.Sleep (sleepTime);

					}

					clientConnected = true;

				}

				if (game == true) {
					//Thread.Sleep (sleepTime);
			
					if (turn == 1 && listening == false) {
						msg = ("Your turn:" + turn);
						listening = true;
					}

					else if (turn == 1) {
						msg = recievedFromClient;
						listening = false;
						turn = 2;
					}

					if (turn == 2 && listening == false) {
						msg = ("Your turn:" + turn);
						listening = true;
					}

					else if (turn == 2) {
						msg = recievedFromClient;
						listening = false;
						turn = 1;
					}
			
				}


				if (msg != oldMsg) {
					writer.WriteLine (msg);
					Thread.Sleep (sleepTime);
					oldMsg = msg;
				}

				if (listening == true) {
					recievedFromClient = reader.ReadLine ();
					print (recievedFromClient);
					Thread.Sleep (sleepTime);
				}
			}
		}
	}
}


