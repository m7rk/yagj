using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    public GameObject correctForm;
    private bool moving;
    private bool finish;

    private float startPosX;
    private float startPosY;

    private Vector3 resetPostition;
    private Quaternion resetRotation;

    [SerializeField]
    private float errorMarginDrop = 0.5f;

    void Start()
    {
        resetPostition = this.transform.localPosition;
    }

    void Update()
    {   
        if (!finish)
        {
            if (moving)
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                // allows to drag the sprite around the world using its local position
                this.gameObject.transform.localPosition =
                    new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    //this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 45.0f));
                    this.transform.Rotate(new Vector3(0.0f, 0.0f, 0.1f));
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    //this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45.0f));
                    this.transform.Rotate(new Vector3(0.0f, 0.0f, -0.1f));
                }
            }

        }
       
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            moving = true;
        }
    }

    private void OnMouseUp()
    {
        moving = false;

        // if the shape moves near a radius of the correct form, snap it,
        // else reset the position

        if (Mathf.Abs(this.transform.localPosition.x - correctForm.transform.localPosition.x) <= errorMarginDrop &&
                Mathf.Abs(this.transform.localPosition.y - correctForm.transform.localPosition.y) <= errorMarginDrop)
        {
            this.transform.position = new Vector3(correctForm.transform.position.x, correctForm.transform.position.y, correctForm.transform.position.z);
            this.transform.rotation = correctForm.transform.rotation;

            finish = true;
        }
        else
        {
            this.transform.localPosition = new Vector3(resetPostition.x, resetPostition.y, resetPostition.z);
            this.transform.rotation = resetRotation;
        }
    }
}
