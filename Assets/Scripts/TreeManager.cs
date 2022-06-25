using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public GameObject tree;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void plantTree(int x, int y)
    {
        var t = Instantiate(tree);
        t.transform.SetParent(this.transform);
        t.transform.localPosition = new Vector3(0.6f * x, 0.6f * y, 0);
    }
}
