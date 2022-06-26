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

    CompletePuzzle completePuzzle;


    void Start()
    {
        resetPostition = this.transform.localPosition;
        completePuzzle = GameObject.FindGameObjectWithTag("PointsHandler").GetComponent<CompletePuzzle>();
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


    public void check()
    {
        // correct shape
        if (matchedShape != null && matchedShape.tag == this.tag)
        {
            // correct position
            bool posGood = (this.GetComponent<RectTransform>().position - matchedShape.GetComponent<RectTransform>().position).magnitude <= 10;
            bool rotGood = Mathf.Abs(Quaternion.Angle(matchedShape.GetComponent<RectTransform>().rotation,GetComponent<RectTransform>().rotation)) < 1f;

            if (posGood && rotGood)
            {
                this.GetComponent<RectTransform>().position = matchedShape.GetComponent<RectTransform>().position;
                this.GetComponent<RectTransform>().rotation = matchedShape.GetComponent<RectTransform>().rotation;
                completePuzzle.AddPoints();
                finish = true;
            }
            else
            {
                Destroy(this.gameObject);
            }
        } else
        {
            Destroy(this.gameObject);
        }    

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        matchedShape = other.gameObject;
    }
}
