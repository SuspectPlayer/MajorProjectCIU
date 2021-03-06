Shader "Hidden/Nature/Tree Soft Occlusion Bark Rendertex Move" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0)
		_MainTex ("Main Texture", 2D) = "white" {}
		_BaseLight ("Base Light", Range(0, 1)) = 0.35
		_AO ("Amb. Occlusion", Range(0, 10)) = 2.4
		
		// These are here only to provide default values
		_Scale ("Scale", Vector) = (1,1,1,1)
		_SquashAmount ("Squash", Float) = 1
	}
	
	SubShader {
		Fog { Mode Off }
		Pass {
			Lighting On
		
			CGPROGRAM
			#pragma vertex bark
			#pragma fragment frag 
			#define WRITE_ALPHA_1 1
			#define USE_CUSTOM_LIGHT_DIR 1
			#include "SH_Vertex_Move.cginc"

			sampler2D _MainTex;
			
			fixed4 frag(v2f input) : COLOR
			{
				fixed4 col = input.color;
				col.rgb *= 2.0f * tex2D( _MainTex, input.uv.xy).rgb;
				return col;
			}
			ENDCG
		}
	}
	
	SubShader {
		Fog { Mode Off }
		Pass {
			Lighting On
			
			CGPROGRAM
			#pragma exclude_renderers gles xbox360 ps3
			#pragma vertex bark
			#define WRITE_ALPHA_1 1
			#define USE_CUSTOM_LIGHT_DIR 1
			#include "SH_Vertex_Move.cginc"
			ENDCG
			
			SetTexture [_MainTex] {
				combine primary * texture double, primary
			}
		}
	}
	
	Fallback Off
}