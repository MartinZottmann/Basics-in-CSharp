using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Graphics.OpenGL
{
    public class Info
    {
        public static void Strings()
        {
            Console.WriteLine("Strings (");
            foreach (StringName parameter in (StringName[])Enum.GetValues(typeof(StringName)))
            {
                Console.WriteLine("\t{0}: {1}", parameter, GL.GetString(parameter));
            }
            Console.WriteLine(")");
        }

        public static void ProgramParameters(int program)
        {
            int info;
            bool geometry_shader = false;
            bool tesslation_shader = false;
            int shader_count;
            GL.GetProgram(program, ProgramParameter.AttachedShaders, out shader_count);
            int shader_index;
            GL.GetAttachedShaders(program, shader_count, out shader_count, out shader_index);
            for (int i = 0; i < shader_count; i++)
            {
                GL.GetShader(shader_index + i, ShaderParameter.ShaderType, out info);
                if (info == (int)ShaderType.GeometryShader)
                {
                    geometry_shader = true;
                }
                if (info == (int)ShaderType.TessControlShader || info == (int)ShaderType.TessEvaluationShader)
                {
                    tesslation_shader = true;
                }
            }

            Console.WriteLine("ProgramParameters {0} (", program);
            foreach (ProgramParameter parameter in (ProgramParameter[])Enum.GetValues(typeof(ProgramParameter)))
            {
                if (
                    !geometry_shader
                    && (
                        parameter == ProgramParameter.GeometryInputType
                        || parameter == ProgramParameter.GeometryOutputType
                        || parameter == ProgramParameter.GeometryShaderInvocations
                        || parameter == ProgramParameter.GeometryVerticesOut
                    )
                )
                {
                    continue;
                }
                if (
                    !tesslation_shader
                    && (
                        parameter == ProgramParameter.TessControlOutputVertices
                        || parameter == ProgramParameter.TessGenMode
                        || parameter == ProgramParameter.TessGenPointMode
                        || parameter == ProgramParameter.TessGenSpacing
                        || parameter == ProgramParameter.TessGenVertexOrder
                    )
                )
                {
                    continue;
                }
                GL.GetProgram(program, parameter, out info);
                Console.WriteLine("\t{0}: {1}", parameter, info);
            }
            Console.WriteLine(")");
        }

        public static void Shader(int shader)
        {
            int info;
            Console.WriteLine("Shader {0} (", shader);
            foreach (ShaderParameter parameter in (ShaderParameter[])Enum.GetValues(typeof(ShaderParameter)))
            {
                GL.GetShader(shader, parameter, out info);
                Console.WriteLine("\t{0}: {1}", parameter, info);
            }
            Console.WriteLine(")");
        }
    }
}
