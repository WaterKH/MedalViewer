using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace MedalViewer.Medal
{
    public class MedalGraphViewLogic : MonoBehaviour
    {
        public MedalFilter MedalFilter;
        public Transform StartPositionY;
        public Transform StartPositionX;
        public Transform ParentY;
        public Transform ParentX;

        private readonly int yOffset = 250;
        private readonly int xOffset = 350;

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

            //var parentY = StartPositionY.parent;
            int tempYOffset = yOffset;

            //StartPositionY.SetParent(parentY.parent, true);
            //ParentY.GetComponent<RectTransform>().sizeDelta = new Vector2(ParentY.GetComponent<RectTransform>().sizeDelta.x, yOffset * (MedalFilter.HighRange - MedalFilter.LowRange));
            ParentY.GetComponent<RectTransform>().offsetMax = new Vector2(ParentY.GetComponent<RectTransform>().offsetMax.x, yOffset * (MedalFilter.HighRange - MedalFilter.LowRange + 2));
            //StartPositionY.SetParent(parentY, false);

            for (int i = MedalFilter.LowRange; i <= MedalFilter.HighRange; ++i)
            {
                var pos = new Vector2(StartPositionY.position.x, StartPositionY.position.y + tempYOffset);
                var row = Instantiate(Resources.Load("NumberY") as GameObject, pos, Quaternion.identity, ParentY.parent);

                row.name = i.ToString();
                row.GetComponent<Text>().text = i.ToString();
                //row.transform.position = new Vector2(StartPositionY.position.x, StartPositionY.position.y + tempYOffset);
                //row.transform.SetParent(StartPositionY.parent, true);

                tempYOffset += yOffset;
                RowsY.Add(row);
            }
            
            ParentY.parent.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Viewport" || x.name != "Content" || x.name != "InitialYPosition").ToList().ForEach(x => x.transform.SetParent(ParentY));

            #endregion

            #region X Column Creation

            int tempXOffset = 150;

            ParentX.GetComponent<RectTransform>().offsetMax = new Vector2(xOffset * (MedalFilter.Tiers.Count - 3), ParentX.GetComponent<RectTransform>().offsetMax.y);

            foreach (var tier in MedalFilter.Tiers)
            {
                var pos = new Vector2(StartPositionX.position.x + tempXOffset, StartPositionX.position.y);
                var column = Instantiate(Resources.Load("NumberX") as GameObject, pos, Quaternion.identity, ParentX.parent);

                column.name = tier.ToString();
                column.GetComponent<Text>().text = tier.ToString();
                //column.transform.position = new Vector2(StartPositionX.position.x, StartPositionX.position.y + tempXOffset);
                //column.transform.SetParent(StartPositionX.parent, true);
                
                tempXOffset += xOffset;
                ColumnsX.Add(column);
            }

            ParentX.parent.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Viewport" || x.name != "Content" || x.name != "InitialXPosition").ToList().ForEach(x => x.transform.SetParent(ParentX));

            #endregion
        }

        public void Reset()
        {
            foreach(var row in RowsY)
            {
                Destroy(row);
            }

            foreach(var column in ColumnsX)
            {
                Destroy(column);
            }
        }
    }
}
