using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameState gs;
    public TMPro.TMP_Text LTTEXT;
    public TMPro.TMP_Text MTTEXT;
    public TMPro.TMP_Text STTEXT;
    public TMPro.TMP_Text STEXT;
    public TMPro.TMP_Text PTEXT;

    public GameObject book;
    // Start is called before the first frame update

    public AudioClip bookOpen;
    public AudioClip bookClose;

    public readonly float COLOR_TRANS_TIME = 2f;

    void Start()
    {
        
    }

    public void setIncrFlash(GameState.GramType gramType)
    {
        switch(gramType)
        {
            case GameState.GramType.LT:
                LTTEXT.color = Color.green; break;
            case GameState.GramType.MT:
                MTTEXT.color = Color.green; break;
            case GameState.GramType.ST:
                STTEXT.color = Color.green; break;
            case GameState.GramType.S:
                STEXT.color = Color.green; break;
            case GameState.GramType.P:
                PTEXT.color = Color.green; break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        LTTEXT.text = "" + gs.p1.LT;
        MTTEXT.text = "" + gs.p1.MT;
        STTEXT.text = "" + gs.p1.ST;
        STEXT.text = "" + gs.p1.S;
        PTEXT.text = "" + gs.p1.P;
        STTEXT.color = Color.Lerp(STTEXT.color, Color.black, Time.deltaTime * COLOR_TRANS_TIME);
    }

    public void openBook()
    {
        book.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(bookOpen);
    }

    public void closeBook()
    {
        book.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(bookClose);
    }
}
