using System.Data;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace MrCrossDrivesAnSTypeJaguar;

class Vector
{
    private float x;
    private float y;
    private float z;

    // input1,2,3 may be cartesion or polar (in the form of theta, magnitude and phi)
    public Vector(float input1, float input2, float input3, bool isPolar = false)
    {
        if(isPolar == true)
        {
            x = input2 * (float)Math.Cos((double)input1) * (float)Math.Cos((double)input3);
            y = input2 * (float)Math.Sin((double)input1);
            z = input2 * (float)Math.Cos((double)input1) * (float)Math.Sin((double)input3);
        }
        else
        {
            x = input1;
            y = input2;
            z = input3;
        }
    }

    public float getX()
    {
        return x;
    }

    public void setX(float inputX)
    {
        x = inputX;
    }

    public float getY()
    {
        return y;
    }

    public void setY(float inputY)
    {
        y = inputY;
    }

    public float getZ()
    {
        return z;
    }

    public void setZ(float inputZ)
    {
        z = inputZ;
    }

    public float Magnitude()
    {
        return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
    }

    public Vector Scale(float a)
    {
        return new Vector(x*a, y*a, z*a);
    }

    public Vector unitVector1()
    {
        if(Magnitude() == 0)
        {
            return new Vector(0, 0, 0);
        }
        else
        {
            return new Vector(x, y, z).Scale(1/Magnitude());
        }
    }

    public Vector unitVector()
    { 
        return new Vector(x, y, z).Scale(1 / Magnitude());
    }

    public Vector addVector(Vector b)
    {
        return new Vector(x + b.getX(), y + b.getY(), z + b.getZ());
    }

    public Vector subVector(Vector b)
    {
        return addVector(b.Scale(-1));
    }

    public Vector getPolarCoords()
    {
        return new Vector((float)Math.Atan(z/x), Magnitude(), (float)Math.Atan(y/x));
        //theta , r , phi
        //ground angle, magnitude, up angle
    }

    public string debugLogVector()
    {
        return x.ToString() + " " + y.ToString() + " " + z.ToString(); 
    }

    public Vector3 getVector3()
    {
        return new Vector3(x, y, z);
    }

    public bool hasCollidedWithGround()
    {
        if (y <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}