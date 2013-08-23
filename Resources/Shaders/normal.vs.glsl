#version 410 core

uniform mat4 in_ModelViewProjection;

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec4 in_Color;

out vec4 Color;

void main(void) {
    gl_Position = in_ModelViewProjection * vec4(in_Position, 1);
    Color = in_Color;
}
