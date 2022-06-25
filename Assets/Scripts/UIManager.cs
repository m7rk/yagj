using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameState gs;
    public TMPro.TMP_Text collected;

    public GameObject book;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        collected.text = "LT: " + gs.p1.LT;
    }

    public void openBook()
    {
        book.SetActive(true);
    }

    public void closeBook()
    {
        book.SetActive(false);
    }
}
