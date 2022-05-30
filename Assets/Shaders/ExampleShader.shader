Shader "Unlit/ExampleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor("BaseColor", Color) = (1,1,1,1)
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" } // when & how shader is rendered
        LOD 100

        HLSLINCLUDE        
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START (UnityPerMaterial)
            float4 _BaseColor;
            float _Smoothness;
        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex); // sampler to sample the main texture

        struct appdata // vertex shader input
        {
            float4 position : POSITION; // vertex position
            float2 uv : TEXCOORD0;
        };

        struct v2f // vertex shader output
        {
            float4 position : SV_POSITION; // pixel position
            float2 uv : TEXCOORD0;
        };
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata i) // vertex shader
            {
                v2f o; // vertex shader output
                o.position = TransformObjectToHClip(i.position.xyz); // transform vertexes from object(model) space to clip space
                o.uv = i.uv; // no special uv coordinate transformation in this shader
                return o;
            }

            float4 frag(v2f i) : SV_Target // fragment(pixel) shader returns final color(float4) of a pixel
            {
                float4 baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv); // sample the main texture
                return baseTex * _BaseColor;
            }
            ENDHLSL
        }
    }
}
