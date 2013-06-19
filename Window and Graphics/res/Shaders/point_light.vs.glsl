#version 330 core

uniform mat4 in_ModelViewProjection;
uniform mat4 in_ModelView;
uniform mat4 in_NormalMatrix;

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec3 in_Normal;
layout(location = 2) in vec2 in_Texcoord;

out vec2 uv;
out vec3 Normal;
out vec3 Position;

void main(void)
{
    uv = in_Texcoord;
    Normal = normalize(mat3(in_NormalMatrix) * in_Normal);
    Position = (in_ModelView * vec4(in_Position, 1.0)).xyz;
    gl_Position = in_ModelViewProjection * vec4(in_Position, 1.0);
}
