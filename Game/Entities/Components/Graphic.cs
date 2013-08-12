using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using Color4 = MartinZottmann.Engine.Graphics.Color4;

namespace MartinZottmann.Game.Entities.Components
{
    public class Graphic : Abstract
    {
        public Engine.Graphics.OpenGL.Entity Model;

#if DEBUG
        protected Engine.Graphics.OpenGL.Entity orientation_model;
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

        public override void Render(double delta_time, RenderContext render_context)
        {
            var t0 = Model.Program;
            var t1 = Model.Texture;
            if (render_context.Program != null)
            {
                Model.Program = render_context.Program;
                Model.Texture = null;
            }
            foreach (var s in Model.Program.UniformLocations)
                switch (s.Key)
                {
                    case "alpha_cutoff": s.Value.Set(render_context.alpha_cutoff); break;
                    case "in_DepthBiasMVP": break;
                    case "in_ShadowTexture": break;
                    case "in_LightPosition": s.Value.Set(new Vector3d(10, 10, 10)); break; // @todo
                    case "in_Model": s.Value.Set(render_context.Model); break;
                    case "in_View": s.Value.Set(render_context.View); break;
                    case "in_ModelView": s.Value.Set(render_context.ViewModel); break;
                    case "in_ModelViewProjection": s.Value.Set(render_context.ProjectionViewModel); break;
                    case "in_NormalView": s.Value.Set(render_context.Normal); break;
                    case "in_Texture": s.Value.Set(0); break; // @todo
                    default: throw new NotImplementedException(s.Key);
                }
            Model.Draw();
            if (render_context.Program != null)
            {
                Model.Program = t0;
                Model.Texture = t1;
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
