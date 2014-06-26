#if DEBUG
using OpenTK.Graphics.OpenGL;
using System;
using System.Text;

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

        public static unsafe uint[] AttachedShaders(uint program)
        {
            int count;
            GL.GetProgram(program, GetProgramParameterName.AttachedShaders, out count);

            uint[] shaders = new uint[count];
            GL.GetAttachedShaders(program, count, out count, shaders);

            return shaders;
        }

        public static void ProgramParameters(uint program)
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
            foreach (GetProgramParameterName parameter in (GetProgramParameterName[])Enum.GetValues(typeof(GetProgramParameterName)))
            {
                if (
                    !geometry_shader
                    && (
                        parameter == GetProgramParameterName.GeometryInputType
                        || parameter == GetProgramParameterName.GeometryOutputType
                        || parameter == GetProgramParameterName.GeometryShaderInvocations
                        || parameter == GetProgramParameterName.GeometryVerticesOut
                    )
                )
                    continue;
                if (
                    !tesslation_shader
                    && (
                        parameter == GetProgramParameterName.TessControlOutputVertices
                        || parameter == GetProgramParameterName.TessGenMode
                        || parameter == GetProgramParameterName.TessGenPointMode
                        || parameter == GetProgramParameterName.TessGenSpacing
                        || parameter == GetProgramParameterName.TessGenVertexOrder
                    )
                )
                    continue;
                GL.GetProgram(program, parameter, out info);
                Console.WriteLine("\t{0}: {1}", parameter, info);
            }
            Console.WriteLine(")");
        }

        public static void Shader(uint shader)
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

        public static void Uniform(uint program)
        {
            int count, info;
            Console.WriteLine("Uniform {0} (", program);
            GL.GetProgram(program, GetProgramParameterName.ActiveUniforms, out count);
            for (uint i = 0; i < count; i++)
            {
                Console.WriteLine("\t{0}: {1} (", i, GL.GetActiveUniformName((int)program, (int)i));
                GL.GetActiveUniformName((int)program, (int)i);
                foreach (ActiveUniformParameter parameter in (ActiveUniformParameter[])Enum.GetValues(typeof(ActiveUniformParameter)))
                {
                    GL.GetActiveUniforms(program, 1, ref i, parameter, out info);
                    Console.WriteLine("\t\t{0}: {1}", parameter, info);
                }
                Console.WriteLine("\t)");
            }
            GL.GetProgram(program, GetProgramParameterName.ActiveUniformBlocks, out count);
            for (uint i = 0; i < count; i++)
            {
                Console.WriteLine("\t{0}: {1} (", i, GL.GetActiveUniformBlockName((int)program, (int)i));
                foreach (ActiveUniformBlockParameter parameter in (ActiveUniformBlockParameter[])Enum.GetValues(typeof(ActiveUniformBlockParameter)))
                {
                    GL.GetActiveUniformBlock(program, i, parameter, out info);
                    Console.WriteLine("\t\t{0}: {1}", parameter, info);
                }
                Console.WriteLine("\t)");
            }
            Console.WriteLine(")");
        }

        public static void Attribue(uint program)
        {
            int count;

            Console.WriteLine("Attribue {0} (", program);
            GL.GetProgram(program, GetProgramParameterName.ActiveAttributes, out count);
            int buffer_size;
            GL.GetProgram(program, GetProgramParameterName.ActiveAttributeMaxLength, out buffer_size);
            for (uint i = 0; i < count; i++)
            {
                int size;
                int length;
                ActiveAttribType type;
                StringBuilder name = new StringBuilder();
                GL.GetActiveAttrib(program, i, buffer_size, out length, out size, out type, name);
                //GL.BindAttribLocation(id, i, name.ToString());
                Console.WriteLine("\t{0}: {1}: {2}: {3}: {4}", i, length, size, type, name);
            }
            Console.WriteLine(")");
        }
    }
}
#endif
