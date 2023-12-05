using ImGuiNET;
using System.Numerics;
using System.Drawing;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Maths;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;

namespace IMGUITEST
{
    public class Program
    {

        public static void Main(string[] args)
        {
             
            WindowOptions options = WindowOptions.Default;
            options.TransparentFramebuffer = true;
            //options.WindowBorder = WindowBorder.Hidden;
            options.Size = new Vector2D<int>(500, 250);
            options.Title = "Fahoom Projectile Motion Simulator";
            
            // Create a Silk.NET window as usual
            using var window = Window.Create(options); 

            // Declare some variables
            ImGuiController controller = null;
            GL gl = null;
            IInputContext inputContext = null;

            // Our loading function
            window.Load += () =>
            {
                controller = new ImGuiController(
                    gl = window.CreateOpenGL(), // load OpenGL
                    window, // pass in our window
                    inputContext = window.CreateInput() // create an input context
                );
            };

            IMGUITEST.Render render = new Render();

            // Handle resizes
            window.FramebufferResize += s =>
            {
                // Adjust the viewport to the new window size
                gl.Viewport(s);
            };

            // The render function
            window.Render += delta =>
            {
                // Make sure ImGui is up-to-date
                controller.Update((float) delta);

                // This is where you'll do any rendering beneath the ImGui context
                // Here, we just have a blank screen.
                gl.ClearColor(Color.FromArgb(255, (int) (.45f * 255), (int) (.55f * 255), (int) (.60f * 255)));
                gl.Clear((uint) ClearBufferMask.ColorBufferBit);

                // This is where you'll do all of your ImGUi rendering
                // Here, we're just showing the ImGui built-in demo window.
                //ImGuiNET.ImGui.ShowDemoWindow();
                render.oRender();

                // Make sure ImGui renders too!
                controller.Render();
            };

            // The closing function
            window.Closing += () =>
            {
                // Dispose our controller first
                controller?.Dispose();

                // Dispose the input context
                inputContext?.Dispose();

                // Unload OpenGL
                gl?.Dispose();
            };

            // Now that everything's defined, let's run this bad boy!
            window.Run();

            window.Dispose();
        }
    }
}