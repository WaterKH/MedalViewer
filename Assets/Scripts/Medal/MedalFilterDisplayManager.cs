﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MedalViewer.Medal
{
    public class MedalFilterDisplayManager : MonoBehaviour
    {
        public MedalFilterManager MedalFilterManager = MedalFilterManager.Instance;
        public MedalManager MedalManager;
        //public MedalGraphViewManager MedalGraphViewManager;
        public SearchManager SearchManager;

        public CanvasGroup FilterMenu;

        public bool IsDisplayingFilters;

        #region Tier

        public Toggle Tier1;
        public Toggle Tier2;
        public Toggle Tier3;
        public Toggle Tier4;
        public Toggle Tier5;
        public Toggle Tier6;
        public Toggle Tier7;
        public Toggle Tier8;
        public Toggle Tier9;
        public Toggle Tier10;

        public CanvasGroup Tier1Panel;
        public CanvasGroup Tier2Panel;
        public CanvasGroup Tier3Panel;
        public CanvasGroup Tier4Panel;
        public CanvasGroup Tier5Panel;
        public CanvasGroup Tier6Panel;
        public CanvasGroup Tier7Panel;
        public CanvasGroup Tier8Panel;
        public CanvasGroup Tier9Panel;
        public CanvasGroup Tier10Panel;

        #endregion

        #region Range

        public Slider RangeMin;
        public Slider RangeMax;

        public Text RangeMinText;
        public Text RangeMaxText;

        #endregion

        #region Attributes

        public Toggle Power;
        public Toggle Speed;
        public Toggle Magic;
        public Toggle Upright;
        public Toggle Reversed;

        public CanvasGroup PowerPanel;
        public CanvasGroup SpeedPanel;
        public CanvasGroup MagicPanel;
        public CanvasGroup UprightPanel;
        public CanvasGroup ReversedPanel;

        #endregion

        #region Target

        public Toggle Single;
        public Toggle Random;
        public Toggle All;

        public CanvasGroup SinglePanel;
        public CanvasGroup RandomPanel;
        public CanvasGroup AllPanel;

        #endregion

        #region Types

        #endregion

        #region Stars

        public Toggle Star1;
        public Toggle Star2;
        public Toggle Star3;
        public Toggle Star4;
        public Toggle Star5;
        public Toggle Star6;
        public Toggle Star7;

        public CanvasGroup Star1Panel;
        public CanvasGroup Star2Panel;
        public CanvasGroup Star3Panel;
        public CanvasGroup Star4Panel;
        public CanvasGroup Star5Panel;
        public CanvasGroup Star6Panel;
        public CanvasGroup Star7Panel;

        #endregion


        private int currentMaxMultiplier = 75;

        private void Awake()
        {
            #region Tier AddListener

            Tier1.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Tier1Panel));
            Tier2.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Tier2Panel));
            Tier3.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Tier3Panel));
            Tier4.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Tier4Panel));
            Tier5.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Tier5Panel));
            Tier6.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Tier6Panel));
            Tier7.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Tier7Panel));
            Tier8.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Tier8Panel));
            Tier9.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Tier9Panel));
            Tier10.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Tier10Panel));

            #endregion

            #region AddListener

            RangeMax.onValueChanged.AddListener(value => UpdateRangeText(currentMaxMultiplier - (int)value, RangeMaxText));
            RangeMin.onValueChanged.AddListener(value => UpdateRangeText((int)value, RangeMinText));

            RangeMax.onValueChanged.AddListener(value => CheckHighRange(currentMaxMultiplier - (int)value));
            RangeMin.onValueChanged.AddListener(value => CheckLowRange((int)value));

            #endregion

            #region Attributes AddListener

            Power.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, PowerPanel));
            Speed.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, SpeedPanel));
            Magic.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, MagicPanel));
            Upright.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, UprightPanel));
            Reversed.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, ReversedPanel));

            #endregion

            #region Target AddListener

            Single.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, SinglePanel));
            Random.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, RandomPanel));
            All.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, AllPanel));

            #endregion

            #region Stars

            Star1.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Star1Panel));
            Star2.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Star2Panel));
            Star3.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Star3Panel));
            Star4.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Star4Panel));
            Star5.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Star5Panel));
            Star6.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Star6Panel));
            Star7.onValueChanged.AddListener(value => SetPanelCanvasGroup(value, Star7Panel));

            #endregion

            ResetFilters();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.C))
            {
                if (this.IsDisplayingFilters)
                {
                    HideFilterMenu();
                }
            }
        }

        public void UpdateRangeText(int value, Text rangeText)
        {
            rangeText.text = value.ToString();
        }

        public void CheckHighRange(int value)
        {
            print(value);
            if(value <= (int)RangeMin.value)
            {
                RangeMax.value = (currentMaxMultiplier - (int)RangeMin.value);
            }
        }

        public void CheckLowRange(int value)
        {
            var rangeMax = (currentMaxMultiplier - (int)RangeMax.value);
            if (value >= rangeMax)
            {
                RangeMin.value = rangeMax;
            }
        }

        public void SetPanelCanvasGroup(bool value, CanvasGroup panel)
        {
            panel.alpha = value == true ? 1f : 0f;
        }

        public void DisplayFilterMenu()
        {
            IsDisplayingFilters = true;
            SearchManager.HideSearch();

            FilterMenu.SetCanvasGroupActive();
        }

        public void HideFilterMenu()
        {
            IsDisplayingFilters = false;

            FilterMenu.SetCanvasGroupInactive();
        }

        public void DefaultFilters()
        {
            MedalFilterManager.DefaultFilters();
        }

        public void ApplyFilters()
        {
            AssignTiers();
            AssignRange();
            AssignAttributes();
            AssignTarget();
            //AssignTypes();
            AssignStars();

            MedalManager.HandleGetMedals(MedalFilterManager);
            MedalCycleLogic.Instance.loadInitial = true;

            HideFilterMenu();
        }

        #region Assign Filters

        public void AssignTiers()
        {
            MedalFilterManager.Tier1 = Tier1.isOn;
            MedalFilterManager.Tier2 = Tier2.isOn;
            MedalFilterManager.Tier3 = Tier3.isOn;
            MedalFilterManager.Tier4 = Tier4.isOn;
            MedalFilterManager.Tier5 = Tier5.isOn;
            MedalFilterManager.Tier6 = Tier6.isOn;
            MedalFilterManager.Tier7 = Tier7.isOn;
            MedalFilterManager.Tier8 = Tier8.isOn;
            MedalFilterManager.Tier9 = Tier9.isOn;
            MedalFilterManager.Tier10 = Tier10.isOn;
        }

        public void AssignRange()
        {
            MedalFilterManager.LowRange = (int)RangeMin.value;
            MedalFilterManager.HighRange = currentMaxMultiplier - (int)RangeMax.value; 
        }

        public void AssignAttributes()
        {
            MedalFilterManager.Power = Power.isOn;
            MedalFilterManager.Speed = Speed.isOn;
            MedalFilterManager.Magic = Magic.isOn;
            MedalFilterManager.Upright = Upright.isOn;
            MedalFilterManager.Reversed = Reversed.isOn;
        }

        public void AssignTarget()
        {
            MedalFilterManager.Single = Single.isOn;
            MedalFilterManager.Random = Random.isOn;
            MedalFilterManager.All = All.isOn;
        }

        public void AssignTypes()
        {
            // TODO
        }

        public void AssignStars()
        {
            MedalFilterManager.OneStar = Star1.isOn;
            MedalFilterManager.TwoStar = Star2.isOn;
            MedalFilterManager.ThreeStar = Star3.isOn;
            MedalFilterManager.FourStar = Star4.isOn;
            MedalFilterManager.FiveStar = Star5.isOn;
            MedalFilterManager.SixStar = Star6.isOn;
            MedalFilterManager.SevenStar = Star7.isOn;
        }

        #endregion

        #region Reset Filters

        public void ResetFilters()
        {
            ResetTiers();
            ResetRange();
            ResetAttributes();
            ResetTarget();
            //ResetTypes();
            ResetStars();
        }

        private void ResetTiers()
        {
            Tier1.isOn = false;
            Tier2.isOn = false;
            Tier3.isOn = false;
            Tier4.isOn = false;
            Tier5.isOn = false;
            Tier6.isOn = false;
            Tier7.isOn = false;
            Tier8.isOn = false;
            Tier9.isOn = false;
        }

        private void ResetRange()
        {
            RangeMin.value = 0;
            RangeMax.value = 0;

            RangeMinText.text = "0";
            RangeMaxText.text = "66";
        }

        private void ResetAttributes()
        {
            Power.isOn = false;
            Speed.isOn = false;
            Magic.isOn = false;
            Upright.isOn = false;
            Reversed.isOn = false;
        }

        private void ResetTarget()
        {
            Single.isOn = false;
            Random.isOn = false;
            All.isOn = false;
        }

        private void ResetTypes()
        {
            // TODO 
        }

        private void ResetStars()
        {
            Star1.isOn = false;
            Star2.isOn = false;
            Star3.isOn = false;
            Star4.isOn = false;
            Star5.isOn = false;
            Star6.isOn = false;
            Star7.isOn = false;
        }

        #endregion
    }
}
