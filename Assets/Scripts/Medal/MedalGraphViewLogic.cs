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

        public Transform MedalContent;
        public Transform InitialMedalContent;

        public ScrollRect MedalView;
        public ScrollRect Vertical;
        public ScrollRect Horizontal;
        
        public Dictionary<int, Dictionary<double, GameObject>> MedalGameObjects = new Dictionary<int, Dictionary<double, GameObject>>();

        private readonly int yOffset = 250;
        private readonly int xOffset = 250;

        public List<GameObject> RowsY = new List<GameObject>();
        public List<GameObject> ColumnsX = new List<GameObject>();
        
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

            PopulateMedals();
            StartCoroutine(PopulateCycleMedals());

            Loading.FinishLoading();
        }

        public void UpdateYRows(int changeValue = 250)
        {
            RowsY = MedalPositionLogic.PlaceYRows(StartPositionY, ParentY, changeValue);
        }

        public void PopulateMedals(bool generate = true)
        {
            //UIMovement.UpdateViewWindow(1080);
            if(generate)
                MedalGameObjects = MedalLogicManager.GenerateMedals(RowsY, ColumnsX, MedalContent);

            MedalPositionLogic.PlaceMedals(RowsY, ColumnsX, MedalGameObjects);

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

        public void ResetGraph()
        {
            MedalContent.GetComponent<RectTransform>().offsetMax = InitialMedalContent.GetComponent<RectTransform>().offsetMax;
            MedalContent.position = InitialMedalContent.position;

            Globals.CycleMedals.Clear();
            MedalCycleLogic.Instance.StopCycleMedals();

            foreach (var row in RowsY)
            {
                Destroy(row);
            }

            RowsY.Clear();

            foreach(var column in ColumnsX)
            {
                Destroy(column);
            }

            ColumnsX.Clear();

            foreach(var kv in MedalGameObjects)
            {
                foreach(var kv2 in kv.Value)
                {
                    Destroy(kv2.Value);
                }

                kv.Value.Clear();
            }

            MedalGameObjects.Clear();
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
    }
}
