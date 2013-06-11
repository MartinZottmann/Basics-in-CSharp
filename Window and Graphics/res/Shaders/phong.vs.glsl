#version 330 core

layout(location = 0) in vec3 Position;
layout(location = 1) in vec3 vertexColor;
in   vec3 vNormal;

out vec3 fragmentColor; // Output data ; will be interpolated for each fragment.
uniform mat4 MVP;
uniform mat4 transformMatrix;
uniform vec4 LightPosition;

// output values that will be interpretated per-fragment
out  vec3 fN;
out  vec3 fE;
out  vec3 fL;

void main()
{
    fN = vNormal;
    fE = Position.xyz;
    fL = LightPosition.xyz;

    if( LightPosition.w != 0.0 ) {
        fL = LightPosition.xyz - Position.xyz;
    }

    // Output position of the vertex, in clip space : MVP * position
    vec4 v = vec4(Position,1); // Transform in homoneneous 4D vector
    gl_Position = MVP * v;
    //gl_Position = MVP * v;

    // The color of each vertex will be interpolated
    // to produce the color of each fragment
    //fragmentColor = vertexColor; // take out at some point
}
