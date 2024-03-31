using rlImGui_cs;
using System.Numerics;
namespace MrCrossDrivesAnSTypeJaguar;

class RumbleAndCo
{
    const int GLSL_VERSION = 330;

    public static void Main()
    {
        //Sims.suvat(); //suvat
        //Sims.missile(); //drag
        //Sims.missile2(); //drag + crosswind 
        //Sims.missile3(); //missile

        Menu.menu();
    }
}