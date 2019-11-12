using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace MedalViewer.Medal
{
    public class MedalLogicManager : MonoBehaviour
    {
        public MedalManager MedalManager;
        //public LoadManager LoadManager;

        public List<Vector3> VerticalPositions = new List<Vector3>();
        public List<Vector3> HorizontalPositions = new List<Vector3>();

        private readonly float yOffset = 250;
        private bool cyclesOn = true;
        //private readonly string urlThumbnails = "https://medalviewer.blob.core.windows.net/thumbnails/";
        private readonly string urlHighQuality = "https://medalviewer.blob.core.windows.net/images/";

        public Medal GetLatestMedal()
        {
            return MedalManager.Medals.LastOrDefault(x => x.Value.Id < 20000).Value;
        }

        public double GetHighestMultiplier(Medal medal)
        {
            return medal.GuiltMultiplierHigh != 0.0f ? medal.GuiltMultiplierHigh :
                    medal.GuiltMultiplierLow != 0.0f ? medal.GuiltMultiplierLow :
                    medal.MaxMultiplierHigh  != 0.0f ? medal.MaxMultiplierHigh * medal.TierConversion[medal.Tier] :
                    medal.MaxMultiplierLow   != 0.0f ? medal.MaxMultiplierLow * medal.TierConversion[medal.Tier] :
                    medal.BaseMultiplierHigh != 0.0f ? medal.BaseMultiplierHigh * medal.TierConversion[medal.Tier] :
                    medal.BaseMultiplierLow * medal.TierConversion[medal.Tier];
        }

        public int GetHighestCalculatedStrengthRange()
        {
            var medal = MedalManager.Medals.OrderByDescending(x => x.Value.BaseAttack * x.Value.MaxMultiplierLow).FirstOrDefault().Value;
            var attack = medal.BaseAttack != 0 ? medal.BaseAttack : medal.MaxAttack;

            var result = Mathf.RoundToInt(attack * (float)medal.MaxMultiplierLow);

            return result;
        }

        public int GetLowestCalculatedStrengthRange()
        {
            var medal = MedalManager.Medals.OrderBy(x => x.Value.BaseAttack * x.Value.MaxMultiplierLow).FirstOrDefault().Value;
            print(medal.Name);
            var attack = medal.BaseAttack != 0 ? medal.BaseAttack : medal.MaxAttack;

            var result = Mathf.RoundToInt(attack * (float) medal.MaxMultiplierLow);

            if (result == 0)
                return result;
            else
                return result - 1;
        }

        public void SetRange(MedalFilterManager filters)
        {
            var highRange = MedalManager.Medals.Values.OrderByDescending(x => x.GuiltMultiplierHigh).FirstOrDefault()?.GuiltMultiplierHigh;
            var highRangeAlt = MedalManager.Medals.Values.OrderByDescending(x => x.GuiltMultiplierLow).FirstOrDefault()?.GuiltMultiplierLow;
            if (highRange < highRangeAlt && highRangeAlt < 1000)
            {
                highRange = highRangeAlt;
            }

            if (highRangeAlt.HasValue)
            {
                int value = (int) highRange.Value;
                filters.HighRange = value + 1;
                filters.LowRange = (value - 50) > 0 ? value - 50 : 0;
            }
        }

        public void SetLatestTiers(MedalFilterManager filters)
        {
            var highestTier = MedalManager.Medals.Values.OrderByDescending(x => x.Tier).FirstOrDefault()?.Tier;

            filters.Tiers = highestTier.HasValue ? new List<int> {highestTier.Value - 3, highestTier.Value - 2, highestTier.Value - 1, highestTier.Value } : new List<int> { 7, 8, 9, 10 };
        }

        public Dictionary<int, Dictionary<double, GameObject>> GenerateMedals(List<GameObject> yParents, List<GameObject> xParents, Transform medalContentHolder, GraphOptions currentGraphOptions/*, MedalFilterManager filters*/)
        {
            var medals = new Dictionary<int, Dictionary<double, GameObject>>();
            //var queryMedals = MedalManager.Medals.Where(x => filters.Tiers.Contains(x.Value.Tier) && x.Value.GuiltMultiplierHigh <= filters.HighRange && x.Value.GuiltMultiplierLow <= filters.HighRange
            //                                                                                      && x.Value.GuiltMultiplierLow >= filters.LowRange && x.Value.GuiltMultiplierHigh >= filters.LowRange);

            foreach (var kv in MedalManager.Medals.OrderBy(x => x.Value.GuiltMultiplierLow))
            {
                var medal = kv.Value;

                var medalGameObject = this.CreateMedal(medal);

                if (!medals.ContainsKey(medal.Tier))
                    medals.Add(medal.Tier, new Dictionary<double, GameObject>());

                var multiplier = this.GetHighestMultiplier(medal);
                var guiltIndex = (int)multiplier < 0 ? 0 : (int)multiplier;

                if (currentGraphOptions == GraphOptions.CalculatedStrength)
                    multiplier *= medal.MaxAttack > medal.BaseAttack ? medal.MaxAttack : medal.BaseAttack;

                if (!medals[medal.Tier].ContainsKey(multiplier))
                {

                    GameObject tempObject = null;
                    if(cyclesOn)
                        tempObject = Instantiate(Resources.Load("MedalCycleDisplay") as GameObject);
                    else
                        tempObject = Instantiate(Resources.Load("MedalDisplay") as GameObject);

                    tempObject.name = multiplier.ToString("0.00");
                    print(medal.Name +  " " + medal.Tier + " " + guiltIndex);
                    tempObject.transform.position = new Vector3(xParents.First(x => x.name == medal.Tier.ToString()).transform.position.x, 
                                                                yParents.First(x => x.name == guiltIndex.ToString()).transform.position.y);
                    
                    tempObject.transform.SetParent(medalContentHolder, false);

                    medals[medal.Tier].Add(multiplier, tempObject);
                }

                if (cyclesOn)
                {
                    medalGameObject.transform.SetParent(medals[medal.Tier][multiplier].GetComponentsInChildren<RectTransform>().First(x => x.name == "SubContent"), false);
                }
                else
                {
                    medalGameObject.transform.SetParent(medals[medal.Tier][multiplier].GetComponentsInChildren<RectTransform>().First(x => x.name == "Content"), true);
                }

                medalGameObject.GetComponent<CanvasGroup>().SetCanvasGroupActive();
            }

            #region Sort Base On Name

            List<Transform> children = new List<Transform>();
            for (int i = medalContentHolder.childCount - 1; i >= 0; i--)
            {
                Transform child = medalContentHolder.GetChild(i);
                children.Add(child);
                child.SetParent(null);
            }
            children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });
            foreach (Transform child in children)
            {
                child.SetParent(medalContentHolder);
            }

            #endregion

            foreach (var kv in medals)
            {
                foreach (var kv2 in kv.Value)
                {
                    if (cyclesOn)
                        this.UpdateMedalCycleContent(kv2.Value.GetComponentsInChildren<RectTransform>().First(x => x.name == "SubContent"));
                        //this.UpdateMedalHolderContent(kv2.Value.GetComponentsInChildren<RectTransform>().First(x => x.name == "Content"));
                    else
                        this.UpdateMedalHolderContent(kv2.Value.GetComponentsInChildren<RectTransform>().First(x => x.name == "Content"));
                }
            }

            return medals;
        }

        public GameObject CreateMedal(Medal medal, bool highQuality = false)
        {
            var medalGameObject = Instantiate(Resources.Load("Medal") as GameObject);
            medalGameObject.GetComponent<CanvasGroup>().SetCanvasGroupInactive();

            medalGameObject.name = medal.Name;
            
            SetMedalImage(medal, medalGameObject, medal.ImageURL, highQuality);

            medalGameObject.GetComponent<MedalDisplay>().MapVariables(medal);
            
            return medalGameObject;
        }
        
        private void SetMedalImage(Medal medalItem, GameObject medalObject, string prevImg, bool highQuality = false)
        {
            var fileName = medalItem.ImageURL;
            // TODO Add thumbnails back?
            //if (!highQuality)
            //    fileName = fileName.Replace(".png", "_tn.png");

            if (fileName == "NULL")
            {
                print(prevImg);
                fileName = prevImg;
            }

            var path = "";
            //if (!highQuality)
            //    path = url + fileName;
            //else
                path = urlHighQuality + fileName;

            StartCoroutine(LoadImage(path, medalObject));
        }

        private IEnumerator LoadImage(string imageUrl, GameObject medalObject)
        {
            UnityWebRequest image = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return image.SendWebRequest();
            if (image.isNetworkError || image.isHttpError)
                Debug.Log(imageUrl + " " + image.error);
            else
                if(medalObject != null)
                    medalObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)image.downloadHandler).texture;
        }

        public void UpdateMedalHolderContent(RectTransform content)
        {
            var children = content.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Content").ToList();
            var gridLayout = content.GetComponent<GridLayoutGroup>();
            var parentGridLayout = content.GetComponent<GridLayoutGroup>();

            if (children.Count == 0) return;

            children.ForEach(x => x.SetParent(content.parent));

            parentGridLayout.enabled = false;
            var updateX = (children.Count + 1) * gridLayout.cellSize.x + children.Count * gridLayout.spacing.x;

            content.sizeDelta = new Vector2(updateX, content.sizeDelta.y);

            parentGridLayout.enabled = true;
            children.ForEach(x => x.SetParent(content));

            if (children.Count <= 3)
            {
                content.parent.GetComponent<RectTransform>().sizeDelta = content.sizeDelta;
            }
        }

        public void UpdateMedalCycleContent(RectTransform content)
        {
            var children = content.GetComponentsInChildren<RectTransform>().Where(x => x.name != "SubContent").ToList();
            var gridLayout = content.GetComponent<GridLayoutGroup>();
            var parentGridLayout = content.GetComponent<GridLayoutGroup>();

            if (children.Count == 0) return;

            children.ForEach(x => x.SetParent(content.parent));

            parentGridLayout.enabled = false;
            var updateY = (children.Count / 2) * (gridLayout.cellSize.y + gridLayout.spacing.y);

            content.sizeDelta = new Vector2(content.sizeDelta.x, updateY);

            parentGridLayout.enabled = true;
            children.ForEach(x => x.SetParent(content));

            if (children.Count <= 3)
            {
                content.parent.GetComponent<RectTransform>().sizeDelta = content.sizeDelta;
            }
        }

        public void PlaceMedals(List<GameObject> Rows, List<GameObject> Columns, Dictionary<int, Dictionary<double, GameObject>> Medals)
        {
            foreach (var tier in Medals)
            {
                foreach (var multiplier in tier.Value)
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

        public List<GameObject> PlaceYRows(int lowRange, int highRange, Transform startPositionY, Transform parentY, float offset)
        {
            var rowsY = new List<GameObject>();
            float tempYOffset = 300;
            var maxY = 0.0f;

            for (int i = lowRange; i <= highRange; ++i)
            {
                var pos = new Vector2(startPositionY.position.x, startPositionY.position.y + tempYOffset);
                var row = Instantiate(Resources.Load("NumberY") as GameObject, pos, Quaternion.identity, parentY.parent);

                row.name = i.ToString();
                row.GetComponent<Text>().text = i.ToString();

                tempYOffset += offset;
                rowsY.Add(row);

                maxY = row.GetComponent<RectTransform>().offsetMax.y;
            }

            parentY.GetComponent<RectTransform>().offsetMax = new Vector2(parentY.GetComponent<RectTransform>().offsetMax.x, maxY - 500);

            parentY.parent.GetComponentsInChildren<RectTransform>().Where(x => x.name != "Viewport" && x.name != "Content" && x.name != "InitialYPosition").ToList().ForEach(x => {
                x.transform.SetParent(parentY);
            });

            return rowsY;
        }

        public List<GameObject> PlaceXColumns(List<int> tiers, Transform StartPositionX, Transform ParentX, int xOffset)
        {
            var ColumnsX = new List<GameObject>();
            int tempXOffset = 150;
            var maxX = 0.0f;

            foreach (var tier in tiers)
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
