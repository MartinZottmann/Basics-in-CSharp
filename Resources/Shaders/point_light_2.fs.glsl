#version 330 core

uniform sampler2D in_Texture;
uniform vec4 in_AmbientLight;
uniform vec4 in_LightColor;
uniform float in_Shininess;
uniform float in_Strength;

in vec2 uv;
in vec3 Normal;
in vec3 LightDirection;
in vec3 HalfVector;
in float Attenuation;

out vec4 FragColor;

void main(void)
{
    float diffuse = max(0.0, dot(Normal, LightDirection));
    float specular = max(0.0, dot(Normal, HalfVector));
    if (diffuse == 0.0)
        specular = 0.0;
    else
        specular = pow(specular, in_Shininess) * in_Strength;
    vec4 scatteredLight = in_AmbientLight + in_LightColor * diffuse * Attenuation;
    vec4 reflectedLight = in_LightColor * specular * Attenuation;
    FragColor = min(texture2D(in_Texture, uv) * scatteredLight + reflectedLight, vec4(1.0));
}
