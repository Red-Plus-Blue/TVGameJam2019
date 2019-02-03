using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Assets
{
    public class MainMenuController : MonoBehaviour
    {
        public static MainMenuController Instance;

        public Text Title;

        public GameObject Menu;
        public Text MenuTitle;
        public Text MenuDescription;

        public GameObject[] CitadaleParts = new GameObject[0];

        private void Awake()
        {
            Instance = this;
            MenuNone();
        }

        private void Start()
        {
            var user = GameManager.Instance.User;
            if(user.FirstMenuLoad)
            {
                DisableMenu();
                StartCoroutine(WaitToEnableMenuCoroutine());
                user.FirstMenuLoad = false;
            }
            else
            {
                Title.gameObject.SetActive(false);
                EnableMenu();
            }
        }

        public IEnumerator WaitToEnableMenuCoroutine()
        {
            var waiting = true;
            while(waiting)
            {
                waiting = !Input.anyKey;
                yield return null;
            }
            Title.gameObject.SetActive(false);
            EnableMenu();
            yield return null;
        }

        public void DisableMenu()
        {
            CitadaleParts.ToList().ForEach(part => part.SetActive(false));
            Menu.SetActive(false);
        }

        public void EnableMenu()
        {
            CitadaleParts.ToList().ForEach(part => part.SetActive(true));
            Menu.SetActive(true);
        }

        public void HighlightMenu(MainMenuType menu)
        {
            switch(menu)
            {
                case MainMenuType.CREDITS: { MenuCredits(); break; }
                case MainMenuType.MISSION: { MenuMission(); break; }
                case MainMenuType.QUIT: { MenuQuit(); break; }
                case MainMenuType.UNKNOWN: { MenuUnknown(); break; }
            }
        }

        public void SelectMenu(MainMenuType menu)
        {
            switch (menu)
            {
                case MainMenuType.MISSION: { GameManager.Instance.StartLevel(); break; }
                case MainMenuType.QUIT: { Application.Quit(); break; }
            }
        }

        public void MenuQuit()
        {
            MenuTitle.text = "Portal Room (Quit)";
            MenuDescription.text = "Return to your home dimension...even though everyone there is useless.";
        }

        public void MenuCredits()
        {
            MenuTitle.text = "Archives (Credits)";
            MenuDescription.text = "Consult the archives to find out who is responsible for this.";
        }

        public void MenuMission()
        {
            MenuTitle.text = "Hangar Bay (Mission)";
            MenuDescription.text = "Board your ship and go fuck shit up.";
        }

        public void MenuUnknown()
        {
            MenuTitle.text = "ACCESS DENIED";
            MenuDescription.text = "[REDACTED]";
        }

        public void MenuNone()
        {
            MenuTitle.text = "";
            MenuDescription.text = "You can select different parts of the Citadel by mousing over them.";
        }
    }
}

