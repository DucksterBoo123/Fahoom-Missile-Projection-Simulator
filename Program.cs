using ImGuiNET;
using ClickableTransparentOverlay;
using System.Numerics;
using SixLabors.ImageSharp.ColorSpaces;
using System.Runtime.InteropServices;
using ClickableTransparentOverlay.Win32;
using SharpDX.DXGI;

namespace IMGUITEST
{
    public class Program : Overlay
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

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

        bool simple = true;
        bool advanced = false;
        bool settings = false;
        bool fahoom = false;

        bool HCM = false;
        bool FM = false;

        int sl;
        int sl_min = 0;
        int sl_max = 10;

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
            ImGui.Begin("Fahoom Projectile Motion Simulator", ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking);

            // menu bar
            if (ImGui.BeginMenuBar())
            {
                // app settings
                if (ImGui.BeginMenu("FPMS"))
                {
                    if (ImGui.MenuItem("Settings"))
                    {
                        settings = true;
                        simple = false;
                        advanced = false;
                        fahoom = false;
                    }
                    if (FM == true)
                    {
                        if (ImGui.MenuItem("Fahoom Mode"))
                        {
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
                        simple = true;
                        advanced = false;
                        settings = false;
                        fahoom = false;
                    }
                    if (ImGui.MenuItem("Advanced"))
                    {
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
            if (simple == true)
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
            if (advanced == true)
            {
                ImGui.SliderInt("slider", ref sl, sl_min, sl_max);
            }
            if (settings == true) 
            {
                ImGui.Checkbox("High Contrast Mode", ref HCM);
                if (HCM == true)
                {
                    if (textG == 1.0f)
                    {
                        textG = 0.0f;
                        textB = 0.0f;

                        buttonB = 0.0f;
                        buttonA = 1.0f;
                    }
                    else if (textG == 0.0f)
                    {
                        textG = 1.0f;
                        textB = 1.0f;

                        buttonB = 1.0f;
                        buttonA = 0.5f;
                    }
                }
                ImGui.Checkbox("Fahoom Mode", ref FM);
            }
            if (fahoom == true) 
            {
                ImGui.Text("fahooooom");
                ImGui.Image(64, new System.Numerics.Vector2(2500, 2500));
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