#version 430 core

uniform mat4 in_ModelViewProjection;
uniform vec3 in_CameraPosition;
uniform vec3 in_CameraUp;
uniform float in_ParticleSize;

layout(points) in;
layout(triangle_strip, max_vertices = 4) out;

layout(location = 0) in vec4 in_Color[];

layout(location = 0) out vec4 out_Color;
layout(location = 1) out vec2 texture_uv;

void main(void) {
    vec3 position = gl_in[0].gl_Position.xyz;
    vec3 distance = normalize(in_CameraPosition - position);
    vec3 right = normalize(cross(distance, in_CameraUp));
    vec3 up = normalize(cross(right, distance));
    vec3 x = right * in_ParticleSize * 0.5;
    vec3 y = up * in_ParticleSize * 0.5;

    gl_Position = in_ModelViewProjection * vec4(position - x - y, 1.0);
    out_Color = in_Color[0];
    texture_uv = vec2(0.0, 0.0);
    EmitVertex();

    gl_Position = in_ModelViewProjection * vec4(position - x + y, 1.0);
    out_Color = in_Color[0];
    texture_uv = vec2(0.0, 1.0);
    EmitVertex();

    gl_Position = in_ModelViewProjection * vec4(position + x - y, 1.0);
    out_Color = in_Color[0];
    texture_uv = vec2(1.0, 0.0);
    EmitVertex();

    gl_Position = in_ModelViewProjection * vec4(position + x + y, 1.0);
    out_Color = in_Color[0];
    texture_uv = vec2(1.0, 1.0);
    EmitVertex();

    EndPrimitive();
}
/*
#version 330 core

layout(points) in;
layout(triangle_strip, max_vertices = 4) out;

in vec4 in_Color[];

out vec4 out_Color;
out vec2 texture_uv;

void main(void) {
    out_Color = in_Color[0];
    texture_uv = vec2(0, 0);
    gl_Position = gl_in[0].gl_Position + vec4(1, 1, 0, 0);
    EmitVertex();

    out_Color = in_Color[0];
    texture_uv = vec2(0, 1);
    gl_Position = gl_in[0].gl_Position + vec4(-1, 1, 0, 0);
    EmitVertex();

    out_Color = in_Color[0];
    texture_uv = vec2(1, 0);
    gl_Position = gl_in[0].gl_Position + vec4(1, -1, 0, 0);
    EmitVertex();

    out_Color = in_Color[0];
    texture_uv = vec2(1, 1);
    gl_Position = gl_in[0].gl_Position + vec4(-1, -1, 0, 0);
    EmitVertex();
}
/*
#version 330 core

uniform mat4 in_Projection;
uniform mat4 in_ModelView;

layout(points) in;
layout(triangle_strip, max_vertices = 4) out;

out vec2 texture_uv;

void main(void) {
    vec4 position = in_ModelView * gl_in[0].gl_Position;

    texture_uv = vec2(0, 0);
    gl_Position = in_Projection * (position + vec4(10, 10, 0, 0));
    EmitVertex();

    texture_uv = vec2(1, 0);
    gl_Position = in_Projection * (position + vec4(10, -10, 0, 0));
    EmitVertex();

    texture_uv = vec2(0, 1);
    gl_Position = in_Projection * (position + vec4(-10, 10, 0, 0));
    EmitVertex();

    texture_uv = vec2(1, 1);
    gl_Position = in_Projection * (position + vec4(-10, -10, 0, 0));
    EmitVertex();
}
*/
