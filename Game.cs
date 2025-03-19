using System.Globalization;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace TestOne
{
    public class Game : GameWindow
    {
        public string RUNHOME = "/home/nova/RiderProjects/OpenTK-OpenGL-HELP/";
        
        
        private int VertexBufferObject;
        private Shader shader;
        public Game(int width, int height, string title) : base(GameWindowSettings.Default,
            new NativeWindowSettings() { Size = (width, height), Title = title }) { }

        protected override void OnUpdateFrame(FrameEventArgs args) //frame updates ig idrktbh
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                 shader = new Shader(RUNHOME + "\\shader.vert", RUNHOME + "\\shader.frag");
            }
            else
            {
                shader = new Shader(RUNHOME + "/shader.vert", RUNHOME + "/shader.frag");
            }
           
            //code here later pls
        }

        protected override void OnRenderFrame(FrameEventArgs args) //runs rendering i think?
        {
            base.OnRenderFrame(args);
            
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            //code here later
            
            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e) //handles window resizing i think
        {
            base.OnFramebufferResize(e);
            
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        public float[] vertices =
        {
            -0.5f, -0.5f, 0.0f, //B-Left Vertex
            0.5f, -0.5f, 0, 0f, //B-Right vertex
            0.0f, 0.5f, 0.0f //Top vertex
        };

        
        
    }

    public class Shader
    {
        private int Handle;

        public Shader(string vertexPath, string fragmentPath)
        {
            string VertexShaderSource = File.ReadAllText(vertexPath);         // takes vertex path
            string FragmentShaderSource = File.ReadAllText(fragmentPath);     // takes Fragment path
            var VertexShader = GL.CreateShader(ShaderType.VertexShader);   // actually makes shader
            GL.ShaderSource(VertexShader, VertexShaderSource);                // idk what this does

            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);
            
            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int successV);
            if (successV == 0)
            {
                string infolog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infolog);
            }
            
            GL.CompileShader(FragmentShader);
            
            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int successF);
            if (successF == 0)
            {
                string infolog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infolog);
            }

            Handle = GL.CreateProgram();
            
            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);
            GL.LinkProgram(Handle);
            
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int successP);

            if (successP == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

            
        }
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    
}
// https://opentk.net/learn/chapter1/2-hello-triangle.html#linking-vertex-attributes