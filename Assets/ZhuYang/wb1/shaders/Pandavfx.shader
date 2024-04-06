// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFX/Pandavfx"
{
	Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)]_Cullmode("Cullmode", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)]_Ztest("Ztest", Float) = 4
		[Enum(UnityEngine.Rendering.BlendMode)]_Scr("Scr", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)]_Dst("Dst", Float) = 10
		_MainTex("MainTex", 2D) = "white" {}
		_MainAlpha("MainAlpha", Range( 0 , 20)) = 1
		[HDR]_MainColor("MainColor", Color) = (1,1,1,1)
		_MainTex_Uspeed("MainTex_Uspeed", Float) = 0
		_MainTex_Vspeed("MainTex_Vspeed", Float) = 0
		_MaskTex("MaskTex", 2D) = "white" {}
		_DistortTex("DistortTex", 2D) = "white" {}
		[Enum(off,0,on,1)]_DistortMainTex("DistortMainTex", Float) = 1
		[Enum(off,0,on,1)]_DistortDisTex("DistortDisTex", Float) = 0
		_DistortFactor("DistortFactor", Float) = 0
		_DistortTex_Uspeed("DistortTex_Uspeed", Float) = 0
		_DistortTex_Vspeed("DistortTex_Vspeed", Float) = 0
		_DissloveTex("DissloveTex", 2D) = "white" {}
		_DIssloveFactor("DIssloveFactor", Range( 0 , 2)) = 1
		_DIssloveSoft("DIssloveSoft", Range( 0 , 1)) = 0.8235294
		[HDR]_DIssloveColor("DIssloveColor", Color) = (1,1,1,1)
		_DisTex_Uspeed("DisTex_Uspeed", Float) = 0
		_DisTex_Vspeed("DisTex_Vspeed", Float) = 0
		_VTOTex("VTOTex", 2D) = "white" {}
		_VTOFactor("VTOFactor", Float) = 0
		_VTOTex_Uspeed("VTOTex_Uspeed", Float) = 0
		_VTOTex_Vspeed("VTOTex_Vspeed", Float) = 0
		_VTOMaskTex("VTOMaskTex", 2D) = "white" {}
		_fnl_power("fnl_power", Range( 1 , 10)) = 1
		_fnl_sacle("fnl_sacle", Float) = 0
		[HDR]_fnl_color("fnl_color", Color) = (1,1,1,0)
		_softFacotr("softFacotr", Range( 1 , 20)) = 1
		_DepthfadeFactor("DepthfadeFactor", Float) = 1
		[Toggle]_MainTex_ar("MainTex_a/r", Float) = 0
		[Toggle]_CustomdataMainTexUV("CustomdataMainTexUV", Float) = 0
		_MainAlphaPower("MainAlphaPower", Range( 1 , 10)) = 1
		[Toggle]_MaskAlphaRA("MaskAlphaRA", Float) = 0
		[Toggle]_CustomdataMaskUV("CustomdataMaskUV", Float) = 0
		_Mask_scale("Mask_scale", Float) = 1
		[Toggle]_AlphaAdd("AlphaAdd", Float) = 0
		_Mask_rotat("Mask_rotat", Range( 0 , 360)) = 0
		_MainTex_rotat("MainTex_rotat", Range( 0 , 360)) = 0
		_DIssolve_rotat("DIssolve_rotat", Range( 0 , 360)) = 0
		[Toggle]_CustomdataDis("CustomdataDis", Float) = 0
		[Toggle]_FNLfanxiangkaiguan("FNLfanxiangkaiguan", Float) = 0
		[Toggle]_FNLkaiguan("FNLkaiguan", Float) = 0
		[Toggle]_ToggleSwitch0("Toggle Switch0", Float) = 0
		[Toggle]_Depthfadeon("Depthfadeon", Float) = 0
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull [_Cullmode]
		ZWrite Off
		ZTest [_Ztest]
		Blend [_Scr] [_Dst]
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 5.0
		#pragma surface surf Unlit keepalpha noshadow vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
			float2 uv3_texcoord3;
			float4 vertexColor : COLOR;
			float2 uv2_texcoord2;
			float4 uv2_tex4coord2;
			float3 viewDir;
			float4 screenPosition88;
		};

		uniform float _Ztest;
		uniform float _Scr;
		uniform float _Dst;
		uniform float _Cullmode;
		uniform sampler2D _VTOTex;
		uniform float _VTOTex_Uspeed;
		uniform float _VTOTex_Vspeed;
		uniform float4 _VTOTex_ST;
		uniform float _VTOFactor;
		uniform sampler2D _VTOMaskTex;
		uniform float4 _VTOMaskTex_ST;
		uniform float _ToggleSwitch0;
		uniform float _FNLkaiguan;
		uniform float _fnl_sacle;
		uniform float _fnl_power;
		uniform float4 _fnl_color;
		uniform float _AlphaAdd;
		uniform float _Mask_scale;
		uniform float _MaskAlphaRA;
		uniform sampler2D _MaskTex;
		uniform float4 _MaskTex_ST;
		uniform float _CustomdataMaskUV;
		uniform float _Mask_rotat;
		uniform float _MainTex_ar;
		uniform sampler2D _MainTex;
		uniform float _MainTex_Uspeed;
		uniform float _MainTex_Vspeed;
		uniform float _CustomdataMainTexUV;
		uniform float4 _MainTex_ST;
		uniform float _DistortFactor;
		uniform sampler2D _DistortTex;
		uniform float _DistortTex_Uspeed;
		uniform float _DistortTex_Vspeed;
		uniform float4 _DistortTex_ST;
		uniform float _DistortMainTex;
		uniform float _MainTex_rotat;
		uniform float4 _MainColor;
		uniform float _MainAlpha;
		uniform float _CustomdataDis;
		uniform float _DIssloveFactor;
		uniform float _DIssloveSoft;
		uniform sampler2D _DissloveTex;
		uniform float _DisTex_Uspeed;
		uniform float _DisTex_Vspeed;
		uniform float4 _DissloveTex_ST;
		uniform float _DistortDisTex;
		uniform float _DIssolve_rotat;
		uniform float _FNLfanxiangkaiguan;
		uniform float _softFacotr;
		uniform float _Depthfadeon;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthfadeFactor;
		uniform float _MainAlphaPower;
		uniform float4 _DIssloveColor;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 appendResult76 = (float2(_VTOTex_Uspeed , _VTOTex_Vspeed));
			float2 uv_VTOTex = v.texcoord.xy * _VTOTex_ST.xy + _VTOTex_ST.zw;
			float2 panner77 = ( 1.0 * _Time.y * appendResult76 + uv_VTOTex);
			float3 ase_vertexNormal = v.normal.xyz;
			float2 uv_VTOMaskTex = v.texcoord * _VTOMaskTex_ST.xy + _VTOMaskTex_ST.zw;
			float3 VTO82 = ( tex2Dlod( _VTOTex, float4( panner77, 0, 0.0) ).r * ase_vertexNormal * _VTOFactor * tex2Dlod( _VTOMaskTex, float4( uv_VTOMaskTex, 0, 0.0) ).r * (( _ToggleSwitch0 )?( v.texcoord1.w ):( 1.0 )) );
			v.vertex.xyz += VTO82;
			v.vertex.w = 1;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 vertexPos88 = ase_vertex3Pos;
			float4 ase_screenPos88 = ComputeScreenPos( UnityObjectToClipPos( vertexPos88 ) );
			o.screenPosition88 = ase_screenPos88;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV91 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode91 = ( 0.0 + _fnl_sacle * pow( 1.0 - fresnelNdotV91, _fnl_power ) );
			float4 fnlColor97 = ( saturate( fresnelNode91 ) * _fnl_color );
			float4 temp_cast_0 = (0.0).xxxx;
			float2 uv_MaskTex = i.uv_texcoord * _MaskTex_ST.xy + _MaskTex_ST.zw;
			float2 temp_cast_1 = (0.0).xx;
			float cos149 = cos( ( ( ( _Mask_rotat / 360.0 ) * UNITY_PI ) * 2.0 ) );
			float sin149 = sin( ( ( ( _Mask_rotat / 360.0 ) * UNITY_PI ) * 2.0 ) );
			float2 rotator149 = mul( ( uv_MaskTex + (( _CustomdataMaskUV )?( i.uv3_texcoord3 ):( temp_cast_1 )) ) - float2( 0.5,0.5 ) , float2x2( cos149 , -sin149 , sin149 , cos149 )) + float2( 0.5,0.5 );
			float4 tex2DNode52 = tex2D( _MaskTex, rotator149 );
			float MaskAlpha136 = ( _Mask_scale * (( _MaskAlphaRA )?( tex2DNode52.r ):( tex2DNode52.a )) );
			float2 appendResult14 = (float2(_MainTex_Uspeed , _MainTex_Vspeed));
			float2 temp_cast_2 = (0.0).xx;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 temp_cast_3 = (0.0).xx;
			float2 appendResult58 = (float2(_DistortTex_Uspeed , _DistortTex_Vspeed));
			float2 uv_DistortTex = i.uv_texcoord * _DistortTex_ST.xy + _DistortTex_ST.zw;
			float2 panner59 = ( 1.0 * _Time.y * appendResult58 + uv_DistortTex);
			float4 tex2DNode54 = tex2D( _DistortTex, panner59 );
			float2 appendResult61 = (float2(tex2DNode54.r , tex2DNode54.g));
			float2 DistortUV60 = ( _DistortFactor * appendResult61 );
			float2 lerpResult118 = lerp( temp_cast_3 , DistortUV60 , _DistortMainTex);
			float2 panner11 = ( 1.0 * _Time.y * appendResult14 + ( (( _CustomdataMainTexUV )?( i.uv2_texcoord2 ):( temp_cast_2 )) + uv_MainTex + lerpResult118 ));
			float cos158 = cos( ( ( ( _MainTex_rotat / 360.0 ) * UNITY_PI ) * 2.0 ) );
			float sin158 = sin( ( ( ( _MainTex_rotat / 360.0 ) * UNITY_PI ) * 2.0 ) );
			float2 rotator158 = mul( panner11 - float2( 0.5,0.5 ) , float2x2( cos158 , -sin158 , sin158 , cos158 )) + float2( 0.5,0.5 );
			float4 tex2DNode1 = tex2D( _MainTex, rotator158 );
			float MainTexAlpha37 = ( i.vertexColor.a * (( _MainTex_ar )?( tex2DNode1.r ):( tex2DNode1.a )) * _MainColor.a * _MainAlpha );
			float2 appendResult48 = (float2(_DisTex_Uspeed , _DisTex_Vspeed));
			float2 uv_DissloveTex = i.uv_texcoord * _DissloveTex_ST.xy + _DissloveTex_ST.zw;
			float2 temp_cast_4 = (0.0).xx;
			float2 lerpResult122 = lerp( temp_cast_4 , DistortUV60 , _DistortDisTex);
			float2 panner49 = ( 1.0 * _Time.y * appendResult48 + ( uv_DissloveTex + lerpResult122 ));
			float cos162 = cos( ( ( ( _DIssolve_rotat / 360.0 ) * UNITY_PI ) * 2.0 ) );
			float sin162 = sin( ( ( ( _DIssolve_rotat / 360.0 ) * UNITY_PI ) * 2.0 ) );
			float2 rotator162 = mul( panner49 - float2( 0.5,0.5 ) , float2x2( cos162 , -sin162 , sin162 , cos162 )) + float2( 0.5,0.5 );
			float smoothstepResult27 = smoothstep( ( (( _CustomdataDis )?( i.uv2_tex4coord2.z ):( _DIssloveFactor )) - _DIssloveSoft ) , (( _CustomdataDis )?( i.uv2_tex4coord2.z ):( _DIssloveFactor )) , tex2D( _DissloveTex, rotator162 ).r);
			float DisAplha42 = smoothstepResult27;
			float dotResult106 = dot( i.viewDir , ase_worldNormal );
			float softedge111 = pow( saturate( dotResult106 ) , _softFacotr );
			float4 ase_screenPos88 = i.screenPosition88;
			float4 ase_screenPosNorm88 = ase_screenPos88 / ase_screenPos88.w;
			ase_screenPosNorm88.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm88.z : ase_screenPosNorm88.z * 0.5 + 0.5;
			float screenDepth88 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm88.xy ));
			float distanceDepth88 = saturate( ( screenDepth88 - LinearEyeDepth( ase_screenPosNorm88.z ) ) / ( _DepthfadeFactor ) );
			float MainAlpha142 = pow( saturate( ( MaskAlpha136 * MainTexAlpha37 * DisAplha42 * (( _FNLfanxiangkaiguan )?( softedge111 ):( 1.0 )) * (( _Depthfadeon )?( distanceDepth88 ):( 1.0 )) ) ) , _MainAlphaPower );
			float4 MainColor36 = ( i.vertexColor * tex2DNode1 * _MainColor );
			float4 lerpResult33 = lerp( _DIssloveColor , MainColor36 , smoothstepResult27);
			float4 DisColor40 = lerpResult33;
			o.Emission = ( (( _FNLkaiguan )?( temp_cast_0 ):( fnlColor97 )) + ( (( _AlphaAdd )?( MainAlpha142 ):( 1.0 )) * DisColor40 ) ).rgb;
			o.Alpha = MainAlpha142;
		}

		ENDCG
	}
	CustomEditor "CommonGUI"
}
/*ASEBEGIN
Version=18912
7;18;2546;1361;1247.384;555.2315;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;62;-2878.959,1585.638;Inherit;False;1773.547;441.7441;Distort;10;63;60;61;57;55;56;54;58;59;64;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-2818.696,1778.542;Inherit;False;Property;_DistortTex_Uspeed;DistortTex_Uspeed;15;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2816.696,1870.244;Inherit;False;Property;_DistortTex_Vspeed;DistortTex_Vspeed;16;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;58;-2583.996,1853.843;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-2828.959,1635.638;Inherit;False;0;54;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;59;-2423.996,1829.842;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;54;-2215.242,1802.156;Inherit;True;Property;_DistortTex;DistortTex;11;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;64;-1921.825,1675.112;Inherit;False;Property;_DistortFactor;DistortFactor;14;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;61;-1882.229,1829.761;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;144;-798.2462,1425.596;Inherit;False;1737.18;651.3225;Mask;16;139;134;132;138;133;52;141;135;140;136;149;152;151;153;154;179;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-1728.043,1724.037;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;21;-3598.235,-238.151;Inherit;False;2415.289;796.3688;Main;27;158;67;119;118;65;36;6;37;7;9;125;5;8;1;11;17;14;13;15;12;127;16;20;157;155;156;178;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;-1517.063,1719.66;Inherit;False;DistortUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;151;-567.4014,1974.145;Inherit;False;Property;_Mask_rotat;Mask_rotat;40;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;134;-780.2462,1921.918;Inherit;False;2;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-3437.713,-177.6959;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;119;-3427,268;Inherit;False;Property;_DistortMainTex;DistortMainTex;12;1;[Enum];Create;True;0;2;off;0;on;1;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;155;-2921.573,486.9138;Inherit;False;Property;_MainTex_rotat;MainTex_rotat;41;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-3411,92;Inherit;False;Constant;_Float1;Float 1;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-3507.26,-57.07033;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;65;-3428.507,174.29;Inherit;False;60;DistortUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;139;-701.9778,1780.096;Inherit;False;Constant;_Float6;Float 6;42;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;44;-3811.802,651.191;Inherit;False;2788.862;811.9171;Disslove;27;49;40;48;42;120;71;46;38;70;122;35;121;47;27;51;45;28;29;25;30;33;159;160;161;162;163;177;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;179;-251.8967,1966.906;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;360;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;118;-3123,108;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;121;-3767.883,1105.054;Inherit;False;Property;_DistortDisTex;DistortDisTex;13;1;[Enum];Create;True;0;2;off;0;on;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-3751.883,929.0543;Inherit;False;Constant;_Float2;Float 2;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-3189.262,323.4048;Inherit;False;Property;_MainTex_Uspeed;MainTex_Uspeed;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;132;-622.3112,1510.174;Inherit;False;0;52;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;159;-3346.516,1181.831;Inherit;False;Property;_DIssolve_rotat;DIssolve_rotat;42;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;178;-2634.505,460.187;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;360;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;138;-549.772,1782.045;Inherit;False;Property;_CustomdataMaskUV;CustomdataMaskUV;37;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PiNode;153;-177.401,1841.145;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;70;-3762.637,1013.132;Inherit;False;60;DistortUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;127;-3229.521,-161.772;Inherit;False;Property;_CustomdataMainTexUV;CustomdataMainTexUV;34;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-3184.865,-24.55683;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-3187.262,403.4047;Inherit;False;Property;_MainTex_Vspeed;MainTex_Vspeed;9;0;Create;True;0;0;0;False;0;False;0;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;114;-2656.002,3878.032;Inherit;False;1527;468.6843;softedge;7;105;110;108;107;109;106;111;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;-2980.262,354.4047;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;133;-301.6036,1512.467;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-2901.351,-43.08985;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-3638.897,1220.713;Inherit;False;Property;_DisTex_Uspeed;DisTex_Uspeed;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-3636.897,1300.715;Inherit;False;Property;_DisTex_Vspeed;DisTex_Vspeed;22;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;122;-3463.883,945.0543;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PiNode;156;-2583.573,336.9138;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;177;-3073.505,1255.187;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;360;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;154;-35.401,1940.145;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-3547.392,796.3203;Inherit;False;0;25;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PiNode;160;-2954.516,1153.831;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;48;-3415.897,1307.715;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;149;-153.4014,1571.145;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldNormalVector;107;-2606.002,4134.717;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-2441.573,435.9138;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;11;-2781.201,193.0904;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-3262.13,872.8496;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;105;-2582.171,3928.032;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WireNode;152;33.62984,1663.632;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-2553.13,1257.2;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-2554.926,1167.549;Inherit;False;Property;_DIssloveFactor;DIssloveFactor;18;0;Create;True;0;0;0;False;0;False;1;0.797;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;158;-2363.573,190.9138;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;106;-2367.282,4000.779;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;49;-3143.729,1015.224;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;161;-2812.516,1252.831;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;108;-2137.003,4000.716;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-2167.003,4230.717;Inherit;False;Property;_softFacotr;softFacotr;31;0;Create;True;0;0;0;False;0;False;1;0;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2286.926,1357.549;Inherit;False;Property;_DIssloveSoft;DIssloveSoft;19;0;Create;True;0;0;0;False;0;False;0.8235294;0.542;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;162;-2733.431,1015.424;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;163;-2246.66,1224.828;Inherit;False;Property;_CustomdataDis;CustomdataDis;43;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-2171.418,168.4658;Inherit;True;Property;_MainTex;MainTex;5;0;Create;True;0;0;0;False;0;False;-1;None;658f2f34af434c44a8fe358f3e5d10a2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;52;65.8403,1742.782;Inherit;True;Property;_MaskTex;MaskTex;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;109;-1867.003,3973.716;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;135;349.9389,1801.122;Inherit;False;Property;_MaskAlphaRA;MaskAlphaRA;36;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;172;-879.1559,-235.0706;Inherit;False;1472.581;1156.105;alpha;16;116;142;128;129;126;130;43;168;137;39;115;169;88;89;174;175;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1895.634,420.8288;Inherit;False;Property;_MainAlpha;MainAlpha;6;0;Create;True;0;0;0;False;0;False;1;1;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;30;-1937.925,1070.549;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;5;-2042.036,-188.151;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;25;-2285.419,983.7234;Inherit;True;Property;_DissloveTex;DissloveTex;17;0;Create;True;0;0;0;False;0;False;-1;None;658f2f34af434c44a8fe358f3e5d10a2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;8;-2081.951,-18.12685;Inherit;False;Property;_MainColor;MainColor;7;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,0.6179246,0.6179246,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;125;-1842.34,215.7968;Inherit;False;Property;_MainTex_ar;MainTex_a/r;33;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;141;305.4109,1650.349;Inherit;False;Property;_Mask_scale;Mask_scale;38;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-835.4634,654.165;Float;False;Property;_DepthfadeFactor;DepthfadeFactor;32;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1594.177,118.102;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;27;-1716.042,1012.961;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;111;-1523.003,3972.716;Inherit;False;softedge;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;561.4107,1785.348;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;116;-819.4634,494.165;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-1390.115,113.1548;Inherit;False;MainTexAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;136;743.4008,1799.397;Inherit;False;MaskAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;115;-829.1559,314.9608;Inherit;False;111;softedge;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;-1425.445,1118.259;Inherit;False;DisAplha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-414.9805,453.4714;Inherit;False;Constant;_Float8;Float 8;48;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;169;-804.9382,223.0366;Inherit;False;Constant;_Float5;Float 5;47;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;88;-595.4634,574.165;Inherit;False;True;True;False;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;-714.3308,44.41697;Inherit;False;37;MainTexAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;174;-272.3294,543.5131;Inherit;False;Property;_Depthfadeon;Depthfadeon;47;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;137;-479.6729,-185.0706;Inherit;False;136;MaskAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;168;-621.7767,285.0652;Inherit;False;Property;_FNLfanxiangkaiguan;FNLfanxiangkaiguan;44;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;43;-713.0215,138.4588;Inherit;False;42;DisAplha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;99;-2592.674,3234.404;Inherit;False;1729;481;fnl;7;94;96;91;93;92;97;95;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;83;-2667.627,2119.888;Inherit;False;1558.463;887.9036;VTO;14;76;77;75;73;74;72;78;79;80;81;82;85;87;171;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-2467.674,3297.404;Inherit;False;Property;_fnl_sacle;fnl_sacle;29;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-323.6386,31.50176;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1772.205,-65.88351;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-2565.674,3474.404;Inherit;False;Property;_fnl_power;fnl_power;28;0;Create;True;0;0;0;False;0;False;1;0;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-2605.364,2404.494;Inherit;False;Property;_VTOTex_Vspeed;VTOTex_Vspeed;26;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;129;-283.1623,255.3162;Inherit;False;Property;_MainAlphaPower;MainAlphaPower;35;0;Create;True;0;0;0;False;0;False;1;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;126;-185.2396,29.56677;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;91;-2286.286,3291.2;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;36;-1634.352,-70.14369;Inherit;False;MainColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-2607.364,2312.793;Inherit;False;Property;_VTOTex_Uspeed;VTOTex_Uspeed;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;-1662.509,910.3918;Inherit;False;36;MainColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;128;-28.19977,-1.213032;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;76;-2372.664,2388.093;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;94;-1975.674,3291.404;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;75;-2617.627,2169.888;Inherit;False;0;72;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;35;-1692.503,701.191;Inherit;False;Property;_DIssloveColor;DIssloveColor;20;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,0.4987022,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;96;-1476,3461;Inherit;False;Property;_fnl_color;fnl_color;30;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;85;-2443.228,2728.387;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;87;-2352.939,2625.345;Inherit;False;Constant;_Float3;Float 3;32;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;33;-1431.704,963.0919;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-1243,3292;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;142;147.1084,-6.815523;Inherit;False;MainAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;77;-2212.664,2364.092;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;148;593.7297,-128.0988;Inherit;False;142;MainAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;97;-1086,3287;Inherit;False;fnlColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;147;598.7297,-266.0988;Inherit;False;Constant;_Float7;Float 7;44;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;72;-1954.938,2334.383;Inherit;True;Property;_VTOTex;VTOTex;23;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;171;-2176.458,2687.421;Inherit;False;Property;_ToggleSwitch0;Toggle Switch0;46;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;79;-1833.289,2529.795;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-1246.94,957.2584;Inherit;False;DisColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;81;-1944.887,2777.792;Inherit;True;Property;_VTOMaskTex;VTOMaskTex;27;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;80;-1809.866,2681.348;Inherit;False;Property;_VTOFactor;VTOFactor;24;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-1506.763,2361.709;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;146;782.7297,-199.0988;Inherit;False;Property;_AlphaAdd;AlphaAdd;39;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;167;834.8112,-356.2628;Inherit;False;Constant;_Float4;Float 4;46;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;101;805.3436,-532.8822;Inherit;False;97;fnlColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;777.3591,-25.29684;Inherit;False;40;DisColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;1062.73,-43.09875;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;170;1032.233,-435.8027;Inherit;False;Property;_FNLkaiguan;FNLkaiguan;45;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;10;-3130.546,-636.0404;Inherit;False;1063.897;312.1366;Comment;4;2;3;4;123;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;82;-1333.164,2356.198;Inherit;False;VTO;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;123;-2794.601,-583.2109;Float;False;Property;_Ztest;Ztest;2;1;[Enum];Create;True;0;1;Option1;0;1;UnityEngine.Rendering.CompareFunction;True;0;False;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-3080.546,-586.0395;Float;False;Property;_Scr;Scr;3;1;[Enum];Create;True;0;1;Option1;0;1;UnityEngine.Rendering.BlendMode;True;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;165;1306.003,-132.2085;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;143;982.2925,95.62549;Inherit;False;142;MainAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-2938.145,-585.0394;Float;False;Property;_Cullmode;Cullmode;1;1;[Enum];Create;True;0;1;Option1;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;988.0677,171.1676;Inherit;False;82;VTO;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-3080.546,-492.0387;Float;False;Property;_Dst;Dst;4;1;[Enum];Create;True;0;1;Option1;0;1;UnityEngine.Rendering.BlendMode;True;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1497.838,-92.08209;Float;False;True;-1;7;ASEMaterialInspector;0;0;Unlit;VFX/Pandavfx;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;7;True;123;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;True;3;10;True;4;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;True;2;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;58;0;55;0
WireConnection;58;1;56;0
WireConnection;59;0;57;0
WireConnection;59;2;58;0
WireConnection;54;1;59;0
WireConnection;61;0;54;1
WireConnection;61;1;54;2
WireConnection;63;0;64;0
WireConnection;63;1;61;0
WireConnection;60;0;63;0
WireConnection;179;0;151;0
WireConnection;118;0;67;0
WireConnection;118;1;65;0
WireConnection;118;2;119;0
WireConnection;178;0;155;0
WireConnection;138;0;139;0
WireConnection;138;1;134;0
WireConnection;153;0;179;0
WireConnection;127;0;20;0
WireConnection;127;1;16;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;133;0;132;0
WireConnection;133;1;138;0
WireConnection;17;0;127;0
WireConnection;17;1;15;0
WireConnection;17;2;118;0
WireConnection;122;0;120;0
WireConnection;122;1;70;0
WireConnection;122;2;121;0
WireConnection;156;0;178;0
WireConnection;177;0;159;0
WireConnection;154;0;153;0
WireConnection;160;0;177;0
WireConnection;48;0;46;0
WireConnection;48;1;47;0
WireConnection;149;0;133;0
WireConnection;149;2;154;0
WireConnection;157;0;156;0
WireConnection;11;0;17;0
WireConnection;11;2;14;0
WireConnection;71;0;45;0
WireConnection;71;1;122;0
WireConnection;152;0;149;0
WireConnection;158;0;11;0
WireConnection;158;2;157;0
WireConnection;106;0;105;0
WireConnection;106;1;107;0
WireConnection;49;0;71;0
WireConnection;49;2;48;0
WireConnection;161;0;160;0
WireConnection;108;0;106;0
WireConnection;162;0;49;0
WireConnection;162;2;161;0
WireConnection;163;0;28;0
WireConnection;163;1;51;3
WireConnection;1;1;158;0
WireConnection;52;1;152;0
WireConnection;109;0;108;0
WireConnection;109;1;110;0
WireConnection;135;0;52;4
WireConnection;135;1;52;1
WireConnection;30;0;163;0
WireConnection;30;1;29;0
WireConnection;25;1;162;0
WireConnection;125;0;1;4
WireConnection;125;1;1;1
WireConnection;7;0;5;4
WireConnection;7;1;125;0
WireConnection;7;2;8;4
WireConnection;7;3;9;0
WireConnection;27;0;25;1
WireConnection;27;1;30;0
WireConnection;27;2;163;0
WireConnection;111;0;109;0
WireConnection;140;0;141;0
WireConnection;140;1;135;0
WireConnection;37;0;7;0
WireConnection;136;0;140;0
WireConnection;42;0;27;0
WireConnection;88;1;116;0
WireConnection;88;0;89;0
WireConnection;174;0;175;0
WireConnection;174;1;88;0
WireConnection;168;0;169;0
WireConnection;168;1;115;0
WireConnection;130;0;137;0
WireConnection;130;1;39;0
WireConnection;130;2;43;0
WireConnection;130;3;168;0
WireConnection;130;4;174;0
WireConnection;6;0;5;0
WireConnection;6;1;1;0
WireConnection;6;2;8;0
WireConnection;126;0;130;0
WireConnection;91;2;92;0
WireConnection;91;3;93;0
WireConnection;36;0;6;0
WireConnection;128;0;126;0
WireConnection;128;1;129;0
WireConnection;76;0;73;0
WireConnection;76;1;74;0
WireConnection;94;0;91;0
WireConnection;33;0;35;0
WireConnection;33;1;38;0
WireConnection;33;2;27;0
WireConnection;95;0;94;0
WireConnection;95;1;96;0
WireConnection;142;0;128;0
WireConnection;77;0;75;0
WireConnection;77;2;76;0
WireConnection;97;0;95;0
WireConnection;72;1;77;0
WireConnection;171;0;87;0
WireConnection;171;1;85;4
WireConnection;40;0;33;0
WireConnection;78;0;72;1
WireConnection;78;1;79;0
WireConnection;78;2;80;0
WireConnection;78;3;81;1
WireConnection;78;4;171;0
WireConnection;146;0;147;0
WireConnection;146;1;148;0
WireConnection;145;0;146;0
WireConnection;145;1;131;0
WireConnection;170;0;101;0
WireConnection;170;1;167;0
WireConnection;82;0;78;0
WireConnection;165;0;170;0
WireConnection;165;1;145;0
WireConnection;0;2;165;0
WireConnection;0;9;143;0
WireConnection;0;11;84;0
ASEEND*/
//CHKSM=071522EDA1080C665648576F450D8C579BF6DE08