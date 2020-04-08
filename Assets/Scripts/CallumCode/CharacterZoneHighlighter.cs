using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CallumCode
{
    public class CharacterZoneHighlighter : MonoBehaviour
    {
        [SerializeField] Color HighlightColour;

        CachingManager m_cachingManager;
        Material m_VATsMaterial;

        // Debug
        Array m_enumValues;

        public void HighlightZone(BodyZone bodyZone)
        {
            m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZone], (float)bodyZone);
        }

        private void Awake()
        {
            m_VATsMaterial = gameObject.GetComponent<SkinnedMeshRenderer>().material;

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
            None = -1,
            Head,
            LeftArm,
            RightArm,
            Torso,
            LeftLeg,
            RightLeg,
            Count
        }

        private enum ShaderValues
        {
            _SelectedZone,
            _HighlightColor
        }
    }

}
