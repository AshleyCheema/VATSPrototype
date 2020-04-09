#define Kara_Debug

using System;
using UnityEngine;


namespace CallumCode
{
    public class CharacterZoneHighlighter : MonoBehaviour
    {
        [SerializeField] MeshRenderer meshRenderer              = default;
        [SerializeField] Color HighlightColour                  = default;
        [Header("Zone Values")]
        [SerializeField][Range(0, 255)] float HeadTexValue      = default;
        [SerializeField][Range(0, 255)] float ArmsTexValue      = default;
        [SerializeField][Range(0, 255)] float TorsoTexValue     = default;
        [SerializeField][Range(0, 255)] float LegsTexValue      = default;
        [Header("Pulse settings")]
        [SerializeField] bool ShouldPulse                       = default;
        [SerializeField] float PulseRangeMin                    = default;
        [SerializeField] float PulseRangeMax                    = default;
        [SerializeField] float PulseSpeed                       = default;

        CachingManager m_cachingManager                         = default;
        Material m_VATsMaterial                                 = default;

        const float m_precisionAllowence = 0.01f;
        BodyZone m_selectedZone = BodyZone.None;


        public void Init()
        {
            m_VATsMaterial = meshRenderer.material;

            m_cachingManager = new CachingManager();
            m_cachingManager.Init(typeof(ShaderValues), Shader.PropertyToID);

            m_VATsMaterial.SetColor(m_cachingManager[ShaderValues._HighlightColor], HighlightColour);
        }

        public void Process()
        {
            if (ShouldPulse)
            {
                float brightness = Mathf.Lerp(PulseRangeMin, PulseRangeMax, ((Mathf.Sin(Time.timeSinceLevelLoad * PulseSpeed) + 1) * 0.5f));
                m_VATsMaterial.SetFloat(m_cachingManager[ShaderValues._HighlightBrightness], brightness);
            }
        }

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




        private void Update()
        {
            Process();
        }

        private void Awake()
        {
            Init();
        }

#if Kara_Debug

        Array debug_EnumVals = (typeof(BodyZone)).GetEnumValues();

        private void OnGUI()
        {
            TogglePulse(GUILayout.Toggle(ShouldPulse, "Pulse"));

            foreach (var enumVal in debug_EnumVals)
            {
                BodyZone cur = (BodyZone)enumVal;

                if (cur == m_selectedZone)
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

#endif
    }

}
