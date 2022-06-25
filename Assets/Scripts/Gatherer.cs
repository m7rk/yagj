using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : MonoBehaviour
{
    GameState gs;

    // ask gamestate where nearest resource is.
    public void Update()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().color = Color.blue;

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
        transform.position -= (other.transform.position - this.transform.position).normalized * Time.deltaTime;
    }
}
