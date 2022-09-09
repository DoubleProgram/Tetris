using System;
using OpenTK;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args){
            using (Game game = new Game(450, 600)){
                game.Run(60.0f);
            }
        }
    }
}
