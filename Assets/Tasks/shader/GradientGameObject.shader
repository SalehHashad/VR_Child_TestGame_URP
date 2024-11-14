Shader "Custom/GradientGameObject"
{
    Properties
    {
        _Color1 ("Bottom Color", Color) = (0, 0.7, 0.74, 1)
        _Color2 ("Top Color", Color) = (0.97, 0.67, 0.51, 1)
        [Space]
        _Intensity ("Intensity", Range(0, 2)) = 1.0
        _Exponent ("Exponent", Range(0, 3)) = 1.0
        _Direction ("Direction", Vector) = (0, 1, 0, 0) // Directional light or axis
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    struct appdata
    {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD0; // This can be used for more texture control if needed
    };
    
    struct v2f
    {
        float4 position : SV_POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    half4 _Color1;
    half4 _Color2;
    half3 _Direction;
    half _Intensity;
    half _Exponent;

    // Vertex shader
    v2f vert(appdata v)
    {
        v2f o;
        o.position = UnityObjectToClipPos(v.position);
        o.texcoord = v.position.xyz; // Pass the world space position to fragment shader
        return o;
    }

    // Fragment shader
    fixed4 frag(v2f i) : COLOR
    {
        // Calculate the direction using the normal of the object and _Direction
        half d = dot(normalize(i.texcoord), _Direction) * 0.5f + 0.5f;
        
        // Apply the gradient effect
        return lerp(_Color1, _Color2, pow(d, _Exponent)) * _Intensity;
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZWrite On
            ZTest LEqual
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }

    Fallback "Diffuse"
}
