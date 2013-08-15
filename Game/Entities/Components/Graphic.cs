using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Game.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using Color4 = MartinZottmann.Engine.Graphics.Color4;

namespace MartinZottmann.Game.Entities.Components
{
    public class Graphic : Abstract, IDisposable
    {
        public Entity Model;

#if DEBUG
        protected Entity orientation_model;
#endif

        public Graphic(GameObject game_object)
            : base(game_object)
        {
#if DEBUG
            var forward = new Color4(1.0f, 0.0f, 0.0f, 0.5f);
            var up = new Color4(0.0f, 1.0f, 0.0f, 0.5f);
            var right = new Color4(0.0f, 0.0f, 1.0f, 0.5f);
            orientation_model = new Engine.Graphics.OpenGL.Entity();
            orientation_model.Mesh(
                new Mesh<VertexP3C4, uint>()
                {
                    Vertices = new VertexP3C4[] {
                        new VertexP3C4(Vector3.Zero, forward),
                        new VertexP3C4((Vector3)GameObject.Forward, forward),
                        new VertexP3C4(Vector3.Zero, up),
                        new VertexP3C4((Vector3)GameObject.Up, up),
                        new VertexP3C4(Vector3.Zero, right),
                        new VertexP3C4((Vector3)GameObject.Right, right),
                    },
                    Indices = new uint[] {
                        0, 1, 2, 3, 4, 5
                    }
                }
            );
            orientation_model.Mode = BeginMode.Lines;
            orientation_model.Program = GameObject.Resources.Programs["normal"];
#endif
        }

        public void Dispose()
        {
            Model.Dispose();
#if DEBUG
            orientation_model.Dispose();
#endif
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            var program = render_context.Program == null
                ? Model.Program
                : render_context.Program;

            foreach (var s in program.UniformLocations)
                switch (s.Key)
                {
                    case "alpha_cutoff": s.Value.Set(render_context.alpha_cutoff); break;
                    case "in_DepthBiasMVP": break;
                    case "in_ShadowTexture":
                        render_context.DepthTexture.Bind();
                        s.Value.Set(render_context.DepthTexture.BindId);
                        break;
                    case "in_LightPosition": s.Value.Set(new Vector3d(10, 10, 10)); break; // @todo
                    case "in_Model": s.Value.Set(render_context.Model); break;
                    case "in_View": s.Value.Set(render_context.View); break;
                    case "in_ModelView": s.Value.Set(render_context.ViewModel); break;
                    case "in_ModelViewProjection": s.Value.Set(render_context.ProjectionViewModel); break;
                    case "in_NormalView": s.Value.Set(render_context.Normal); break;
                    case "in_Texture":
                        Model.Texture.Bind();
                        s.Value.Set(Model.Texture.BindId);
                        break;
                    default: throw new NotImplementedException(s.Key);
                }

            Model.Draw(program);

            foreach (var s in program.UniformLocations)
                switch (s.Key)
                {
                    case "alpha_cutoff": break;
                    case "in_DepthBiasMVP": break;
                    case "in_ShadowTexture":
                        render_context.DepthTexture.UnBind();
                        break;
                    case "in_LightPosition": break;
                    case "in_Model": break;
                    case "in_View": break;
                    case "in_ModelView": break;
                    case "in_ModelViewProjection": break;
                    case "in_NormalView": break;
                    case "in_Texture":
                        Model.Texture.UnBind();
                        break;
                    default: throw new NotImplementedException(s.Key);
                }

#if DEBUG
            if (render_context.Debug)
            {
                GL.LineWidth(5);
                orientation_model.Draw();
            }
#endif
        }
    }
}
