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
    int playerNum;
    int diceNum;
    float[] xPoses = { 17f, 13.3f, 9.6f, 5.9f, 2.2f, -1.5f, -5.2f, -8.9f, -12.6f, -16.3f, -16.3f, -12.6f, -8.9f, -5.2f, -1.5f, 2.2f, 5.9f, 9.6f, 13.3f, 17f, 17f, 13.3f, 9.6f, 5.9f, 2.2f, -1.5f, -5.2f, -8.9f, -12.6f, -16.3f, -16.3f, -12.6f, -8.9f, -5.2f, -1.5f, 2.2f, 5.9f, 9.6f, 13.3f, 17f, 17f, 13.3f, 9.6f, 5.9f, 2.2f, -1.5f, -5.2f, -8.9f, -12.6f, -16.3f };
    float[] yPoses = { -6f, -6f, -6f, -6f, -6f, -6f, -6f, -6f, -6f, -6f, -2f, -2f, -2f, -2f, -2f, -2f, -2f, -2f, -2f, -2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f };
    TcpClient client;
	NetworkStream stream;
	StreamWriter writer;
	StreamReader reader;

	//System.Random rnd = new System.Random();

	bool connected = false;

	public GameObject inputField;

	static GameObject player1;
	static GameObject player2;
	static GameObject player3;
	static GameObject player4;

 


	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
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
          else 
            {
                playerNum = Int32.Parse(lineReceived.Split('-')[0]);
                diceNum = Int32.Parse(lineReceived.Split('-')[1]);
                if (playerNum == 1)
                {
                    sumbreror1 += diceNum;

                    player1.transform.position = new Vector3(xPoses[sumbreror1], yPoses[sumbreror1]);
                }

                if (playerNum == 2)
                {
                    sumbreror2 += diceNum;

                    player2.transform.position = new Vector3(xPoses[sumbreror2], yPoses[sumbreror1]);
                }


                if (playerNum == 3)
                {
                    sumbreror3 += diceNum;

                    player3.transform.position = new Vector3(xPoses[sumbreror3], yPoses[sumbreror3]);
                }


                if (playerNum == 4)
                {
                    sumbreror4 += diceNum;

                    player4.transform.position = new Vector3(xPoses[sumbreror4], yPoses[sumbreror4]);
                }

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
 
			client = new TcpClient ("172.20.10.4", port);
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
