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

using System.Text.Json;

namespace HelloWorld;

class Program : Overlay
{
    //raylib vars

    const int GLSL_VERSION = 330;

    //imgui vars

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();
    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int hideConsole = 0;
    const int showConsole = 1;

    string homePage = "";
    string fahoomPath = "fahoom.jpg";

    float suvatSInput;
    float suvatUInput;
    float suvatVInput;
    float suvatAInput;
    float suvatTInput;
    float dragUInput;
    float dragAInput;
    float dragMInput;
    float dragDInput;

    bool home = true;
    bool simple = false;
    bool advanced = false;
    bool settings = false;
    bool fahoom = false;
    bool about = false;
    bool srgb = true;
    bool fahoomBool = false;

    bool HCM = false;
    bool FM = false;

    int windowxsize = 500;
    int windowysize = 250;
    int homeCounter = 0;
    int pageCounter = 0;
    int spaceCounter = 0;

    uint fahoomWidth;
    uint fahoomHeight;

    IntPtr fahoomImage;

    Vector4 col = new Vector4(1f, 1f, 1f, 1f);
    Vector4 hcm = new Vector4(1f, 0f, 0f, 1f);
    Vector4 regButton = new Vector4(1f, 1f, 1f, 0.5f);
    Vector4 hcmButton = new Vector4(1f, 1f, 0f, 1f);

        // array setup
    public float[]? suvats { get; set; }
    public float[]? drag { get; set; }


    //shared vars
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

        //shaders
        Raylib_cs.Shader shader = LoadShader(
            "lighting.vs",
            "lighting.fs"
        );

        //fps setting
        SetTargetFPS(6000);



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
                Raylib.DrawSphere(new Vector3(0, 0, 0), 1, Color.RED); //test enable object in raylib via imgui
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
        /*
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
        */

        void page(string page)
        {
            if (page == "about")
            {
                about = true;
                settings = false;
                home = false;
                simple = false;
                advanced = false;
                fahoom = false;
            }

            if (page == "settings")
            {
                about = false;
                settings = true;
                home = false;
                simple = false;
                advanced = false;
                fahoom = false;
            }

            if (page == "home")
            {
                about = false;
                settings = false;
                home = true;
                simple = false;
                advanced = false;
                fahoom = false;
            }

            if (page == "simple")
            {
                about = false;
                settings = false;
                home = false;
                simple = true;
                advanced = false;
                fahoom = false;
            }

            if (page == "advanced")
            {
                about = false;
                settings = false;
                home = false;
                simple = false;
                advanced = true;
                fahoom = false;
            }

            if (page == "fahoom")
            {
                about = false;
                settings = false;
                home = false;
                simple = false;
                advanced = false;
                fahoom = true;
            }
        }

        // styling
        ImGuiStylePtr style = ImGui.GetStyle();
        style.WindowBorderSize = 2.0f;
        style.Colors[(int)ImGuiCol.Border] = col;
        style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(1.0f, 1.0f, 1.0f, 0.45f);
        style.Colors[(int)ImGuiCol.Button] = regButton;
        // style selection 
        if (HCM == true)
        {
            style.Colors[(int)ImGuiCol.Text] = hcm;
            style.Colors[(int)ImGuiCol.Button] = hcmButton;
        }
        else if (HCM == false)
        {
            style.Colors[(int)ImGuiCol.Text] = col;
            style.Colors[(int)ImGuiCol.Button] = regButton;
        }


        // instancing window (with flags that instance a menu bar | disables the collapse window feature | disables the docking feature)
        ImGui.Begin("Fahoom Projectile Motion Simulator", ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoResize);
        ImGui.SetWindowSize(new Vector2(windowxsize, windowysize));
        // menu bar
        if (ImGui.BeginMenuBar())
        {
            // app settings
            if (ImGui.BeginMenu("FPMS"))
            {
                if (ImGui.MenuItem("About"))
                {
                    page("about");
                }
                if (ImGui.MenuItem("Settings"))
                {
                    page("settings");
                }
                if (FM == true)
                {
                    if (ImGui.MenuItem("Fahoom Mode"))
                    {
                        page("fahoom");
                    }
                }
                if (ImGui.MenuItem("Exit"))
                {
                    // exits the program
                    Environment.Exit(0);
                }
                ImGui.EndMenu();
            }
            // app mode
            if (ImGui.BeginMenu("Mode"))
            {
                if (ImGui.MenuItem("Simple"))
                {
                    page("simple");
                }
                if (ImGui.MenuItem("Advanced"))
                {
                    page("advanced");
                }
                ImGui.EndMenu();
            }
            //development menu
            if (ImGui.BeginMenu("Debug"))
            {
                if (ImGui.MenuItem("Home"))
                {
                    page("home");
                }
                ImGui.EndMenu();
            }
            ImGui.EndMenuBar();
        }
        // page selector
        // test page
        if (home == true)
        {
            ImGui.Text("Home Page");
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();
            ImGui.NewLine();

            ImGui.Text(homePage);
            ImGui.NewLine();

            Vector2 windowSize = ImGui.GetWindowSize();

            ImGui.BeginChild("home", new System.Numerics.Vector2(windowSize.X - 400, 0));
            if (ImGui.Button("Home", new System.Numerics.Vector2(100, 50)))
            {
                homePage = homePage + "Home";
                homeCounter = homeCounter + 1;
            }
            ImGui.Text(homeCounter.ToString());
            ImGui.EndChild();

            ImGui.SameLine();

            ImGui.BeginChild("page", new System.Numerics.Vector2(windowSize.X - 400, 0));
            if (ImGui.Button("Page", new System.Numerics.Vector2(100, 50)))
            {
                homePage = homePage + "Page";
                pageCounter = pageCounter + 1;
            }
            ImGui.Text(pageCounter.ToString());
            ImGui.EndChild();

            ImGui.SameLine();

            ImGui.BeginChild("space", new System.Numerics.Vector2(windowSize.X - 400, 0));
            if (ImGui.Button("Space", new System.Numerics.Vector2(100, 50)))
            {
                homePage = homePage + " ";
                spaceCounter = spaceCounter + 1;
            }
            ImGui.Text(spaceCounter.ToString());
            ImGui.EndChild();
            ImGui.SameLine();
            if (ImGui.Button("Reset", new System.Numerics.Vector2(100, 50)))
            {
                homePage = "";
                homeCounter = 0;
                pageCounter = 0;
                spaceCounter = 0;
            }

            if (homePage == "HomePagePage Page")
            {
                page("fahoom");
                fahoomBool = true;
            }
        }
        // simple page
        if (simple == true)
        {
            // title
            ImGui.Text("Simple Mode");

            // gets the of the window and stores it in vector (2 dimensional) form
            Vector2 windowSize = ImGui.GetWindowSize();

            // left frame
            ImGui.BeginChild("1", new System.Numerics.Vector2(windowSize.X - 175, 0));

            // suvat inputs
            ImGui.NewLine();
            ImGui.PushItemWidth(150);
            ImGui.InputFloat("(S) Displacement", ref suvatSInput, 1);
            ImGui.PushItemWidth(150);
            ImGui.InputFloat("(U) Initial Velocity", ref suvatUInput, 1);
            ImGui.PushItemWidth(150);
            ImGui.InputFloat("(V) Final Velocity", ref suvatVInput, 1);
            ImGui.PushItemWidth(150);
            ImGui.InputFloat("(A) Acceleration", ref suvatAInput, 1);
            ImGui.PushItemWidth(150);
            ImGui.InputFloat("(T) Time", ref suvatTInput, 1);
            if (ImGui.Button("Reset"))
            {
                suvatSInput = 0f;
                suvatUInput = 0f;
                suvatVInput = 0f;
                suvatAInput = 0f;
                suvatTInput = 0f;
            }

            // sets negative values for time to 0 (as time is scalar and cannot be negative)
            if (suvatTInput < 0)
            {
                suvatTInput = 0;
            }

            ImGui.EndChild();

            // positions frame below inline with the fram above
            ImGui.SameLine();

            // right frame
            ImGui.BeginChild("2");

            // variable display panel
            ImGui.NewLine();

            ImGui.Text("Displacement - " + suvatSInput.ToString());
            ImGui.Text("Initial Velocity - " + suvatUInput.ToString());
            ImGui.Text("Final Velocity - " + suvatVInput.ToString());
            ImGui.Text("Acceleration - " + suvatAInput.ToString());
            ImGui.Text("Time - " + suvatTInput.ToString());

            // array storing suvat variables
            suvats = new[] { suvatSInput, suvatUInput, suvatVInput, suvatAInput, suvatTInput };

            // serializes array data to json file
            if (ImGui.Button("Send To Render"))
            {
                string fileName = "suvatProperties.json";
                string jsonString = JsonSerializer.Serialize(suvats);
                File.WriteAllText(fileName, jsonString);

                // launch next application
            }

            ImGui.EndChild();
        }
        // advanced page
        if (advanced == true)
        {
            ImGui.Text("Advanced Mode");

            Vector2 windowSize = ImGui.GetWindowSize();

            ImGui.BeginChild("1", new System.Numerics.Vector2(windowSize.X - 175, 0));

            ImGui.NewLine();
            ImGui.PushItemWidth(150);
            ImGui.InputFloat("(U) Initial Velocity", ref dragUInput, 1);
            ImGui.PushItemWidth(150);
            ImGui.InputFloat("(A) Initial Angle", ref dragAInput, 1);
            ImGui.PushItemWidth(150);
            ImGui.InputFloat("(M) Mass", ref dragMInput, 1);
            ImGui.PushItemWidth(150);
            ImGui.InputFloat("(D) Diameter", ref dragDInput, 1);
            if (ImGui.Button("Reset"))
            {
                dragUInput = 0f;
                dragAInput = 0f;
                dragMInput = 0f;
                dragDInput = 0f;
            }
            if (dragMInput < 0)
            {
                dragMInput = 0;
            }

            if (dragAInput < 0)
            {
                dragAInput = 0;
            }

            ImGui.EndChild();

            ImGui.SameLine();

            ImGui.BeginChild("2");
            ImGui.NewLine();
            ImGui.Text("Initial Velocity - " + dragUInput.ToString());
            ImGui.Text("Initial Angle - " + dragAInput.ToString());
            ImGui.Text("Mass - " + dragMInput.ToString());
            ImGui.Text("Diameter - " + dragDInput.ToString());

            drag = new[] { dragUInput, dragAInput, dragMInput, dragDInput };

            if (ImGui.Button("Send To Render"))
            {
                string fileName = "dragProperties.json";
                string jsonString = JsonSerializer.Serialize(drag);
                File.WriteAllText(fileName, jsonString);

                // launch next application
            }

            ImGui.EndChild();

        }
        // about page
        if (about == true)
        {
            ImGui.Text("Developer: Muhammad Khan");
            ImGui.Text("Special Thanks To: Fahim Hamid");
            ImGui.Text("My Github: https://github.com/DucksterBoo123");
            ImGui.Text("Fahim's Github: https://github.com/Fahoom");
            ImGui.Text("Dev Start: 22/11/23");
            ImGui.Text("Dev End: ");
            ImGui.Text("License: MIT");
        }
        // settings page
        if (settings == true)
        {
            Vector2 windowSize = ImGui.GetWindowSize();

            ImGui.BeginChild("1", new Vector2(windowSize.X - 250, 0));
            // options
            ImGui.SeparatorText("Options");
            ImGui.Checkbox("High Contrast Mode", ref HCM);
            ImGui.Checkbox("Fahoom Mode", ref FM);
            if (ImGui.Button("Reset App Colour"))
            {
                HCM = false;
                col = new Vector4(1f, 1f, 1f, 1f);
            }
            ImGui.SeparatorText("Sizing");
            ImGui.PushItemWidth(175);
            ImGui.SliderInt("WindowX", ref windowxsize, 500, 1000);
            ImGui.SliderInt("WindowY", ref windowysize, 150, 1000);
            // window sizing
            if (ImGui.Button("Reset Window Size"))
            {
                windowxsize = 500;
                windowysize = 250;
            }

            ImGui.EndChild();

            ImGui.SameLine();

            ImGui.BeginChild("2");
            // colour picker
            ImGui.ColorPicker4("App Colour", ref col);
            ImGui.EndChild();
        }
        // FAHOOM PAGE
        if (fahoom == true)
        {
            ImGui.Checkbox("Fahoom Pics", ref fahoomBool);
            if (fahoomBool == true)
            {
                windowxsize = 1000;
                windowysize = 1000;
                AddOrGetImagePointer(fahoomPath, srgb, out fahoomImage, out fahoomWidth, out fahoomHeight);
                ImGui.Image(fahoomImage, new Vector2(1000, 1000));
            }
            if (fahoomBool == false)
            {
                windowxsize = 500;
                windowysize = 250;
            }
            ImGui.End();
        }
    }

}