using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceMinigameBase : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    public GameObject spawnable;

    private GameObject v;
    public UIManager um;
    public GameState gs;


    public void OnPointerDown(PointerEventData eventData)
    {
        // this is fucking filthy
        if(GetComponent<UnityEngine.UI.Image>().color == Color.white)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            v = Instantiate(spawnable);
            v.transform.SetParent(this.transform.parent);
            v.GetComponent<RectTransform>().transform.position = this.GetComponent<RectTransform>().transform.position;
            gs.p1.decr(v.tag);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0) && v != null)
        {
            if(v.GetComponent<PlaceablePiece>().check())
            {
                tag = "COMPLETE";
                um.stepCompleted();
            }
            else
            {
                gs.p1.incr(v.tag);
            }
            v = null;
        }
    }
}
