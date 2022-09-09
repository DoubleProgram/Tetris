using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Tetris{
    public class Shader{
        int Handle; //our Program were we store the shader.
        int vertexShader, fragmentShader;
        public Shader(string vertexpath, string fragmentpath){
            string vertexsource;
            using (StreamReader reader = new StreamReader(vertexpath, Encoding.UTF8))
                vertexsource = reader.ReadToEnd();
            string fragmentsource;
            using (StreamReader reader = new StreamReader(fragmentpath, Encoding.UTF8))
                fragmentsource = reader.ReadToEnd();
            
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexsource);
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentsource);

            GL.CompileShader(vertexShader);
            string infoLog = GL.GetShaderInfoLog(vertexShader);
            if (infoLog != String.Empty) Console.WriteLine(infoLog);

            GL.CompileShader(fragmentShader);
            infoLog = GL.GetShaderInfoLog(fragmentShader);
            if (infoLog != String.Empty) Console.WriteLine(infoLog);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }
        public int GetUniformLocation(string name) => GL.GetUniformLocation(Handle, name);
        public void SetVectorToUniform(Vector3 vector, int location) { GL.Uniform3(location, vector); }
        public void SetMatrix4(ref Matrix4 matrix, string name){
            int location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(location, false, ref matrix);
        }
        public void SetInt(string name, int value){
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location, value);
        }
        public void Use() => GL.UseProgram(Handle);
        
    }
}