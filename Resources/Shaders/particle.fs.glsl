#version 430 core

uniform sampler2D in_Texture;

layout(location = 0) in vec4 in_Color;
layout(location = 1) in vec2 in_UV;

layout(location = 0) out vec4 out_Color;

void main(void) {
    out_Color = in_Color * texture(in_Texture, in_UV);

    if (out_Color.a == 0)
        discard;
}
