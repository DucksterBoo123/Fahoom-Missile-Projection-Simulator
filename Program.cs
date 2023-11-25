using ImGuiNET;
using ClickableTransparentOverlay;
using System.Numerics;
using SixLabors.ImageSharp.ColorSpaces;
using System.Runtime.InteropServices;
using ClickableTransparentOverlay.Win32;
using SharpDX.DXGI;
using System.Text.Json;

namespace IMGUITEST
{
    public class Program : Overlay
    {
        // hides console 
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // variables
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
        public float[]? suvats {  get; set; }
        public float[]? drag { get; set; }

        // update function
        protected override void Render()
        {
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
                        about = true;
                        settings = false;
                        home = false;
                        simple = false;
                        advanced = false;
                        fahoom = false;
                    }
                    if (ImGui.MenuItem("Settings"))
                    {
                        about = false;
                        settings = true;
                        home = false;
                        simple = false;
                        advanced = false;
                        fahoom = false;
                    }
                    if (FM == true)
                    {
                        if (ImGui.MenuItem("Fahoom Mode"))
                        {
                            about = false;
                            home = false;
                            simple = false;
                            advanced = false;
                            settings = false;
                            fahoom = true;
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
                        about = false;
                        home = false;
                        simple = true;
                        advanced = false;
                        settings = false;
                        fahoom = false;
                    }
                    if (ImGui.MenuItem("Advanced"))
                    {
                        about = false;
                        home = false;
                        simple = false;
                        advanced = true;
                        settings = false;
                        fahoom = false;
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
                    about = false;
                    home = false;
                    simple = false;
                    advanced = false;
                    settings = false;
                    fahoom = true;
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
                ImGui.Text("Initial Angle - " + suvatAInput.ToString());
                ImGui.Text("Mass - " + dragMInput.ToString());
                ImGui.Text("Diameter - " + dragDInput.ToString());

                drag = new[] { dragUInput, dragAInput, dragMInput, dragDInput};

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

                ImGui.BeginChild("1", new System.Numerics.Vector2(windowSize.X - 250, 0));
                // options
                ImGui.SeparatorText("Options");
                ImGui.Checkbox("High Contrast Mode", ref HCM);
                ImGui.Checkbox("Fahoom Mode", ref FM);
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

        public static void Main(string[] args)
        {
            // console text and program execution and hiding console
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, hideConsole);
            Console.WriteLine("Starting GUI...");
            Program program = new Program();
            program.Start().Wait();
        }
    }
}