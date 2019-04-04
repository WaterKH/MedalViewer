﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace MedalViewer.Medal
{
    public class MedalLogicManager : MonoBehaviour
    {
        public Loading Loading;

        private bool cyclesOn = true;
        private readonly string url = "https://medalviewer.blob.core.windows.net/thumbnails/";
        private readonly string urlHighQuality = "https://medalviewer.blob.core.windows.net/images/";

        public Dictionary<int, Dictionary<double, GameObject>> GenerateMedals(List<GameObject> YParents, List<GameObject> XParents, Transform MedalContentHolder)
        {
            var medals = new Dictionary<int, Dictionary<double, GameObject>>();

            foreach (var kv in Globals.Medals.OrderBy(x => x.Value.GuiltMultiplierLow))
            {
                var medal = kv.Value;

                var medalGameObject = this.CreateMedal(medal);

                if (!medals.ContainsKey(medal.Tier))
                    medals.Add(medal.Tier, new Dictionary<double, GameObject>());

                var multiplier = medal.GuiltMultiplierHigh != 0.0f ? medal.GuiltMultiplierHigh : 
                                 medal.GuiltMultiplierLow != 0.0f ? medal.GuiltMultiplierLow : 
                                 medal.MaxMultiplierHigh != 0.0f ? medal.MaxMultiplierHigh :
                                 medal.MaxMultiplierLow != 0.0f ? medal.MaxMultiplierLow :
                                 medal.BaseMultiplierHigh != 0.0f ? medal.BaseMultiplierHigh :
                                 medal.BaseMultiplierLow;

                if (!medals[medal.Tier].ContainsKey(multiplier))
                {
                    var guiltIndex = (int)multiplier < 0 ? 0 : (int)multiplier;

                    GameObject tempObject = null;
                    if(cyclesOn)
                        tempObject = Instantiate(Resources.Load("MedalCycleDisplay") as GameObject);
                    else
                        tempObject = Instantiate(Resources.Load("MedalDisplay") as GameObject);

                    tempObject.name = multiplier.ToString("0.00");
                    //print(tempObject.name);
                    tempObject.transform.position = new Vector3(XParents.First(x => x.name == medal.Tier.ToString()).transform.position.x, YParents.First(x => x.name == guiltIndex.ToString()).transform.position.y);
                    
                    tempObject.transform.SetParent(MedalContentHolder, false);

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
            for (int i = MedalContentHolder.childCount - 1; i >= 0; i--)
            {
                Transform child = MedalContentHolder.GetChild(i);
                children.Add(child);
                child.SetParent(null);
            }
            children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });
            foreach (Transform child in children)
            {
                child.SetParent(MedalContentHolder);
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

        // TODO Do a sub loading to generate the images and *THEN* display them
        public List<GameObject> GenerateSearchMedals(Transform MedalContentHolder)
        {
            var medals = new List<GameObject>();

            foreach (var kv in Globals.SearchMedals)
            {
                var medal = kv.Value;

                var medalGameObject = this.CreateMedal(medal, true);

                medalGameObject.transform.SetParent(MedalContentHolder, false);

                medalGameObject.GetComponent<CanvasGroup>().SetCanvasGroupActive();

                medals.Add(medalGameObject);
            }

            //foreach (var kv in medals)
            //{
            //        if (cyclesOn)
            //            this.UpdateMedalCycleContent(kv2.Value.GetComponentsInChildren<RectTransform>().First(x => x.name == "SubContent"));
            //        //this.UpdateMedalHolderContent(kv2.Value.GetComponentsInChildren<RectTransform>().First(x => x.name == "Content"));
            //        else
            //            this.UpdateMedalHolderContent(kv2.Value.GetComponentsInChildren<RectTransform>().First(x => x.name == "Content"));
            //}
            
            Loading.FinishLoading();

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
            if (!highQuality)
                fileName = fileName.Replace(".png", "_tn.png");

            if (fileName == "NULL")
            {
                print(prevImg);
                fileName = prevImg;
            }

            var path = "";
            if (!highQuality)
                path = url + fileName;
            else
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
            var updateX = (children.Count + 1) * gridLayout.cellSize.x + children.Count * gridLayout.spacing.x;

            content.sizeDelta = new Vector2(updateX, content.sizeDelta.y);

            parentGridLayout.enabled = true;
            children.ForEach(x => x.SetParent(content));

            if (children.Count <= 3)
            {
                content.parent.GetComponent<RectTransform>().sizeDelta = content.sizeDelta;
            }
        }
    }
}
