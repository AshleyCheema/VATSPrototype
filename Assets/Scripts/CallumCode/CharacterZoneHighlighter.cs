using System;
using UnityEngine;

namespace CallumCode
{
    public class CharacterZoneHighlighter : MonoBehaviour
    {
        [SerializeField] MeshRenderer meshRenderer;
        [SerializeField] Color HighlightColour;
        [Header("Zone Values")]
        [SerializeField][Range(0, 255)] float HeadTexValue;
        [SerializeField][Range(0, 255)] float ArmsTexValue;
        [SerializeField][Range(0, 255)] float TorsoTexValue;
        [SerializeField][Range(0, 255)] float LegsTexValue;
        [Header("Pulse settings")]
        [SerializeField] bool ShouldPulse;
        [SerializeField] float PulseRangeMin;
        [SerializeField] float PulseRangeMax;
        [SerializeField] float PulseSpeed;

        CachingManager m_cachingManager;
        Material m_VATsMaterial;

        const float m_precisionAllowence = 0.01f;
        BodyZone m_selectedZone = BodyZone.None;

        // Debug
        Array m_enumValues;

        public void HighlightZone(BodyZone bodyZone)
        {
            m_selectedZone = bodyZone;

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
                case BodyZone.None:
                    m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZoneMin], 0.0f);
                    m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZoneMax], 0.0f);
                    break;
                default:
                    break;
            }
            
        }

        public void TogglePulse(bool shouldPulse)
        {
            ShouldPulse = shouldPulse;
            m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._HighlightBrightness], 1.0f);
        }

        private void SetSelectedZone(float zoneID)
        {
            float conv = zoneID / 256;

            m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZoneMin], conv - m_precisionAllowence);
            m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._SelectedZoneMax], conv + m_precisionAllowence);
        }

        private void Update()
        {
            if(ShouldPulse)
            {
                float brightness = Mathf.Lerp(PulseRangeMin, PulseRangeMax, ((Mathf.Sin(Time.timeSinceLevelLoad * PulseSpeed) + 1) * 0.5f));
                m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._HighlightBrightness], brightness);
            }
        }

        private void Awake()
        {
            m_VATsMaterial = meshRenderer.material;

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
                BodyZone cur = (BodyZone)enumVal;

                if(cur == m_selectedZone)
                {
                    GUI.color = Color.green;
                }
                if (GUILayout.Button($"Select {enumVal.ToString()}"))
                {
                    HighlightZone(cur);
                }
                GUI.color = Color.white;
            }
        }

        public enum BodyZone
        {
            Head = 1,
            Arms,
            Torso,
            Legs,
            None
        }

        private enum ShaderValues
        {
            _SelectedZoneMin,
            _SelectedZoneMax,
            _HighlightColor,
            _HighlightBrightness
        }
    }

}
