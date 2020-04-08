Shader "Callum/VATs"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _ZonesTex("Zones map", 2D) = "white" {}
        _HighlightColor ("Highlight Color", Color) = (0, 1, 0, 1)
        _SelectedZoneMin ("Selected_Zone_Min", Range(0,1)) = 0.0
        _SelectedZoneMax ("Selected_Zone_Max", Range(0,1)) = 0.0
        _HighlightBrightness ("_HighlightBrightness", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _ZonesTex;

        float _SelectedZoneMin;
        float _SelectedZoneMax;
        float _HighlightBrightness;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _HighlightColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

            float zoneID = tex2D(_ZonesTex, IN.uv_MainTex).r;
            
            float scaleMin = step(_SelectedZoneMin, zoneID);
            float scaleMax = 1 - step(_SelectedZoneMax, zoneID);

            float scale = scaleMin * scaleMax;

            // Above "scale" is this but branchless
            //if (zoneID >= _SelectedZoneMin && zoneID <= _SelectedZoneMax)
            //{
            //    c.rgb += (_HighlightColor * _HighlightBrightness);
            //}

            c.rgb += (_HighlightColor * _HighlightBrightness) * scale;

            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
