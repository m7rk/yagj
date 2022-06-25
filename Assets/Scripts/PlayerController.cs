using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += new Vector3(0,Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += new Vector3(-Time.deltaTime, 0);
            this.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += new Vector3(0, -Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(Time.deltaTime,0);
            this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}
