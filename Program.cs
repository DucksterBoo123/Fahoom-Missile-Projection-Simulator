using ImGuiNET;
using ClickableTransparentOverlay;
using SixLabors.ImageSharp.ColorSpaces;
using System.Runtime.InteropServices;
using ClickableTransparentOverlay.Win32;
using SharpDX.DXGI;
using System.ComponentModel;
using System.Diagnostics.Tracing;

using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;
using Vulkan;

namespace HelloWorld;

class Program : Overlay
{
    const int GLSL_VERSION = 330;

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();
    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int hideConsole = 0;
    const int showConsole = 1;


    bool var1;
    
    public static void Main(string[] args) //imgui and raylib
    {

        IntPtr handle = GetConsoleWindow();
        ShowWindow(handle, hideConsole);
        Console.WriteLine("Starting GUI...");
        Program program = new Program();
        program.Start().Wait();


        Raylib.InitWindow(800, 480, "Fahoom Projectile Motion Renderer");

        Camera3D camera = new();
        camera.Position = new Vector3(10f, 5f, 0f);
        camera.Target = new Vector3(0f, 0f, 0f);
        camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        camera.FovY = 45.0f;
        camera.Projection = CameraProjection.CAMERA_PERSPECTIVE;



        while (!Raylib.WindowShouldClose())
        {
            program.RayLib(camera);
        }
        
        Raylib.CloseWindow();

    }

    protected override void Render() //imgui
    {
        ImGui.Begin("Goofy Ahhh");
        ImGui.Text("hello");
        ImGui.Checkbox("saodtesticles", ref var1);
        if (var1 == true)
        {
            ImGui.Text("help");
        }
        if (ImGui.Button("Exit"))
        {
            Environment.Exit(0);
        }
        ImGui.End();
    }
    void RayLib(Camera3D camera) //raylib
    {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Color.WHITE);

        Raylib.BeginMode3D(camera);

        Raylib.DrawGrid(50, 1);

        if (var1 == true)
        {
            Raylib.DrawSphere(new Vector3(0, 0, 0), 1, Color.GREEN); //test enable object in raylib via imgui
        }

        Raylib.EndMode3D();

        Raylib.EndDrawing();
    }

}