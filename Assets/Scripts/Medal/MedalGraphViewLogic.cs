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
        private readonly int xOffset = 500;

        private List<GameObject> RowsY = new List<GameObject>();
        private List<GameObject> ColumnsX = new List<GameObject>();
        
        public IEnumerator Display()
        {
            while(Loading.IsLoading)
            {
                yield return null;
            }
            
            Loading.StartLoading();

            this.ResetGraph();

            #region Y Row Creation
            
            int tempYOffset = 400;
            
            ParentY.GetComponent<RectTransform>().offsetMax = new Vector2(ParentY.GetComponent<RectTransform>().offsetMax.x, yOffset * (MedalFilter.HighRange - MedalFilter.LowRange + 2));
            
            for (int i = MedalFilter.LowRange; i <= MedalFilter.HighRange; ++i)
            {
                var pos = new Vector2(StartPositionY.position.x, StartPositionY.position.y + tempYOffset);
                var row = Instantiate(Resources.Load("NumberY") as GameObject, pos, Quaternion.identity, ParentY.parent);

                row.name = i.ToString();
                row.GetComponent<Text>().text = i.ToString();
                
                tempYOffset += yOffset;
                RowsY.Add(row);
            }
            
            ParentY.parent.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Viewport" || x.name != "Content" || x.name != "InitialYPosition").ToList().ForEach(x => x.transform.SetParent(ParentY));

            #endregion

            #region X Column Creation

            int tempXOffset = 300;

            ParentX.GetComponent<RectTransform>().offsetMax = new Vector2(xOffset * (MedalFilter.Tiers.Count - 3), ParentX.GetComponent<RectTransform>().offsetMax.y);

            foreach (var tier in MedalFilter.Tiers)
            {
                var pos = new Vector2(StartPositionX.position.x + tempXOffset, StartPositionX.position.y);
                var column = Instantiate(Resources.Load("NumberX") as GameObject, pos, Quaternion.identity, ParentX.parent);

                column.name = tier.ToString();
                column.GetComponent<Text>().text = tier.ToString();
                
                tempXOffset += xOffset;
                ColumnsX.Add(column);
            }

            ParentX.parent.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Viewport" || x.name != "Content" || x.name != "InitialXPosition").ToList().ForEach(x => x.transform.SetParent(ParentX));

            #endregion

            MedalContent.GetComponent<RectTransform>().offsetMax = new Vector2(xOffset * (MedalFilter.Tiers.Count - 3), yOffset * (MedalFilter.HighRange - MedalFilter.LowRange + 2));

            PopulateMedals();
            PopulateCycleMedals();
            
            Loading.FinishLoading();
        }

        public void PopulateMedals()
        {
            MedalGameObjects = MedalLogicManager.GenerateMedals(RowsY, ColumnsX, MedalContent);
            MedalPositionLogic.PlaceMedals(RowsY, ColumnsX, MedalGameObjects);
        }
        
        public void PopulateCycleMedals()
        {
            foreach(var tier in MedalGameObjects)
            {
                foreach(var mult in tier.Value)
                {
                    var medal = mult.Value;

                    Globals.CycleMedals.Add(medal, 0);// medal.transform.childCount);
                }
            }

            MedalCycleLogic.Instance.StartCycleMedals();
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
