Shader "Custom/SolidColorSprite_KeepOne"
{
    Properties
    {
        _SolidColor ("Solid Color", Color) = (0.8745, 0.9725, 0.8156, 1) // #DFF8D0
        _KeepColor ("Keep Color", Color) = (0.0196, 0.0941, 0.1215, 1)   // #05181F
        _KeepAlpha ("Keep Alpha", Range(0,1)) = 0.502
        _ColorTolerance ("Color Tolerance", Range(0,0.5)) = 0.06
        _AlphaTolerance ("Alpha Tolerance", Range(0,1)) = 0.06
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _SolidColor;
            float4 _KeepColor;
            float _KeepAlpha;
            float _ColorTolerance;
            float _AlphaTolerance;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);

                // porównanie kolorów - odległość w przestrzeni RGB (squared)
                float3 diff = tex.rgb - _KeepColor.rgb;
                float dist2 = dot(diff, diff);
                float tol2 = _ColorTolerance * _ColorTolerance;

                // sprawdź alfa
                float alphaDiff = abs(tex.a - _KeepAlpha);

                if (dist2 <= tol2 && alphaDiff <= _AlphaTolerance)
                {
                    // zostawiamy oryginalny piksel (kolor i alfa)
                    return tex;
                }

                // nadpisujemy RGB na wybrany kolor, ale zachowujemy alfę oryginalnego sprite'a
                return fixed4(_SolidColor.rgb, tex.a);
            }
            ENDCG
        }
    }
}
