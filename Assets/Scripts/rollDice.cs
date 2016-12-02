using UnityEngine;
using System.Collections;

public class rollDice : MonoBehaviour {

    int diceNum;
    int playerNum;

    float[] xPoses = { 17f, 13.3f, 9.6f, 5.9f, 2.2f, -1.5f, -5.2f, -8.9f, -12.6f, -16.3f, -16.3f, -12.6f, -8.9f, -5.2f, -1.5f, 2.2f, 5.9f, 9.6f, 13.3f, 17f, 17f, 13.3f, 9.6f, 5.9f, 2.2f, -1.5f, -5.2f, -8.9f, -12.6f, -16.3f, -16.3f, -12.6f, -8.9f, -5.2f, -1.5f, 2.2f, 5.9f, 9.6f, 13.3f, 17f, 17f, 13.3f, 9.6f, 5.9f, 2.2f, -1.5f, -5.2f, -8.9f, -12.6f, -16.3f };
    float[] yPoses = { -6f, -6f, -6f, -6f, -6f, -6f, -6f, -6f, -6f, -6f, -2f, -2f, -2f, -2f, -2f, -2f, -2f, -2f, -2f, -2f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 5.3f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f, 8.7f };
    int p1, p2, p3, p4;

    int p1Pos, p2Pos, p3Pos, p4Pos;

    int newPos;

    GameObject player1;
    GameObject player2;
    GameObject player3;
    GameObject player4;

    public void RollDice()
    {
       
        diceNum = Random.Range(1, 7);
        playerNum++;

        if(playerNum >= 5)
            playerNum = 1;

        if (playerNum == 1)
        {          
            p1Pos += diceNum;

            player1.transform.position = new Vector3(xPoses[p1Pos], yPoses[p1Pos]);         
        }

        if (playerNum == 2)
        {
            p2Pos += diceNum;

            player2.transform.position = new Vector3(xPoses[p2Pos], yPoses[p2Pos]);
        }


        if (playerNum == 3)
        {
            p3Pos += diceNum;

            player3.transform.position = new Vector3(xPoses[p3Pos], yPoses[p3Pos]);
        }


        if (playerNum == 4)
        {
            p4Pos += diceNum;

            player4.transform.position = new Vector3(xPoses[p4Pos], yPoses[p4Pos]);
        }


        checkPosition(player1);
        checkPosition(player2);
        checkPosition(player3);
        checkPosition(player4);

    }

    // Use this for initialization
    void Start () {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        player3 = GameObject.Find("Player3");
        player4 = GameObject.Find("Player4");
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    /*A function to check the ladders and move the player. I made it to work just for the first ladder as this seems to be
        too much of code to repeat over and over again, so I think maybe you will find a shorter way to do that.
    */
    void checkPosition(GameObject player)
    {
        if (player.transform.position == new Vector3(xPoses[3], yPoses[3]))

        {
            player.transform.position = new Vector3(xPoses[16], yPoses[16]);

            if (player.name == "Player1")
            {
                p1Pos = 16;
            }

            if (player.name == "Player2")
            {
                p2Pos = 16;
            }

            if (player.name == "Player3")
            {
                p3Pos = 16;
            }

            if (player.name == "Player4")
            {
                p4Pos = 16;
            }
        }


    }


}
