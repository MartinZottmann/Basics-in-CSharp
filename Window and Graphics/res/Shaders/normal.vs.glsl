#version 330 compatibility

in vec3 in_Position;
in vec4 in_Color;
out vec4 ex_Color;

void main(void) {
    gl_Position = ftransform();
    ex_Color = in_Color;
}
