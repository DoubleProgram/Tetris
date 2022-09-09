using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Tetris{
    class Camera{
        float yaw = -90.0f, pitch = 0.0f;
        const float cameraSpeed = 8.5f;
        public Vector3 Right;
        public Vector3 Forward;
        public Vector3 Position;
        public static Matrix4 view;
        public static Matrix4 projection;
        public Camera(){
            Right = new Vector3(1, 0, 0);
            Forward = new Vector3(0, 0, -1);
            Position = new Vector3(2, 20, 5);
        }
        public void Move(KeyboardState input,float deltaTime){
            switch (KeyPressed(input)){
                case Key.A: Position -= Vector3.Normalize(Vector3.Cross(Forward,Vector3.UnitY)) * cameraSpeed * deltaTime; break;
                case Key.D: Position += Vector3.Normalize(Vector3.Cross(Forward, Vector3.UnitY)) * cameraSpeed * deltaTime; break;
                case Key.W: Position += Forward * cameraSpeed * deltaTime; break;
                case Key.S: Position -= Forward * cameraSpeed * deltaTime; break;
                case Key.LShift: Position -= Vector3.UnitY * cameraSpeed * deltaTime; break;
                case Key.Space: Position += Vector3.UnitY * cameraSpeed * deltaTime; break;
                case Key.Down: Brick.TimeDown = 0.04f; break;
                default: break;
            }
        }
        Key? KeyPressed(KeyboardState state){
            Key? key = null;
            void DownPressed(Key testkey){ if (state.IsKeyDown(testkey)) key = testkey; }
            if (!state.IsAnyKeyDown) return null;
            DownPressed(Key.A);
            DownPressed(Key.W);
            DownPressed(Key.S);
            DownPressed(Key.D);
            DownPressed(Key.LShift);
            DownPressed(Key.Space);
            DownPressed(Key.Down);
            return key;
        }
        public void Update(){
            Vector3 front = Vector3.Zero;
            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            Forward = Vector3.Normalize(front);
            view = Matrix4.LookAt(Position, Position + Forward, Vector3.UnitY);
            float aspect = (float)(Game.WIDTH / Game.HEIGHT);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f),(float) Game.WIDTH / Game.HEIGHT, 0.1f, 100.0f);
        }
        public void Rotate(float pitch,float yaw){
            this.pitch += pitch;
            this.yaw += yaw;
            if (this.pitch >= 89.0f) this.pitch = 89.0f;
            if (this.pitch <= -89.0f) this.pitch = -89.0f;
        }
    }
}