using MedalViewer.Medal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PresenterManager : MonoBehaviour
{
    public CanvasGroup PresenterOverlay;
    public CanvasGroup PresenterMedals;
    public CanvasGroup PresenterTools;

    public Button[] Layouts;
    // Key: int (Current Page), Value: templates (1, 2, 4, 6)
    public Dictionary<int, List<CanvasGroup>> Templates = new Dictionary<int, List<CanvasGroup>>();

    public GameObject PageParent;
    public GameObject PageButtonParent;
    public GameObject MedalPresenterParent;
    public List<GameObject> Pages = new List<GameObject>();
    public List<GameObject> PageButtons = new List<GameObject>();

    public Dictionary<int, CanvasGroup> CurrTemplate = new Dictionary<int, CanvasGroup>();
    public int CurrentPage = 0;

    public InputField SearchBar;

    public Color PageColor;
    public Color CurrentPageColor;

    private bool firstPass = true;
    private MedalPresenterDisplayManager currMedalPresenterDisplayManager;

    public List<GameObject> medalList = new List<GameObject>();

    private bool presenting = false;

    private void Awake()
    {
        SearchBar.onEndEdit.AddListener(x => GetMedals(x));
        AddPage();
    }

    private void Update()
    {
        if(presenting)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                this.DisplayPresenterTools();
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (CurrentPage - 1 > 0)
                {
                    DisplayPage(CurrentPage - 1);
                }
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                if(CurrentPage + 1 <= Pages.Count)
                {
                    DisplayPage(CurrentPage + 1);
                }
            }
        }
    }

    #region Display

    #region Presenter Overall

    public void DisplayPresenter()
    {
        MedalCycleLogic.Instance.StopCycleMedals();

        PresenterOverlay.SetCanvasGroupActive();

        if (firstPass)
        {
            firstPass = false;
            PopulateMedalList();
        }
    }

    public void DisplayPresenterTools()
    {
        presenting = false;

        PresenterTools.SetCanvasGroupActive();
        GameObject.FindGameObjectsWithTag("RemoveMedal").ToList().ForEach(x => x.GetComponent<CanvasGroup>().SetCanvasGroupActive());
    }

    public void HidePresenter()
    {
        MedalCycleLogic.Instance.StartCycleMedals();

        PresenterOverlay.SetCanvasGroupInactive();
    }

    public void HidePresenterTools()
    {
        presenting = true;

        PresenterTools.SetCanvasGroupInactive();
        GameObject.FindGameObjectsWithTag("RemoveMedal").ToList().ForEach(x => x.GetComponent<CanvasGroup>().SetCanvasGroupInactive());
    }

    #endregion

    #region Templates

    public void DisplayTemplate(CanvasGroup template)
    {
        if (template == CurrTemplate[CurrentPage] && Pages.Count > 1)
            return;

        HideTemplate(CurrTemplate[CurrentPage]);

        template.SetCanvasGroupActive();
        CurrTemplate[CurrentPage] = template;
    }

    public void HideTemplate(CanvasGroup template)
    {
        template.SetCanvasGroupInactive();
    }

    #endregion

    #region Pages

    public void AddPage()
    {
        HidePage(CurrentPage);
        CurrentPage = Pages.Count + 1;

        var page = Instantiate(Resources.Load("Page")) as GameObject;
        Pages.Add(page);
        page.transform.SetParent(PageParent.transform, false);

        var pageButton = Instantiate(Resources.Load("PageButton")) as GameObject;
        PageButtons.Add(pageButton);
        pageButton.transform.SetParent(PageButtonParent.transform, false);
        pageButton.transform.SetSiblingIndex(CurrentPage - 1);
        pageButton.GetComponentInChildren<Text>().text = CurrentPage.ToString();
        pageButton.GetComponent<Image>().color = CurrentPageColor;

        pageButton.GetComponent<Button>().onClick.AddListener(delegate { DisplayPage(int.Parse(pageButton.GetComponentInChildren<Text>().text)); });
        pageButton.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { DeletePage(pageButton); });

        var templates = Pages[CurrentPage - 1].GetComponentsInChildren<Image>().ToList().Where(x => x.name.Contains("Template")).ToList();

        for(int x = 0; x < templates.Count; ++x)
        {
            if (!Templates.ContainsKey(CurrentPage))
                Templates.Add(CurrentPage, new List<CanvasGroup>());

            Templates[CurrentPage].Add(templates[x].GetComponent<CanvasGroup>());

            var x2 = x;
            Layouts[x].onClick.AddListener(delegate { DisplayTemplate(Templates[CurrentPage][x2]); });

            //templates[x].GetComponentInChildren<Button>().onClick.AddListener(delegate { DisplayMedals(); });
        }

        foreach(var t in Templates)
        {
            foreach(var c in t.Value)
            {
                c.GetComponentsInChildren<Button>().Where(x => x.name != "Delete").ToList().ForEach(x => x.onClick.AddListener(delegate { DisplayMedals(x.transform.parent.GetComponent<MedalPresenterDisplayManager>()); }));
            }
        }

        if (!CurrTemplate.ContainsKey(CurrentPage))
            CurrTemplate.Add(CurrentPage, null);

        CurrTemplate[CurrentPage] = Templates[CurrentPage][3];
        Templates[CurrentPage][3].SetCanvasGroupActive();
        DisplayPage(CurrentPage, false);
    }

    public void DeletePage(GameObject button)
    {
        var index = int.Parse(button.GetComponentInChildren<Text>().text) - 1;
        if (CurrentPage > index)
        {
            //HidePage(index);
            CurrentPage -= 1;
        }

        GameObject.Destroy(Pages[index]);
        Pages.RemoveAt(index);

        GameObject.Destroy(PageButtons[index]);
        PageButtons.RemoveAt(index);

        Templates.Remove(index + 1);
        CurrTemplate.Remove(index + 1);

        for(int i = index; i < Pages.Count; ++i)
        {
            PageButtons[i].GetComponentInChildren<Text>().text = (i + 1).ToString();

            Templates.Add(i + 1, Templates[i + 2]);
            Templates.Remove(i + 2);

            CurrTemplate.Add(i + 1, CurrTemplate[i + 2]);
            CurrTemplate.Remove(i + 2);
        }
    }

    public void DisplayPage(int i, bool hide = true)
    {
        if(hide)
            HidePage(CurrentPage);
        CurrentPage = i;

        Pages[CurrentPage - 1].GetComponent<CanvasGroup>().SetCanvasGroupActive();
        PageButtons[CurrentPage - 1].GetComponent<Image>().color = CurrentPageColor;
    }

    public void HidePage(int i)
    {
        if ((i - 1) < Pages.Count && (i - 1) >= 0)
        {
            Pages[i - 1].GetComponent<CanvasGroup>().SetCanvasGroupInactive();
            PageButtons[i - 1].GetComponent<Image>().color = PageColor;
        }
    }

    #endregion

    #region Medal Presenter
    public void DisplayMedals(MedalPresenterDisplayManager manager)
    {
        currMedalPresenterDisplayManager = manager;
        
        PresenterMedals.SetCanvasGroupActive();
    }

    public void SelectMedal(MedalDisplay medalDisplay)
    {
        StartCoroutine(currMedalPresenterDisplayManager.Display(null, medalDisplay));
        this.HideMedals();
    }

    public void HideMedals()
    {
        PresenterMedals.SetCanvasGroupInactive();
    }

    public void PopulateMedalList()
    {
        StartCoroutine(GetSearchMedalsFromPHP());
    }

    private readonly string selectFilteredMedalsPHP = "https://mvphp.azurewebsites.net/selectFilteredMedals.php";
    private MedalFilter medalFilter = new MedalFilter
    {
        SixStar = true,
        SevenStar = true
    };
    private IEnumerator GetSearchMedalsFromPHP()
    {
        Dictionary<int, Medal> medals = new Dictionary<int, Medal>();

        //var query = "Select * From MedalUpdated Where (Star = 6 Or Star = 7) And Type = 'Attack' Order By Id Desc";
        var query = medalFilter.GenerateFilterQuery() + " ORDER BY MUId DESC";
        WWWForm form = new WWWForm();
        form.AddField("sqlQuery", query);

        using (UnityWebRequest www = UnityWebRequest.Post(selectFilteredMedalsPHP, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("ERROR:: " + www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                var rows = www.downloadHandler.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var row in rows)
                {
                    var splitRow = row.Split(new char[] { '|' }, StringSplitOptions.None);

                    if (splitRow.Length == 30)
                    {
                        var medal = new Medal
                        {
                            Id = string.IsNullOrEmpty(splitRow[0]) ? -1 : int.Parse(splitRow[0]),
                            Name = string.IsNullOrEmpty(splitRow[1]) ? "" : splitRow[1],
                            ImageURL = string.IsNullOrEmpty(splitRow[2]) ? "" : splitRow[2],
                            Star = string.IsNullOrEmpty(splitRow[3]) ? 0 : int.Parse(splitRow[3]),
                            Class = string.IsNullOrEmpty(splitRow[4]) ? "" : splitRow[4],
                            Type = string.IsNullOrEmpty(splitRow[5]) ? "" : splitRow[5],
                            Attribute_PSM = string.IsNullOrEmpty(splitRow[6]) ? "" : splitRow[6],
                            Attribute_UR = string.IsNullOrEmpty(splitRow[7]) ? "" : splitRow[7],
                            BaseAttack = string.IsNullOrWhiteSpace(splitRow[8]) ? 0 : int.Parse(splitRow[8]),
                            MaxAttack = string.IsNullOrEmpty(splitRow[9]) ? 0 : int.Parse(splitRow[9]),
                            BaseDefense = string.IsNullOrEmpty(splitRow[10]) ? 0 : int.Parse(splitRow[10]),
                            MaxDefense = string.IsNullOrEmpty(splitRow[11]) ? 0 : int.Parse(splitRow[11]),
                            TraitSlots = string.IsNullOrEmpty(splitRow[12]) ? 0 : int.Parse(splitRow[12]),
                            BasePetPoints = string.IsNullOrEmpty(splitRow[13]) ? 0 : int.Parse(splitRow[13]),
                            MaxPetPoints = string.IsNullOrEmpty(splitRow[14]) ? 0 : int.Parse(splitRow[14]),
                            Ability = string.IsNullOrEmpty(splitRow[15]) ? "" : splitRow[15],
                            AbilityDescription = string.IsNullOrEmpty(splitRow[16]) ? "" : splitRow[16],
                            Target = string.IsNullOrEmpty(splitRow[17]) ? "" : splitRow[17],
                            Gauge = string.IsNullOrEmpty(splitRow[18]) ? 0 : int.Parse(splitRow[18]),
                            BaseMultiplierLow = string.IsNullOrEmpty(splitRow[19]) ? 0.0 : double.Parse(splitRow[19]),
                            BaseMultiplierHigh = string.IsNullOrEmpty(splitRow[20]) ? 0.0 : double.Parse(splitRow[20]),
                            MaxMultiplierLow = string.IsNullOrEmpty(splitRow[21]) ? 0.0 : double.Parse(splitRow[21]),
                            MaxMultiplierHigh = string.IsNullOrEmpty(splitRow[22]) ? 0.0 : double.Parse(splitRow[22]),
                            SubslotMultiplier = string.IsNullOrEmpty(splitRow[23]) ? 0.0 : double.Parse(splitRow[23]),
                            Tier = string.IsNullOrEmpty(splitRow[24]) ? 0 : int.Parse(splitRow[24]),
                            SupernovaName = string.IsNullOrEmpty(splitRow[25]) ? "" : splitRow[25],
                            SupernovaDescription = string.IsNullOrEmpty(splitRow[26]) ? "" : splitRow[26],
                            SupernovaDamage = string.IsNullOrEmpty(splitRow[27]) ? "" : splitRow[27],
                            SupernovaTarget = string.IsNullOrEmpty(splitRow[28]) ? "" : splitRow[28],
                            Effect_Description = string.IsNullOrEmpty(splitRow[29]) ? "" : splitRow[29]
                        };

                        medal.GuiltMultiplierLow = medal.MaxMultiplierLow != 0 ? medal.MaxMultiplierLow * Globals.TierConversion[medal.Tier] :
                                                   medal.BaseMultiplierLow * Globals.TierConversion[medal.Tier];
                        medal.GuiltMultiplierHigh = medal.MaxMultiplierHigh != 0 ? medal.MaxMultiplierHigh * Globals.TierConversion[medal.Tier] :
                                                   medal.BaseMultiplierHigh * Globals.TierConversion[medal.Tier];

                        medal.GuiltMultiplierLow = Math.Round(medal.GuiltMultiplierLow, 2);
                        medal.GuiltMultiplierHigh = Math.Round(medal.GuiltMultiplierHigh, 2);

                        medals.Add(medal.Id, medal);
                    }
                }
            }
        }

        foreach (var medal in medals)
        {
            var medalObject = CreateMedal(medal.Value);
            medalObject.transform.SetParent(MedalPresenterParent.transform, false);
            medalList.Add(medalObject);

            medalObject.GetComponent<Button>().onClick.AddListener(delegate { SelectMedal(medalObject.GetComponent<MedalDisplay>()); });
        }

        var y = (MedalPresenterParent.GetComponent<GridLayoutGroup>().cellSize.y + MedalPresenterParent.GetComponent<GridLayoutGroup>().spacing.y) * ((medals.Values.Count / 5) + 1);
        MedalPresenterParent.GetComponent<RectTransform>().offsetMin = new Vector2(MedalPresenterParent.GetComponent<RectTransform>().offsetMin.x, -y);
    }

    private GameObject CreateMedal(Medal medal)
    {
        var medalGameObject = Instantiate(Resources.Load("MedalPresenter") as GameObject);

        medalGameObject.name = medal.Name;

        SetMedalImage(medal, medalGameObject, medal.ImageURL);

        medalGameObject.GetComponent<MedalDisplay>().MapVariables(medal);

        return medalGameObject;
    }

    private readonly string url = "https://medalviewer.blob.core.windows.net/images/";
    private void SetMedalImage(Medal medalItem, GameObject medalObject, string prevImg)
    {
        var fileName = medalItem.ImageURL;//.Replace(".png", "_tn.png");

        if (fileName == "NULL")
        {
            print(prevImg);
            fileName = prevImg;
        }

        var path = url + fileName;

        StartCoroutine(LoadImage(path, medalObject));
    }

    private IEnumerator LoadImage(string imageUrl, GameObject medalObject)
    {
        UnityWebRequest image = UnityWebRequestTexture.GetTexture(imageUrl);
        
        yield return image.SendWebRequest();
        if (image.isNetworkError || image.isHttpError)
            Debug.Log("ERROR: " + imageUrl + " " + image.error);
        else
            medalObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)image.downloadHandler).texture;
    }

    #endregion

    #region Search

    private void GetMedals(string lookFor)
    {
        if (lookFor.Length < 3)
        {
            ResetMedals();
            return;
        }

        ResetMedals();
        foreach (var medal in medalList)
        {
            if(!medal.name.Contains(lookFor))
            {
                medal.SetActive(false);
            }
        }
    }

    private void ResetMedals()
    {
        foreach(var medal in medalList)
        {
            medal.SetActive(true);
        }
    }

    #endregion

    #endregion
}
