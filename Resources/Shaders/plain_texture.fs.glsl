#version 330 core

uniform sampler2D in_Texture;

in vec2 UV;

out vec4 FragColor;

void main() {
    FragColor = texture2D(in_Texture, UV);
}
