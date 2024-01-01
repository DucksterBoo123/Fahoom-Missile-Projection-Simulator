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
using Veldrid;

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

        float camX = 0f;
        float camY = 0f;
        float camZ = 50f;

        //models
        Model sphere = LoadModelFromMesh(GenMeshSphere(1.0f, 25, 25));
        Model nullsphere = LoadModelFromMesh(GenMeshSphere(0f, 25, 25));
        Model plane = LoadModelFromMesh(GenMeshSphere(1.0f, 25, 25));
        Model lineSphere = LoadModelFromMesh(GenMeshSphere(0.25f, 25, 25));

        /*
        //shaders
        Shader shader = LoadShader(
            "lighting.vs",
            "lighting.fs"
        );
        */
        //fps setting
        SetTargetFPS(60);



        while (!Raylib.WindowShouldClose())
        {
            //Camera
            Camera3D camera = new();
            camera.Position = new Vector3(camX, camY, camZ);
            camera.Target = new Vector3(0.0f, 0.0f, 0.0f);      // Camera looking at point
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);          // Camera up vector (rotation towards target)
            camera.FovY = 60.0f;                                // Camera field-of-view Y  
            camera.Projection = CameraProjection.CAMERA_PERSPECTIVE;

            //Camera Controls
            //X Movement
            if (Raylib.IsKeyDown(KeyboardKey.KEY_ONE))
            {
                camX = camX + 1f;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_TWO))
            {
                camX = camX - 1f;
            }
            //Y Movement
            if (Raylib.IsKeyDown(KeyboardKey.KEY_THREE))
            {
                camY = camY + 1f;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_FOUR))
            {
                camY = camY - 1f;
            }
            //Z Movement
            if (Raylib.IsKeyDown(KeyboardKey.KEY_FIVE))
            {
                camZ = camZ + 1f;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_SIX))
            {
                camZ = camZ - 1f;
            }
            //Reset Camera Position
            if (Raylib.IsKeyDown(KeyboardKey.KEY_R))
            {
                camX = -50f;
                camY = 0f;
                camZ = 0f;
            }

            //vars
            float gravac = -9.81f;
            float speed = 17f;
            float vspeed;
            float hspeed;
            float degAngle = 45f; //angle to the vertical, e.g. straight up is 0, horizontal is 90
            float radAngle = degAngle * 1 / 180 * (float)Math.PI;
            float t = 0;
            double posX;
            double posY;
            double deltaTime = GetTime();
            deltaTime = deltaTime - t;

            //calc
            //s = ut + 1/2(a(t^2))
            vspeed = speed * (float)Math.Cos(radAngle);
            hspeed = speed * (float)Math.Sin(radAngle);

            posY = vspeed * deltaTime + 0.5 * gravac * deltaTime * deltaTime;
            posX = deltaTime * hspeed;

            //positioning
            float vecY = (float)posY;
            float vecX = (float)posX;

            Vector3 vecsphere = new Vector3(vecX, vecY, 0);
            Vector3 vecplane = new Vector3(0f, -5f, 0f);

            //collision
            bool collision = false;

            collision = CheckCollisionSpheres(vecsphere, 0.1f, vecplane, 0.1f);
            if (collision)
            {
                Raylib.DrawText("HIT", 50, 50, 50, Color.BLACK);
            }

            //Vector Positions
            Vector3 StartPos = new Vector3(0, 0, 0);
            Vector3 CurrentPos = new Vector3(vecX, vecY, 0);

            //displacement
            Vector3 displacementvec = CurrentPos - StartPos;
            float displacement = (float)Math.Sqrt(displacementvec.Y * displacementvec.Y + displacementvec.Z * displacementvec.Z);

            //hide cursor
            //DisableCursor();

            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.WHITE);

            Raylib.BeginMode3D(camera);

            if (program.var1 == true)
            {
                Raylib.DrawSphere(new Vector3(0, 0, 0), 1, Color.GREEN); //test enable object in raylib via imgui
            }

            //Grid
            DrawGrid(200, 1);

            //Objects
            DrawModel(sphere, vecsphere, 1.0f, Color.BLUE);
            DrawModel(plane, vecplane, 1.0f, Color.RED);


            //Lines
            DrawLine3D(StartPos, CurrentPos, Color.GREEN);


            Raylib.EndMode3D();
            
            float screeny = GetScreenHeight() / 2;
            float screenx = GetScreenWidth() / 2;

            //DrawCircle((int)screenx, (int)screeny, 5, Color.GREEN);
            DrawCircle((int)screenx, (int)screeny, 5, Color.GREEN);

            Vector2 k = GetWorldToScreen(CurrentPos, camera);

            //Raylib.DrawText("Fahoom Projectile Motion Renderer", 20,  10, 20, Color.BLACK);
            Raylib.DrawText(k.ToString(), 20, 10, 20, Color.BLACK);
            Raylib.DrawText("X Vector: " + vecX.ToString(), 20, 40, 20, Color.BLACK);
            Raylib.DrawText("Y Vector: " + vecY.ToString(), 20, 70, 20, Color.BLACK);
            Raylib.DrawText("Displacement: " + displacement, 20, 100, 20, Color.BLACK);

            DrawFPS(510, 10);

            Raylib.EndDrawing();
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

}