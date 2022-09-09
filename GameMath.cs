using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace Tetris
{
    public static class GameMath{
       public static Matrix4 TransformMatrix(Vector3 translation, float scalex = 1.0f, float scaley = 1.0f, float scalez = 1.0f, float anglex = 0.0f, float angley = 0.0f, float anglez = 0.0f){
            Matrix4 model = new Matrix4();
            model = Matrix4.Identity;
            Matrix4 rotation = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(anglex)) * 
                Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(anglez)) * 
                Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(anglez));
            model = Matrix4.CreateTranslation(translation);
            model = model * Matrix4.CreateScale(new Vector3(scalex, scaley, scalez)) * rotation;
            return model;
        }
    }
}