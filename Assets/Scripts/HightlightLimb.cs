using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float highlightNone = 0f;

    // Start is called before the first frame update
    void Start()
    {
        HighlightSelectedLimb(highlightNone);
    }

    // Update is called once per frame
    void Update()
    {

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
