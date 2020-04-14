using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HightlightLimb : MonoBehaviour
{
    [SerializeField]
    private Material hightlightShader;

    [SerializeField]
    private float highlightHead;
    [SerializeField]
    private float highlightTorso;
    [SerializeField]
    private float highlightArms;
    [SerializeField]
    private float highlightLegs;
    [SerializeField]
    private GameObject vatsNumbers;

    private float highlightNone = 0f;


    // Start is called before the first frame update
    void Start()
    {
        HighlightSelectedLimb(highlightNone);
    }

    public void LimbSelection(string limbName)
    {
        Limbs limbNameEnum = (Limbs)System.Enum.Parse(typeof(Limbs), limbName);

        switch (limbNameEnum)
        {
            case Limbs.Head:
                HighlightSelectedLimb(highlightHead);
                break;
            case Limbs.Torso:
                HighlightSelectedLimb(highlightTorso);
                break;
            case Limbs.Arms:
                HighlightSelectedLimb(highlightArms);
                break;
            case Limbs.Legs:
                HighlightSelectedLimb(highlightLegs);
                break;
            case Limbs.None:
                HighlightSelectedLimb(highlightNone);
                break;
        }
    }

    private void HighlightSelectedLimb(float limb)
    {
        hightlightShader.SetFloat("_HighlightSelected", limb);
        if (limb != highlightNone)
        {
            vatsNumbers.SetActive(true);
        }
        else
        {
            vatsNumbers.SetActive(false);
        }
    }

    public enum Limbs
    {
        None,
        Head,
        Torso,
        Arms,
        Legs
    }
}
