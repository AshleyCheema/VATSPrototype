using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CallumCode
{
    public class CharacterZoneHighlighter : MonoBehaviour
    {
        [SerializeField] Color HighlightColour;
        [Header("Zone Values")]
        [SerializeField] Color HeadTexValue;
        [SerializeField] Color ArmsTexValue;
        [SerializeField] Color TorsoTexValue;
        [SerializeField] Color LegsTexValue;

        CachingManager m_cachingManager;
        Material m_VATsMaterial;

        // Debug
        Array m_enumValues;

        public void HighlightZone(BodyZone bodyZone)
        {
            switch (bodyZone)
            {
                case BodyZone.Head:
                    SetSelectedZone(HeadTexValue);
                    break;
                case BodyZone.Arms:
                    SetSelectedZone(ArmsTexValue);
                    break;
                case BodyZone.Torso:
                    SetSelectedZone(TorsoTexValue);
                    break;
                case BodyZone.Legs:
                    SetSelectedZone(LegsTexValue);
                    break;
                default:
                    break;
            }
            
        }

        private void SetSelectedZone(Color zoneID)
        {
            m_VATsMaterial.SetColor(m_cachingManager[ShaderValues._SelectedZone], zoneID);
        }

        private void Awake()
        {
            m_VATsMaterial = gameObject.GetComponent<MeshRenderer>().material;

            m_cachingManager = new CachingManager();
            m_cachingManager.Init(typeof(ShaderValues), Shader.PropertyToID);

            m_VATsMaterial.SetColor(m_cachingManager[ShaderValues._HighlightColor], HighlightColour);

            // Debug
            m_enumValues = (typeof(BodyZone)).GetEnumValues();
        }

        private void OnGUI()
        {
            foreach (var enumVal in m_enumValues)
            {
                if(GUILayout.Button($"Select {enumVal.ToString()}"))
                {
                    HighlightZone((BodyZone)enumVal);
                }
            }
        }

        public enum BodyZone
        {
            Head = 1,
            Arms,
            Torso,
            Legs
        }

        private enum ShaderValues
        {
            _SelectedZone,
            _HighlightColor
        }
    }

}
