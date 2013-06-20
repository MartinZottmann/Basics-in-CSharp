using OpenTK.Graphics.OpenGL;
using System;

namespace MartinZottmann.Engine.Graphics.OpenGL
{
    public class Info
    {
        public static void Strings()
        {
            Console.WriteLine("Strings (");
            foreach (StringName parameter in (StringName[])Enum.GetValues(typeof(StringName)))
                Console.WriteLine("\t{0}: {1}", parameter, GL.GetString(parameter));
            Console.WriteLine(")");
        }

#if DEBUG
        public static unsafe int[] AttachedShaders(int program)
        {
            int icount = 0;
            int* count = (int*)&icount;
            GL.GetProgram(program, ProgramParameter.AttachedShaders, count);

            int[] shaders = new int[*count];
            GL.GetAttachedShaders(program, *count, count, shaders);

            return shaders;
        }
#endif

        public static void ProgramParameters(int program)
        {
            int info;
            bool geometry_shader = false;
            bool tesslation_shader = false;
            foreach (var i in AttachedShaders(program))
            {
                GL.GetShader(i, ShaderParameter.ShaderType, out info);
                if (info == (int)ShaderType.GeometryShader)
                    geometry_shader = true;
                if (info == (int)ShaderType.TessControlShader || info == (int)ShaderType.TessEvaluationShader)
                    tesslation_shader = true;
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
                    continue;
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
                    continue;
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

        public static void Uniform(int program)
        {
            int count, info;
            Console.WriteLine("Uniform {0} (", program);
            GL.GetProgram(program, ProgramParameter.ActiveUniforms, out count);
            for (int i = 0; i < count; i++)
            {
                foreach (ActiveUniformParameter parameter in (ActiveUniformParameter[])Enum.GetValues(typeof(ActiveUniformParameter)))
                {
                    GL.GetActiveUniforms(program, 1, ref i, parameter, out info);
                    Console.WriteLine("\t{0}: {1}", parameter, info);
                }
            }
            GL.GetProgram(program, ProgramParameter.ActiveUniformBlocks, out count);
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("\t{0}", GL.GetActiveUniformBlockName(program, i));
                foreach (ActiveUniformBlockParameter parameter in (ActiveUniformBlockParameter[])Enum.GetValues(typeof(ActiveUniformBlockParameter)))
                {
                    GL.GetActiveUniformBlock(program, i, parameter, out info);
                    Console.WriteLine("\t{0}: {1}", parameter, info);
                }
            }
            Console.WriteLine(")");
        }
    }
}
