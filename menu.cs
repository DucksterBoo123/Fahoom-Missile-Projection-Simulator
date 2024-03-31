using System.Security.Cryptography.X509Certificates;
using Raylib_cs;
using static Raylib_cs.Raylib;
using rlImGui_cs;
using ImGuiNET;
using System.Runtime.InteropServices;

namespace MrCrossDrivesAnSTypeJaguar;

public class Menu
{   
    public const int NumProcesses = 6;

    enum Sim{suvat = 0, suvatdrag, suvatdragcrosswind, missile, settings, exit}

    static string[] processText = { "SUVAT", "SUVAT + DRAG", "SUVAT + DRAG + CROSSWIND", "MISSILE", "SETTINGS", "EXIT" };

    public static float userG = 1;
    public static float dT = 0.01f;

    public static float suvatU = 20;
    public static float suvatDeg = 45;

    public static float suvatdragS = 20;
    public static float suvatdragM = 100;
    public static float suvatdragA = 2;
    public static float suvatdragDeg = 45;

    public static float suvatdragcwS = 20;
    public static float suvatdragcwM = 100;
    public static float suvatdragcwA = 2;
    public static float suvatdragcwCW = -500;
    public static float suvatdragcwDeg = 25;
    public static float suvatdragcwDegHor = 45;
    public static float suvatdragcwDegVer = 25;

    public static float missileS = 1;
    public static float missileT = 35;
    public static float missileA = 2;
    public static float missileRM = 100;
    public static float missileFM = 25;
    public static float missileFB = 0.05f;
    public static float missileDeg = 75;

    static void resetVars()
    {
        userG = 1;
        dT = 0.01f;

        suvatU = 20;
        suvatDeg = 45;

        suvatdragS = 20;
        suvatdragM = 100;
        suvatdragA = 2;
        suvatdragDeg = 45;

        suvatdragcwS = 20;
        suvatdragcwM = 100;
        suvatdragcwA = 2;
        suvatdragcwCW = -500;
        suvatdragcwDeg = 25;
        suvatdragcwDegHor = 45;
        suvatdragcwDegVer = 25;

        missileS = 1;
        missileT = 35;
        missileA = 2;
        missileRM = 100;
        missileFM = 25;
        missileFB = 0.05f;
        missileDeg = 75;
    }

    public static unsafe int menu()
    {

        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 640;
        const int screenHeight = 400;

        InitWindow(screenWidth, screenHeight, "Fahoom Projectile Motion Simulator");

        // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
        Image imageOrigin = LoadImage("storage/images/suvat320.png");
        ImageFormat(ref imageOrigin, PixelFormat.UncompressedR8G8B8A8);
        Texture2D texture = LoadTextureFromImage(imageOrigin);

        Image imageCopy = ImageCopy(imageOrigin);

        Sim currentProcess = Sim.suvat;
        bool textureReload = false;

        Rectangle[] toggleRecs = new Rectangle[NumProcesses];
        int mouseHoverRec = -1;

        for (int i = 0; i < NumProcesses; i++)
        {
            toggleRecs[i] = new Rectangle(40, 50 + 32 * i, 150, 30);
        }

        //int n = 0;
        int j = 0;

        SetTargetFPS(60);
        //---------------------------------------------------------------------------------------

        rlImGui.Setup(true);

        // Main game loop
        while (!WindowShouldClose())
        {
            // Update
            //----------------------------------------------------------------------------------

            // Mouse toggle group logic
            for (int i = 0; i < NumProcesses; i++)
            {
                if (CheckCollisionPointRec(GetMousePosition(), toggleRecs[i]))
                {
                    mouseHoverRec = i;

                    if (IsMouseButtonReleased(MouseButton.Left))
                    {
                        currentProcess = (Sim)i;
                        textureReload = true;
                    }

                    //j = i;
                    break;
                }
                else
                {
                    mouseHoverRec = -1;
                }
            }

            // Keyboard toggle group logic
            if (IsKeyPressed(KeyboardKey.Down))
            {
                currentProcess++;
                if ((int)currentProcess > (NumProcesses - 1))
                {
                    currentProcess = 0;
                }

                textureReload = true;
            }
            else if (IsKeyPressed(KeyboardKey.Up))
            {
                currentProcess--;
                if (currentProcess < 0)
                {
                    currentProcess = Sim.exit;
                }

                textureReload = true;
            }

            if(currentProcess == Sim.suvat && IsKeyPressed(KeyboardKey.Enter) || currentProcess == Sim.suvat && CheckCollisionPointRec(GetMousePosition(), toggleRecs[0]) && IsMouseButtonPressed(MouseButton.Left))
            {
                Raylib.CloseWindow();
                Sims.suvat();
                Menu.menu();
            }

            if(currentProcess == Sim.suvatdrag && IsKeyPressed(KeyboardKey.Enter) || currentProcess == Sim.suvatdrag && CheckCollisionPointRec(GetMousePosition(), toggleRecs[1]) && IsMouseButtonPressed(MouseButton.Left))
            {
                Raylib.CloseWindow();
                Sims.missile();
                Menu.menu();
            }

            if(currentProcess == Sim.suvatdragcrosswind && IsKeyPressed(KeyboardKey.Enter) || currentProcess == Sim.suvatdragcrosswind && CheckCollisionPointRec(GetMousePosition(), toggleRecs[2]) && IsMouseButtonPressed(MouseButton.Left))
            {
                Raylib.CloseWindow();
                Sims.missile2();
                Menu.menu();
            }

            if(currentProcess == Sim.missile && IsKeyPressed(KeyboardKey.Enter) || currentProcess == Sim.missile && CheckCollisionPointRec(GetMousePosition(), toggleRecs[3]) && IsMouseButtonPressed(MouseButton.Left))
            {
                Raylib.CloseWindow();
                Sims.missile3();
                Menu.menu();
            }

            if(currentProcess == Sim.settings && IsKeyPressed(KeyboardKey.Enter) || currentProcess == Sim.settings && CheckCollisionPointRec(GetMousePosition(), toggleRecs[4]) && IsMouseButtonPressed(MouseButton.Left))
            {
                
            }

            if(currentProcess == Sim.exit && IsKeyPressed(KeyboardKey.Enter) || currentProcess == Sim.exit && CheckCollisionPointRec(GetMousePosition(), toggleRecs[5]) && IsMouseButtonPressed(MouseButton.Left))
            {
                Environment.Exit(0);
            }

            if (textureReload)
            {
                UnloadImage(imageCopy);
                imageCopy = ImageCopy(imageOrigin);


                switch (currentProcess)
                {
                    case Sim.suvat:
                        UnloadImage(imageOrigin);
                        UnloadImage(imageCopy);
                        imageOrigin = LoadImage("storage/images/suvat320.png");
                        imageCopy = LoadImage("storage/images/suvat320.png");
                        break;
                    case Sim.suvatdrag:
                        UnloadImage(imageOrigin);
                        UnloadImage(imageCopy);
                        imageOrigin = LoadImage("storage/images/suvat+drag320.png");
                        imageCopy = LoadImage("storage/images/suvat+drag320.png");
                        break;
                    case Sim.suvatdragcrosswind:
                        UnloadImage(imageOrigin);
                        UnloadImage(imageCopy);
                        imageOrigin = LoadImage("storage/images/suvat+drag+crosswind320.png");
                        imageCopy = LoadImage("storage/images/suvat+drag+crosswind320.png");
                        break;
                    case Sim.missile:
                        UnloadImage(imageOrigin);
                        UnloadImage(imageCopy);
                        imageOrigin = LoadImage("storage/images/missile320.png");
                        imageCopy = LoadImage("storage/images/missile320.png");
                        break;
                    case Sim.settings:
                        UnloadImage(imageOrigin);
                        UnloadImage(imageCopy);
                        imageOrigin = LoadImage("storage/images/settings320.png");
                        imageCopy = LoadImage("storage/images/settings320.png");
                        break;
                    case Sim.exit:
                        UnloadImage(imageOrigin);
                        UnloadImage(imageCopy);
                        imageOrigin = LoadImage("storage/images/menu320.png");
                        imageCopy = LoadImage("storage/images/menu320.png");
                        break;
                }

                // Get pixel data from image (RGBA 32bit)
                Color* pixels = LoadImageColors(imageCopy);
                UpdateTexture(texture, pixels);
                UnloadImageColors(pixels);

                textureReload = false;
            }
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("Fahoom Projectile Motion Simulator:", 40, 30, 10, Color.DarkGray);

            // Draw rectangles
            for (int i = 0; i < NumProcesses; i++)
            {
                DrawRectangleRec(toggleRecs[i], (i == (int)currentProcess) ? Color.SkyBlue : Color.LightGray);
                DrawRectangleLines(
                    (int)toggleRecs[i].X,
                    (int)toggleRecs[i].Y,
                    (int)toggleRecs[i].Width,
                    (int)toggleRecs[i].Height,
                    (i == (int)currentProcess) ? Color.Blue : Color.Gray
                );

                int labelX = (int)(toggleRecs[i].X + toggleRecs[i].Width / 2);
                DrawText(
                    processText[i],
                    (int)(labelX - MeasureText(processText[i], 10) / 2),
                    (int)toggleRecs[i].Y + 11,
                    10,
                    (i == (int)currentProcess) ? Color.DarkBlue : Color.DarkGray
                );
            }

            int x = screenWidth - texture.Width - 60;
            int y = screenHeight / 2 - texture.Height / 2;
            DrawTexture(texture, x, y, Color.White);
            DrawRectangleLines(x, y, texture.Width, texture.Height, Color.Black);

            rlImGui.Begin();

            if(currentProcess != Sim.settings && currentProcess != Sim.exit)
            {
                ImGui.Begin("Variables",
                    ImGuiWindowFlags.NoBackground |
                    ImGuiWindowFlags.NoResize |
                    ImGuiWindowFlags.NoDocking |
                    ImGuiWindowFlags.NoCollapse |
                    ImGuiWindowFlags.NoTitleBar |
                    ImGuiWindowFlags.NoMove);
                ImGui.SetWindowPos(new System.Numerics.Vector2(40, 250));
                ImGui.SetWindowSize(new System.Numerics.Vector2(150, 150));
                ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(0, 0, 0, 255));

                if (currentProcess == Sim.suvat)
                {
                    ImGui.Text("Initial Velocity");
                    ImGui.InputFloat("  ", ref suvatU, 0.5f);
                    ImGui.Text("Angle");
                    ImGui.InputFloat("      ", ref suvatDeg, 5f);
                }

                if (currentProcess == Sim.suvatdrag)
                {
                    ImGui.Text("Current Speed");
                    ImGui.InputFloat(" ", ref suvatdragS, 0.5f);
                    ImGui.Text("Mass");
                    ImGui.InputFloat("  ", ref suvatdragM, 0.5f);
                    ImGui.Text("Cross-Sectional Area");
                    ImGui.InputFloat("   ", ref suvatdragA, 0.5f);
                    ImGui.Text("Angle");
                    ImGui.InputFloat("    ", ref suvatdragDeg, 0.5f);
                }

                if (currentProcess == Sim.suvatdragcrosswind)
                {
                    ImGui.Text("Current Speed");
                    ImGui.InputFloat(" ", ref suvatdragcwS, 0.5f);
                    ImGui.Text("Mass");
                    ImGui.InputFloat("  ", ref suvatdragcwM, 0.5f);
                    ImGui.Text("Cross-Sectional Area");
                    ImGui.InputFloat("   ", ref suvatdragcwA, 0.5f);
                    ImGui.Text("Crosswind");
                    ImGui.InputFloat("    ", ref suvatdragcwCW, 0.5f);
                    ImGui.Text("Crosswind Angle To The Vertical");
                    ImGui.InputFloat("     ", ref suvatdragcwDegVer, 0.5f);
                    ImGui.Text("Crosswind Angle To The Horizontal");
                    ImGui.InputFloat("      ", ref suvatdragcwDegHor, 0.5f);
                    ImGui.Text("Angle");
                    ImGui.InputFloat("       ", ref suvatdragcwDeg, 0.5f);
                }

                if (currentProcess == Sim.missile)
                {
                    ImGui.Text("Current Speed");
                    ImGui.InputFloat(" ", ref missileS, 0.5f);
                    ImGui.Text("Thrust");
                    ImGui.InputFloat("  ", ref missileT, 0.5f);
                    ImGui.Text("Cross-Sectional Area");
                    ImGui.InputFloat("   ", ref missileA, 0.5f);
                    ImGui.Text("Rocket Mass");
                    ImGui.InputFloat("    ", ref missileRM, 0.5f);
                    ImGui.Text("Fuel Mass");
                    ImGui.InputFloat("     ", ref missileFM, 0.5f);
                    ImGui.Text("Rate of Fuel Burn");
                    ImGui.InputFloat("      ", ref missileFB, 0.5f);
                    ImGui.Text("Angle");
                    ImGui.InputFloat("       ", ref missileDeg, 0.5f);
                }

                ImGui.Text("Acceleration");
                ImGui.Text("Relative to Earth");
                ImGui.InputFloat("                      ", ref userG, 0.5f);
                ImGui.Text("Time");
                ImGui.InputFloat("", ref dT, 0.005f);
                if (ImGui.Button("Reset All Variables"))
                {
                    Menu.resetVars();
                }
            }

            ImGui.End();
            rlImGui.End();

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadTexture(texture);
        UnloadImage(imageOrigin);
        UnloadImage(imageCopy);

        rlImGui.Shutdown();
        CloseWindow();
        //--------------------------------------------------------------------------------------

        return 0;
    }
}