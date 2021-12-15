// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RefractionInvisible"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
        _BumpMap("Noise Map", 2D) = "bump" {}
        _Magnitude("Magnitude", Range(0,1)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent" "IgnoreProjector" = "True" }
        Blend One Zero
        GrabPass{"_GrabTexture"}
        
        // Pass
        // {
        //     CGPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag
        //     // make fog work
        //     #pragma multi_compile_fog

        //     #include "UnityCG.cginc"

        //     struct appdata
        //     {
        //         float4 vertex : POSITION;
        //         float2 uv : TEXCOORD0;
        //     };

        //     struct v2f
        //     {
        //         float2 uv : TEXCOORD0;
        //         float4 vertex : SV_POSITION;
        //     };

        //     sampler2D _MainTex;
        //     float4 _MainTex_ST;

        //     v2f vert (appdata v)
        //     {
        //         v2f o;
        //         float4 dis = float4(0.0f, sin(v.vertex.x + _Time.y *2), 0.0f, 0.0f);
        //         v.vertex += dis;
        //         o.vertex = UnityObjectToClipPos(v.vertex);
        //         o.uv = v.uv;
        //         return o;
        //     }

        //     fixed4 frag (v2f i) : SV_Target
        //     {
        //         // sample the texture
        //         fixed4 col = tex2D(_MainTex, i.uv);
        //         // apply fog
        //         UNITY_APPLY_FOG(i.fogCoord, col);
        //         return col;
        //     }
        //     ENDCG
        // }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 position : SV_POSITION;
            };

            sampler2D _GrabTexture;
            sampler2D _BumpMap;
            float _Magnitude;

            v2f vert (appdata IN)
            {
                v2f OUT;
                OUT.position = UnityObjectToClipPos(IN.vertex);
                OUT.uv = ComputeGrabScreenPos(OUT.position);
                return OUT;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 bump = tex2D(_BumpMap, i.uv);
                half2 distortion = UnpackNormal(bump).rg;
                // sample the texture
                i.uv.xy += distortion * sin(_Magnitude*_Time.y);
                fixed4 color = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uv)).r;
                return color;
            }
            ENDCG
        }
    }
}
