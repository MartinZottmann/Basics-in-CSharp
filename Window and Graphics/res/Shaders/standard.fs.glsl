#version 330 core

uniform sampler2D in_Texture;
uniform vec3 in_LightPosition;

in vec2 UV;
in vec3 Position_worldspace;
in vec3 Normal_cameraspace;
in vec3 EyeDirection_cameraspace;
in vec3 LightDirection_cameraspace;

out vec3 color;

void main() {
	vec3 LightColor = vec3(1,1,1);
	float LightPower = 50.0f;

	vec3 MaterialDiffuseColor = texture2D(in_Texture, UV).rgb;
	vec3 MaterialAmbientColor = vec3(0, 0, 0) * MaterialDiffuseColor;
	vec3 MaterialSpecularColor = vec3(0.5, 0.5, 0.5);

	float distance = length(in_LightPosition - Position_worldspace);

	vec3 n = normalize( Normal_cameraspace );
	vec3 l = normalize( LightDirection_cameraspace );
	float cosTheta = clamp(dot(n, l), 0, 1);

	vec3 E = normalize(EyeDirection_cameraspace);
	vec3 R = reflect(-l, n);
	float cosAlpha = clamp(dot(E, R), 0, 1);

	color = MaterialAmbientColor
		+ MaterialDiffuseColor * LightColor * LightPower * cosTheta / (distance * distance)
		+ MaterialSpecularColor * LightColor * LightPower * pow(cosAlpha, 5) / (distance * distance);

}