using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject pivot;

    public GameObject projectile;

    public float coolDown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pivot.transform.right = FindObjectOfType<PlayerController>().transform.position - pivot.transform.position;

        coolDown -= Time.deltaTime;
        if(coolDown < 0)
        {

        }
    }
}
