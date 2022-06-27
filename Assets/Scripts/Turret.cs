using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject pivot;

    public GameObject projectileR;
    public GameObject projectileB;

    public float coolDown = 0f;

    private float turretRange = 5f;
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
                Vector3 targ = go.transform.position;

                if(go.transform.tag == "Building")
                {
                    // want middle of building
                    targ = new Vector3((0.6f * (int)(targ.x / 0.6f)) + 0.3f, (0.6f*(int)(targ.y / 0.6f)) + 0.3f, targ.z);   
                }

                pivot.transform.right = targ - pivot.transform.position;
                var projectile = name[0] == 'R' ? projectileR : projectileB;
                var v = Instantiate(projectile);
                v.GetComponent<Projectile>().vel = 4 * pivot.transform.right;
                v.transform.position = pivot.transform.position;
                pivot.transform.Rotate(new Vector3(0, 0, 45));
                v.transform.SetParent(null);
            }
            coolDown = 2f;
        }
    }

    public GameObject attackCritter()
    {
        GameObject closest = null;
        float bestDist = 100000000;
        foreach (var v in FindObjectsOfType<Critter>())
        {
            var dist = Vector2.Distance(this.transform.position, v.transform.position);
            if (v.name[0] != name[0] && dist < bestDist)
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

    public GameObject attackBuilding()
    {
        GameObject closest = null;
        float bestDist = 100000000;
        foreach (var v in GameObject.FindGameObjectsWithTag("Building"))
        {
            var dist = Vector2.Distance(this.transform.position, v.transform.position);
            if (v.name[0] != name[0] && dist < bestDist)
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

    public GameObject attackPlayer()
    {
        if(!FindObjectOfType<PlayerController>())
        {
            return null;
        }
        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        GameObject antag = FindObjectOfType<Antag>().gameObject;

        if(Vector2.Distance(player.transform.position,this.transform.position) < turretRange && player.GetComponent<PlayerController>().team != name[0])
        {
            return player;
        }

        if (Vector2.Distance(antag.transform.position, this.transform.position) < turretRange && antag.GetComponent<Antag>().team != name[0])
        {
            return antag;
        }

        return null;
    }


    public GameObject acquireTarget()
    {
        if(attackCritter() != null)
        {
            return attackCritter();
        }

        if (attackPlayer() != null)
        {
            return attackPlayer();
        }

        if (attackBuilding() != null)
        {
            return attackBuilding();
        }


        return null;
    }
}
