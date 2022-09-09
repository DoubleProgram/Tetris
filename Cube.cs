using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Tetris{
    enum CubeState{
        Empty,
        Solid
    }
    class Cube{
        public static float[] vertices ={
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
         0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };
        const float Zposition = -30.0f;
        public Matrix4 model;
        public Texture BrickTexture = new Texture(Game.ProjectPlace + @"\res\Blue.png");
        public static Shader shader = new Shader(Game.ProjectPlace + @"\BrickShader.vert", Game.ProjectPlace + @"\BrickShader.frag");
        public Vector3 Vecposition;
        public Position position;
        public string texturepath;
        public CubeState state;
        public bool Destroyed = false;
        public Cube(int x , int y){
            state = CubeState.Empty;
            position.y = y;
            position.x = x;
            this.Vecposition = new Vector3(x, y,Zposition);
            model = GameMath.TransformMatrix(Vecposition, 2.0f, 2.0f, 2.0f);
        }
        public void SetCubeVisible(string texturepath){
            this.texturepath = texturepath;
            state = CubeState.Solid;
            BrickTexture.Create(texturepath);
        }
        float sinceDestroyed = 0f;
        public void Render(float deltaTime){
            if (Destroyed) {
                Playground.brick.isDown = false;
                BrickTexture.Create(Game.ProjectPlace + @"\res\Destroyed.png");
                sinceDestroyed += deltaTime;
                if (sinceDestroyed >= 0.4f) {
                    Playground.NotDestroyed = false;
                    state = CubeState.Empty;
                    Destroyed = false;
                    sinceDestroyed = 0f;
                }
            }
            if (state == CubeState.Empty && !Destroyed) return;
            BrickTexture.Use();
            shader.SetMatrix4(ref model, "model");
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }
        public void Render(){
            BrickTexture.Use();
            model = GameMath.TransformMatrix(Vecposition, 2.0f, 2.0f, 2.0f);
            shader.SetMatrix4(ref model, "model");
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }
    }
    
    struct Position{
        public int x, y;
        public Position(int x, int y){
            this.x = x;
            this.y = y;
        }
    }
}