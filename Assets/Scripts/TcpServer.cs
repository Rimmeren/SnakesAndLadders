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

public class TcpServer : MonoBehaviour
{

	int numOfClients = 0;
	public string[] playerIds = new string[5];

	public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;

	TcpListener listener;
	TcpClient client;

	bool server;

	// Use this for initialization
	void Start ()
	{
		server = false;
	

	}

	// Update is called once per frame
	void Update ()
	{

		if (server == true ) {

			if (listener.Pending () == true) {
				client = listener.AcceptTcpClient ();
				ThreadPool.QueueUserWorkItem (threadProc, client);
				numOfClients++;
				print ("Number of clients: " + numOfClients);

				player1.GetComponent<SpriteRenderer> ().enabled = true;
			}

		}
			

	}

	public void startServer ()
	{
		listener = new TcpListener (IPAddress.Any, 13456);


		listener.Start ();
		print ("Server has been created, and is looking for clients...");
		server = true;
	}

	public void stopServer(){
		listener.Stop ();

		print ("Server has been stopped");
		server = false;
	}

	void threadProc (object obj)
	{

		//TcpEchoServer server = new TcpEchoServer();

		//TCP Client object
		var client = (TcpClient)obj;


	}

	public void setPlayerId (string ip)
	{
		for (int i = 0; i < playerIds.Length; i++) {
			if (playerIds [i] == "" && Array.IndexOf (playerIds, ip) == -1) {
				playerIds [i] = (i + 1).ToString ();
			}
		}
	}

	string getPlayerId (string ip)
	{
		return Array.IndexOf (playerIds, ip).ToString ();
	}

	void sendAndReceive(){

		string clientIP = client.Client.RemoteEndPoint.ToString ();

		NetworkStream stream = client.GetStream ();
		StreamWriter writer = new StreamWriter (stream, Encoding.ASCII) { AutoFlush = true };
		StreamReader reader = new StreamReader (stream, Encoding.ASCII);

		//server.setPlayerId(clientIP);

		writer.WriteLine ("Welcome. You are player " + numOfClients);
		writer.WriteLine ("Welcome player " + numOfClients);

		string inputLine = reader.ReadLine ();
		print ("From player: " + inputLine);
		writer.WriteLine ("Thanks!");

	}

}
