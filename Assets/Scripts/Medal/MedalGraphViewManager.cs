using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

namespace MedalViewer.Medal
{
    public class MedalGraphViewManager : MonoBehaviour
    {
        //public MedalFilterDisplayManager MedalFilterDisplayManager;
        public MedalLogicManager MedalLogicManager;


        //This should really be in the MedalLogicManager?
        //public MedalPositionLogic MedalPositionLogic;

        public LoadManager LoadManager;
        public UIController UIController;
        public MedalSpotlightDisplayManager MedalSpotlightDisplayManager;
        public MedalFilterManager MedalFilterManager = MedalFilterManager.Instance;

        public Transform StartPositionY;
        public Transform StartPositionX;
        public Transform ParentY;
        public Transform ParentX;
        public Transform GraphElementsY;
        public Transform GraphElementsX;

        public Transform MedalContent;
        public Transform InitialMedalContent;
        public Transform InitialGraphContent;

        public ScrollRect MedalView;
        public ScrollRect Vertical;
        public ScrollRect Horizontal;

        public GameObject CurrSublistMedal;

        public Dictionary<int, Dictionary<double, GameObject>> MedalGameObjects = new Dictionary<int, Dictionary<double, GameObject>>();
        //public Dictionary<GameObject, int> CycleMedals = new Dictionary<GameObject, int>();

        private readonly int yOffset = 250;
        private readonly int xOffset = 250;

        public List<GameObject> RowsY = new List<GameObject>();
        public List<GameObject> ColumnsX = new List<GameObject>();

        public string PointerObjectName = "";

        private bool firstPass = false;
        private bool isDisplayingSublist = false;

        public bool IsDisplayingMedal = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.C))
            {
                if (isDisplayingSublist)
                {
                    this.HideSublistOfMedals(true);
                }
            }
        }


        public void HandleDisplay(GameObject clickedOn)
        {
            MedalCycleLogic.Instance.StopCycleMedals();
            var medalHolder = clickedOn.GetComponentInChildren<GridLayoutGroup>();

            if (medalHolder != null)
            {
                if (medalHolder.transform.childCount > 1)
                {
                    this.DisplaySublistOfMedals(clickedOn);
                }
                else
                {
                    this.Display(medalHolder.transform.GetChild(0).gameObject);
                }
            }
            else
            {
                this.Display(clickedOn);
            }
        }

        public void Display(GameObject medal)
        {
            IsDisplayingMedal = true;
            if (CurrSublistMedal != null)
                this.HideSublistOfMedals();

            StartCoroutine(MedalSpotlightDisplayManager.Display(medal));
        }

        public void UpdateYRows(float changeValue = 250)
        {
            var lowRange = MedalFilterManager.LowRange;
            var highRange = MedalFilterManager.HighRange;

            RowsY = MedalLogicManager.PlaceYRows(lowRange, highRange, StartPositionY, ParentY, changeValue);
        }

        public void PopulateMedals(bool generate = true)
        {
            //UIMovement.UpdateViewWindow(1080);
            if(generate)
                MedalGameObjects = MedalLogicManager.GenerateMedals(RowsY, ColumnsX, MedalContent);

            MedalLogicManager.PlaceMedals(RowsY, ColumnsX, MedalGameObjects);

            //this.PlaceGraphLines();
            //UIMovement.UpdateViewWindow();
        }

        // https://stackoverflow.com/questions/30766020/how-to-scroll-to-a-specific-element-in-scrollrect-with-unity-ui
        public Vector2 CenterToItem(RectTransform child)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = MedalView.viewport.localPosition;
            Vector2 childLocalPosition = child.localPosition;
            Vector2 result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y)
            );
            return result;
        }

        public void UpdateScrollBars(Vector2 vector)
        {
            if (PointerObjectName == "Scroll View")
            {
                Horizontal.horizontalNormalizedPosition = vector.x;
                Vertical.verticalNormalizedPosition = vector.y;
            }
        }

        public void UpdateScrollMedalView(Vector2 vector)
        {
            MedalView.horizontalNormalizedPosition = vector.x;
            MedalView.verticalNormalizedPosition = vector.y;
        }

        public void UpdateScrollMedalViewX(Vector2 vector)
        {
            if (PointerObjectName == "Horizontal")
                MedalView.horizontalNormalizedPosition = vector.x;
        }

        public void UpdateScrollMedalViewY(Vector2 vector)
        {
            if (PointerObjectName == "Vertical")
                MedalView.verticalNormalizedPosition = vector.y;
        }

        public void PlaceGraphLines()
        {
            GraphElementsX.SetParent(MedalContent, true);
            GraphElementsY.SetParent(MedalContent, true);

            GraphElementsX.SetAsFirstSibling();
            GraphElementsY.SetAsFirstSibling();

            //print("X " + MedalContent.GetComponent<RectTransform>().offsetMax.y + " " + MedalContent.GetComponent<RectTransform>().offsetMin.y);
            //print("Y " + MedalContent.GetComponent<RectTransform>().offsetMax.x + " " + MedalContent.GetComponent<RectTransform>().offsetMin.x);

            foreach (var x in ParentX.GetComponentsInChildren<Text>())
            {
                var p = Instantiate(Resources.Load("ColumnTemplate") as GameObject);
                p.transform.SetParent(GraphElementsX, false);

                p.GetComponent<RectTransform>().position = x.GetComponent<RectTransform>().position;

                p.GetComponent<RectTransform>().offsetMax = new Vector2(p.GetComponent<RectTransform>().offsetMax.x, MedalContent.GetComponent<RectTransform>().offsetMax.y);
                p.GetComponent<RectTransform>().offsetMin = new Vector2(p.GetComponent<RectTransform>().offsetMin.x, MedalContent.GetComponent<RectTransform>().offsetMin.y);
                //print(p.GetComponent<RectTransform>().offsetMax.x + " " + p.GetComponent<RectTransform>().offsetMin.x);
            }

            foreach (var x in ParentY.GetComponentsInChildren<Text>())
            {
                var p = Instantiate(Resources.Load("RowTemplate") as GameObject);
                p.transform.SetParent(GraphElementsY, false);

                p.GetComponent<RectTransform>().position = x.GetComponent<RectTransform>().position;
                
                p.GetComponent<RectTransform>().offsetMax = new Vector2(MedalContent.GetComponent<RectTransform>().offsetMax.x, p.GetComponent<RectTransform>().offsetMax.y);
                p.GetComponent<RectTransform>().offsetMin = new Vector2(MedalContent.GetComponent<RectTransform>().offsetMin.x, p.GetComponent<RectTransform>().offsetMin.y);
                //print(p.GetComponent<RectTransform>().offsetMax.y + " " + p.GetComponent<RectTransform>().offsetMin.y);
            }
        }

        public void ResetGraph()
        {
            UIController.ResetViewWindow();

            MedalContent.GetComponent<RectTransform>().offsetMax = InitialMedalContent.GetComponent<RectTransform>().offsetMax;
            MedalContent.GetComponent<RectTransform>().offsetMin = InitialMedalContent.GetComponent<RectTransform>().offsetMin;
            //MedalContent.position = InitialMedalContent.position;
            GraphElementsX.position = InitialGraphContent.position;
            GraphElementsY.position = InitialGraphContent.position;

            MedalCycleLogic.Instance.StopCycleMedals();
            //Globals.CycleMedals.Clear();

            foreach (var child in ParentX.GetComponentsInChildren<Text>())
            {
                GameObject.DestroyImmediate(child.gameObject);
            }

            foreach (var child in ParentY.GetComponentsInChildren<Text>())
            {
                GameObject.DestroyImmediate(child.gameObject);
            }

            RowsY.Clear();
            ColumnsX.Clear();

            foreach (var kv in MedalGameObjects)
            {
                foreach (var kv2 in kv.Value)
                {
                    Destroy(kv2.Value);
                }

                kv.Value.Clear();
            }

            //var xElems = GraphElementsX.GetComponentsInChildren<Image>();
            //var yElems = GraphElementsY.GetComponentsInChildren<Image>();

            foreach (var child in GraphElementsX.GetComponentsInChildren<Image>())
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (var child in GraphElementsY.GetComponentsInChildren<Image>())
            {
                GameObject.Destroy(child.gameObject);
            }

            //var xParent = ParentX.GetComponentsInChildren<Text>();
            //var yParent = ParentY.GetComponentsInChildren<Text>();

            foreach (var child in ParentX.GetComponentsInChildren<Text>())
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (var child in ParentY.GetComponentsInChildren<Text>())
            {
                GameObject.Destroy(child.gameObject);
            }

            GraphElementsX.SetParent(InitialMedalContent, false);
            GraphElementsY.SetParent(InitialMedalContent, false);

            MedalGameObjects.Clear();
        }

        public void DisplaySublistOfMedals(GameObject clickedOn)
        {
            isDisplayingSublist = true;

            if (CurrSublistMedal != null)
                HideSublistOfMedals();

            CurrSublistMedal = clickedOn;

            var canvasGroup = clickedOn.GetComponentsInChildren<CanvasGroup>().First(x => x.name == "SublistContent");

            canvasGroup.SetCanvasGroupActive();
        }

        public void HideSublistOfMedals(bool closed = false)
        {
            isDisplayingSublist = false;

            var canvasGroup = CurrSublistMedal.GetComponentsInChildren<CanvasGroup>().First(x => x.name == "SublistContent");

            canvasGroup.SetCanvasGroupInactive();

            CurrSublistMedal = null;

            if (closed)
            {
                MedalCycleLogic.Instance.StartCycleMedals();
            }
        }

        public IEnumerator Display()
        {
            while (LoadManager.IsLoading)
            {
                yield return null;
            }

            LoadManager.StartLoading();

            this.ResetGraph();

            var lowRange = MedalFilterManager.LowRange;
            var highRange = MedalFilterManager.HighRange;
            var tiers = MedalFilterManager.Tiers;

            RowsY = MedalLogicManager.PlaceYRows(lowRange, highRange, StartPositionY, ParentY, yOffset);
            ColumnsX = MedalLogicManager.PlaceXColumns(tiers, StartPositionX, ParentX, xOffset);

            MedalContent.GetComponent<RectTransform>().offsetMax = new Vector2(ParentX.GetComponent<RectTransform>().offsetMax.x, ParentY.GetComponent<RectTransform>().offsetMax.y);

            this.PopulateMedals();
            StartCoroutine(MedalCycleLogic.Instance.PopulateCycleMedals(MedalGameObjects));

            this.PlaceGraphLines();

            // Shift placement to display latest medal
            var latestMedal = MedalLogicManager.GetLatestMedal();

            var multiplier = MedalLogicManager.GetHighestMultiplier(latestMedal);

            var medalObject = MedalGameObjects[latestMedal.Tier][multiplier].GetComponent<RectTransform>();
            MedalView.content.localPosition = this.CenterToItem(medalObject);

            LoadManager.FinishLoading();
        }
    }
}
