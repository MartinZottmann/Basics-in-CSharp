#version 330 core

uniform mat4 in_ModelViewProjection;

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec3 in_Normal;
layout(location = 2) in vec2 in_Texcoord;

out vec2 UV;

void main() {
    gl_Position = in_ModelViewProjection * vec4(in_Position, 1);
    UV = in_Texcoord;
}
