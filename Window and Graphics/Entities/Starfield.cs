using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace MartinZottmann.Entities
{
    class Starfield : Entity
    {
        //struct Vbo {
        //    public uint vbo_id;
        //    public uint ebo_id;
        //    public uint num_elements;
        //}

        //struct Vpc
        //{
        //    public Vector3 position;
        //    public uint color;
        //}

        //Vpc[] stars;

        const int num_stars = 100000;

        //Vbo vbo;

        Graphics.Entity graphic;

        public Starfield()
            : base()
        {
            graphic = new Graphics.Entity();
            graphic.mode = BeginMode.Points;
            graphic.vertices = new Graphics.Vertex3[num_stars];
            //stars = new Vpc[num_stars];
            for (int i = 0; i < num_stars; i++)
            {
                graphic.vertices[i].x = randomNumber.Next(-1000, 1000);
                graphic.vertices[i].y = randomNumber.Next(-1000, 1000);
                graphic.vertices[i].z = randomNumber.Next(-1000, 1000);
                //stars[i].position = new Vector3(
                //    randomNumber.Next(-1000, 1000),
                //    randomNumber.Next(-1000, 1000),
                //    randomNumber.Next(-1000, 1000)
                //);
                //stars[i].color = 0xffffffff;
            }
            graphic.Load();

//            vbo = new Vbo();

//            GL.GenBuffers(1, out vbo.vbo_id);
//            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.vbo_id);
//            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(stars.Length * BlittableValueType.StrideOf(stars)), stars, BufferUsageHint.StaticDraw);
//#if DEBUG
//            int size;
//            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
//            System.Diagnostics.Debug.Assert(stars.Length * BlittableValueType.StrideOf(stars) == size);
//#endif

//            //GL.GenBuffers(1, out vbo.ebo_id);
//            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo.ebo_id);
//            //GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(elements.Length * sizeof(short)), elements, BufferUsageHint.StaticDraw);
//            //GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
//            //System.Diagnostics.Debug.Assert(elements.Length * BlittableValueType.StrideOf(stars) == size);

//            //vbo.num_elements = elements.Length;

            color = Color.LightGray;
        }

        public override void Render(double delta_time)
        {
            GL.PointSize(3);
            graphic.Draw();
            //GL.PointSize(3);

            //GL.EnableClientState(ArrayCap.ColorArray);
            //GL.EnableClientState(ArrayCap.VertexArray);

            //GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.vbo_id);
            ////GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo.ebo_id);

            //GL.VertexPointer(3, VertexPointerType.Float, BlittableValueType.StrideOf(stars), new IntPtr(0));
            //GL.ColorPointer(4, ColorPointerType.UnsignedByte, BlittableValueType.StrideOf(stars), new IntPtr(12));

            //GL.DrawArrays(BeginMode.Points, 0, stars.Length);
            ////GL.DrawElements(BeginMode.Triangles, vbo.num_elements, DrawElementsType.UnsignedShort, IntPtr.Zero);
        }
    }
}
