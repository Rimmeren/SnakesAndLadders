using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class TcpClient1 : MonoBehaviour {


	int port = 13456;
	string login;
	string lineReceived;
    int sumbreror1, sumbreror2, sumbreror3, sumbreror4;
    int myPlayerNumber;
    int diceNum;

    TcpClient client;
	NetworkStream stream;
	StreamWriter writer;
	StreamReader reader;

	//System.Random rnd = new System.Random();

	bool connected = false;

	public GameObject inputField;

	public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;

 


	// Use this for initialization
	void Start () {
		player1 = GameObject.Find ("Player1");
		player2 = GameObject.Find ("Player2");
		player3 = GameObject.Find ("Player3");
		player4 = GameObject.Find ("Player4");
        sumbreror1 = 0;
        sumbreror2 = 0;
        sumbreror3 = 0;
        sumbreror4 = 0;
	}

	// Update is called once per frame
	void Update () {

		if (connected) {
			communication ();
			lineReceived = reader.ReadLine();
            
		}
        if (SceneManager.GetActiveScene().name == "Game")
        {
          if( lineReceived == "Your turn")
            {
                rollDice();
            }
          else if(lineReceived =="")
            {

            }
        }

        if (SceneManager.GetActiveScene().name == "Lobby") {
            

			if (lineReceived == "Welcome player 1") {
				player1.GetComponent<SpriteRenderer> ().enabled = true;
			}

			if (lineReceived == "Welcome player 2") {
				player2.GetComponent<SpriteRenderer> ().enabled = true;
			}

			if (lineReceived == "Welcome player 3") {
				player3.GetComponent<SpriteRenderer> ().enabled = true;
			}

			if (lineReceived == "Welcome player 4") {
				player4.GetComponent<SpriteRenderer> ().enabled = true;
			}
				

		}

		if (SceneManager.GetActiveScene ().name == "Game") {

		}

	}
		
    public void rollDice()
    {
        System.Random rand = new System.Random();
        diceNum = rand.Next(1, 7);
    }

    public void connectToServer() {

		if (SceneManager.GetActiveScene ().name == "Join") {
			login = inputField.GetComponent<InputField> ().text;

			client = new TcpClient (login, port);
			stream = client.GetStream ();
			writer = new StreamWriter (stream, Encoding.ASCII) { AutoFlush = true };
			reader = new StreamReader (stream, Encoding.ASCII);
            writer.WriteLine("Player joined");

            //This is where we get our player number
            myPlayerNumber = Int32.Parse(reader.ReadLine());

		}

		if (SceneManager.GetActiveScene ().name == "Create") {

			client = new TcpClient ("localhost", port);
			stream = client.GetStream ();
			writer = new StreamWriter (stream, Encoding.ASCII) { AutoFlush = true };
			reader = new StreamReader (stream, Encoding.ASCII);
		}

		connected = true;


	}

	void communication () {


		print("Received from server: " + lineReceived);


	}
}
