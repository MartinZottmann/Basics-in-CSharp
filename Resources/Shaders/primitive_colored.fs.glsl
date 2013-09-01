#version 430 core

uniform vec4 in_Color;

layout(location = 0) out vec4 out_Color;

void main(void) {
    out_Color = in_Color;
}
