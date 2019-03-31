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

        public Transform StartPositionY;
        public Transform StartPositionX;
        public Transform ParentY;
        public Transform ParentX;

        public Transform MedalContent;

        public ScrollRect MedalView;
        public ScrollRect Vertical;
        public ScrollRect Horizontal;
        
        public Dictionary<int, Dictionary<double, GameObject>> MedalGameObjects;

        private readonly int yOffset = 250;
        private readonly int xOffset = 700;

        private List<GameObject> RowsY = new List<GameObject>();
        private List<GameObject> ColumnsX = new List<GameObject>();

        private void Start()
        {
            Display();
        }

        public void Display()
        {
            this.Reset();

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

            ParentX.GetComponent<RectTransform>().offsetMax = new Vector2(xOffset * (MedalFilter.Tiers.Count - 2), ParentX.GetComponent<RectTransform>().offsetMax.y);

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

            MedalContent.GetComponent<RectTransform>().offsetMax = new Vector2(xOffset * (MedalFilter.Tiers.Count - 2), yOffset * (MedalFilter.HighRange - MedalFilter.LowRange + 2));

            PopulateMedals();
            PopulateCycleMedals();
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

                    Globals.CycleMedals.Add(medal, medal.GetComponentsInChildren<Transform>().Where(x => x.name != medal.name).ToList().Count);
                }
            }

            MedalCycleLogic.Instance.StartCycleMedals();
        }

        public void Reset()
        {
            Globals.CycleMedals.Clear();

            foreach (var row in RowsY)
            {
                Destroy(row);
            }

            foreach(var column in ColumnsX)
            {
                Destroy(column);
            }
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
