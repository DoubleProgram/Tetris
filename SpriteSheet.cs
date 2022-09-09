using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Tetris
{
    class SpriteSheet{
        Bitmap Sprite;
        int spriteWidth,spriteHeight;
        int spriteSheetWidth, spriteSheetHeight;
        byte columns, rows;

        public SpriteSheet(string filename, int spriteWidth, int spriteHeight){
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            Sprite = new Bitmap(Game.ProjectPlace + filename);
            spriteSheetWidth = Sprite.Width;
            spriteSheetHeight = Sprite.Height;
            columns = (byte)(spriteSheetWidth / spriteWidth);
            rows = (byte)(spriteSheetHeight / spriteHeight);
        }
        public Bitmap getImage(int id){
            Console.Write(rows);
            Console.Write(columns);
            int i = 0;
            for (int x = 0; x < rows; x++)
                for (int y = 0; y < columns; y++) {
                    if (i == id)
                        return Extract(Sprite, new Rectangle(y*spriteWidth, x * spriteHeight, spriteWidth, spriteHeight));
                    i++;
                }
            return null;
        }
        public static Bitmap Extract(Bitmap src, Rectangle section){
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            using (Graphics g = Graphics.FromImage(bmp)){
                g.DrawImage(src, -10, 0, section, GraphicsUnit.Pixel);
            }
            return bmp;
        }
    }
}