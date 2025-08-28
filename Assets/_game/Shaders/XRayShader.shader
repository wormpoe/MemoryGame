Shader "Custom/XRayShader"
{
	SubShader
	{
		Tags {"Queue" = "Transparent+1"}

		Pass { Blend Zero One }
	}
	SubShader
	{
		Tags {"Queue" = "Transparent+3"}
		Pass { }
	}
}
