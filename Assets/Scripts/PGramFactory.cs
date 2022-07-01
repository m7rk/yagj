using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGramFactory : Entity
{
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            FindObjectOfType<GameState>().addPickupable("p", this.transform.position + new Vector3(Random.Range(0.2f, 0.4f), Random.Range(-0.7f, -0.4f), 0f));
            timer = 20f;
        }
    }
}
