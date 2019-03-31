using MedalViewer.Medal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RowResizer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject CurrRow;
    public GameObject PrevRow;

    public GameObject Parent;
    public List<Transform> PreviousRows;
    public List<Transform> NextRows;
    public bool ChangingRowSize;
    public bool SettingsChanged;

    public Dictionary<int, List<RectTransform>> Medals = new Dictionary<int, List<RectTransform>>();
    public MedalPositionLogic MedalPositionLogic;
    public MedalLogicManager MedalLogicManager;
    public MedalSortLogic MedalSortLogic;

    private float minYValue = 500; // TODO don't allow change if this is met
    private float maxYValue = 3000;
    private float prevYValue;
    private float comparePreviousRowY = 0.0f;
    private float compareNextRowY = 0.0f; 
    private float mouseDelta;
    private float mousePosY;
    private Vector3 v3;

    private GameObject currRow;
    private Transform previousRow;
    private Transform nextRow;
    private Regex OnlyDigits = new Regex(@"^\d+\.");
    
    // Use this for initialization
    void Start()
    {
        CurrRow = GameObject.FindGameObjectWithTag("CurrRow");
        PrevRow = GameObject.FindGameObjectWithTag("PrevRow");

        Parent = GameObject.FindGameObjectWithTag("YParent");
        MedalPositionLogic = GameObject.FindGameObjectWithTag("ScriptHolder").GetComponent<MedalPositionLogic>();
        MedalSortLogic = GameObject.FindGameObjectWithTag("ScriptHolder").GetComponent<MedalSortLogic>();
        MedalLogicManager = GameObject.FindGameObjectWithTag("ScriptHolder").GetComponent<MedalLogicManager>();

        if (PreviousRows.FirstOrDefault() != null)
        {
            minYValue = transform.position.y - PreviousRows.FirstOrDefault().position.y;
        }
        else if (NextRows.LastOrDefault() != null)
        {
            minYValue = transform.position.y - NextRows.LastOrDefault().position.y;
        }

        PreviousRows = Parent.GetComponentsInChildren<Transform>().Where(x =>
            {
                if (x.name == Parent.name)
                    return false;

                var y = 0;
                int.TryParse(x.name, out y);

                if (y != 0 && y < int.Parse(gameObject.name))
                {
                    return x;
                }

                return false;
            }
        ).ToList();

        NextRows = Parent.GetComponentsInChildren<Transform>().Where(x =>
            {
                if (x.name == Parent.name)
                    return false;

                var y = 0;
                var t = int.Parse(gameObject.name);

                int.TryParse(x.name, out y);

                if (y != 0 && y > t)
                {
                    return x;
                }

                return false;
            }
        ).ToList();
    }
    
    void Update()
    {
        if (!ChangingRowSize)
            return;
        
        v3 = Input.mousePosition;
        v3.z = -Camera.main.transform.position.z;
        mousePosY = Camera.main.ScreenToWorldPoint(v3).y;

        mouseDelta = (mousePosY - prevYValue);
        
        if (prevYValue == 0.0f)
        {
            prevYValue = transform.position.y;
            mousePosY = 0;
            return;
        }

        // Move row
        transform.position = new Vector3(transform.position.x, mousePosY, transform.position.z);

        if (mouseDelta != 0)
        {
            comparePreviousRowY = Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.y -
                                        PreviousRows.FirstOrDefault().GetComponent<RectTransform>().anchoredPosition.y);

            compareNextRowY = NextRows.LastOrDefault() != null ? Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.y -
                                        NextRows.LastOrDefault().GetComponent<RectTransform>().anchoredPosition.y) 
                : 0; // TODO Fix the else part?
        }

        //CurrRow.GetComponentsInChildren<Transform>().Where(x => x.name != CurrRow.name && OnlyDigits.Match(x.name).Success).ToList().ForEach(x => MedalPositionLogic.UpdateMedalHolderPosition(x.gameObject, currRow.transform, nextRow));
        //PrevRow.GetComponentsInChildren<Transform>().Where(x => x.name != PrevRow.name && OnlyDigits.Match(x.name).Success).ToList().ForEach(x => MedalPositionLogic.UpdateMedalHolderPosition(x.gameObject, previousRow, currRow.transform));

        if (mouseDelta > 0)
        {
            // Move the top up to stay attached to the object at a reasonable height
            if (compareNextRowY >= minYValue)
            {
                prevYValue = transform.position.y;
                return;
            }

            NextRows.ForEach(p => p.position = new Vector3(p.position.x, p.position.y + mouseDelta));

            // Move the bottom up to keep up with the rest of the stretch
            if (comparePreviousRowY >= maxYValue)
            {
                PreviousRows.ForEach(p => p.position = new Vector3(p.position.x, p.position.y + mouseDelta));
            }
        }
        else if (mouseDelta < 0)
        {
            // Move the bottom down to stay attached to the object at a reasonable height
            if (comparePreviousRowY >= minYValue)
            {
                prevYValue = transform.position.y;
                return;
            }

            PreviousRows.ForEach(p => p.position = new Vector3(p.position.x, p.position.y + mouseDelta));
            
            // Move the top down to keep up with the rest of the stretch
            if (compareNextRowY >= maxYValue)
            {
                NextRows.ForEach(p => p.position = new Vector3(p.position.x, p.position.y + mouseDelta));
            }
        }
        
        //MedalPositionLogic.UpdateContent(); TODO Readd this
        prevYValue = transform.position.y;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerEnter.name.Length > 2) return;

        GameObject.FindGameObjectWithTag("Scroll").GetComponent<ScrollRect>().enabled = false;
        ChangingRowSize = true;
        prevYValue = 0;

        currRow = eventData.pointerEnter;
        previousRow = PreviousRows.FirstOrDefault();
        nextRow = NextRows.LastOrDefault();

        currRow.GetComponentsInChildren<Transform>().Where(x => x.name != currRow.name && OnlyDigits.Match(x.name).Success).ToList().ForEach(x => x.SetParent(CurrRow.transform));
        previousRow.GetComponentsInChildren<Transform>().Where(x => x.name != previousRow.name && OnlyDigits.Match(x.name).Success).ToList().ForEach(x => x.SetParent(PrevRow.transform));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("Scroll").GetComponent<ScrollRect>().enabled = true;
        ChangingRowSize = false;

        // TODO UpdateContent?
        CurrRow.GetComponentsInChildren<Transform>().Where(x => x.name != CurrRow.name && OnlyDigits.Match(x.name).Success).ToList().ForEach(x => x.SetParent(currRow.transform));
        PrevRow.GetComponentsInChildren<Transform>().Where(x => x.name != PrevRow.name && OnlyDigits.Match(x.name).Success).ToList().ForEach(x => x.SetParent(previousRow.transform));
    }
}
