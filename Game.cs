using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;

namespace Tetris{
    class Game:GameWindow{
        public static float WIDTH;
        public static float HEIGHT;
        public Game(int width, int height):base(width,height,GraphicsMode.Default,"Tetris"){
            WIDTH = Width;
            HEIGHT = Height;
            camera = new Camera();
        }
        Playground playground;
        public Text Score;
        public static Text score;
        public Text Level;
        public static Text level;
        public Text Lines;
        public static Text lines;
        public Text NextUpBrick;
        public static string ProjectPlace { get { return @"C:\Users\Slava\source\repos\Tetris"; } }
        Background background;
        Camera camera;
        static string Blue = @"\res\Blue.png";
        static string Green = @"\res\Green.png";
        static string Red = @"\res\Red.png";
        static string Pink = @"\res\Pink.png";
        static string Yellow = @"\res\Yellow.png";
        static string DarkBlue = @"\res\DarkBlue.png";
        static string Cyan = @"\res\Cyan.png";
        public static int points = 0;
        public static int currentlevel = 10;
        public static int linesCleared = 0;
        protected override void OnLoad(EventArgs e){
            GL.ClearColor(0.5f, 0.5f, 0.5f,1.0f);
            playground = new Playground();
            background = new Background();
            Level = new Text("Level", new Vector3(-2.5f, 9, 0), new Vector3(0.7f, 0.7f, 0.7f));
            Lines = new Text("Lines", new Vector3(-2.5f, 7, 0), new Vector3(0.7f, 0.7f, 0.7f));
            //lines = new Text(linesCleared.ToString(), new Vector3(-2.5f, 6, 0), new Vector3(1f, 1f, 1f));
            Score = new Text("Score", new Vector3(-2.5f, 5, 0), new Vector3(0.7f, 0.7f, 0.7f));
            //score = new Text(points.ToString(), new Vector3(-2.5f, 4, 0), new Vector3(1f, 1f, 1f));
            NextUpBrick = new Text("Next up:",new Vector3(-2.5f, -3, 0), new Vector3(0.7f, 0.7f, 0.7f));
            GL.Enable(EnableCap.DepthTest);
            base.OnLoad(e);
        }
        bool firstMouse = true;
        float sensitivity = 0.2f;
        Vector2 lastMousePos = Vector2.Zero;
        protected override void OnUpdateFrame(FrameEventArgs e){
            base.OnUpdateFrame(e);
            camera.Update();
            playground.Update();
            KeyboardState input = Keyboard.GetState();
            camera.Move(input, (float) e.Time);
            Vector2 mouse = new Vector2(Mouse.GetCursorState().X, Mouse.GetCursorState().Y);
            if (firstMouse){
                lastMousePos = mouse;
                firstMouse = false;
            }
            float deltaX = mouse.X - lastMousePos.X;
            float deltaY = lastMousePos.Y - mouse.Y;
            lastMousePos = mouse;
          //  camera.Rotate(deltaY * sensitivity, deltaX * sensitivity);
        }
        Cube[] NextUpCubes = new Cube[4];
        Position[] ShowBrickPosition = new Position[4];
        public static bool showNextBrick = false;
       
        public void ShowNextUpBrick(){
            if (showNextBrick == true) { 
            ShowBrickPosition = Playground.NextPositions;
                for(int i = 0; i<ShowBrickPosition.Length; i++){
                    int positionx = Playground.NextPositions[i].x - 10;
                    int positiony = Playground.NextPositions[i].y - 15;
                    ShowBrickPosition[i].x = positionx;
                    ShowBrickPosition[i].y = positiony;
                    NextUpCubes[i] = new Cube(ShowBrickPosition[i].x, ShowBrickPosition[i].y);
                    NextUpCubes[i].SetCubeVisible(Brick.TexturePath(Playground.NextBrickColor));
                }
                showNextBrick = false;
            }
            GL.BindVertexArray(playground.vao);
            for (int i = 0; i< NextUpCubes.Length; i++)
                NextUpCubes[i].Render();
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e){
            if (e.Key == Key.Down) Brick.TimeDown = Brick.LevelTimeDown();
            base.OnKeyUp(e);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e){
            Playground.brick.Move(e.Key);
            if (e.Key == Key.Enter && Playground.gameover)
                playground = new Playground();
            if (e.Key == Key.Z)
                Playground.brick.Rotate((float)-Math.PI / 2);
            if (e.Key == Key.X) 
                Playground.brick.Rotate((float)Math.PI / 2);
            base.OnKeyDown(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e){
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            background.Render();
            RenderAllText();
            playground.Render((float)e.Time);
            ShowNextUpBrick();
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
        
        void RenderAllText(){
            GL.BindVertexArray(Text.vao);
            score.Render();
            Level.Render();
            Score.Render();
            Lines.Render();
            lines.Render();
            level.Render();
            NextUpBrick.Render();
        }
        protected override void OnResize(EventArgs e){
            GL.Viewport(0, 0, Width, Height);
            WIDTH = Width;
            HEIGHT = Height;
            base.OnResize(e);
        }
        protected override void OnUnload(EventArgs e){ base.OnUnload(e); }
    }
}