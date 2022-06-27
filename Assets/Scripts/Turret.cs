using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject pivot;

    public GameObject projectile;

    public float coolDown = 0f;

    public float turretRange = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coolDown -= Time.deltaTime;
        if(coolDown < 0)
        {
            var go = acquireTarget();
            if (go != null)
            {
                pivot.transform.right = go.transform.position - pivot.transform.position;
                var v = Instantiate(projectile);
                v.GetComponent<Projectile>().vel = pivot.transform.right;
                v.transform.position = pivot.transform.position;
                v.transform.SetParent(null);
            }
            coolDown = 2f;
        }
    }

    public GameObject acquireTarget()
    {
        GameObject closest = null;
        float bestDist = 100000000;

        foreach(var v in FindObjectsOfType<Critter>())
        {
            var dist = Vector2.Distance(this.transform.position, v.transform.position);
            if(v.name[0] != name[0] && dist < bestDist)
            {
                closest = v.gameObject;
                bestDist = dist;
            }
        }
        if (bestDist < turretRange)
        {
            return closest;
        }
        return null;
    }
}
