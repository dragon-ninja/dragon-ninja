//noise 控制 溶解
Shader "BF_SRP/Particles/Dissolve"
{
    Properties
    {
        [Enum(Suyu.CullType)] _Cull ("Cull", Float) = 0.0
        [BlendMode]_BlendMode("Blend", Float) = 1
        [HDR]_Color ("Main Color", Color) = (1, 1, 1, 1)
        _MainTex ("MainTex", 2D) = "white" {}
        _NoiseTex ("NoiseTex(R)", 2D) = "white" {}
        [KWEnum(_, None, _, R, _USE_MASK_R, A, _USE_MASK_A)]_UseChannel("Use Noise Channel", Float) = 0.0
        _MaskTex ("MaskTex", 2D) = "black" {}
        [Main(time, _USE_AUTO_TIME, 4)]_UseAutoTime("UseTimeSpeed(TEXCOORD1.zw)", Float) = 1.0
        [Sub(time)]_MainU ("Main U", Range(-5, 5)) = 0
        [Sub(time)]_MainV ("Main V", Range(-5, 5)) = 0
        [Main(edge, _USE_EDGE, 4)] _UseEdge("UseEdge", Float) = 0
        [Sub(edge)]_EdgeWidth ("EdgeWidth", Range(0, 1)) = 0.1
        [Sub(edge)][HDR]_EdgeColor ("EdgeColor", Color) = (1, 1, 1, 1)

        [Main(custom, _USE_CUTOFF, 4)]_UseCutOff("UseCutoff (custom TEXCOORD0.z)", Float) = 0
        [Sub(custom)]_Cutoff ("Cutoff", Range(0, 1)) = 0

        [HideInInspector] _SrcBlend ("__src", Float) = 1.0
        [HideInInspector] _DstBlend ("__dst", Float) = 0.0
        [HideInInspector] _ZWrite ("__zw", Float) = 1.0        
    }
    SubShader
    {
        Tags { "IgnoreProjector" = "True" "Queue"="Transparent"}
        Blend [_SrcBlend] [_DstBlend]
        ZWrite [_ZWrite]
        Cull [_Cull]
        ZTest On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            
            #pragma shader_feature _ _PARTICLE_ALPHA _PARTICLE_ADD
            #pragma shader_feature _USE_EDGE
            #pragma shader_feature _USE_CUTOFF
            #pragma shader_feature _ _USE_MASK_R _USE_MASK_A
            #pragma shader_feature _USE_AUTO_TIME
            #pragma multi_compile _ _BF_SOFT_PARTICLE

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                fixed4 vertexColor : COLOR;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 custom:TEXCOORD1;
                #ifdef _BF_SOFT_PARTICLE
                float4 projPos : TEXCOORD2;
                #endif
                float4 vertex : SV_POSITION;
                fixed4 vertexColor : COLOR;
            };

            UNITY_DECLARE_DEPTH_TEXTURE(_BFDepthTexture);
            float _BFInvFade;

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            half _EdgeWidth;
            half3 _EdgeColor;
            half4 _Color;

            half _Cutoff;
            half _MainU;
            half _MainV;

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                o.vertex = UnityObjectToClipPos(v.vertex);

                const float2 uv = v.uv.xy;
                #if defined(_USE_AUTO_TIME)
                    const float2 uvOffset = _Time.y * float2(_MainU, _MainV) + uv;
                    o.uv.xy = TRANSFORM_TEX(uvOffset, _MainTex);
                #else
                    float2 uvOffset = v.texcoord1.xy + uv;
                    o.uv.xy = TRANSFORM_TEX(uvOffset, _MainTex);
                #endif
                
                o.uv.zw = TRANSFORM_TEX(v.uv.xy, _NoiseTex);
                o.vertexColor = v.vertexColor;
                o.custom.xy = v.uv.zw;
                o.custom.zw = TRANSFORM_TEX(v.uv.xy, _MaskTex);
                
                #ifdef _BF_SOFT_PARTICLE
                o.projPos = ComputeScreenPos (o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 main_color = tex2D(_MainTex, i.uv.xy);
                fixed3 baseCol = main_color.rgb * i.vertexColor.rgb * _Color.rgb;
                fixed noise = tex2D(_NoiseTex, i.uv.zw).r;

                #if defined(_USE_CUTOFF)
                    half cutoff = _Cutoff;                    
                #else
                    half cutoff = i.custom.x;
                #endif

                half diff = noise - cutoff;

                #ifdef _USE_EDGE
                    half edgeWidth = max(_EdgeWidth, 1e-4f);
                    half haveAlpha = (1 - step(0.999, cutoff));
                    half haveEdge = step(1e-4f, _EdgeWidth) * step(1e-4f, cutoff);
                    half edge = rcp(edgeWidth) * clamp(diff, -edgeWidth, edgeWidth);
                    //内边缘  rcp(diffCut) * (nl - diffStrenght) + 0.5;
                    //baseCol = lerp(_EdgeColor, baseCol, saturate(100 * (edge - 0.9) + 0.5));
                    //外边缘
                    //平滑处理
                    half factor = lerp(-0.8, -0.1, smoothstep(0, 0.1, _EdgeWidth));
                    baseCol = lerp(baseCol, _EdgeColor, (1 - smoothstep(factor, 0, edge)) * haveEdge);

                    //half alpha = step(0.001, diff + edgeWidth);  
                    half alpha = smoothstep(-edgeWidth, -edgeWidth + 0.01, diff) * haveAlpha;
                #else
                    half alpha = smoothstep(0, 0.01, diff);
                #endif

                alpha *= main_color.a * _Color.a * i.vertexColor.a;

                #ifdef _USE_MASK_R
                    alpha *= tex2D(_MaskTex, i.custom.zw).r;
                #elif _USE_MASK_A
                    alpha *= tex2D(_MaskTex, i.custom.zw).a;
                #endif
                
                half3 finalColor = baseCol.rgb;
                
                #ifdef _BF_SOFT_PARTICLE
                    float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_BFDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                    float partZ = i.projPos.z;
                    float fade = saturate(_BFInvFade * (sceneZ-partZ));
                    alpha *= fade;
                #endif

                #ifdef _PARTICLE_ALPHA
                    return fixed4(finalColor, alpha);
                #endif

                #ifdef _PARTICLE_ADD
                    finalColor *= alpha;
                    half lum = Luminance(finalColor);
                    return fixed4(finalColor, lum);
                #endif

                return fixed4(finalColor, 1);
            }
            ENDCG
        }
    }
    CustomEditor "Suyu.BFSRPGUI"
}
