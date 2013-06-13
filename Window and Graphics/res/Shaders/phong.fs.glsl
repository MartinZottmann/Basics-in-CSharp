#version 330 core

uniform sampler2D in_Texture;
uniform vec4 in_AmbientColor;
uniform vec4 in_DiffuseColor;
//uniform vec4 in_SpecularColor;

in vec2 uv;
in vec3 Normal;
in vec3 LightDirection;

out vec4 FragColor;

void main(void)
{
    float diff = max(0.0, dot(normalize(Normal), normalize(LightDirection)));
    FragColor = texture2D(in_Texture, uv) * diff * in_DiffuseColor + in_AmbientColor;
    if (diff != 0)
    {
    vec3 reflection = normalize(reflect(-normalize(LightDirection), normalize(Normal)));
    float spec = max(0.0, dot(normalize(Normal), reflection));
        FragColor.rgb += vec3(pow(spec, 32.0));
    }
}
