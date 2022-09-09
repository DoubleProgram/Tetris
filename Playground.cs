using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK;

namespace Tetris{
    class Playground{
        public static Cube[,] map;
        public static Brick brick;
        public static bool NotDestroyed;
        public static Position[] NextPositions = new Position[4];
        public static BrickKind NextBrickKind;
        public static BrickColor NextBrickColor;
        public int vbo, vao;
        public Playground(){
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Cube.vertices.Length * sizeof(float), Cube.vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            map = new Cube[10, 20];
            Game.points = 0;
            Game.level = new Text(Game.currentlevel.ToString(), new Vector3(-2.5f, 8, 0), new Vector3(1f, 1f, 1f));
          
            Game.lines = new Text(Game.linesCleared.ToString(), new Vector3(-2.5f, 6, 0), new Vector3(1f, 1f, 1f));
            Game.score = new Text(Game.points.ToString(), new Vector3(-2.5f, 4, 0), new Vector3(1f, 1f, 1f));
            Game.currentlevel = 0;
            Game.score.ChangeText(Game.points.ToString());
            Game.level.ChangeText(Game.currentlevel.ToString());
            Game.linesCleared = 0;
            Game.lines.ChangeText(Game.linesCleared.ToString());
            NotDestroyed = true;
            gameover = false;
            for (int y = 0; y<20;y++)
                for (int x = 0; x < 10; x++) 
                    map[x, y] = new Cube(x,y);
            NextBrickColor = RandomBrickColor();
            NextBrickKind = RandomBrickKind();
            NextPositions = Brick.GetPositions(NextBrickKind);
            brick = RandomBrick();
            Game.showNextBrick = true;
        }
        public void Update(){
            bool[] IsSolidRow = new bool[20];
            if (brick.isDown && !gameover){
                brick.isDown = false;
                    for (int y = 0; y < 20; y++){
                    if (SolidRow(y)){
                        IsSolidRow[y] = true;
                        Game.points += 100;
                        Game.score.ChangeText(Game.points.ToString());
                        Game.linesCleared += 1;
                        Game.lines.ChangeText(Game.linesCleared.ToString());
                    }
                }
                CheckForStatus();
                MoveDownRow(IsSolidRow);
                if (EmptyRows(IsSolidRow)){
                    brick = new Brick(NextBrickKind, NextBrickColor);
                    NextBrickColor = RandomBrickColor();
                    NextBrickKind = RandomBrickKind();
                    NextPositions = Brick.GetPositions(NextBrickKind);
                    Game.showNextBrick = true;
                }
               
            }
            if (NotDestroyed == false) {
                NotDestroyed = true;
                MoveDownRow(IsSolidRow);
                brick = new Brick(NextBrickKind, NextBrickColor);
                NextBrickColor = RandomBrickColor();
                NextBrickKind = RandomBrickKind();
                NextPositions = Brick.GetPositions(NextBrickKind);
                Game.showNextBrick = true;
            }
        }
        public static bool gameover;
        void CheckForStatus(){
            if (map[6, 19].state == CubeState.Solid) gameover = true;
        }
        bool EmptyRows(bool[] IsSolidRow){
            for (int i = 0; i < IsSolidRow.Length; i++)
                if (IsSolidRow[i] == true) return false;
            return true;
        }
        void MoveDownRow(bool[] IsSolidRow){
            for (int y = 0; y < 20; y++)
                if (IsSolidRow[y])
                    DestroyRow(y);
            for (int y = 0; y < 20; y++)
                if (map[0, y].Destroyed) return;
            MoveAllRowsDown();
        }
        void MoveAllRowsDown(){
            for (int y = 0; y<19;y++) MoveDown(y);
        }
        void MoveDown(int y){
            for (; y > 0; y--)
                if (CanMoveDown(y)){
                    for(int x = 0; x<10; x++){
                        if (map[x, y].state == CubeState.Empty) continue; 
                        map[x, y].state = CubeState.Empty;
                        map[x, y-1].SetCubeVisible(map[x, y].texturepath);
                    }
                }
        }
        bool CanMoveDown(int y){
            for (int x = 0; x < 10; x++) if (y - 1 < 0 || map[x, y - 1].state != CubeState.Empty) return false;
            for (int x = 0; x < 10; x++) if (map[x, y].state != CubeState.Empty) return true;
            return false;
        }
        void DestroyRow(int y){
            for (int x = 0; x < 10; x++) {
                map[x, y].Destroyed = true;
            }
        }
        bool SolidRow(int y){
            for (int x=0;x<10; x++)
                if (map[x, y].state != CubeState.Solid) return false;
            return true;
        }
        Random rnd = new Random();
        Brick RandomBrick(){
            BrickColor color = BrickColor.Cyan;
            BrickKind kind = BrickKind.IBrick;
            color = RandomBrickColor();
            kind = RandomBrickKind();
            if (brick == null || brick.kind != kind) return new Brick(kind, color);
            else return RandomBrick();
        }
        public BrickColor RandomBrickColor(){
            switch (rnd.Next(0, 7)){
                case 0: return BrickColor.Blue;
                case 1: return BrickColor.Red; 
                case 2: return BrickColor.Green;
                case 3: return BrickColor.Cyan;
                case 4: return BrickColor.DarkBlue;
                case 5: return BrickColor.Yellow;
                case 6: return BrickColor.Pink;
                default: return BrickColor.Blue;
            }
        }
        public BrickKind RandomBrickKind(){
            switch (rnd.Next(0, 7)){
                case 0: return BrickKind.IBrick; 
                case 1: return  BrickKind.SBrick;
                case 2: return BrickKind.ZBrick; 
                case 3: return BrickKind.JBrick; 
                case 4: return BrickKind.LBrick; 
                case 5: return BrickKind.OBrick; 
                case 6: return BrickKind.TBrick;
                default: return BrickKind.TBrick;
            }
        }
        public Text GameStatus = new Text("GAME OVER!",new OpenTK.Vector3(6.3f, 0.5f,0), new OpenTK.Vector3(0.2f, 0.4f, 0.3f),
        new OpenTK.Vector3(35f, 35f, 2.0f));
        public void Render(float deltaTime){
            GL.BindVertexArray(vao);
            if (gameover){
                GameStatus.Render();
                return;
            }
            Cube.shader.Use();
            Cube.shader.SetMatrix4(ref Camera.view, "view");
            Cube.shader.SetMatrix4(ref Camera.projection, "projection");
            
            for (int y = 0; y < 20; y++)
                for (int x = 0; x < 10; x++)
                    map[x, y].Render(deltaTime);
            brick.Render(deltaTime);
        }
        public static Cube VisibleCube(int x , int y,Brick brick){
            map[x, y].SetCubeVisible(brick.texturePath);
            return map[x, y];
        }
    }
}