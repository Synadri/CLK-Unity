Shader "Graph/Point Surface"{

    Properties{ // Creates config option
        _Smoothness("Smoothness", Range(0,1)) = 0.5
    }

    SubShader{
        CGPROGRAM
            // Compiler directives
            #pragma surface ConfigureSurface Standard fullforwardshadows
            #pragma target 3.0
            
            struct Input{
                float3 worldPos;
            };

            float _Smoothness;

            void ConfigureSurface(Input input, inout SurfaceOutputStandard surface){
                surface.Albedo.rg = input.worldPos.xy * 0.5 + 0.5;
                surface.Smoothness = _Smoothness;
            }
        ENDCG
    }

    FallBack "Diffuse"
}