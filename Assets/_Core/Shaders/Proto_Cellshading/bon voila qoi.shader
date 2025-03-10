Shader "Custom/TorchContactGlow"
{
    Properties
    {
        _GlowColor ("Glow Color", Color) = (1, 0.5, 0, 1)  // Orange feu
        _Threshold ("Contact Threshold", Range(0, 0.1)) = 0.02
        _GlowSize ("Glow Size", Range(0.01, 0.1)) = 0.05
        _Intensity ("Glow Intensity", Range(0, 3)) = 1.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha // Transparence additive

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float depth : DEPTH;
            };

            float4 _GlowColor;
            float _Threshold;
            float _GlowSize;
            float _Intensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.depth = o.pos.z / o.pos.w;
                return o;
            }

            sampler2D _CameraDepthTexture;

            fixed4 frag (v2f i) : SV_Target
            {
                float sceneDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv));
                float fragDepth = LinearEyeDepth(i.depth);
                float diff = abs(sceneDepth - fragDepth);

                // Effet de glow autour du contact
                float glowFactor = smoothstep(_Threshold, _Threshold + _GlowSize, diff) * _Intensity;

                if (glowFactor > 0)
                    return float4(_GlowColor.rgb * glowFactor, glowFactor); // Effet lumineux
                else
                    return float4(0, 0, 0, 0); // Invisible par défaut
            }
            ENDCG
        }
    }
}
