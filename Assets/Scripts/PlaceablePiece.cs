using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceablePiece : MonoBehaviour
{

    // the corrisponding black tangrams
    private GameObject matchedShape;

    private bool finish;


    void Start()
    {
    }

    void Update()
    {   
        if (!finish)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;

            // allows to drag the sprite around the world using its local position
            this.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(mousePos.x , mousePos.y, this.gameObject.transform.localPosition.z);

            if (Input.GetKeyDown(KeyCode.A))
            {
                this.transform.Rotate(new Vector3(0.0f, 0.0f, 45f));
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                this.transform.Rotate(new Vector3(0.0f, 0.0f, -45f));
            }

        }
       
    }


    public bool check()
    {
        // correct shape
        if (matchedShape != null && matchedShape.tag == this.tag)
        {
            var rot = Mathf.Abs(Quaternion.Angle(matchedShape.GetComponent<RectTransform>().rotation, GetComponent<RectTransform>().rotation));

            // correct position
            bool rotGood = Mathf.Abs(rot) < 1f;

            // check for square symmetry
            if(matchedShape.tag == "sq")
            {
                // diff of 90 okay!
                rotGood = (rot < 1f) || (Mathf.Abs(rot - 90f) < 1f) || (Mathf.Abs(rot - 180f) < 1f);
            }

            // check for pgram symm.
            if (matchedShape.tag == "p")
            {
                // diff of 180 okay!
                rotGood = (rot < 1f) || (Mathf.Abs(rot - 180f) < 1f);
            }


            if (rotGood)
            {
                this.GetComponent<RectTransform>().position = matchedShape.GetComponent<RectTransform>().position;
                this.GetComponent<RectTransform>().rotation = matchedShape.GetComponent<RectTransform>().rotation;
                matchedShape.tag = "COMPLETE";
                finish = true;
                return true;
            }
            else
            {
                Destroy(this.gameObject);
            }
        } else
        {
            Destroy(this.gameObject);
        }
        return false;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        matchedShape = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        matchedShape = null;
    }
}
