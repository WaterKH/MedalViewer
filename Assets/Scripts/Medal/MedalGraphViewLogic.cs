using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MedalViewer.Medal
{
    public class MedalGraphViewLogic : MonoBehaviour
    {
        public MedalFilter MedalFilter;
        public Transform StartPositionY;
        public Transform StartPositionX;

        private readonly int yOffset = 250;
        private readonly int xOffset = 50;

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

            int tempYOffset = 0;
            for(int i = MedalFilter.LowRange; i <= MedalFilter.HighRange; ++i)
            {
                var pos = new Vector2(StartPositionY.position.x, StartPositionY.position.y + tempYOffset);
                var row = Instantiate(Resources.Load("NumberY") as GameObject, pos, Quaternion.identity, StartPositionY.parent);

                row.name = i.ToString();
                row.GetComponent<Text>().text = i.ToString();
                //row.transform.position = new Vector2(StartPositionY.position.x, StartPositionY.position.y + tempYOffset);
                //row.transform.SetParent(StartPositionY.parent, true);

                tempYOffset += yOffset;
                RowsY.Add(row);
            }

            #endregion
            
            #region X Column Creation

            int tempXOffset = 0;
            foreach(var tier in new int[] { 4, 5, 6, 7 })//MedalFilter.Tiers)
            {
                var pos = new Vector2(StartPositionX.position.x + tempXOffset, StartPositionX.position.y);
                var column = Instantiate(Resources.Load("NumberX") as GameObject, pos, Quaternion.identity, StartPositionX.parent);

                column.name = tier.ToString();
                column.GetComponent<Text>().text = tier.ToString();
                //column.transform.position = new Vector2(StartPositionX.position.x, StartPositionX.position.y + tempXOffset);
                //column.transform.SetParent(StartPositionX.parent, true);
                
                tempXOffset += xOffset;
                ColumnsX.Add(column);
            }

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
