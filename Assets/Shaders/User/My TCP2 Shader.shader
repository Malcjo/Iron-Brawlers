// Toony Colors Pro+Mobile 2
// (c) 2014-2020 Jean Moreno

Shader "Toony Colors Pro 2/User/My TCP2 Shader"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_Color ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		_MainTex ("Albedo", 2D) = "white" {}
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		_RampThreshold ("Threshold", Range(0.01,1)) = 0.5
		_RampSmoothing ("Smoothing", Range(0.001,1)) = 0.5
		[TCP2Separator]
		
		[Header(Vertex Waves Animation)]
		_WavesSpeed ("Speed", Float) = 2
		_WavesHeight ("Height", Float) = 0.1
		_WavesFrequency ("Frequency", Range(0,10)) = 1
		
		[Header(Depth Based Effects)]
		_FoamSpread ("Foam Spread", Range(0,5)) = 2
		_FoamStrength ("Foam Strength", Range(0,1)) = 0.8
		_FoamColor ("Foam Color (RGB) Opacity (A)", Color) = (0.9,0.9,0.9,1)
		_FoamTex ("Foam Texture Custom", 2D) = "black" {}
		
		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}
		
		CGINCLUDE

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"	// needed for LightColor

		// Shader Properties
		sampler2D _MainTex;
		sampler2D _FoamTex;
		
		// Shader Properties
		float _WavesFrequency;
		float _WavesHeight;
		float _WavesSpeed;
		float4 _MainTex_ST;
		fixed4 _Color;
		float _FoamSpread;
		float _FoamStrength;
		fixed4 _FoamColor;
		float4 _FoamTex_ST;
		float _RampThreshold;
		float _RampSmoothing;
		fixed4 _HColor;
		fixed4 _SColor;
		
		UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

		ENDCG

		// Main Surface Shader

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha nolightmap nofog nolppv
		#pragma target 3.0

		//================================================================
		// STRUCTS

		//Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord0 : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
		#if defined(LIGHTMAP_ON) && defined(DIRLIGHTMAP_COMBINED)
			half4 tangent : TANGENT;
		#endif
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Input
		{
			float4 screenPosition;
			float2 texcoord0;
		};

		//================================================================
		// VERTEX FUNCTION

		void vertex_surface(inout appdata_tcp2 v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);

			// Texture Coordinates
			output.texcoord0.xy = v.texcoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
			// Shader Properties Sampling
			float __wavesFrequency = ( _WavesFrequency );
			float __wavesHeight = ( _WavesHeight );
			float __wavesSpeed = ( _WavesSpeed );
			float4 __wavesSinOffsets1 = ( float4(1,2.2,0.6,1.3) );
			float4 __wavesPhaseOffsets1 = ( float4(1,1.3,2.2,0.4) );

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			
			// Vertex water waves
			float _waveFrequency = __wavesFrequency;
			float _waveHeight = __wavesHeight;
			float3 _vertexWavePos = worldPos.xyz * _waveFrequency;
			float _phase = _Time.y * __wavesSpeed;
			half4 vsw_offsets_x = __wavesSinOffsets1;
			half4 vsw_ph_offsets_x = __wavesPhaseOffsets1;
			half4 waveXZ = sin((_vertexWavePos.xxzz * vsw_offsets_x) + (_phase.xxxx * vsw_ph_offsets_x));
			float waveFactorX = dot(waveXZ.xy, 1) * _waveHeight / 2;
			float waveFactorZ = dot(waveXZ.zw, 1) * _waveHeight / 2;
			v.vertex.xyz += v.normal.xyz * (waveFactorX + waveFactorZ);
			half4 waveXZn = cos((_vertexWavePos.xxzz * vsw_offsets_x) + (_phase.xxxx * vsw_ph_offsets_x)) * (vsw_offsets_x / 2);
			float xn = -_waveHeight * (waveXZn.x + waveXZn.y);
			float zn = -_waveHeight * (waveXZn.z + waveXZn.w);
			v.normal = normalize(float3(xn, 1, zn));
			float4 clipPos = UnityObjectToClipPos(v.vertex);

			//Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			output.screenPosition = screenPos;
			COMPUTE_EYEDEPTH(output.screenPosition.z);

		}

		//================================================================

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Specular;
			half Gloss;
			half Alpha;

			Input input;
			
			// Shader Properties
			float __rampThreshold;
			float __rampSmoothing;
			float3 __highlightColor;
			float3 __shadowColor;
			float __ambientIntensity;
		};

		//================================================================
		// SURFACE FUNCTION

		void surf(Input input, inout SurfaceOutputCustom output)
		{
			
			// Shader Properties Sampling
			float4 __albedo = ( tex2D(_MainTex, input.texcoord0.xy).rgba );
			float4 __mainColor = ( _Color.rgba );
			float __alpha = ( __albedo.a * __mainColor.a );
			float __foamSpread = ( _FoamSpread );
			float __foamStrength = ( _FoamStrength );
			float4 __foamColor = ( _FoamColor.rgba );
			float3 __foamTextureCustom = ( tex2D(_FoamTex, input.texcoord0.xy * _FoamTex_ST.xy + _FoamTex_ST.zw).rgb );
			float __foamMask = ( 1.0 );
			output.__rampThreshold = ( _RampThreshold );
			output.__rampSmoothing = ( _RampSmoothing );
			output.__highlightColor = ( _HColor.rgb );
			output.__shadowColor = ( _SColor.rgb );
			output.__ambientIntensity = ( 1.0 );

			output.input = input;

			// Sample depth texture and calculate difference with local depth
			float sceneDepth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(input.screenPosition));
			if (unity_OrthoParams.w > 0.0)
			{
				// Orthographic camera
				#if defined(UNITY_REVERSED_Z)
					sceneDepth = 1.0 - sceneDepth;
				#endif
				sceneDepth = (sceneDepth * _ProjectionParams.z) + _ProjectionParams.y;
			}
			else
			{
				// Perspective camera
				sceneDepth = LinearEyeDepth(sceneDepth);
			}
			
			float localDepth = input.screenPosition.z;
			float depthDiff = abs(sceneDepth - localDepth);

			output.Albedo = __albedo.rgb;
			output.Alpha = __alpha;
			
			output.Albedo *= __mainColor.rgb;
			
			// Depth-based water foam
			half foamSpread = __foamSpread;
			half foamStrength = __foamStrength;
			half4 foamColor = __foamColor;
			
			half3 foam = __foamTextureCustom;
			float foamDepth = saturate(foamSpread * depthDiff) * (1.0 - __foamMask);
			half foamTerm = (step(foam.rgb, saturate(foamStrength - foamDepth)) * saturate(foamStrength - foamDepth)) * foamColor.a;
			output.Albedo.rgb = lerp(output.Albedo.rgb, foamColor.rgb, foamTerm);
			output.Alpha = lerp(output.Alpha, foamColor.a, foamTerm);

		}

		//================================================================
		// LIGHTING FUNCTION

		inline half4 LightingToonyColorsCustom(inout SurfaceOutputCustom surface, UnityGI gi)
		{
			half3 lightDir = gi.light.dir;
			#if defined(UNITY_PASS_FORWARDBASE)
				half3 lightColor = _LightColor0.rgb;
				half atten = surface.atten;
			#else
				//extract attenuation from point/spot lights
				half3 lightColor = _LightColor0.rgb;
				half atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / max(_LightColor0.r, max(_LightColor0.g, _LightColor0.b));
			#endif

			half3 normal = normalize(surface.Normal);
			half ndl = dot(normal, lightDir);
			half3 ramp;
			
			#define		RAMP_THRESHOLD	surface.__rampThreshold
			#define		RAMP_SMOOTH		surface.__rampSmoothing
			ndl = saturate(ndl);
			ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, ndl);
			half3 rampGrayscale = ramp;

			//Apply attenuation (shadowmaps & point/spot lights attenuation)
			ramp *= atten;

			//Highlight/Shadow Colors
			#if !defined(UNITY_PASS_FORWARDBASE)
				ramp = lerp(half3(0,0,0), surface.__highlightColor, ramp);
			#else
				ramp = lerp(surface.__shadowColor, surface.__highlightColor, ramp);
			#endif

			//Output color
			half4 color;
			color.rgb = surface.Albedo * lightColor.rgb * ramp;
			color.a = surface.Alpha;

			// Apply indirect lighting (ambient)
			half occlusion = 1;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				half3 ambient = gi.indirect.diffuse;
				ambient *= surface.Albedo * occlusion * surface.__ambientIntensity;

				color.rgb += ambient;
			#endif

			return color;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom surface, UnityGIInput data, inout UnityGI gi)
		{
			half3 normal = surface.Normal;

			//GI without reflection probes
			gi = UnityGlobalIllumination(data, 1.0, normal); // occlusion is applied in the lighting function, if necessary

			surface.atten = data.atten; // transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb; // remove attenuation

		}

		ENDCG

	}

	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(unity:"2020.1.3f1";ver:"2.6.0";tmplt:"SG2_Template_Default";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","VERTEX_SIN_WAVES","VSW_2","VSW_WORLDPOS","VERTEX_SIN_NORMALS","VSW_FOLLOWNORM","DEPTH_BUFFER_FOAM"];flags:list[];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0"];shaderProperties:list[];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False)) */
/* TCP_HASH fe3f7e9b21b853edee1ca1598274da44 */
