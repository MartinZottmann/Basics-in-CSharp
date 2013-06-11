#version 330 core

uniform sampler2D in_Texture;
uniform vec4 in_AmbientLight;
uniform vec4 in_LightColor;
uniform vec3 in_LightDirection;
uniform vec3 in_HalfVector;
uniform float in_Shininess;
uniform float in_Strength;

in vec2 uv;
in vec3 Normal;

out vec4 FragColor;

void main(void)
{
    float diffuse = max(0.0, dot(Normal, in_LightDirection));
    float specular = max(0.0, dot(Normal, in_HalfVector));
    if (diffuse == 0.0)
        specular = 0.0;
    else
        specular = pow(specular, in_Shininess);
    vec3 scatteredLight = in_AmbientLight + in_LightColor * diffuse;
    vec3 reflectedLight = in_LightColor * specular * in_Strength;

    vec3 rgb = min(texture2D(in_Texture, uv).rgb * scatteredLight + reflectedLight, vec3(1.0));
    FragColor = vec4(rgb, 1.0);
}
