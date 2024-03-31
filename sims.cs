using Raylib_cs;
using System.Diagnostics;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;

namespace MrCrossDrivesAnSTypeJaguar;

class Sims
{

    public static void initWindow()
    {
        Raylib.InitWindow(640, 400, "Fahoom Projectile Motion Simulator");
    }

    public static void drawBackground()
    {
        Texture2D background = LoadTexture("storage/images/background2.png");
        Rectangle backgroundSource = new Rectangle(0, 0, 590, 410);
        Rectangle backgroundDestination = new Rectangle(0, 0, 800, 450);
        DrawTexturePro(background, backgroundSource, backgroundDestination, new System.Numerics.Vector2(0,0), 0, Color.White);
    }

    public static void drawTrails(Vector3 a, Vector b, List<Vector3> trails)
    {
        Vector3 StartPos = a;
        Vector currentPosition = b;
        List<Vector3> trail = trails;
        trail.Add(currentPosition.getVector3());
        if (trail.Count > 1)
        {
            for (int i = 1; i < trail.Count; i++)
            {
                Raylib.DrawLine3D(trail[i - 1], trail[i], Color.Red);
            }
        }

        DrawLine3D(StartPos, currentPosition.getVector3(), Color.Green);
    }

    public static float MinValue(float a, float b)
    {
        if (a < b)
        {
            return a;
        }
        else
        {
            return b;
        }
    }

    public static float MaxValue(float a, float b)
    {
        if (a > b)
        {
            return a;
        }
        else
        {
            return b;
        }
    }

    const int GLSL_VERSION = 330;
    public static void suvat()
    {
        //----------------------------------------------INIT-------------------------------------------------

        initWindow();
        
        //vars
        float screeny = GetScreenHeight() / 2;
        float screenx = GetScreenWidth() / 2;

        float minCamX = float.PositiveInfinity;
        float maxCamX = float.NegativeInfinity;
        float minCamY = float.PositiveInfinity;
        float maxCamY = float.NegativeInfinity;
        
        float currentSpeed = Menu.suvatU;
        float userDeg = Menu.suvatDeg;
        float initialAngleRad = userDeg * 1/180 * (float)Math.PI;
        float mass = 100f;
        float earthRadius = 6371000f;
        float earthMass = 5.972e24f;
        float bigG = 6.67e-11f;
        float userG = Menu.userG;
        //slider for dT / "accuracy of simulation"
        float dT = Menu.dT;

        float magnitudeOfFDTG = userG * -(bigG * earthMass * mass)/(earthRadius * earthRadius);

        //Vectors
        List<Vector3> trail = new List<Vector3>();
        Vector currentVelocity = new Vector(userDeg, currentSpeed, 0, true);
        Vector forceDueToGravity = new Vector(0, magnitudeOfFDTG, 0);
        Vector totalForce = forceDueToGravity;
        Vector currentPosition = new Vector(0, 0, 0);
        Vector currentAcceleration;

        //Models
        Model sphere = LoadModelFromMesh(GenMeshSphere(1.0f, 25, 25));
        Model plane = LoadModelFromMesh(GenMeshPlane(50,50,20,20));

        //Shaders
        Shader shader = LoadShader("lighting.vs", "lighting.fs");

        //Testing
        //Vector testVec = new Vector((float)(Math.Atan(1/Math.Sqrt(2))), (float)Math.Sqrt(3), (float)(Math.PI)/4, true);
        //Vector testVec2 = new Vector(0, 0, 0, false);
        //Console.WriteLine(testVec.debugLogVector());
        //Vector unitTestVec = testVec.unitVector();
        //Console.WriteLine(unitTestVec.debugLogVector());
        //Console.WriteLine(unitTestVec.Magnitude());

        //Console.WriteLine(testVec.subVector(testVec2).debugLogVector());
        //Console.WriteLine(testVec2.getVector3().X);
        //Console.WriteLine(testVec2.getVector3().Y);
        //Console.WriteLine(testVec2.getVector3().Z);

        //fps setting
        SetTargetFPS(60);
        
        //--------------------------------------------RENDERING----------------------------------------------
        
        while (!Raylib.WindowShouldClose())
        {
            //Camera
            Camera3D camera = new();
            
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 60.0f;
            camera.Projection = CameraProjection.Perspective;

            currentSpeed = currentVelocity.Magnitude();
            magnitudeOfFDTG = userG * -(bigG * earthMass * mass)/(earthRadius * earthRadius);

            forceDueToGravity = new Vector(0, magnitudeOfFDTG, 0);
            totalForce = forceDueToGravity;

            currentAcceleration = totalForce.Scale(1/mass);
            // v = u + at
            currentVelocity = currentVelocity.addVector(currentAcceleration.Scale(dT));
            // s = vt - 1/2at^2
            currentPosition = currentPosition.addVector(currentVelocity.Scale(dT).subVector(currentAcceleration.Scale(0.5f * dT * dT)));
            
            //Collision
            if(currentPosition.hasCollidedWithGround())
            {
                dT = 0;
            }

            minCamX = MinValue(currentPosition.getX(), minCamX);
            minCamY = MinValue(currentPosition.getY(), minCamY);
            maxCamX = MaxValue(currentPosition.getX(), maxCamX);
            maxCamY = MaxValue(currentPosition.getY(), maxCamY);

            float dY = maxCamY - minCamY;
            float dX = maxCamX - minCamX;

            float maxCamDistance = MaxValue(dY, dX);
            float maxCamZDistance = maxCamDistance / (2 * (float)Math.Tan(((camera.FovY/180)*Math.PI)/2));
            float averageCamYDistance = (maxCamY + minCamY)/2;
            float averageCamXDistance = (maxCamX + minCamX)/2;
            
            //Vectors
            Vector camPos = new Vector(averageCamXDistance, averageCamYDistance, maxCamZDistance * (float)1.1);
            Vector targetPos = new Vector(averageCamXDistance, averageCamYDistance, 0);
            Vector3 StartPos = new Vector3(0, 0, 0);

            camera.Position = camPos.getVector3();
            camera.Target = targetPos.getVector3();

            //hide cursor
            //DisableCursor();

            //---------------------------------------------DRAWING-----------------------------------------------

            Raylib.BeginDrawing();
            Raylib.ClearBackground(White);
            drawBackground();
            Raylib.BeginMode3D(camera);

                //Grid
                DrawGrid(200, 1);

                //3D Objects
                DrawModel(sphere, currentPosition.getVector3(), 0.5f, Color.Blue);
                DrawModel(plane, new Vector(0, 0, 0, false).getVector3(), 2f, Color.Black);

            //Lines
            drawTrails(StartPos, currentPosition, trail);

            Raylib.EndMode3D();

            //Raylib.DrawText("Fahoom Projectile Motion Renderer", 20,  10, 20, Color.Black);
            Raylib.DrawText("Current Speed: " + currentSpeed, 20, 10, 20, Color.Black);
            
            DrawFPS(510, 10);
        
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    public static void missile()
    {

        initWindow();
        
        //vars
        float screeny = GetScreenHeight() / 2;
        float screenx = GetScreenWidth() / 2;

        float minCamX = float.PositiveInfinity;
        float maxCamX = float.NegativeInfinity;
        float minCamY = float.PositiveInfinity;
        float maxCamY = float.NegativeInfinity;
        
        float currentSpeed = Menu.suvatdragS;
        float userDeg = Menu.suvatdragDeg;
        float initialAngleRad = userDeg * 1/180 * (float)Math.PI;
        float mass = Menu.suvatdragM;
        float crossectionalArea = Menu.suvatdragA;
        float p = 1.293f;
        float earthRadius = 6371000f;
        float earthMass = 5.972e24f;
        float bigG = 6.67e-11f;
        float userG = Menu.userG;
        //slider for dT / "accuracy of simulation"
        float dT = Menu.dT;

        float magnitudeOfFDTG = userG * -(bigG * earthMass * mass)/(earthRadius * earthRadius);
        float magnitudeOfFDTD = ((-p * crossectionalArea)/4) * currentSpeed * currentSpeed;

        //Vectors
        List<Vector3> trail = new List<Vector3>();
        Vector currentVelocity = new Vector(userDeg, currentSpeed, 0, true);
        Vector forceDueToGravity = new Vector(0, magnitudeOfFDTG, 0);
        Vector forceDueToDrag = currentVelocity.unitVector().Scale(magnitudeOfFDTD);
        Vector totalForce = forceDueToDrag.addVector(forceDueToGravity);
        Vector currentPosition = new Vector(0, 0, 0);
        Vector currentAcceleration;

        //Models
        Model sphere = LoadModelFromMesh(GenMeshSphere(1.0f, 25, 25));
        Model plane = LoadModelFromMesh(GenMeshPlane(50,50,20,20));

        //Shaders
        Shader shader = LoadShader("lighting.vs", "lighting.fs");

        //fps setting
        SetTargetFPS(60);
        
        //--------------------------------------------RENDERING----------------------------------------------
        
        while (!Raylib.WindowShouldClose())
        {
            //Camera
            Camera3D camera = new();
            
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 60.0f;
            camera.Projection = CameraProjection.Perspective;

            currentSpeed = currentVelocity.Magnitude();
            magnitudeOfFDTG = userG * -(bigG * earthMass * mass)/(earthRadius * earthRadius);
            magnitudeOfFDTD = ((-p * crossectionalArea)/4) * currentSpeed * currentSpeed;

            forceDueToGravity = new Vector(0, magnitudeOfFDTG, 0);
            forceDueToDrag = currentVelocity.unitVector().Scale(magnitudeOfFDTD);
            totalForce = forceDueToDrag.addVector(forceDueToGravity);

            currentAcceleration = totalForce.Scale(1/mass);
            // v = u + at
            currentVelocity = currentVelocity.addVector(currentAcceleration.Scale(dT));
            // s = vt - 1/2at^2
            currentPosition = currentPosition.addVector(currentVelocity.Scale(dT).subVector(currentAcceleration.Scale(0.5f * dT * dT)));
            
            //Collision
            if(currentPosition.hasCollidedWithGround())
            {
                dT = 0;
            }

            minCamX = MinValue(currentPosition.getX(), minCamX);
            minCamY = MinValue(currentPosition.getY(), minCamY);
            maxCamX = MaxValue(currentPosition.getX(), maxCamX);
            maxCamY = MaxValue(currentPosition.getY(), maxCamY);

            float dY = maxCamY - minCamY;
            float dX = maxCamX - minCamX;

            float maxCamDistance = MaxValue(dY, dX);
            float maxCamZDistance = maxCamDistance / (2 * (float)Math.Tan(((camera.FovY/180)*Math.PI)/2));
            float averageCamYDistance = (maxCamY + minCamY)/2;
            float averageCamXDistance = (maxCamX + minCamX)/2;
            
            //Vectors
            Vector camPos = new Vector(averageCamXDistance, averageCamYDistance, maxCamZDistance * (float)1.1);
            Vector targetPos = new Vector(averageCamXDistance, averageCamYDistance, 0);
            Vector3 StartPos = new Vector3(0, 0, 0);

            camera.Position = camPos.getVector3();
            camera.Target = targetPos.getVector3();

            //hide cursor
            //DisableCursor();

            //---------------------------------------------DRAWING-----------------------------------------------

            Raylib.BeginDrawing();
            Raylib.ClearBackground(White);
            drawBackground();
            Raylib.BeginMode3D(camera);

                //Grid
                //DrawGrid(200, 1);

                //3D Objects
                DrawModel(sphere, currentPosition.getVector3(), 0.5f, Color.Blue);
                DrawModel(plane, new Vector(0, 0, 0, false).getVector3(), 2f, Color.Black);

            //Lines
            drawTrails(StartPos, currentPosition, trail);

            Raylib.EndMode3D();

            //Raylib.DrawText("Fahoom Projectile Motion Renderer", 20,  10, 20, Color.Black);
            Raylib.DrawText("Current Speed: " + currentSpeed, 20, 10, 20, Color.Black);
            
            DrawFPS(510, 10);
        
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    public static void missile2()
    {

        initWindow();
        
        //vars
        float screeny = GetScreenHeight() / 2;
        float screenx = GetScreenWidth() / 2;

        float minCamX = float.PositiveInfinity;
        float maxCamX = float.NegativeInfinity;
        float minCamY = float.PositiveInfinity;
        float maxCamY = float.NegativeInfinity;
        
        float currentSpeed = Menu.suvatdragcwS;
        float userDeg = Menu.suvatdragcwDeg;
        float initialAngleRad = userDeg * 1/180 * (float)Math.PI;
        float mass = Menu.suvatdragcwM;
        float crossectionalArea = Menu.suvatdragcwA;
        float p = 1.293f;
        float earthRadius = 6371000f;
        float earthMass = 5.972e24f;
        float bigG = 6.67e-11f;
        float userG = Menu.userG;
        //slider for dT / "accuracy of simulation"
        float dT = Menu.dT;

        float crosswindMagnitude = Menu.suvatdragcwCW;
        float crosswindThetad = Menu.suvatdragcwDegHor;
        float crosswindThetar = crosswindThetad * 1/180 * (float)Math.PI;
        float crosswindPhid = Menu.suvatdragcwDegVer;
        float crosswindPhir = crosswindPhid * 1/180 * (float)Math.PI;

        float magnitudeOfFDTG = userG * -(bigG * earthMass * mass)/(earthRadius * earthRadius);
        float magnitudeOfFDTD = ((-p * crossectionalArea)/4) * currentSpeed * currentSpeed;

        //Vectors
        List<Vector3> trail = new List<Vector3>();
        Vector currentVelocity = new Vector(userDeg, currentSpeed, 0, true);
        Vector forceDueToGravity = new Vector(0, magnitudeOfFDTG, 0);
        Vector forceDueToDrag = currentVelocity.unitVector().Scale(magnitudeOfFDTD);
        Vector crosswind = new Vector(crosswindThetar, crosswindMagnitude, crosswindPhir);
        Vector totalForce = forceDueToDrag.addVector(forceDueToGravity).addVector(crosswind);
        Vector currentPosition = new Vector(0, 0, 0);
        Vector currentAcceleration;

        //Models
        Model sphere = LoadModelFromMesh(GenMeshSphere(1.0f, 25, 25));
        Model plane = LoadModelFromMesh(GenMeshPlane(50,50,20,20));

        //Shaders
        Shader shader = LoadShader("lighting.vs", "lighting.fs");

        //fps setting
        SetTargetFPS(60);
        
        //--------------------------------------------RENDERING----------------------------------------------
        
        while (!Raylib.WindowShouldClose())
        {
            //Camera
            Camera3D camera = new();
            
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 60.0f;
            camera.Projection = CameraProjection.Perspective;

            currentSpeed = currentVelocity.Magnitude();
            magnitudeOfFDTG = userG * -(bigG * earthMass * mass)/(earthRadius * earthRadius);
            magnitudeOfFDTD = ((-p * crossectionalArea)/4) * currentSpeed * currentSpeed;

            forceDueToGravity = new Vector(0, magnitudeOfFDTG, 0);
            forceDueToDrag = currentVelocity.unitVector().Scale(magnitudeOfFDTD);
            totalForce = forceDueToDrag.addVector(forceDueToGravity).addVector(crosswind);

            currentAcceleration = totalForce.Scale(1/mass);
            // v = u + at
            currentVelocity = currentVelocity.addVector(currentAcceleration.Scale(dT));
            // s = vt - 1/2at^2
            currentPosition = currentPosition.addVector(currentVelocity.Scale(dT).subVector(currentAcceleration.Scale(0.5f * dT * dT)));
            
            //Collision
            if(currentPosition.hasCollidedWithGround())
            {
                dT = 0;
            }

            minCamX = MinValue(currentPosition.getX(), minCamX);
            minCamY = MinValue(currentPosition.getY(), minCamY);
            maxCamX = MaxValue(currentPosition.getX(), maxCamX);
            maxCamY = MaxValue(currentPosition.getY(), maxCamY);

            float dY = maxCamY - minCamY;
            float dX = maxCamX - minCamX;

            float maxCamDistance = MaxValue(dY, dX);
            float maxCamZDistance = maxCamDistance / (2 * (float)Math.Tan(((camera.FovY/180)*Math.PI)/2));
            float averageCamYDistance = (maxCamY + minCamY)/2;
            float averageCamXDistance = (maxCamX + minCamX)/2;

            //Vectors
            Vector camPos = new Vector(averageCamXDistance, averageCamYDistance, maxCamZDistance * (float)1.1);
            Vector targetPos = new Vector(averageCamXDistance, averageCamYDistance, 0);
            Vector3 StartPos = new Vector3(0, 0, 0);

            camera.Position = camPos.getVector3();
            camera.Target = targetPos.getVector3();

            //hide cursor
            //DisableCursor();

            //---------------------------------------------DRAWING-----------------------------------------------

            Raylib.BeginDrawing();
            Raylib.ClearBackground(White);
            drawBackground();
            Raylib.BeginMode3D(camera);

                //Grid
                DrawGrid(200, 1);

                //3D Objects
                DrawModel(sphere, currentPosition.getVector3(), 0.5f, Color.Blue);
                DrawModel(plane, new Vector(0, 0, 0, false).getVector3(), 2f, Color.Black);

            //Lines
            drawTrails(StartPos, currentPosition, trail);

            Raylib.EndMode3D();

            //Raylib.DrawText("Fahoom Projectile Motion Renderer", 20,  10, 20, Color.Black);
            Raylib.DrawText("Current Speed: " + currentSpeed, 20, 10, 20, Color.Black);
            
            DrawFPS(510, 10);
        
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    public static void missile3()
    {

        initWindow();
        
        //vars
        float screeny = GetScreenHeight() / 2;
        float screenx = GetScreenWidth() / 2;

        float minCamX = float.PositiveInfinity;
        float maxCamX = float.NegativeInfinity;
        float minCamY = float.PositiveInfinity;
        float maxCamY = float.NegativeInfinity;
        
        float currentSpeed = Menu.missileS;
        float thrustpkg = Menu.missileT;
        float userDeg = Menu.missileDeg;
        float initialAngleRad = userDeg * 1/180 * (float)Math.PI;
        float rocketmass = Menu.missileRM;
        float fuelmass = Menu.missileFM;
        float totalmass = rocketmass + fuelmass;
        float rateOfFuelDecrease = Menu.missileFB;
        float crossectionalArea = Menu.missileA;
        float p = 1.293f;
        float earthRadius = 6371000f;
        float earthMass = 5.972e24f;
        float bigG = 6.67e-11f;
        float userG = Menu.userG;
        //slider for dT / "accuracy of simulation"
        float dT = Menu.dT;

        float magnitudeOfFDTG = userG * -(bigG * earthMass * totalmass)/(earthRadius * earthRadius);
        float magnitudeOfFDTD = ((-p * crossectionalArea)/4) * currentSpeed * currentSpeed;
        float magnitudeOfFDTT = thrustpkg * totalmass;

        //Vectors
        List<Vector3> trail = new List<Vector3>();
        Vector currentVelocity = new Vector(initialAngleRad, currentSpeed, 0, true);
        Vector forceDueToGravity = new Vector(0, magnitudeOfFDTG, 0);
        Vector forceDueToDrag = currentVelocity.unitVector().Scale(magnitudeOfFDTD);
        Vector forceDueToThrust = new Vector(initialAngleRad, magnitudeOfFDTT, 0, true);
        Vector totalForce = forceDueToDrag.addVector(forceDueToGravity);
        Vector currentPosition = new Vector(0, 0, 0);
        Vector currentAcceleration;

        //Models
        Model sphere = LoadModelFromMesh(GenMeshSphere(1.0f, 25, 25));
        Model plane = LoadModelFromMesh(GenMeshPlane(50,50,20,20));

        //Shaders
        Shader shader = LoadShader("lighting.vs", "lighting.fs");

        //fps setting
        SetTargetFPS(60);
        
        //--------------------------------------------RENDERING----------------------------------------------
        
        while (!Raylib.WindowShouldClose())
        {
            //Camera
            Camera3D camera = new();
            
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 60.0f;
            camera.Projection = CameraProjection.Perspective;

            totalmass = rocketmass + fuelmass;
            currentSpeed = currentVelocity.Magnitude();
            magnitudeOfFDTG = userG * -(bigG * earthMass * totalmass)/(earthRadius * earthRadius);
            magnitudeOfFDTD = ((-p * crossectionalArea)/4) * currentSpeed * currentSpeed;
            if(fuelmass == 0)
            {
                magnitudeOfFDTT = 0;
            }

            forceDueToGravity = new Vector(0, magnitudeOfFDTG, 0);
            forceDueToDrag = currentVelocity.unitVector().Scale(magnitudeOfFDTD);
            forceDueToThrust = currentVelocity.unitVector().Scale(magnitudeOfFDTT);
            totalForce = forceDueToThrust.addVector(forceDueToGravity.addVector(forceDueToDrag));

            currentAcceleration = totalForce.Scale(1/totalmass);
            // v = u + at
            currentVelocity = currentVelocity.addVector(currentAcceleration.Scale(dT));
            // s = vt - 1/2at^2
            currentPosition = currentPosition.addVector(currentVelocity.Scale(dT).subVector(currentAcceleration.Scale(0.5f * dT * dT)));
            
            // equates fuelmass to either the current fuelmass or 0 once all fuel has depleted
            fuelmass = MaxValue(fuelmass - rateOfFuelDecrease, 0);

            //Collision
            if(currentPosition.hasCollidedWithGround())
            {
                dT = 0;
            }

            minCamX = MinValue(currentPosition.getX(), minCamX);
            minCamY = MinValue(currentPosition.getY(), minCamY);
            maxCamX = MaxValue(currentPosition.getX(), maxCamX);
            maxCamY = MaxValue(currentPosition.getY(), maxCamY);

            float dY = maxCamY - minCamY;
            float dX = maxCamX - minCamX;

            float maxCamDistance = MaxValue(dY, dX);
            float maxCamZDistance = maxCamDistance / (2 * (float)Math.Tan(((camera.FovY/180)*Math.PI)/2));
            float averageCamYDistance = (maxCamY + minCamY)/2;
            float averageCamXDistance = (maxCamX + minCamX)/2;

            //Vectors
            // render cuts out after moving 950 units away
            Vector camPos;
            Vector targetPos;
            Console.WriteLine(maxCamZDistance);

            if(maxCamZDistance < 950)
            {
                camPos = new Vector(averageCamXDistance, averageCamYDistance, maxCamZDistance * (float)1.1);
                targetPos = new Vector(averageCamXDistance, averageCamYDistance, 0);
            }
            else
            {        
                camPos = new Vector(currentPosition.getX(), currentPosition.getY(), 5f);
                targetPos = new Vector(currentPosition.getX(), currentPosition.getY(), currentPosition.getZ());
            }

            
            Vector3 StartPos = new Vector3(0, 0, 0);

            camera.Position = camPos.getVector3();
            camera.Target = targetPos.getVector3();

            //hide cursor
            //DisableCursor();

            //---------------------------------------------DRAWING-----------------------------------------------

            Raylib.BeginDrawing();
            Raylib.ClearBackground(White);
            drawBackground();
            Raylib.BeginMode3D(camera);

                //Grid
                DrawGrid(200, 1);

                //3D Objects
                DrawModel(sphere, currentPosition.getVector3(), 0.5f, Color.Blue);
                DrawModel(plane, new Vector(0, 0, 0, false).getVector3(), 2f, Color.Black);

            //Lines
            drawTrails(StartPos, currentPosition, trail);

            Raylib.EndMode3D();

            //Raylib.DrawText("Fahoom Projectile Motion Renderer", 20,  10, 20, Color.Black);
            Raylib.DrawText("Current Speed: " + currentSpeed, 20, 10, 20, Color.Black);
            
            DrawFPS(510, 10);
        
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}