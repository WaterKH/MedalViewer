using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RowResizer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject Parent;
    public List<Transform> PreviousRows;
    public List<Transform> NextRows;
    public bool ChangingRowSize;

    public Dictionary<int, List<RectTransform>> Medals = new Dictionary<int, List<RectTransform>>();
    public MedalPositionLogic MedalPositionLogic;
    public MedalLogicManager MedalLogicManager;
    public MedalSortLogic MedalSortLogic;

    private float minYValue = 500; // TODO don't allow change if this is met
    private float maxYValue = 3000;
    private float prevYValue;
    private bool firstPass;
    private float comparePreviousRowY = 0.0f;
    private float compareNextRowY = 0.0f;

    // Use this for initialization
    void Start()
    {
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
        if (!firstPass)
        {
            UpdateMedals();
        }

        if (!ChangingRowSize)
            return;
        // DO SOMETHING HERE

        foreach (var medals in Medals)
        {
            foreach (var medal in medals.Value)
            {
                MedalPositionLogic.UpateMedalHolderPosition(medal.gameObject);
            }
        }

        var mouseDelta = Input.mousePosition.y - prevYValue;

        if (prevYValue == 0.0f)
        {
            prevYValue = transform.position.y;
            mouseDelta = 0;
            return;
        }

        transform.position = new Vector3(transform.position.x, Input.mousePosition.y);

        if (mouseDelta != 0)
        {
            comparePreviousRowY = Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.y -
                                        PreviousRows.FirstOrDefault().GetComponent<RectTransform>().anchoredPosition.y);

            compareNextRowY = NextRows.LastOrDefault() != null ? Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.y -
                                        NextRows.LastOrDefault().GetComponent<RectTransform>().anchoredPosition.y) 
                : 0; // TODO Fix the else part?
        }

        if (mouseDelta > 0)
        {
            // Move the top up to stay attached to the object at a reasonable height
            if (compareNextRowY >= minYValue)
            {
                prevYValue = transform.position.y;
                return;
            }

            foreach (var l in NextRows)
            {
                l.position = new Vector3(l.position.x, l.position.y + mouseDelta);
            }
            
            // Move the bottom up to keep up with the rest of the stretch
            if (comparePreviousRowY >= maxYValue)
            {
                foreach (var l in PreviousRows)
                {
                    l.position = new Vector3(l.position.x, l.position.y + mouseDelta);
                }
            }
        }
        else if(mouseDelta < 0)
        {
            // Move the bottom down to stay attached to the object at a reasonable height
            if (comparePreviousRowY >= minYValue)
            {
                prevYValue = transform.position.y;
                return;
            }

            foreach (var l in PreviousRows)
            {
                l.position = new Vector3(l.position.x, l.position.y + mouseDelta);
            }

            // Move the top down to keep up with the rest of the stretch
            if (compareNextRowY >= maxYValue)
            {
                foreach (var l in NextRows)
                {
                    l.position = new Vector3(l.position.x, l.position.y + mouseDelta);
                }
            }
        }

        

        //MedalLogicManager.SetupMedalsByTierAndMult(MedalSortLogic.medals_by_tier);

        //MedalPositionLogic.UpdateContent();

        prevYValue = transform.position.y;
    }

    public void UpdateMedals()
    {
        var next = NextRows.FirstOrDefault();
        var prev = PreviousRows.LastOrDefault();

        if (next == null)
        {
            next = transform;
        }
        if (prev == null)
        {
            prev = transform;
        }

        var rangeBottom = float.Parse(prev.name);
        var rangeTop = float.Parse(next.name);

        foreach (var tier in Globals.TierFilter.ToggleChildrenActivated)
        {
            if (Medals.ContainsKey(tier))
            {
                Medals.Remove(tier);
            }

            var currTierParent = GameObject.Find("Tier" + tier + "Parent");

            if (currTierParent.GetComponentsInChildren<RectTransform>().Length > 0)
            {
                firstPass = true;
            }

            foreach (var child in currTierParent.GetComponentsInChildren<RectTransform>())
            {
                float y = 0.0f;
                float.TryParse(child.name, out y);
                //print(child.name + " " + y);
                if (y == 0.0f)
                    continue;
                //print(rangeBottom + " " + y + " " + rangeTop);
                if (y >= rangeBottom && y <= rangeTop)
                {
                    if(!Medals.ContainsKey(tier))
                        Medals.Add(tier, new List<RectTransform>());

                    Medals[tier].Add(child);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("Scroll").GetComponent<ScrollRect>().enabled = false;
        ChangingRowSize = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("Scroll").GetComponent<ScrollRect>().enabled = true;
        ChangingRowSize = false;
    }
}
