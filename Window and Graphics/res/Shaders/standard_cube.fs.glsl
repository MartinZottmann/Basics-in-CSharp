#version 330 core

uniform samplerCube in_Texture;
uniform vec3 in_LightPosition;

in vec3 UV;
in vec3 Position_worldspace;
in vec3 Normal_cameraspace;
in vec3 EyeDirection_cameraspace;
in vec3 LightDirection_cameraspace;

out vec4 color;

void main() {
    vec4 LightColor = vec4(1, 1, 1, 1);
    float LightPower = 50.0f;

    vec4 MaterialDiffuseColor = texture(in_Texture, UV);
    vec4 MaterialAmbientColor = vec4(0, 0, 0, 1) * MaterialDiffuseColor;
    vec4 MaterialSpecularColor = vec4(0.5, 0.5, 0.5, 1);

    float distance = length(in_LightPosition - Position_worldspace) / 10;

    vec3 n = normalize(Normal_cameraspace);
    vec3 l = normalize(LightDirection_cameraspace);
    float cosTheta = clamp(dot(n, l), 0, 1);

    vec3 E = normalize(EyeDirection_cameraspace);
    vec3 R = reflect(-l, n);
    float cosAlpha = clamp(dot(E, R), 0, 1);

    color = MaterialAmbientColor
        + MaterialDiffuseColor * LightColor * LightPower * cosTheta / (distance * distance)
        + MaterialSpecularColor * LightColor * LightPower * pow(cosAlpha, 5) / (distance * distance);
}
