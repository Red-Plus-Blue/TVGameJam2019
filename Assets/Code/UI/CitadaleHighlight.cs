using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Assets
{
    public enum MainMenuType
    {
        UNKNOWN,
        CREDITS,
        MISSION,
        QUIT
    }

    public class CitadaleHighlight : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;
        public MainMenuType Menu;

        private void OnMouseOver()
        {
            SpriteRenderer.enabled = true;
            MainMenuController.Instance.HighlightMenu(Menu);
        }

        private void OnMouseExit()
        {
            SpriteRenderer.enabled = false;
            MainMenuController.Instance.MenuNone();
        }

        private void OnMouseDown()
        {
            MainMenuController.Instance.SelectMenu(Menu);
        }
    }
}

