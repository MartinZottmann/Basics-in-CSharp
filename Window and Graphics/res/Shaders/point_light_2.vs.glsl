#version 330 core

uniform mat4 in_ModelViewProjection;
//uniform mat4 in_ModelView;
uniform mat4 in_NormalMatrix;
uniform vec3 in_LightPosition;
uniform vec3 in_EyeDirection;
uniform float in_ConstantAttenuation;
uniform float in_LinearAttenuation;
uniform float in_QuadraticAttenuation;

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec3 in_Normal;
layout(location = 2) in vec2 in_Texcoord;

out vec2 uv;
out vec3 Normal;
out vec3 LightDirection;
out vec3 HalfVector;
out float Attenuation;

void main(void)
{
    uv = in_Texcoord;
    Normal = normalize(vec3(in_NormalMatrix * vec4(in_Normal, 0.0)));
    LightDirection = in_LightPosition - in_Position;
    //LightDirection = vec3(in_ModelView * vec4(in_LightPosition, 0.0) - in_ModelView * vec4(in_Position, 1.0));
    float lightDistance = length(LightDirection);
    LightDirection = LightDirection / lightDistance;
    Attenuation = 1.0 / (in_ConstantAttenuation + in_LinearAttenuation * lightDistance + in_QuadraticAttenuation * lightDistance * lightDistance);
    HalfVector = normalize(LightDirection + in_EyeDirection);
    gl_Position = in_ModelViewProjection * vec4(in_Position, 1.0);
}
