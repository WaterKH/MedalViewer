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

        public MedalLogicManager MedalLogicManager;

        public GameObject main_parent;
        public GameObject VerticalParent;
        public GameObject HorizontalParent;
        public GameObject VerticalTempParent;
        public GameObject HorizontalTempParent;

        public GameObject Placeholder;
        public RectTransform Content;
        public GameObject ContentTempParent;
        public GameObject[] ContentChildren;

        public Dictionary<int, RectTransform> vert_children = new Dictionary<int, RectTransform>();
        public Dictionary<int, RectTransform> hori_children = new Dictionary<int, RectTransform>();

        public List<Vector3> VerticalPositions = new List<Vector3>();
        public List<Vector3> HorizontalPositions = new List<Vector3>();

        private float yValueConstant = 500;
        private int xValueConstant = 2500;
        private Vector2 initialSizeDelta;
        private Vector3 initialPosition;
        private Parser parser = new Parser();

        private Regex OnlyDigits = new Regex(@"^\d+\.");

        public void Awake()
        {
            SetInitialPositions();

            initialSizeDelta = new Vector2(Content.sizeDelta.x, Content.sizeDelta.y);
            initialPosition = new Vector3(Content.position.x, Content.position.y, Content.position.z);
        }

        public void AssignMedals(GameObject medalGameObject)
        {
            var medal = medalGameObject.GetComponent<MedalDisplay>();
            var guiltFloat = parser.ParseGuilt(medal.GuiltMultiplier);

            medalGameObject.transform.SetParent(MedalLogicManager.AllMedalDisplayObjects[medal.Tier][guiltFloat].GetComponentsInChildren<Transform>().First(x => x.name == "Content").transform);
        }

        public void AssignMedalHolders(GameObject medalHolder)
        {
            var guiltFloat = float.Parse(medalHolder.name);
            var guiltIndex = (int)guiltFloat - 1;

            medalHolder.transform.SetParent(MedalLogicManager.parents[guiltIndex].transform);
        }

        public void SetInitialPositions()
        {
            foreach (var child in VerticalTempParent.GetComponentsInChildren<RectTransform>())
            {
                if (child.name != "Vertical_TEMP (Y)")
                {
                    VerticalPositions.Add(child.position);
                }
            }

            foreach (var child in HorizontalTempParent.GetComponentsInChildren<RectTransform>())
            {
                if (child.name != "Horizontal_TEMP (X)")
                {
                    HorizontalPositions.Add(child.position);
                }
            }
        }

        public void Initialize()
        {
            ResetContent();

            var tempChildren = new List<RectTransform>();

            // Setting up the Y rows
            foreach (var child in VerticalTempParent.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Vertical_TEMP (Y)"))
            {

                if (OnlyDigits.Match(child.name).Success)
                {
                    AssignMedalHolders(child.gameObject);
                }
                else if (child.name.Length == 2 || child.name.Length == 1)
                {
                    //if (int.Parse(child.name) >= Globals.MultiplierFilter.Min.value &&
                    //    int.Parse(child.name) <= Mathf.Abs(Globals.MultiplierFilter.Max.value))
                    //{
                        tempChildren.Add(child);
                    //}
                }
                else if (child.name != "Content" && child.name != "Scrollbar Horizontal" && child.name != "Sliding Area" && child.name != "Handle") // TODO Maybe just add a tag to medals: Medal - And just check for that
                {
                    AssignMedals(child.gameObject);
                }
            }

            foreach (var child in tempChildren.OrderByDescending(x => int.Parse(x.name)))
            {
                child.SetParent(VerticalParent.transform);
            }

            // Setting up the X columns
            HorizontalTempParent.GetComponentsInChildren<RectTransform>()
                .Where(x => x.name != "Horizontal_TEMP (X)") //&&
                            //!Globals.TierFilter.ToggleChildren[int.Parse(x.name) - 1].isOn)
                .OrderBy(x => int.Parse(x.name))
                .ToList()
                .ForEach(x => x.SetParent(HorizontalParent.transform));

            var yValue = 0.0f;
            var xValue = 0.0f;
            var tempHolder = VerticalParent.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Vertical (Y)").ToArray();

            for (int i = tempHolder.Length - 1; i >= 0; --i)
            {
                if (tempHolder[i].name.Length != 2 && tempHolder[i].name.Length != 1) continue;

                vert_children.Add(int.Parse(tempHolder[i].name), tempHolder[i]);
                tempHolder[i].position = new Vector2(0.0f, yValue + yValueConstant);
                print(tempHolder[i].name);
                tempHolder[i].GetComponent<Text>().enabled = true;
                yValue += yValueConstant;
            }

            foreach (var x in HorizontalParent.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Horizontal (X)"))
            {
                hori_children.Add(int.Parse(x.name), x);
                x.position = new Vector2(xValue + xValueConstant, 0.0f);
                x.GetComponent<Text>().enabled = true;
                xValue += xValueConstant;
            }

            UpdateContent();
        }

        public void SetMedalHolderPosition(GameObject medalHolder, float guiltFloat, int tier)
        {
            if (tier - 1 < 0)
            {
                medalHolder.SetActive(false);
                return;
            }

            var xIndex = tier;
            var y = guiltFloat;

            var yIndex = (int)y;
            float yAfterDecimal = y - yIndex;

            // TODO Do we not already check for these? Why are they here?
            //if (Globals.TierFilter.ToggleChildren[xIndex - 1].isOn) return;

            //if (yIndex <= Mathf.Abs(Globals.MultiplierFilter.Max.value) && yIndex >= Globals.MultiplierFilter.Min.value)
            //{
                if (!vert_children.ContainsKey(yIndex) || !hori_children.ContainsKey(xIndex))
                    return;

                var yTransform = vert_children[yIndex].position;
                var xTransform = hori_children[xIndex].position;

                var nextY = yValueConstant;

                if (yIndex + 1 < vert_children.Count)
                {
                    nextY = vert_children[yIndex + 1].position.y - yTransform.y;
                }

                var newTransform = new Vector3(xTransform.x, yTransform.y + (nextY * yAfterDecimal));

                medalHolder.GetComponent<RectTransform>().position = newTransform;
                medalHolder.SetActive(true);
            //}
            //else
            //{
            //    medalHolder.SetActive(false);
            //}
        }

        public void ResetContent()
        {
            Content.position = initialPosition;
            Content.sizeDelta = initialSizeDelta;

            vert_children.Clear();
            hori_children.Clear();
            foreach (var child in VerticalParent.GetComponentsInChildren<Transform>().OrderByDescending(x => x.name))
            {
                if (child.name != "Vertical (Y)" && child.name != "Content" && child.name != "Scrollbar Horizontal" && child.name != "Sliding Area" && child.name != "Handle") // TODO Maybe just add a tag to medals: Medal - And just check for that
                {
                    child.SetParent(VerticalTempParent.transform);
                    if (child.name.Length != 2 || child.name.Length != 1) continue;
                    child.position = VerticalPositions[int.Parse(child.name) - 1];
                }
            }
            foreach (var child in HorizontalParent.GetComponentsInChildren<Transform>().OrderByDescending(x => x.name))
            {
                if (child.name != "Horizontal (X)")
                {
                    child.SetParent(HorizontalTempParent.transform);
                    child.position = HorizontalPositions[int.Parse(child.name) - 1];
                }
            }
        }

        // http://answers.unity.com/answers/1302142/view.html
        public void UpdateContent()
        {
            float yMin = 0.0f;
            float yMax = 0.0f;
            float xMin = 0.0f;
            float xMax = 0.0f;

            foreach (var kv in vert_children)
            {
                var child = kv.Value;

                if (child.name != "Vertical (Y)")
                {
                    yMin = Mathf.Min(yMin, child.offsetMin.y);
                    yMax = Mathf.Max(yMax, child.offsetMax.y);

                    xMin = Mathf.Min(xMin, child.offsetMin.x);
                }
            }
            foreach (var kv in hori_children)
            {
                var child = kv.Value;

                if (child.name != "Horizontal (X)")
                {
                    xMax = Mathf.Max(xMax, child.offsetMax.x);
                }
            }

            float finalSizeY = yMax - yMin;
            float finalSizeX = xMax - xMin;

            Content.sizeDelta = new Vector2(finalSizeX, finalSizeY);
        }

        // TODO Call this method when you are ready to shoot the medals into different objects
        public void UpdateMedalHolderPosition(GameObject medalHolder, Transform currRow, Transform upperLowerRow)
        {
            var multiplier = float.Parse(medalHolder.name);
            var lower = currRow.position.y;
            var upper = upperLowerRow.position.y;

            var percentage = multiplier - (int)multiplier;
            var position = (upper - lower) * percentage;

            medalHolder.GetComponent<RectTransform>().position = new Vector3(medalHolder.transform.position.x, lower + position);
        }
    }
}
