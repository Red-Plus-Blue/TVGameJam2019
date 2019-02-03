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
        public Animator Animator;

        public Text LeftCharacterName;
        public Image LeftCharacterImage;
        public Image LeftCharacterShade;

        public Text RightCharacterName;
        public Image RightCharacterImage;
        public Image RightCharacterShade;

        public Text DialogText;

        public Text DialogOption1;
        public Text DialogOption2;

        public Button DialogButton1;
        public Button DialogButton2;

        public GameObject[] Views = new GameObject[0];

        protected bool _waiting;

        public void RunDialog(DialogData data, Action onDialogComplete)
        {
            var user = GameManager.Instance.User;

            LeftCharacterName.text = user.Interpolate(data.LeftCharacterName);
            LeftCharacterImage.sprite = data.LeftCharacterImage;

            RightCharacterName.text = user.Interpolate(data.RightCharacterName);
            RightCharacterImage.sprite = data.RightCharacterImage;

            StartCoroutine(DialogCoroutine(data, onDialogComplete));

            DialogButton1.onClick.AddListener(AdvanceDialog);
            DialogButton2.onClick.AddListener(AdvanceDialog);
        }

        protected IEnumerator DialogCoroutine(DialogData data, Action onDialogComplete)
        {
            Show();
            yield return new WaitForSeconds(.25f);

            foreach(var segment in data.Segments)
            {
                ShowSegment(segment);

                _waiting = true;

                while(_waiting)
                {
                    yield return null;
                }
            }

            Hide();
            yield return new WaitForSeconds(.25f);
            onDialogComplete();
            yield return null;
        }

        protected void AdvanceDialog()
        {
            _waiting = false;
        }

        protected void ShowSegment(DialogSegment segment)
        {
            LeftCharacterShade.enabled = !segment.IsLeftCharacter;
            RightCharacterShade.enabled = segment.IsLeftCharacter;

            var user = GameManager.Instance.User;

            DialogText.text = user.Interpolate(segment.Text);
            DialogOption1.text = user.Interpolate(segment.Option1);
            DialogOption2.text = user.Interpolate(segment.Option2);
        }

        public void Show()
        {
            Animator.SetTrigger("FadeIn");
        }

        public void Hide()
        {
            Animator.SetTrigger("FadeOut");

        }
    }
}