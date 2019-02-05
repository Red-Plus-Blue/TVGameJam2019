using Game.Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnInfo : MonoBehaviour
{
    public Text Name;
    public Text Dimension;
    public Text Status;

    public GameObject HealthPipPrefab;
    public RectTransform HealthBarHolder;
    public Text Health;

    public Text Move;
    public Text Defence;

    public Text Weapon;
    public Text Attack;
    public Text Range;
    public Text Damage;

    private void Awake()
    {
        Clear();
    }

    public void Show(Pawn pawn)
    {
        Clear();

        if(pawn == null)
        {
            return;
        }

        Name.text = pawn.Name;
        Dimension.text = pawn.Dimension;
        Status.text = pawn.Status;

        Health.text = pawn.Health.ToString();
        Move.text = string.Format("{0}/{1}", pawn.MovePoints, pawn.MovePointsMax);
        Defence.text = pawn.Defence.ToString();

        Attack.text = pawn.Attack.ToString();
        Range.text = pawn.AttackRange.ToString();
        Damage.text = pawn.Damage.ToString();
    }

    public void Clear()
    {
        Name.text           = "";
        Dimension.text      = "";
        Status.text         = "";

        Health.text         = "";
        Move.text           = "";
        Defence.text        = "";

        Attack.text         = "";
        Range.text          = "";
        Damage.text         = "";
    }
}
