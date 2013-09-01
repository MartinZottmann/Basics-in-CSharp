#version 430 core

uniform sampler2D in_Texture;

layout(location = 0) in vec2 in_UV;

layout(location = 0) out vec4 out_Color;

void main(void) {
    out_Color = texture(in_Texture, in_UV);

    if (out_Color.a == 0)
        discard;
}
