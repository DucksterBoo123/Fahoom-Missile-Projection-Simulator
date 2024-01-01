using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.BlendMode;
using Color = Raylib_cs.Color;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;

namespace MrCrossDrivesAnSTypeJaguar;

class RumbleAndCo
{

    const int GLSL_VERSION = 330;
    public static void Main()
    {

        //----------------------------------------------INIT-------------------------------------------------

        Raylib.InitWindow(600, 350, "Fahoom Projectile Motion Simulator");
        
        //vars
        float camX = 0f;
        float camY = 0f;
        float camZ = 50f;

        //models
        //Model duck = LoadModel("duck.obj");
        Model sphere = LoadModelFromMesh(GenMeshSphere(1.0f, 25, 25));
        Model nullsphere = LoadModelFromMesh(GenMeshSphere(0f, 25, 25));
        //Model plane = LoadModelFromMesh(GenMeshPlane(25,25,10,10));
        Model plane = LoadModelFromMesh(GenMeshSphere(1.0f, 25, 25));
        Model lineSphere = LoadModelFromMesh(GenMeshSphere(0.25f, 25, 25));

        //shaders
        Shader shader = LoadShader(
            "lighting.vs",
            "lighting.fs"
        );
   
        //shader.locs[(int)ShaderLocationIndex.SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");

        //fps setting
        SetTargetFPS(60);

        //----------------------------------------------NOTES------------------------------------------------

        // air resistance info
        //-----------------------
        // F(d) = -F(d)v(hat)
        // Drag Force = -Drag Force * unit vector of velocity

        // F(d) = 0.25 * p * A * v^2
        // Drag Force = 0.25 * density of fluid (air) * crossectional area of objects * speed^2

        // C(d) = F(d) / 0.5 * p * A * v^2
        // Drag Co-Efficient = Drag Force / 0.5 * density of fluid (air) * crossectional area of object * speed^2 



        // notes
        //-----------------------
        // store values in an array, add slider for playback, possible add ffmpeg support for baking video?
        // play/pause
        // static / dynamic camera?
            //camera modes?
        // instance small spheres at every point of posX and posY to make a traced path (could be heavy on renderin)

        
        //--------------------------------------------RENDERING----------------------------------------------
        
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
            if (Raylib.IsKeyDown(KEY_ONE))
            {
                camX = camX + 1f;
            }
            if (Raylib.IsKeyDown(KEY_TWO))
            {
                camX = camX - 1f;
            }
            //Y Movement
            if (Raylib.IsKeyDown(KEY_THREE))
            {
                camY = camY + 1f;
            }
            if (Raylib.IsKeyDown(KEY_FOUR))
            {
                camY = camY - 1f;
            }
            //Z Movement
            if (Raylib.IsKeyDown(KEY_FIVE))
            {
                camZ = camZ + 1f;
            }
            if (Raylib.IsKeyDown(KEY_SIX))
            {
                camZ = camZ - 1f;
            }
            //Reset Camera Position
            if (Raylib.IsKeyDown(KEY_R))
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
            float radAngle = degAngle * 1/180 * (float)Math.PI;
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
            DisableCursor();

            //---------------------------------------------DRAWING-----------------------------------------------

            Raylib.BeginDrawing();
            Raylib.ClearBackground(WHITE);
            
            Raylib.BeginMode3D(camera);

                //Grid
                DrawGrid(200, 1);

                //Objects
                DrawModel(sphere, vecsphere, 1.0f, Color.BLUE);
                DrawModel(plane, vecplane, 1.0f, Color.RED);
                
                //DrawModel(lineSphere, new Vector3(0, 10, 0), 5, Color.BLACK);
                
                //Lines
                DrawLine3D(StartPos, CurrentPos, Color.GREEN);

            Raylib.EndMode3D();

            float screeny = GetScreenHeight() / 2;
            float screenx = GetScreenWidth() / 2;

            //DrawCircle((int)screenx, (int)screeny, 5, Color.GREEN);
            DrawCircle((int)screenx, (int)screeny, 5, Color.GREEN);

            Vector2 k = GetWorldToScreen(CurrentPos, camera);

            //Raylib.DrawText("Fahoom Projectile Motion Renderer", 20,  10, 20, Color.BLACK);
            Raylib.DrawText(k.ToString(), 20,  10, 20, Color.BLACK);
            Raylib.DrawText("X Vector: " + vecX.ToString(), 20, 40, 20, Color.BLACK);
            Raylib.DrawText("Y Vector: " + vecY.ToString(), 20, 70, 20, Color.BLACK);
            Raylib.DrawText("Displacement: " + displacement, 20, 100, 20, Color.BLACK);
            
            DrawFPS(510, 10);
        
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}