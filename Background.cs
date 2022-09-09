using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Tetris
{
    class Background{
        public float[] vertices ={
        -0.5f, -0.5f, -0.5f,
         0.5f, -0.5f, -0.5f,
         0.5f,  0.5f, -0.5f,
         0.5f,  0.5f, -0.5f,
        -0.5f,  0.5f, -0.5f,
        -0.5f, -0.5f, -0.5f
        };
        int vbo,vao;
        Vector2[] Positions = new Vector2[200];
        public static Shader shader;
        public Matrix4 model;
        public Background(){
            shader = new Shader(Game.ProjectPlace + @"\Background.vert", Game.ProjectPlace + @"\Background.frag");
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            Create();
        }
        void Create(){
            int index = 0;
            for(int i = 0; i < 20; i++)
                for (int j = 0; j < 10; j++) {
                    Positions[index].X = j;
                    Positions[index].Y = i;
                    index++;
                }
        }
        public void Render(){
            GL.BindVertexArray(vao);
            shader.Use();
            shader.SetMatrix4(ref Camera.view, "view");
            shader.SetMatrix4(ref Camera.projection, "projection");
            shader.SetVectorToUniform(new Vector3(0.0f, 0.0f, 0.0f), shader.GetUniformLocation("color"));
            /*   GL.Enable(EnableCap.LineSmooth);
               GL.Enable(EnableCap.PolygonSmooth);
               GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
               GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);*/
            for (int i = 0; i < Positions.Length; i++) {
                
                model = GameMath.TransformMatrix(new Vector3(Positions[i].X, Positions[i].Y, -30.0f),2.0f,2.0f,2.0f);
                shader.SetMatrix4(ref model, "model");
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
            }
           
            /*GL.Disable(EnableCap.LineSmooth);
            GL.Disable(EnableCap.PolygonSmooth);*/
        }
    }
}
