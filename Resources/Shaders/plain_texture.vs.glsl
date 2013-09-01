#version 430 core

uniform mat4 in_ModelViewProjection;

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec3 in_Normal;
layout(location = 2) in vec2 in_Texcoord;

layout(location = 0) out vec2 out_UV;

void main(void) {
    gl_Position = in_ModelViewProjection * vec4(in_Position, 1);
    out_UV = in_Texcoord;
}
