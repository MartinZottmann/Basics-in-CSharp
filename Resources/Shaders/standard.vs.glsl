#version 330 core

uniform mat4 in_ModelViewProjection;
uniform mat4 in_ModelView;
uniform mat4 in_View;
uniform mat4 in_Model;
uniform mat4 in_NormalView;
uniform vec3 in_LightPosition;

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec3 in_Normal;
layout(location = 2) in vec2 in_Texcoord;

out vec2 UV;
out vec3 Position_worldspace;
out vec3 Normal_cameraspace;
out vec3 EyeDirection_cameraspace;
out vec3 LightDirection_cameraspace;

void main() {
    gl_Position = in_ModelViewProjection * vec4(in_Position, 1);

    Position_worldspace = (in_Model * vec4(in_Position, 1)).xyz;

    vec3 vertexPosition_cameraspace = (in_ModelView * vec4(in_Position, 1)).xyz;
    EyeDirection_cameraspace = vec3(0, 0, 0) - vertexPosition_cameraspace;

    vec3 LightPosition_cameraspace = (in_View * vec4(in_LightPosition, 1)).xyz;
    LightDirection_cameraspace = LightPosition_cameraspace + EyeDirection_cameraspace;

    Normal_cameraspace = (in_NormalView * vec4(in_Normal, 0)).xyz;

    UV = in_Texcoord;
}