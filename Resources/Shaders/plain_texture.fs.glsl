#version 430 core

uniform sampler2D in_Texture;

layout(location = 0) in vec2 UV;

layout(location = 0) out vec4 FragColor;

void main(void) {
    FragColor = texture(in_Texture, UV);
}
