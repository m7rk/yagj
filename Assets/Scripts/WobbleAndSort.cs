using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this also sorts so kind of a misnomer.
public class WobbleAndSort : MonoBehaviour
{
    public float offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        int i = 0;
        foreach (Transform child in transform)
        {
            // save originals later and just rot
            child.transform.position += (0.00003f * new Vector3(Mathf.Sin(i+Time.unscaledTime), Mathf.Cos(i+Time.unscaledTime),0));
            i++;
        }
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1 + ((this.transform.position.y + offset) * 0.001f));

    }
}
