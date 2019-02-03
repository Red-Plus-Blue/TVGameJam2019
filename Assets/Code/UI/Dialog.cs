using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Assets
{
    public class Dialog : MonoBehaviour
    {
        public Text LeftCharacterName;
        public Image LeftCharacterImage;
        public Image LeftCharacterShade;

        public Text RightCharacterName;
        public Image RightCharacterImage;
        public Image RightCharacterShade;

        public Text DialogText;

        public GameObject[] Views = new GameObject[0];

        public void RunDialog(DialogData data, Action onDialogComplete)
        {
            LeftCharacterName.text = data.LeftCharacterName;
            LeftCharacterImage.sprite = data.LeftCharacterImage;

            RightCharacterName.text = data.RightCharacterName;
            RightCharacterImage.sprite = data.RightCharacterImage;

            StartCoroutine(DialogCoroutine(data, onDialogComplete));
        }

        protected IEnumerator DialogCoroutine(DialogData data, Action onDialogComplete)
        {
            Show();

            foreach(var segment in data.Segments)
            {
                ShowSegment(segment);

                var waiting = true;
                while(waiting)
                {
                    waiting = !Input.GetMouseButtonDown(0);
                    yield return null;
                }
            }

            Hide();
            onDialogComplete();
            yield return null;
        }

        protected void ShowSegment(DialogSegment segment)
        {
            LeftCharacterShade.enabled = !segment.IsLeftCharacter;
            RightCharacterShade.enabled = segment.IsLeftCharacter;
            DialogText.text = segment.Text;
        }

        public void Show()
        {
            Views.ToList().ForEach(view => view.SetActive(true));
        }

        public void Hide()
        {
            Views.ToList().ForEach(view => view.SetActive(false));
        }
    }
}