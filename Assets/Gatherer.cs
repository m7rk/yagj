using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : MonoBehaviour
{
    GameState gs;

    // ask gamestate where nearest resource is.
    public void Update()
    {

    }

    public void OnTriggerStay2D(Collider2D other)
    {
        transform.position -= (other.transform.position - this.transform.position) * Time.deltaTime;
    }
}
