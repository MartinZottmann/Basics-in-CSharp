#version 430 core

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec4 in_Color;

layout(location = 0) out vec4 out_Color;

void main(void) {
    gl_Position = vec4(in_Position, 1);
    out_Color = in_Color;
}
