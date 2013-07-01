#version 330 core

uniform mat4 PVM;

layout(location = 0) in vec3 in_Position;
layout(location = 1) in vec4 in_Color;

out vec4 Color;

void main(void) {
    gl_Position = PVM * vec4(in_Position, 1);
    Color = in_Color;
}
