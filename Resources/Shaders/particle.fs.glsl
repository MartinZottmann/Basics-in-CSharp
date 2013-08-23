#version 410 core

uniform sampler2D in_Texture;

layout(location = 0) in vec4 Color;
layout(location = 1) in vec2 texture_uv;

layout(location = 0) out vec4 FragColor;

void main(void) {
    FragColor = Color * texture(in_Texture, texture_uv);

    if (FragColor.r == 0 && FragColor.g == 0 && FragColor.b == 0) {
        discard;
    }
}
