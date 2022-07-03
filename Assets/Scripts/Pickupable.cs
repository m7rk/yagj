using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private const float ROT_SPEED = 90f;

    public GameState gs;
    public Player.GramType gt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y + (90f * Time.deltaTime), this.transform.eulerAngles.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
        GameState.gs.players[collision.GetComponent<Player>().team].gc.incr(collision.GetComponent<Player>().team == GameState.gs.playerTeam,gt);
        FindObjectOfType<Player>().GetComponent<AudioSource>().Play();

    }
}
