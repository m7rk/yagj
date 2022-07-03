using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // really need to clean this up.
    public GameObject book;

    public GameObject btnBase;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var p = GameState.gs.getControlledPlayer();
        this.transform.position = new Vector3(p.transform.position.x, p.transform.position.y, -30);
    }
}
