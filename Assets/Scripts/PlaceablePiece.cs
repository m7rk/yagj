using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceablePiece : MonoBehaviour
{

    // the corrisponding black tangrams
    private GameObject matchedShape;

    private bool finish;
    private bool isMatching;

    private Vector3 resetPostition;
    private Quaternion resetRotation;

    public UIManager um;


    void Start()
    {
        resetPostition = this.transform.localPosition;
        isMatching = false;
    }

    void Update()
    {   
        if (!finish)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;

            // allows to drag the sprite around the world using its local position
            this.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(mousePos.x , mousePos.y, this.gameObject.transform.localPosition.z);

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.transform.Rotate(new Vector3(0.0f, 0.0f, 45f));
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
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
            // correct position
            bool rotGood = Mathf.Abs(Quaternion.Angle(matchedShape.GetComponent<RectTransform>().rotation,GetComponent<RectTransform>().rotation)) < 1f;

            Debug.Log(rotGood);
            if (rotGood)
            {
                this.GetComponent<RectTransform>().position = matchedShape.GetComponent<RectTransform>().position;
                this.GetComponent<RectTransform>().rotation = matchedShape.GetComponent<RectTransform>().rotation;
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
        Debug.Log("enter!!");
        matchedShape = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        matchedShape = null;
    }
}
