#version 330 core

uniform sampler2D in_Texture;
uniform vec4 in_AmbientLight;
uniform vec4 in_LightColor;
uniform vec3 in_LightPosition;
uniform float in_Shininess;
uniform float in_Strength;
uniform vec3 in_EyeDirection;
uniform float in_ConstantAttenuation;
uniform float in_LinearAttenuation;
uniform float in_QuadraticAttenuation;

in vec2 uv;
in vec3 Normal;
in vec3 Position;

out vec4 FragColor;

void main(void)
{
    vec3 lightDirection = in_LightPosition - vec3(Position);
    float lightDistance = length(lightDirection);
    lightDirection = lightDirection / lightDistance;
    float attenuation = 1.0 / (in_ConstantAttenuation + in_LinearAttenuation * lightDistance + in_QuadraticAttenuation * lightDistance * lightDistance);
    vec3 halfVector = normalize(lightDirection + in_EyeDirection);
    float diffuse = max(0.0, dot(Normal, lightDirection));
    float specular = max(0.0, dot(Normal, halfVector));
    if (diffuse == 0.0)
        specular = 0.0;
    else
        specular = pow(specular, in_Shininess) * in_Strength;
    vec4 scatteredLight = in_AmbientLight + in_LightColor * diffuse * attenuation;
    vec4 reflectedLight = in_LightColor * specular * attenuation;
    FragColor = min(texture2D(in_Texture, uv) * scatteredLight + reflectedLight, vec4(1.0));
}
