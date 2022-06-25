using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : MonoBehaviour
{
    GameState gs;

    public float PLAYER_PUSH_FORCE = 0.5f;
    public float WATER_PUSH_FORCE = 0.9f;

    public Vector3 last;


    // ask gamestate where nearest resource is.
    public void Update()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<SpriteRenderer>())
            {
                child.GetComponent<SpriteRenderer>().color = Color.blue;
            }

            foreach (Transform child2 in child)
            {
                // color if name ends with cap C
                if (child2.name[child2.name.Length - 1] == 'C')
                {
                    child2.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            last = this.transform.position;
            transform.position -= (other.transform.position - this.transform.position).normalized * Time.deltaTime * PLAYER_PUSH_FORCE;
        }

        if (other.tag == "Water")
        {
            transform.position = last;
        }

    }
}
