#version 430 core

const float bias = 0.005;

uniform sampler2D in_Texture;
uniform sampler2DShadow in_ShadowTexture;
uniform vec3 in_LightPosition;
uniform float alpha_cutoff;

layout(location = 0) in vec2 UV;
layout(location = 1) in vec3 Position_worldspace;
layout(location = 2) in vec3 Normal_cameraspace;
layout(location = 3) in vec3 EyeDirection_cameraspace;
layout(location = 4) in vec3 LightDirection_cameraspace;
layout(location = 5) in vec4 shadowUV;

layout(location = 0) out vec4 color;

void main(void) {
    vec4 LightColor = vec4(1, 1, 1, 1);
    float LightPower = 50.0f;

    vec4 MaterialDiffuseColor = texture2D(in_Texture, UV);
    vec4 MaterialAmbientColor = vec4(0.1, 0.1, 0.1, 1) * MaterialDiffuseColor;
    vec4 MaterialSpecularColor = vec4(0.5, 0.5, 0.5, 1);

    float distance = length(in_LightPosition - Position_worldspace) / 10;

    vec3 n = normalize(Normal_cameraspace);
    vec3 l = normalize(LightDirection_cameraspace);
    float cosTheta = clamp(dot(n, l), 0, 1);

    vec3 E = normalize(EyeDirection_cameraspace);
    vec3 R = reflect(-l, n);
    float cosAlpha = clamp(dot(E, R), 0, 1);

    //float visibility = textureProj(in_ShadowTexture, vec4(shadowUV.xy, shadowUV.z - bias, shadowUV.w));
    float visibility = (
        textureProjOffset(in_ShadowTexture, vec4(shadowUV.xy, shadowUV.z - bias, shadowUV.w), ivec2(-1, 1))
        + textureProjOffset(in_ShadowTexture, vec4(shadowUV.xy, shadowUV.z - bias, shadowUV.w), ivec2(1, 1))
        + textureProjOffset(in_ShadowTexture, vec4(shadowUV.xy, shadowUV.z - bias, shadowUV.w), ivec2(-1, -1))
        + textureProjOffset(in_ShadowTexture, vec4(shadowUV.xy, shadowUV.z - bias, shadowUV.w), ivec2(1, -1))
    ) / 4.0;

    color = MaterialAmbientColor
        + visibility * MaterialDiffuseColor * LightColor * LightPower * cosTheta / (distance * distance)
        + visibility * MaterialSpecularColor * LightColor * LightPower * pow(cosAlpha, 5) / (distance * distance);

    if(color.a < alpha_cutoff)
        discard;
}
