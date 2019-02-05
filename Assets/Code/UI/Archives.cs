using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Game.Assets
{
    public class Archives : MonoBehaviour
    {
        public Text PageTitle;

        public RectTransform ContentHolder;
        public RectTransform ButtonHolder;

        public GameObject ButtonPrefab;

        public GameObject[] Pages;

        protected Action _onHide;

        protected int _currentPage;

        public void Show(Action onHide)
        {
            gameObject.SetActive(true);
            _onHide = onHide;
            GoToPage(0);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ClearButtons();
            _onHide();
        }

        public void GoToPage(int page)
        {
            ClearButtons();
            _currentPage = page;
            ShowPage(_currentPage);
            ShowButtons();
        }

        protected void ShowPage(int pageNumber)
        {
            foreach (Transform transform in ContentHolder)
            {
                GameObject.Destroy(transform.gameObject);
            }

            var page = Pages[pageNumber];

            foreach(Transform child in page.transform)
            {
                var pageSegment = GameObject.Instantiate(child.gameObject, ContentHolder);
                pageSegment.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            PageTitle.text = page.name;
        }

        public void ShowButtons()
        {
            AddButton("Close", Hide);

            if (_currentPage != 0)
            {
                AddButton("<< First", () => { GoToPage(0); });
            }

            if (_currentPage > 0)
            {
                AddButton("< Previous", () => { GoToPage(_currentPage - 1); });
            }

            if(_currentPage < Pages.Length - 1)
            {
                AddButton("Next >", () => { GoToPage(_currentPage + 1); });
            }
     
            if(_currentPage != Pages.Length -1)
            {
                AddButton("Last >>", () => { GoToPage(Pages.Length - 1); });
            }
        }

        protected void AddButton(string text, UnityAction onClick)
        {
            var buttonObject = GameObject.Instantiate(ButtonPrefab, ButtonHolder);
            buttonObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(onClick);

            var buttonText = buttonObject.GetComponentInChildren<Text>();
            buttonText.text = text;
        }

        public void ClearButtons()
        {
            foreach(Transform transform in ButtonHolder)
            {
                var button = transform.gameObject.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                GameObject.Destroy(transform.gameObject);
            }
        }
    }
}

