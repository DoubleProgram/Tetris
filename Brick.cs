using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Tetris
{
    enum BrickKind{
        IBrick,
        LBrick,
        OBrick,
        TBrick,
        JBrick,
        ZBrick,
        SBrick
    }
    enum BrickColor{
        Blue,
        Green,
        Red,
        Pink,
        Yellow,
        DarkBlue,
        Cyan
    }
    class Brick{
        public Cube[] cubes = new Cube[4];
        public BrickKind kind;
        public string texturePath;
        const int x = 6, y = 19;
        public Brick(BrickKind kind,BrickColor color){
            this.kind = kind;
            texturePath = TexturePath(color);
            cubes[0] = Playground.map[x, y];
            SetBrickKind();
            SetBricksVisible();
            TimeDown = LevelTimeDown();
        }
        public void ShowAsNextUp(float deltaTime){
            for (int i = 0; i < cubes.Length; i++)
                cubes[i].Render();
        }
        public static String TexturePath(BrickColor color){
            switch (color){
                case BrickColor.Blue: return Game.ProjectPlace + @"\res\Blue.png";
                case BrickColor.Red: return Game.ProjectPlace + @"\res\Red.png";
                case BrickColor.Green: return Game.ProjectPlace + @"\res\Green.png";
                case BrickColor.DarkBlue: return Game.ProjectPlace + @"\res\DarkBlue.png";
                case BrickColor.Pink: return Game.ProjectPlace + @"\res\Pink.png";
                case BrickColor.Cyan: return Game.ProjectPlace + @"\res\Cyan.png";
                default: return Game.ProjectPlace + @"\res\Yellow.png";
            }
        }
        public static float LevelTimeDown(){
            if (Game.points < 600) return 1.0f;
            if (IsGivenLevel(600, 0)) return 0.8f;
            if (IsGivenLevel(1800, 1)) return 0.72f;
            if (IsGivenLevel(2800, 2)) return 0.63f;
            if (IsGivenLevel(3600, 3)) return 0.55f;
            if (IsGivenLevel(5800, 4)) return 0.47f;
            if (IsGivenLevel(6400, 5)) return 0.38f;
            if (IsGivenLevel(9800, 6)) return 0.3f;
            if (IsGivenLevel(15200, 7)) return 0.22f;
            if (IsGivenLevel(18800, 8)) return 0.13f;
            if (IsGivenLevel(14000, 9)) return 0.1f;
            if (IsGivenLevel(24800, 10)) return 0.08f;
            if (IsGivenLevel(28800, 11)) return 0.08f;
            if (IsGivenLevel(32800, 12)) return 0.08f;
            if (IsGivenLevel(67600, 13)) return 0.07f;
            if (IsGivenLevel(82800, 14)) return 0.07f;
            if (IsGivenLevel(122400, 15)) return 0.07f;
            if (IsGivenLevel(200800, 16)) return 0.05f;
            if (IsGivenLevel(329200, 17)) return 0.05f;
            if (IsGivenLevel(498800, 18)) return 0.05f;
            if (IsGivenLevel(648000, 19)) return 0.03f;
            if (IsGivenLevel(724800, 20)) return 0.03f;
            if (IsGivenLevel(778800, 21)) return 0.03f;
            if (IsGivenLevel(862800, 22)) return 0.03f;
            if (IsGivenLevel(921600, 23)) return 0.03f;
            if (IsGivenLevel(999999, 24)) return 0.03f;
            if (IsGivenLevel(999999, 25)) return 0.03f;
            if (IsGivenLevel(999999, 26)) return 0.03f;
            if (IsGivenLevel(999999, 27)) return 0.03f;
            if (IsGivenLevel(999999, 28)) return 0.03f;
            return 1.0f;
        }
        static bool IsGivenLevel(int score, int level){
            if (Game.currentlevel == level && Game.points >= score){
                Game.currentlevel += 1;
                Game.level.ChangeText(Game.currentlevel.ToString());
                return true;
            }
            return false;
        }
        public void SetBricksVisible(){
            for (int i = 0; i < cubes.Length; i++)
                cubes[i].SetCubeVisible(texturePath);
        }
        Cube initialPosition;
        void SetBrickKind(){
            Position[] positions = GetPositions(kind);
            for (int i = 1; i < cubes.Length; i++) cubes[i] = Playground.map[positions[i].x, positions[i].y];
            switch (kind){
                case BrickKind.IBrick: initialPosition = cubes[2]; break;
                case BrickKind.LBrick: initialPosition = cubes[1]; break;
                case BrickKind.OBrick: break;
                case BrickKind.TBrick: initialPosition = cubes[1]; break;
                case BrickKind.JBrick: initialPosition = cubes[1]; break;
                case BrickKind.ZBrick: initialPosition = cubes[0]; break;
                case BrickKind.SBrick: initialPosition = cubes[0]; break;
            }
        }
        public static Position[] GetPositions(BrickKind kind){
            Position[] positions = new Position[4];
            positions[0] = new Position(x,y);
            switch (kind){
                case BrickKind.IBrick:
                    for (int i = 1; i < positions.Length; i++)
                        positions[i] = new Position(x - i, y);
                    break;
                case BrickKind.LBrick:
                    for (int i = 1; i < positions.Length - 1; i++)
                        positions[i] = new Position(x - i, y);
                    positions[3] = new Position(x-2,y-1);
                    break;
                case BrickKind.OBrick:
                    positions[1] = new Position(x + 1, y);
                    positions[2] = new Position(x, y - 1);
                    positions[3] = new Position(x + 1, y - 1);
                    break;
                case BrickKind.TBrick:
                    for (int i = 1; i < positions.Length - 1; i++)
                        positions[i] = new Position(x-i,y);
                    positions[3] = new Position(x - 1, y - 1);
                    break;
                case BrickKind.JBrick:
                    for (int i = 1; i < positions.Length - 1; i++)
                        positions[i] = new Position(x - i, y);
                    positions[3] = new Position(x,y-1);
                    break;
                case BrickKind.ZBrick:
                    positions[1] = new Position(x - 1, y);
                    positions[2] = new Position(x, y - 1);
                    positions[3] = new Position(x + 1, y - 1);
                    break;
                case BrickKind.SBrick:
                    positions[1] = new Position(x + 1, y);
                    positions[2] = new Position(x, y - 1);
                    positions[3] = new Position(x - 1, y - 1);
                    break;
            }
            return positions;
        }
        int rotated = 0;
        public static float TimeDown;
        public bool isDown = false;
        Matrix2 RotationMatrix;
        public void Rotate(float degree){
            if (initialPosition == null) return;
            RotationMatrix = Matrix2.CreateRotation(degree);
            Vector2[] newPositions = new Vector2[4];
            for (int i = 0; i< newPositions.Length; i++){
                if (cubes[i] == initialPosition) continue;
                Vector2 RelativePosition = new Vector2(cubes[i].position.x - initialPosition.position.x, cubes[i].position.y - initialPosition.position.y); //The relative position to the initalPosition
                Vector2 TransFormPosition = new Vector2(RotationMatrix.M11 * RelativePosition.X  + RotationMatrix.M12 * RelativePosition.Y,
                   RotationMatrix.M21 * RelativePosition.X + RotationMatrix.M22 * RelativePosition.Y );
                newPositions[i] = new Vector2(TransFormPosition.X + initialPosition.position.x,
                TransFormPosition.Y + initialPosition.position.y);
            }
            if (!CanRotateBrick(newPositions)) return;
            for (int i = 0; i < 4; i++){
                if (cubes[i] == initialPosition) continue;
                cubes[i].state = CubeState.Empty;
            }
            for(int i = 0; i<4; i++){
                if (cubes[i] == initialPosition) continue;
                cubes[i] = Playground.VisibleCube((int)newPositions[i].X, (int)newPositions[i].Y, this);
            }
        }
        bool CanRotateBrick(Vector2[] positions){
            for (int i = 0; i < positions.Length; i++) {
                Console.WriteLine((int)positions[i].X);
                Console.WriteLine((int)positions[i].Y);
                if (cubes[i] == initialPosition) continue;
                if (isSolid((int)positions[i].X, (int)positions[i].Y)) return false;
            }
            return true;
        }
        bool isSolid(int x, int y) {
            if (y <= 0 || x <= 0 || x >= 10 || y >= 20) return true;
            for (int i = 0; i < cubes.Length; i++)
                if (Playground.map[x, y] == cubes[i]) return false;
          
            if (Playground.map[x, y].state == CubeState.Solid) return true;
            return false;
        }
        void MoveDown(){
            if(currentTime >= TimeDown){
                currentTime = 0.0f;
                if (cubes[3].position.y == 0) { 
                    isDown = true; return; }
                for (int i = 0; i < cubes.Length; i++) {
                    Position position = cubes[i].position;
                    if (!CanMove(position.x, position.y-1)) { 
                        isDown = true; return; }
                }
                for (int i = 0; i < cubes.Length; i++) cubes[i].state = CubeState.Empty;
                for (int i = 0; i < cubes.Length; i++)
                    cubes[i] = Playground.VisibleCube(cubes[i].position.x, cubes[i].position.y - 1, this);
                if (initialPosition != null) initialPosition = Playground.map[initialPosition.position.x, initialPosition.position.y-1];
            }
        }
        void Controll(int dir){
            for (int i = 0; i < cubes.Length; i++){
                Position position = cubes[i].position;
                if (position.x == (dir == -1 ? 0:9)) return;
                if (!CanMove(position.x + dir, position.y)) return;
            }
            for (int i = 0; i < cubes.Length; i++) cubes[i].state = CubeState.Empty;
            for (int i = 0; i < cubes.Length; i++)
                cubes[i] = Playground.VisibleCube(cubes[i].position.x + dir, cubes[i].position.y, this);
            if(initialPosition != null) initialPosition = Playground.map[initialPosition.position.x + dir, initialPosition.position.y];
        }
        public void Move(Key key){
            if (key == Key.Left) Controll(-1);
            if (key == Key.Right) Controll(1);
        }
        bool CanMove(int x, int y){
            if (y < 0) return false;
            if (!isCurrentBrick(x, y) && Playground.map[x, y].state == CubeState.Solid) return false;
            return true;
        }
        bool isCurrentBrick(int x, int y){
            for (int i = 0; i<cubes.Length; i++)
                if (Playground.map[x, y] == cubes[i]) return true;
            return false;
        }
        float currentTime = 0.0f;
        public void Render(float deltaTime){
            currentTime += deltaTime;
            MoveDown();
        }
        public void Render(){
            for (int i = 0; i < cubes.Length; i++)
                cubes[i].Render();
        }
    }
}