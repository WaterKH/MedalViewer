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
        //public Dictionary<int, RectTransform> vert_children = new Dictionary<int, RectTransform>();
        //public Dictionary<int, RectTransform> hori_children = new Dictionary<int, RectTransform>();

        public List<Vector3> VerticalPositions = new List<Vector3>();
        public List<Vector3> HorizontalPositions = new List<Vector3>();

        private float yOffset = 250;

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
    }
}
