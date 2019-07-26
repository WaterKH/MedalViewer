using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

namespace MedalViewer.Medal
{
    public class MedalGraphViewLogic : MonoBehaviour
    {
        public MedalFilter MedalFilter;
        public MedalLogicManager MedalLogicManager;
        public MedalPositionLogic MedalPositionLogic;
        public Loading Loading;
        public UIMovement UIMovement;

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
        
        public Dictionary<int, Dictionary<double, GameObject>> MedalGameObjects = new Dictionary<int, Dictionary<double, GameObject>>();

        private readonly int yOffset = 250;
        private readonly int xOffset = 250;

        public List<GameObject> RowsY = new List<GameObject>();
        public List<GameObject> ColumnsX = new List<GameObject>();

        private bool firstPass = false;

        public IEnumerator Display()
        {
            while(Loading.IsLoading)
            {
                yield return null;
            }
            
            Loading.StartLoading();

            this.ResetGraph();

            RowsY = MedalPositionLogic.PlaceYRows(StartPositionY, ParentY, yOffset);
            ColumnsX = MedalPositionLogic.PlaceXColumns(StartPositionX, ParentX, xOffset);


            MedalContent.GetComponent<RectTransform>().offsetMax = new Vector2(ParentX.GetComponent<RectTransform>().offsetMax.x, ParentY.GetComponent<RectTransform>().offsetMax.y);

            this.PopulateMedals();
            StartCoroutine(this.PopulateCycleMedals());

            this.PlaceGraphLines();
            Loading.FinishLoading();
        }

        public void UpdateYRows(float changeValue = 250)
        {
            RowsY = MedalPositionLogic.PlaceYRows(StartPositionY, ParentY, changeValue);
        }

        public void PopulateMedals(bool generate = true)
        {
            //UIMovement.UpdateViewWindow(1080);
            if(generate)
                MedalGameObjects = MedalLogicManager.GenerateMedals(RowsY, ColumnsX, MedalContent);

            MedalPositionLogic.PlaceMedals(RowsY, ColumnsX, MedalGameObjects);
            
            //this.PlaceGraphLines();
            //UIMovement.UpdateViewWindow();
        }
        
        public IEnumerator PopulateCycleMedals()
        {
            while(Loading.IsLoading)
            {
                yield return null;
            }

            Loading.StartLoading();

            foreach(var tier in MedalGameObjects)
            {
                foreach(var mult in tier.Value)
                {
                    var medal = mult.Value;
                    var subContent = medal.GetComponentsInChildren<RectTransform>().First(x => x.name == "SubContent");
                    var subTexture = subContent.GetComponentInChildren<RawImage>().texture;
                    var medalTexture = medal.GetComponentsInChildren<RawImage>().First(x => x.name == "MedalImage").texture;

                    Globals.CycleMedals.Add(medal, 0);
                }
            }

            MedalCycleLogic.Instance.StartCycleMedals();
            //Loading.FinishLoading();
        }

        
        public void UpdateScrollBars(Vector2 vector)
        {
            if (Globals.PointerObjectName == "Scroll View")
            {
                Horizontal.horizontalNormalizedPosition = vector.x;
                Vertical.verticalNormalizedPosition = vector.y;
            }
        }

        public void UpdateScrollMedalViewX(Vector2 vector)
        {
            if (Globals.PointerObjectName == "Horizontal")
                MedalView.horizontalNormalizedPosition = vector.x;
        }

        public void UpdateScrollMedalViewY(Vector2 vector)
        {
            if (Globals.PointerObjectName == "Vertical")
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
            UIMovement.ResetViewWindow();

            MedalContent.GetComponent<RectTransform>().offsetMax = InitialMedalContent.GetComponent<RectTransform>().offsetMax;
            MedalContent.GetComponent<RectTransform>().offsetMin = InitialMedalContent.GetComponent<RectTransform>().offsetMin;
            //MedalContent.position = InitialMedalContent.position;
            GraphElementsX.position = InitialGraphContent.position;
            GraphElementsY.position = InitialGraphContent.position;

            MedalCycleLogic.Instance.StopCycleMedals();
            Globals.CycleMedals.Clear();

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
    }
}
