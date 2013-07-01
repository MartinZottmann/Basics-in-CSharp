#version 330 core

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec3 in_Normal;
layout(location = 2) in vec2 in_Texcoord;

//uniform mat4 in_Projection;
//uniform mat4 in_ModelView;
uniform mat4 in_ModelViewProjection;
uniform mat4 in_NormalMatrix;

out vec2 uv;

out vec3 Normal;

void main(void)
{
    uv = in_Texcoord;
    Normal = normalize(mat3(in_NormalMatrix) * in_Normal);
    gl_Position = in_ModelViewProjection * vec4(in_Position, 1.0);
}
