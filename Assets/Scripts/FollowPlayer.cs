using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour {

    public GameObject playerObject;
    private Player player;
    private PlayerManager playerManager;
    private Canvas canvas;

	// Use this for initialization
	void Start () {
        player = playerObject.GetComponentInChildren<Player>();
        playerManager = playerObject.GetComponentInChildren<PlayerManager>();
        transform.position = player.transform.position;
        canvas = gameObject.GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
        if(player == null)
        {
            player = playerObject.GetComponentInChildren<Player>();
        }
        transform.position = player.transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.55f, transform.position.z);
        if (player.GetComponent<Fairy>() != null)
        {
            canvas.enabled = false;
        } else
        {
            canvas.enabled = true;
        }
	}
}
