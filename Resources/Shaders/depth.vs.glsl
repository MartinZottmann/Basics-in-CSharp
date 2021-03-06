#version 430 core

uniform mat4 in_ModelViewProjection;

layout(location = 0) in vec3 in_Position;

void main(void) {
    gl_Position = in_ModelViewProjection * vec4(in_Position, 1);
}
