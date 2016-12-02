using UnityEngine;
using System.Collections;

public class movePlayer : MonoBehaviour {

    GameObject player;

    float playerX;
    float playerY;

    public int dice;

    public float xDist;
    public float yDist;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");

        playerX = player.transform.position.x;
        playerY = player.transform.position.y;

    }

    // Update is called once per frame
    void Update () {
        player.transform.position = new Vector3(playerX, playerY, 0);

        playerX -= xDist * dice;
    }
}
