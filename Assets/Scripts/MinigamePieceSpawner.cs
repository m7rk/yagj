using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinigamePieceSpawner : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    public GameObject spawnable;

    private GameObject currPiece;
    private List<GameObject> spawnedPieces;
    public UIManager um;
    public GameState gs;

    public void Start()
    {
        currPiece = null;
        spawnedPieces = new List<GameObject>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // this is fucking filthy
        if(GetComponent<UnityEngine.UI.Image>().color == Color.white)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            currPiece = Instantiate(spawnable);
            currPiece.transform.SetParent(this.transform.parent);
            currPiece.GetComponent<RectTransform>().transform.position = this.GetComponent<RectTransform>().transform.position;
            gs.p1.decr(currPiece.tag);
        }
    }

    public void refund()
    {
        foreach(var v in spawnedPieces)
        {
            gs.p1.incr(v.tag);
            Destroy(v);
        }
        spawnedPieces = new List<GameObject>();
    }

    public void accept()
    {
        foreach (var v in spawnedPieces)
        {
            Destroy(v);
        }
        spawnedPieces = new List<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        // if mouse up and hold a piece. it was placed?
        if(Input.GetMouseButtonUp(0) && currPiece != null)
        {
            if(currPiece.GetComponent<PlaceablePiece>().check())
            {
                um.stepCompleted();
                spawnedPieces.Add(currPiece);
            }
            else
            {
                gs.p1.incr(currPiece.tag);
            }
            currPiece = null;
        }
    }
}
