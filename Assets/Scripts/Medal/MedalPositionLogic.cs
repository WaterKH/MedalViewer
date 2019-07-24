using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace MedalViewer.Medal
{
    public class MedalPositionLogic : MonoBehaviour
    {
        public MedalFilter MedalFilter;
        //public Dictionary<int, RectTransform> vert_children = new Dictionary<int, RectTransform>();
        //public Dictionary<int, RectTransform> hori_children = new Dictionary<int, RectTransform>();

        public List<Vector3> VerticalPositions = new List<Vector3>();
        public List<Vector3> HorizontalPositions = new List<Vector3>();

        private readonly float yOffset = 250;

        public void PlaceMedals(List<GameObject> Rows, List<GameObject> Columns, Dictionary<int, Dictionary<double, GameObject>> Medals)
        {
            foreach(var tier in Medals)
            {
                foreach(var multiplier in tier.Value)
                {
                    var medal = multiplier.Value;
                    
                    var yIndex = (int)multiplier.Key;
                    double yAfterDecimal = multiplier.Key - yIndex;

                    var yTransform = Rows.FirstOrDefault(x => x.name == yIndex.ToString()).transform.position;
                    var xPosition = Columns.FirstOrDefault(x => x.name == tier.Key.ToString()).transform.position;

                    var nextY = yOffset;
                    
                    // TODO FIX THIS
                    //if (yIndex + 1 < Rows.Count)
                    //{
                    //    nextY = RowsyIndex + 1].position.y - yTransform.y;
                    //}
                    
                    medal.transform.position = new Vector2(xPosition.x, yTransform.y + (nextY * (float)yAfterDecimal));
                }
            }
        }

        public List<GameObject> PlaceYRows(Transform StartPositionY, Transform ParentY, float yOffset)
        {
            var RowsY = new List<GameObject>();
            float tempYOffset = 300;
            var maxY = 0.0f;

            for (int i = MedalFilter.LowRange; i <= MedalFilter.HighRange; ++i)
            {
                var pos = new Vector2(StartPositionY.position.x, StartPositionY.position.y + tempYOffset);
                var row = Instantiate(Resources.Load("NumberY") as GameObject, pos, Quaternion.identity, ParentY.parent);

                row.name = i.ToString();
                row.GetComponent<Text>().text = i.ToString();

                tempYOffset += yOffset;
                RowsY.Add(row);

                maxY = row.GetComponent<RectTransform>().offsetMax.y;
            }

            ParentY.GetComponent<RectTransform>().offsetMax = new Vector2(ParentY.GetComponent<RectTransform>().offsetMax.x, maxY - 500);

            ParentY.parent.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Viewport" && x.name != "Content" && x.name != "InitialYPosition").ToList().ForEach(x => {
                x.transform.SetParent(ParentY);
            });

            return RowsY;
        }

        public List<GameObject> PlaceXColumns(Transform StartPositionX, Transform ParentX, int xOffset)
        {
            var ColumnsX = new List<GameObject>();
            int tempXOffset = 150;
            var maxX = 0.0f;

            foreach (var tier in MedalFilter.Tiers)
            {
                var pos = new Vector2(StartPositionX.position.x + tempXOffset, StartPositionX.position.y);
                var column = Instantiate(Resources.Load("NumberX") as GameObject, pos, Quaternion.identity, ParentX.parent);

                column.name = tier.ToString();
                column.GetComponent<Text>().text = tier.ToString();

                tempXOffset += xOffset;
                ColumnsX.Add(column);

                maxX = column.GetComponent<RectTransform>().offsetMax.x;
            }

            ParentX.GetComponent<RectTransform>().offsetMax = new Vector2(maxX - 1500, ParentX.GetComponent<RectTransform>().offsetMax.y);

            ParentX.parent.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Viewport" && x.name != "Content" && x.name != "InitialXPosition").ToList().ForEach(x => {
                x.transform.SetParent(ParentX);
            });

            return ColumnsX;
        }
    }
}
