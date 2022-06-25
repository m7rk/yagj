using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    // the corrisponding black tangrams
    private GameObject correctForm;

    private bool moving;
    private bool finish;
    private bool isMatching;

    private float startPosX;
    private float startPosY;

    private Vector3 resetPostition;
    private Quaternion resetRotation;

    CompletePuzzle completePuzzle;

    [SerializeField]
    private float errorMarginDrop = 0.1f;

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
            if (moving)
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
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;
            moving = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        moving = false;

        if (isMatching)
        {

            // if the shape moves near a radius of the correct form, snap it,
            // else reset the position
            if ((this.GetComponent<RectTransform>().position - correctForm.GetComponent<RectTransform>().position).magnitude <= errorMarginDrop)
            {
                this.GetComponent<RectTransform>().position = correctForm.GetComponent<RectTransform>().position;
                this.GetComponent<RectTransform>().rotation = correctForm.GetComponent<RectTransform>().rotation;
                completePuzzle.AddPoints();
                finish = true;
            }
            else
            {
                this.transform.localPosition = new Vector3(resetPostition.x, resetPostition.y, resetPostition.z);
                this.transform.rotation = resetRotation;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.tag == other.gameObject.tag)
        {
            Debug.Log(this.tag + " " + other.tag);
            isMatching = true;
            correctForm = other.gameObject;
        }
    }
}
