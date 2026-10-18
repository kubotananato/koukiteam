// Unity 6.3 / Universal Renderer / Full Screen Pass learning template.
Shader "ReplayCRT/Learning"
{
    Properties
    {
        // Learning controls: used only in lessons 01 and 02.
        _Brightness ("01 Brightness", Range(0, 1)) = 1
        _Offset ("02 UV Offset", Vector) = (0, 0, 0, 0)
        _Curvature ("03 Curvature", Range(0, 0.3)) = 0.06
        _Noise ("05 Noise", Range(0, 0.1)) = 0.018
        _NoiseTime ("06 Noise Time (-1 = Unity time)", Float) = -1
        _Intensity ("07 Intensity", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            Name "CRT Lesson"
            ZWrite Off ZTest Always Cull Off
            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Offset;
                float _Brightness, _Curvature, _Noise, _NoiseTime, _Intensity;
            CBUFFER_END

            // Used from lesson 05 onward. Deterministic pseudo-random value in [0, 1).
            float Hash(float2 p)
            {
                float3 q = frac(float3(p.xyx) * 0.1031);
                q += dot(q, q.yzx + 33.33);
                return frac((q.x + q.y) * q.z);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                // BEGIN LESSON: replace only the code between these comments.
                float2 uv = input.texcoord;
                half4 original = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);
                if (_Intensity <= 0.0) return original;

                float2 p = uv * 2.0 - 1.0;
                float distanceSquared = dot(p, p);
                float2 sampleUV = p * (1.0 + _Curvature * distanceSquared) * 0.5 + 0.5;
                half4 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, sampleUV);
                float inside = step(0.0, sampleUV.x) * step(sampleUV.x, 1.0)
                            * step(0.0, sampleUV.y) * step(sampleUV.y, 1.0);

                float seconds = _NoiseTime >= 0.0 ? _NoiseTime : _Time.y;
                float noiseFrame = floor(seconds * 24.0);
                float2 pixel = floor(input.positionCS.xy);
                float noise = Hash(pixel + float2(noiseFrame * 17.0, noiseFrame * 31.0));
                color.rgb = lerp(color.rgb, half3(noise, noise, noise), _Noise);
                color.rgb *= inside;
                return half4(lerp(original.rgb, color.rgb, _Intensity), original.a);
                // END LESSON
            }
            ENDHLSL
        }
    }
}
