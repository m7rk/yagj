using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameState gs;
    public TMPro.TMP_Text LTTEXT;
    public TMPro.TMP_Text MTTEXT;
    public TMPro.TMP_Text STTEXT;
    public TMPro.TMP_Text STEXT;
    public TMPro.TMP_Text PTEXT;

    public TMPro.TMP_Text LTTEXT2;
    public TMPro.TMP_Text MTTEXT2;
    public TMPro.TMP_Text STTEXT2;
    public TMPro.TMP_Text STEXT2;
    public TMPro.TMP_Text PTEXT2;

    // FUCK
    public Image LTTIM;
    public Image MTTIM;
    public Image STTIM;
    public Image STIM;
    public Image PTIM;

    private Color ltcol;
    private Color mtcol;
    private Color stcol;
    private Color scol;
    private Color pcol;


    public GameObject book;
    // Start is called before the first frame update

    public AudioClip bookOpen;
    public AudioClip bookClose;

    public GameObject[] recipies;

    public GameObject recipeParent;

    private GameObject loadedRecipe;

    public readonly float COLOR_TRANS_TIME = 2f;

    void Start()
    {
        ltcol = LTTIM.color;
        mtcol = MTTIM.color;
        stcol = STTIM.color;
        scol = STIM.color;
        pcol = PTIM.color;
        loadRecipe(0);
    }

    private int completionSteps = 0;

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

    public void stepCompleted()
    {
        completionSteps++;
    }

    void Update()
    {
        LTTEXT.text = "" + gs.p1.LT;
        MTTEXT.text = "" + gs.p1.MT;
        STTEXT.text = "" + gs.p1.ST;
        STEXT.text = "" + gs.p1.S;
        PTEXT.text = "" + gs.p1.P;

        LTTEXT2.text = "X" + gs.p1.LT;
        MTTEXT2.text = "X" + gs.p1.MT;
        STTEXT2.text = "X" + gs.p1.ST;
        STEXT2.text = "X" + gs.p1.S;
        PTEXT2.text = "X" + gs.p1.P;

        LTTIM.color = gs.p1.LT == 0 ? Color.white : ltcol;
        MTTIM.color = gs.p1.MT == 0 ? Color.white : mtcol;
        STTIM.color=  gs.p1.ST == 0 ? Color.white : stcol;
        STIM.color =  gs.p1.S == 0 ? Color.white : scol;
        PTIM.color = gs.p1.P == 0 ? Color.white : pcol;

        STTEXT.color = Color.Lerp(STTEXT.color, Color.black, Time.deltaTime * COLOR_TRANS_TIME);
        MTTEXT.color = Color.Lerp(MTTEXT.color, Color.black, Time.deltaTime * COLOR_TRANS_TIME);
        LTTEXT.color = Color.Lerp(LTTEXT.color, Color.black, Time.deltaTime * COLOR_TRANS_TIME);
        STEXT.color = Color.Lerp(STEXT.color, Color.black, Time.deltaTime * COLOR_TRANS_TIME);
        PTEXT.color = Color.Lerp(PTEXT.color, Color.black, Time.deltaTime * COLOR_TRANS_TIME);


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

    public void loadRecipe(System.Int32 rec)
    {
        Destroy(loadedRecipe);
        loadedRecipe = Instantiate(recipies[rec]);
        loadedRecipe.transform.SetParent(recipeParent.transform);
        loadedRecipe.transform.localPosition = Vector3.zero;
    }
}
