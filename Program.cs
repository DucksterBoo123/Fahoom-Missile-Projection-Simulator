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

        string hello = "hello";

        float textR = 1.0f;
        float textG = 1.0f;
        float textB = 1.0f;
        float textA = 1.0f;
        float buttonR = 1.0f;
        float buttonG = 1.0f;
        float buttonB = 1.0f;
        float buttonA = 0.5f;
        float suvatSInput;
        float suvatUInput;
        float suvatVInput;
        float suvatAInput;
        float suvatTInput;

        bool test = true;
        bool simple = false;
        bool advanced = false;
        bool settings = false;
        bool fahoom = false;
        bool about = false;

        bool HCM = false;
        bool FM = false;

        int sl;
        int sl_min = 0;
        int sl_max = 10;
        int windowxsize = 500;
        int windowysize = 250;

        // array setup
        public float[]? suvats {  get; set; }

        // update function
        protected override void Render()
        {
            // styling
            ImGuiStylePtr style = ImGui.GetStyle();
            style.WindowBorderSize = 2.0f;
            style.Colors[(int)ImGuiCol.Border] = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(1.0f, 1.0f, 1.0f, 0.45f);
            style.Colors[(int)ImGuiCol.Button] = new Vector4(buttonR, buttonG, buttonB, buttonA);
            style.Colors[(int)ImGuiCol.Text] = new Vector4(textR, textG, textB, textA);
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
                        test = false;
                        simple = false;
                        advanced = false;
                        fahoom = false;
                    }
                    if (ImGui.MenuItem("Settings"))
                    {
                        about = false;
                        settings = true;
                        test = false;
                        simple = false;
                        advanced = false;
                        fahoom = false;
                    }
                    if (FM == true)
                    {
                        if (ImGui.MenuItem("Fahoom Mode"))
                        {
                            about = false;
                            test = false;
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
                    if (ImGui.MenuItem("Test"))
                    {
                        about = false;
                        test = true;
                        simple = false;
                        advanced = false;
                        settings = false;
                        fahoom = false;
                    }
                    if (ImGui.MenuItem("Simple"))
                    {
                        about = false;
                        test = false;
                        simple = true;
                        advanced = false;
                        settings = false;
                        fahoom = false;
                    }
                    if (ImGui.MenuItem("Advanced"))
                    {
                        about = false;
                        test = false;
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
            if (test == true)
            {
                ImGui.Text(hello);
                if (ImGui.Button("Simple", new System.Numerics.Vector2(100, 50)))
                {
                    hello = "Simple";
                }
                if (ImGui.Button("Advanced", new System.Numerics.Vector2(100, 50)))
                {
                    hello = "Advanced";
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
                    string jsonString = JsonSerializer.Serialize(windowSize.Y);
                    File.WriteAllText(fileName, jsonString);

                    // launch next application
                }

                ImGui.EndChild();
            }
            // advanced page
            if (advanced == true)
            {
                ImGui.SliderInt("slider", ref sl, sl_min, sl_max);
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
                ImGui.Checkbox("High Contrast Mode", ref HCM);
                ImGui.Checkbox("Fahoom Mode", ref FM);
                ImGui.SliderInt("WindowX", ref windowxsize, 500, 1000);
                ImGui.SliderInt("WindowY", ref windowysize, 150, 1000);
                if (ImGui.Button("Reset Window Size"))
                {
                    windowxsize = 500;
                    windowysize = 250;
                }
            }
            if (HCM == true)
            {
                if (textG == 1.0f)
                {
                    textG = 0.0f;
                    textB = 0.0f;

                    buttonB = 0.0f;
                    buttonA = 1.0f;
                }
            }
            if (HCM == false)
            {
                if (textG == 0.0f)
                {
                    textG = 1.0f;
                    textB = 1.0f;

                    buttonB = 1.0f;
                    buttonA = 0.5f;
                }
            }
            // FAHOOM PAGE
            if (fahoom == true) 
            {
                ImGui.Text("fahooooom");
                ImGui.Image(64, new System.Numerics.Vector2(300, 100));
            }
            ImGui.End();
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