using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 90 * Time.deltaTime));
        this.transform.position += vel * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name[0] != name[0])
        {
            if (collision.GetComponent<Critter>())
            {
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.tag == "Building")
            {
                FindObjectOfType<StructureManager>().attack((int)(collision.gameObject.transform.position.x / 0.6f), (int)(collision.gameObject.transform.position.y / 0.6f));
            }
            Destroy(this.gameObject);
        }
    }
}
