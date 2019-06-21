using System;
//using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
//using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using openview.OpenGL;
using System.Windows.Media.Imaging;
using System.Windows.Media;
//using System.Text;
//using System.Threading;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Collections.Generic;
//using static openview.View3D;
//using Microsoft.VisualBasic;
//using openview.GUIControls;
//using WMPLib;
//using OpenGL;
//using System.Windows;
//using System.Linq;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Xml;
namespace openview
{
    public static class DateAndTime
    {
        public const int HourMinute = 60, MinuteSec = 60, HourSec = HourMinute * MinuteSec;
        public static float Timer => DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * HourSec + DateTime.Now.Millisecond * 0.001f;
        public static int SecondOfDay => DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
        public static int Milisecs => (DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600) * 1000 + DateTime.Now.Millisecond;
    }
    public struct Rectangle
    {
        public int x, y, Width, Height;
        public int Right => x + Width;
        public int Bottom => y + Height;
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            Width = width;
            Height = height;
        }
    }
    public struct Size
    {
        public int Width, Height;
        public int Area => Width * Height;
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
    public struct SizeF
    {
        public float Width, Height;
        public float Area => Width * Height;
        public SizeF(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
    public struct RectangleF
    {
        public float x, y, Width, Height;

        public RectangleF(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            Width = width;
            Height = height;
        }

        public float Right => x + Width;
        public float Bottom => y + Height;
    }
    public struct Polygon3D
    {
        public Vector3[] Points;
        public Brush Fill;
    }
    public struct Polygon2D
    {
        public Vector2[] Points;
        public Brush Fill;
        public Polygon2D(int length, Brush fill)
        {
            Points = new Vector2[length];
            Fill = fill;
        }
    }
    public struct Point3D
    {
        public int x, y, z;

        public Point3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point3D))
            {
                return false;
            }
            var d = (Point3D)obj;
            return x == d.x &&
                   y == d.y &&
                   z == d.z;
        }
        public override int GetHashCode()
        {
            var hashCode = 373119288;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + z.GetHashCode();
            return hashCode;
        }
        public bool Equals(Point3D other) => x == other.x && y == other.y && z == other.z;
    }
    public static class Generic
    {
        public struct NormalList<T> : IEnumerable<T>
        {
            public override string ToString()
            {
                return "Count = " + Items.Length;
            }
            public void Add(T item)
            {
                T[] newl = new T[Items.Length + 1];
                for (int i = 0; i < Items.Length; i++)
                {
                    newl[i] = Items[i];
                }
                newl[Items.Length] = item;
                Items = newl;
            }
            public void AddRange(params T[] items)
            {
                T[] oldl = Items;
                int i = 0, oldcnt = oldl.Length, newcnt = oldcnt + items.Length;
                T[] newl = new T[newcnt];
                for (; i < oldcnt; i++)
                {
                    newl[i] = oldl[i];
                }
                for (; i < newl.Length; i++)
                {
                    newl[i] = items[i - oldcnt];
                }
                Items = newl;
            }
            public T this[int index] { get => Items[index]; set => Items[index] = value; }
            public int Count => Items.Length;
            public T[] Items;

            public NormalList(params T[] Items) => this.Items = Items;
            public NormalList(int Count) => Items = new T[Count];

            public bool Contains(T item)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (item.Equals(Items[i])) return true;
                }
                return false;
            }
            public int IndexOf(T item)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (item.Equals(Items[i])) return i;
                }
                return -1;
            }
            public void Remove(T item)
            {
                int index = IndexOf(item);
                if (index > -1) RemoveAt(index);
            }
            public void RemoveAt(int index)
            {
                T[] NewList = new T[Items.Length - 1];
                int i;
                for (i = 0; i < index; i++)
                {
                    NewList[i] = Items[i];
                }
                for (i++; i < Items.Length; i++)
                {
                    NewList[i - 1] = Items[i];
                }
                Items = NewList;
            }
            public void Shuffle(int seed)
            {
                Random r = new Random(seed);
                //List


            }
            public void Sort()
            {


            }
            static void Sort(int L, int R, byte[] bytes, int[] tris, float[] Distances)
            {//sort from high to low(0:high,max-1:low)
                int oldl = L, oldr = R;
                float mid = Distances[((L + R) / 2)];
                do
                {
                    while (Distances[L] < mid)
                    {
                        L++;
                    }
                    while (mid < Distances[R])
                    {
                        R--;
                    }

                    if (L <= R)
                    {
                        if (L < R)
                        {
                            int index1a = L * 3,
                                index1b = index1a + 1,
                                index1c = index1b + 1,
                                index2a = R * 3,
                                index2b = index2a + 1,
                                index2c = index2b + 1;
                            //swap
                            int t0 = tris[index1a], t1 = tris[index1b], t2 = tris[index1c];
                            tris[index1a] = tris[index2a];
                            tris[index1b] = tris[index2b];
                            tris[index1c] = tris[index2c];
                            tris[index2a] = t0;
                            tris[index2b] = t1;
                            tris[index2c] = t2;
                            byte b0 = bytes[index1a], b1 = bytes[index1b], b2 = bytes[index1c];
                            bytes[index1a] = bytes[index2a];
                            bytes[index1b] = bytes[index2b];
                            bytes[index1c] = bytes[index2c];
                            bytes[index2a] = b0;
                            bytes[index2b] = b1;
                            bytes[index2c] = b2;
                            float d = Distances[R];
                            Distances[R] = Distances[L];
                            Distances[L] = d;
                        }
                        L++;
                        R--;
                    }
                } while (L < R);

                if (oldl < R)
                {
                    Sort(oldl, R, bytes, tris, Distances);
                }
                if (L < oldr)
                {
                    Sort(L, oldr, bytes, tris, Distances);
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator { index = 0, list = this };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator { index = 0, list = this };
            }
            public struct Enumerator : IEnumerator, IEnumerator<T>
            {
                public int index;
                public NormalList<T> list;
                public T Current => list[index];

                object IEnumerator.Current => throw new NotImplementedException();

                public void Dispose()
                {
                    //list
                }

                public bool MoveNext()
                {
                    index++;
                    return index < list.Items.Length;
                }

                public void Reset()
                {
                    index = 0;
                }
            }
        }
        public static T[] RemoveItem<T>(T[] list, int index)
        {
            T[] NewList = new T[list.Length - 1];
            BlockCopy(list, 0, NewList, 0, index);
            BlockCopy(list, index + 1, NewList, index, list.Length - index - 1);
            return NewList;
        }
        public static void BlockCopy<T>(T[] src, int srcoff, T[] dest, int destoff, int count)
        {
            for (int i = 0; i < count; i++)
            {
                dest[i + destoff] = src[i + srcoff];
            }
        }
        public static T GetRandomItem<T>(Random random, params T[] Args) => Args[random.Next(Args.Length)];
        public static T[] Combine<T>(params T[] Args) => Args;
        public static List<T> CombineToList<T>(params T[] Args) => new List<T>(Args);
        public static T[] To1DArray<T>(T fill, int l)
        {
            T[] matrix = new T[l];
            for (int i = 0; i < l; i++)
            {
                matrix[i] = fill;
            }
            return matrix;
        }
        public static T[,,] To3DArray<T>(T Fill, int w, int l, int h)
        {
            T[,,] matrix = new T[w, l, h];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    for (int k = 0; k < h; k++)
                    {
                        matrix[i, j, k] = Fill;
                    }
                }
            }
            return matrix;
        }
        public static T[,] Get2DArray<T>(Rectangle r, T[,] array)
        {
            T[,] matrix = new T[r.Width, r.Height];

            for (int i = 0; i < r.Width; i++)
            {
                for (int j = 0; j < r.Height; j++)
                {
                    matrix[i, j] = array[i + r.x, j + r.y];
                }
            }
            return matrix;
        }
    }
    /// <summary>
    /// Stores an color with ARGB from 0 to 1.
    /// </summary>
    public struct ColorF
    {
        public const float Divider = 255, Multiplier = 1 / 255f;
        public float A, R, G, B;
        public static ColorF Paint(ColorF FirstColor, ColorF SecondColor)
        {
            return new ColorF
            {
                R = Mathf.Lerp(FirstColor.R, SecondColor.R, SecondColor.A),
                G = Mathf.Lerp(FirstColor.G, SecondColor.G, SecondColor.A),
                B = Mathf.Lerp(FirstColor.B, SecondColor.B, SecondColor.A),
                A = FirstColor.A + SecondColor.A * (1 - FirstColor.A)
            };
        }
        public static ColorF White = new ColorF { A = 1, R = 1, G = 1, B = 1 };
        public static ColorF Black = new ColorF { A = 1 };
        public static ColorF Blue = new ColorF { B = 1, A = 1 };
        public static ColorF Green = new ColorF { G = 1, A = 1 };
        public static ColorF Red = new ColorF { R = 1, A = 1 };
        public static ColorF Yellow = new ColorF { R = 1, G = 1, A = 1 };
        public static ColorF Purple = new ColorF { R = 1, B = 1, A = 1 };
        public static ColorF Transparent = new ColorF { };
        public ColorF(float a, float r, float g, float b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
        public ColorF(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
            A = 255;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return B;
                    case 1: return G;
                    case 2: return R;
                    default: return A;
                }
            }
            set
            {
                switch (index)
                {
                    case 0: B = value; return;
                    case 1: G = value; return;
                    case 2: R = value; return;
                    default: A = value; return;
                }
            }
        }
        public float[] ToFloat { get => new float[] { B, G, R, A }; set { B = value[0]; G = value[1]; R = value[2]; A = value[3]; } }
        public static implicit operator Color(ColorF c) => Color.FromArgb((byte)(c.A * 255), (byte)(c.R * 255), (byte)(c.G * 255), (byte)(c.B * 255));
        public static implicit operator ColorF(Color c) => new ColorF { A = c.A / 255f, R = c.R / 255f, G = c.G / 255f, B = c.B / 255f };
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct IntColor
    {
        public static IntColor Scale(IntColor i1, IntColor i2, int scale1, int scale2) => 
            new IntColor {
                A = (byte)((i1.A * scale1 + i2.A * scale2) / 255),
                R = (byte)((i1.R * scale1 + i2.R * scale2) / 255),
                G = (byte)((i1.G * scale1 + i2.G * scale2) / 255),
                B = (byte)((i1.B * scale1 + i2.B * scale2) / 255)
            };
        public byte this[int index]
        {
            set
            {
                switch (index)
                {
                    case 0:
                        B = value; return;
                    case 1:
                        G = value; return;
                    case 2:
                        R = value; return;
                    case 3:
                        A = value; return;
                }

            }
            get
            {
                switch (index)
                {
                    case 0:
                        return B;
                    case 1:
                        return G;
                    case 2:
                        return R;
                    case 3:
                        return A;
                }
                throw new IndexOutOfRangeException();
            }
        }
        [FieldOffset(0)]
        public byte B;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(2)]
        public byte R;
        [FieldOffset(3)]
        public byte A;
        [FieldOffset(0)]
        public int ToInt;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct NormalColor
    {
        public override string ToString()
        {
            return "A = " + A.ToString() + ", R = " + R.ToString() + ",G = " + G.ToString() + ",B = " + B.ToString();
        }
        public static bool operator ==(NormalColor left, NormalColor right) => left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B;
        public static bool operator !=(NormalColor left, NormalColor right) => left.A != right.A || left.R != right.R || left.G != right.G || left.B != right.B;
        const byte half = 128;
        public static NormalColor
            Black = new NormalColor(0, 0, 0),
            White = new NormalColor(255, 255, 255),
            Blue = new NormalColor(0, 0, 255),
            Green = new NormalColor(0, 255, 0),
            Red = new NormalColor(255, 0, 0),
            SandyBrown = new NormalColor(244, 164, 96),
            Grey = new NormalColor(half, half, half),
            DarkBlue = new NormalColor(0, 0, half),
            DarkGreen = new NormalColor(0, half, 0),
            Transparent = new NormalColor(),
            AliceBlue = new NormalColor(240, 248, 255);
        public static explicit operator NormalColor(Color c) => new NormalColor { A = c.A, B = c.B, R = c.R, G = c.G };
        public static implicit operator Color(NormalColor c) => Color.FromArgb(c.A, c.R, c.G, c.B);
        public static explicit operator ColorF(NormalColor c) => new ColorF { A = c.A * ColorF.Multiplier, B = c.B * ColorF.Multiplier, R = c.R * ColorF.Multiplier, G = c.G * ColorF.Multiplier };
        [FieldOffset(0)]
        public byte A;
        [FieldOffset(1)]
        public byte R;
        [FieldOffset(2)]
        public byte G;
        [FieldOffset(3)]
        public byte B;
        public NormalColor(byte[] bytes)
        {
            B = bytes[0];
            G = bytes[1];
            R = bytes[2];
            A = 255;
        }
        public NormalColor(byte r, byte g, byte b)
        {
            A = 255;
            R = r;
            G = g;
            B = b;
        }
        public NormalColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
        public NormalColor(NormalColor c, byte a)
        {
            A = a;
            R = c.R;
            G = c.G;
            B = c.B;
        }
        const int tm2 = 256 * 256;
        const int tm3 = 256 * 256 * 256;

        public int ToArgb() => BitConverter.ToInt32(Bytes, 0);
        public byte this[int index]
        {
            set
            {
                switch (index)
                {
                    case 0: B = value; return;
                    case 1: G = value; return;
                    case 2: R = value; return;
                    case 3: A = value; return;
                }
                throw new IndexOutOfRangeException();
            }
            get
            {
                switch (index)
                {
                    case 0: return B;
                    case 1: return G;
                    case 2: return R;
                    case 3: return A;
                }
                throw new IndexOutOfRangeException();
            }
        }
        public byte[] Bytes { get => new byte[] { B, G, R, A }; set { B = value[0]; G = value[1]; R = value[2]; A = value[3]; } }
        public static NormalColor FromArgb(int clr) => new NormalColor { Bytes = BitConverter.GetBytes(clr) };

        public override bool Equals(object obj)
        {
            if (!(obj is NormalColor))
            {
                return false;
            }

            var color = (NormalColor)obj;
            return A == color.A &&
                   R == color.R &&
                   G == color.G &&
                   B == color.B;
        }

        public override int GetHashCode()
        {
            var hashCode = -1749689076;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }
    }
    public static class Mathf
    {
        //https://blogs.msdn.microsoft.com/cjacks/2006/04/12/converting-from-hsb-to-rgb-in-net/
        //https://codepen.io/HunorMarton/details/eWvewo
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="hue">ranges from 0 to 360</param>
        /// <param name="saturation">ranges from 0 to 1</param>
        /// <param name="brightness">ranges from 0 to 1</param>
        /// <returns></returns>
        public static Color ColorFromAhsb(float a, float hue, float saturation, float brightness)
        {

            if (0f > hue || 360f < hue)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (0f > saturation || 1f < saturation)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (0f > brightness || 1f < brightness)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (0 == saturation)
            {
                return new Color { ScA = a, ScR = brightness, ScG = brightness, ScB = brightness };
            }

            float fMax, fMid, fMin;
            int iSextant;

            if (0.5 < brightness)
            {
                fMax = brightness - (brightness * saturation) + saturation;
                fMin = brightness + (brightness * saturation) - saturation;
            }
            else
            {
                fMax = brightness + (brightness * saturation);
                fMin = brightness - (brightness * saturation);
            }

            iSextant = (int)Math.Floor(hue / 60f);
            if (300f <= hue)
            {
                hue -= 360f;
            }
            hue /= 60f;
            hue -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = hue * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - hue * (fMax - fMin);
            }
            //byte iMax, iMid, iMin;
            //iMax = Convert.ToByte(fMax * 255);
            //iMid = Convert.ToByte(fMid * 255);
            //iMin = Convert.ToByte(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return new Color { ScA = a, ScR = fMid, ScG = fMax, ScB = fMin };
                //return Color.FromArgb(a, iMid, iMax, iMin);
                case 2:
                    return new Color { ScA = a, ScR = fMin, ScG = fMax, ScB = fMid };
                //return Color.FromArgb(a, iMin, iMax, iMid);
                case 3:
                    return new Color { ScA = a, ScR = fMin, ScG = fMid, ScB = fMax };
                //return Color.FromArgb(a, iMin, iMid, iMax);
                case 4:
                    return new Color { ScA = a, ScR = fMid, ScG = fMin, ScB = fMax };
                //return Color.FromArgb(a, iMid, iMin, iMax);
                case 5:
                    return new Color { ScA = a, ScR = fMax, ScG = fMin, ScB = fMid };
                //return Color.FromArgb(a, iMax, iMin, iMid);
                default:
                    return new Color { ScA = a, ScR = fMax, ScG = fMid, ScB = fMin };
                    //return Color.FromArgb(a, iMax, iMid, iMin);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float SinSpeed(float x)
        {
            const float B = 4 / PI;
            const float C = -4 / (PI * PI);
            if (x < -PI)
            {
                float n = (x + PI) % PI2 - PI;
            }
            x = (x < -PI) ? PI - (-x % PI2) : x > PI ? (x + PI) % PI2 - PI : x;//get pi in range[-pi,pi]
            return -(B * x + C * x * ((x < 0) ? -x : x));
        }
        public static int FloorToPower(int value, int basevalue)
        {
            int floored = 1;
            while (floored * basevalue < value)
            {
                floored *= basevalue;
            }
            return floored;
        }
        public static float FloorToPower(float value, float basevalue)
        {
            float floored = 1;
            while (floored * basevalue < value)
            {
                floored *= basevalue;
            }
            return floored;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">must be in range[-pi,pi]</param>
        /// <returns></returns>
        public static float SinSpeedRanged(float x)
        {
            const float B = 4 / PI;
            const float C = -4 / (PI * PI);
            return -(B * x + C * x * ((x < 0) ? -x : x));
        }
        public static string FirstUpper(string s) => (s.Length > 0) ? s[0].ToString().ToUpper() + s.Substring(1, s.Length - 1).ToLower() : "";
        public static float CalculateSquareSizeToFit(SizeF s, int count)
        {
            float xindex = 0;
            float yindex = 0;
            float sqsize = float.PositiveInfinity;
            while ((int)xindex * (int)yindex < count)
            {
                int nextx = (int)xindex + 1;
                int nexty = (int)yindex + 1;
                if (s.Width / nextx < s.Height / nexty)//y step
                {
                    sqsize = s.Height / nexty;
                    yindex = nexty;
                    xindex = s.Width / sqsize;
                }
                else
                {//x step
                    sqsize = s.Width / nextx;
                    xindex = nextx;
                    yindex = s.Height / sqsize;
                }
                //use last size, which was too big, to calculate an new size
                //sqsize = Math.Max(s.Width, s.Height)/sqsize;
            }
            return sqsize;
        }
        public const float FloatEpsilon = 0.0001f;
        public const float OneEpsilon = 1 - FloatEpsilon;
        public static bool Like(NormalColor c1, NormalColor c2, int MaxDiff) => Diff(c1, c2) <= MaxDiff;
        public static bool LikeAlpha(NormalColor c1, NormalColor c2, int MaxDiff) => DiffAlpha(c1, c2) <= MaxDiff;
        public static int Diff(NormalColor c1, NormalColor c2) => Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
        public static int DiffAlpha(NormalColor c1, NormalColor c2) => Math.Abs(c1.A - c2.A) + Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B);
        public static bool InArea(float Min, float Max, float value) => Min <= value && Max >= value;
        public static bool InArea(float xMin, float xMax, float yMin, float yMax, float x, float y) => x >= xMin && x <= xMax && y >= yMin && y <= yMax;
        public static int Fix(float inp) { return (inp - (int)inp == 0) ? (int)inp : Floor(inp) + 1; }
        public static int Floor(float inp) { return (inp > 0) ? (int)inp : ((int)inp == inp) ? (int)inp : (int)inp - 1; }
        public static void Scale(RectangleF r, params Vector2[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                p[i] = new Vector2(Lerp(r.x, r.Right, p[i].x), Lerp(r.y, r.Bottom, p[i].y));
            }
        }
        public static RectangleF Scale(RectangleF r, RectangleF toscale) => new RectangleF(Lerp(r.x, r.Right, toscale.x), Lerp(r.y, r.Bottom, toscale.y), toscale.Width * r.Width, toscale.Height * r.Height);
        public static void MirrorX(float x, params Vector2[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                p[i] = new Vector2(x - (p[i].x - x), p[i].y);
            }
        }
        public static int Sgn(float f)
        {
            return (f > 0) ? 1 : (f == 0) ? 0 : -1;
        }
        public static float Sum(params float[] f)
        {
            float total = 0;
            for (int i = 0; i < f.Length; i++)
            {
                total += f[i];
            }
            return total;
        }
        public static float Max(params float[] f)
        {
            if (f.Length == 0) throw new Exception("Input must always be at least one value");
            float max = f[0];
            for (int i = 1; i < f.Length; i++)
            {
                if (f[i] > max) max = f[i];
            }
            return max;
        }
        public static int FindMax(params float[] f)
        {
            if (f.Length == 0) return -1;
            float max = f[0];
            int m = 0;
            for (int i = 1; i < f.Length; i++)
            {
                if (f[i] > max)
                {
                    max = f[i];
                    m = i;
                }
            }
            return m;
        }
        public static float Min(params float[] f)
        {
            if (f.Length == 0) throw new Exception("Input must always be at least one value");
            float min = f[0];
            for (int i = 1; i < f.Length; i++)
            {
                if (f[i] < min) min = f[i];
            }
            return min;
        }
        public static float Getrotation(float fx, float fy)
        {
            float l = Mathf.Sqrt(fx * fx + fy * fy);
            fx /= l;
            fy /= l;
            float rotation = (float)Math.Asin(fx);
            if (fy < 0)
                rotation = PI - rotation;//rotation = PI - rotation;
            if (rotation < 0)
            { rotation = PI2 + rotation; }
            return rotation;
        }
        public static Vector3 Getrotation(float fx, float fy, float fz)
        {
            float l = Mathf.Sqrt(fx * fx + fy * fy);
            fx /= l;
            fy /= l;
            Vector3 rotation = new Vector3
            {
                x = (float)Math.Asin(fx)
            };
            if (fy < 0)
                rotation.x = PI - rotation.x;//rotation = PI - rotation;
            if (rotation.x < 0)
            { rotation.x = PI2 + rotation.x; }
            rotation.y = -(float)Math.Asin(fz);
            return rotation;
        }
        public static void Turn(float x, float y, float rotation, out float outx, out float outy)
        {
            //rotation *= (PI180);
            outx = (x * (float)Math.Cos(rotation)) - (y * (float)Math.Sin(rotation));
            outy = (x * (float)Math.Sin(rotation)) + (y * (float)Math.Cos(rotation));
        }
        public static Vector2 Turn(Vector2 p, float rotation) => new Vector2(
            (p.x * (float)Math.Cos(rotation)) - (p.y * (float)Math.Sin(rotation)),
            (p.x * (float)Math.Sin(rotation)) + (p.y * (float)Math.Cos(rotation))
        );//rotation *= (PI180);
        public static Vector3 Turn(float l, Vector3 rotation)
        {
            float rz = (float)Math.Cos(rotation.y) * l;
            return new Vector3
            {
                z = (float)Math.Sin(rotation.y) * l,
                y = (float)Math.Sin(rotation.x) * rz,
                x = (float)Math.Cos(rotation.x) * rz
            };
        }
        public static float Cos(float r) => (float)Math.Cos(r);
        //        public static float Sin(float r) => (float)Math.Sin(r);
        public static float Sin(float r) => (float)Math.Sin(r);
        /*            //calculate sinus with taylor series
                    float lastanswer, newanswer = r;
                    int times = 3;
                    int f = 6;//faculteit = 1 * 2 * 3
                    do
                    {
                        r *= r * -r;//power 2 lifted, maked negative
                        lastanswer = newanswer;
                        newanswer += r / f;
                        f *= ++times * ++times;//faculteit van times, times 2 lifted
                    }
                    while (lastanswer != newanswer);
                    return newanswer;
                }
          */
        public static float CosAngle(float r) => (float)Math.Cos(r) / PI180;
        public static float SinAngle(float r) => (float)Math.Sin(r) / PI180;
        public static float Pow(float x, float y) => (float)Math.Pow(x, y);
        public const float PI = (float)Math.PI, PI180 = PI / 180, PI2 = PI * 2, QuarterAngle = PI * .5f;

        public const float Rad2Deg = 180 / PI, Deg2Rad = PI / 180;
        public static float Pyt(float f1, float f2, float f3) => ((float)Math.Pow(f1 * f1 + f2 * f2 + f3 * f3, 0.5));
        public static float Mod(float d, float Divider) { return (d < 0) ? Divider - (-d % Divider) : d % Divider; }
        public static int Sign(float f) => (f > 0) ? 1 : (f < 0) ? -1 : 0;
        public static float Pow(float x, uint y)
        {
            if (y == 0) { return 1; }
            float f = Pow(x, y / 2);
            return (y % 2 == 0) ? f * f : x * f * f;
        }
        public static float Lerp(float f1, float f2, float w) => f1 + (f2 - f1) * w;
        public static byte Lerp(byte b1, byte b2, float w) => (byte)(b1 + (b2 - b1) * w);
        public static Color Lerp(Color c1, Color c2, float w) => Color.FromArgb((byte)(c1.A + (c2.A - c1.A) * w), (byte)(c1.R + (c2.R - c1.R) * w), (byte)(c1.G + (c2.G - c1.G) * w), (byte)(c1.B + (c2.B - c1.B) * w));
        public static int Sqrt(int i)
        {
            int min, max, average, squared;
            if (i < 0) { throw new ArgumentOutOfRangeException(); }
            if (i < 2) { return i; }
            min = 0; max = i;
            average = i > short.MaxValue ? short.MaxValue - 1 : i - 1;
            do
            {
                squared = average * average;
                if (squared > i) { if (max != average) { max = average; } else break; }
                else if (squared < i) { if (min != average) { min = average; } else break; }
                else { min = average; break; }
                average = (min + max) / 2;
            } while (max != min);//smallest difference
            return min;
        }
        public static float Sqrt(float f)
        {
            float min, max, average, squared;
            if (f < 0) { throw new ArgumentOutOfRangeException(); }
            if (f > 1) { min = 1; max = f; }
            else { min = f; max = 1; }
            do
            {
                average = (min + max) * 0.5f;
                squared = average * average;
                if (squared > f) { if (max != average) { max = average; } else break; }
                else if (squared < f) { if (min != average) { min = average; } else break; }
                else { min = average; break; }
            } while (max != min);//smallest difference
            return min;
        }
        public static double Sqrt(double f)
        {
            double min, max, average, squared;
            if (f < 0) { throw new ArgumentOutOfRangeException(); }
            if (f > 1) { min = 1; max = f; }
            else { min = f; max = 1; }
            do
            {
                average = (min + max) * 0.5f;
                squared = average * average;
                if (squared > f) { if (max != average) { max = average; } else break; }
                else if (squared < f) { if (min != average) { min = average; } else break; }
                else { min = average; break; }
            } while (max != min);//smallest difference
            return min;
        }
        public static float Abs(float f) => (f < 0) ? -f : f;

        public static int LogInt(float multiplied, float multiplier)
        {
            int cnt = 0;
            float val = 1;
            while (val <= multiplied)
            {
                val *= multiplier;
                cnt++;
            }

            return cnt - 1;
        }

        public static float Acos(float v)
        {
            return (float)Math.Acos(v);
        }
    }
    public static class InputHelper
    {
        //        public static Color GetColor()
        //        {
        //            Cursor.Show();
        //            ColorDialog c = new ColorDialog();
        //c.ShowDialog();
        //    return c.Color;
        //}
        public static string GetFileToOpen(string extension)
        {
            Cursor.Show();
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            if (extension == "map") { OpenFileDialog1.RestoreDirectory = true; }
            else if (extension == "") { OpenFileDialog1.RestoreDirectory = false; OpenFileDialog1.Filter = "WereldWijd bestanden(*)|*"; }
            else
            { OpenFileDialog1.RestoreDirectory = false; OpenFileDialog1.Filter = "WereldWijd bestanden(*." + extension + "|*." + extension; }
            string wb;
            OpenFileDialog1.ShowDialog();
            wb = OpenFileDialog1.FileName;
            return wb;
        }
        public static string GetFileToSave(string extension)
        {
            Cursor.Show();
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            if (extension.Length > 0)
            {
                extension = "WereldWijd bestanden(*." + extension + "|*." + extension;
            }
            else
            {
                extension = "WereldWijd bestanden(*)|*";
            }
            //            SaveFileDialog1.InitialDirectory = Padn("");
            SaveFileDialog1.Filter = extension;
            string wb;
            SaveFileDialog1.ShowDialog();
            wb = SaveFileDialog1.FileName;
            return wb;
        }
    }
    static class Randoms
    {
        public static int GetVal(int[] Chances, Random random)
        {
            int total = Chances[0];
            for (int i = 1; i < Chances.Length; i++)
            {
                total += Chances[i];
            }
            int index = random.Next(total);

            for (int i = 0; i < Chances.Length; i++)
            {
                index -= Chances[i];
                if (index < 0)
                    return i;
            }
            throw new Exception();
        }
        public static float[,] GetSingles(int w, int h, int Seed)
        {
            Random r = new Random(Seed);
            float[,] Singles = new float[w, h];
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    Singles[i, j] = (float)r.NextDouble();
                }
            }
            return Singles;
        }
    }
    static class Names
    {
        struct RatedName
        {
            /// <summary>
            /// The name.
            /// </summary>
            public string Name;
            /// <summary>
            /// The number of births of this name in 2003 in US.
            /// </summary>
            public int BirthCount;
            public RatedName(string name, int birthCount)
            {
                Name = name;
                BirthCount = birthCount;
            }
        }
        static RatedName[] Init()
        {
            RatedName[] boys = new RatedName[1000], girls = new RatedName[1000];
            string[] s = System.IO.File.ReadAllLines(boyloc);
            char[] c = new char[] { ' ' };
            for (int i = 0; i < 1000; i++)
            {
                string[] split = s[i].Split(c);
                RatedName name = new RatedName(split[0], int.Parse(split[1]));
                TotalMaleBirths += name.BirthCount;
                boys[i] = name;
            }

            MaleNames = boys;
            s = System.IO.File.ReadAllLines(girlloc);
            for (int i = 0; i < 1000; i++)
            {
                string[] split = s[i].Split(c);
                RatedName name = new RatedName(split[0], int.Parse(split[1]));
                TotalFemaleBirths += name.BirthCount;
                girls[i] = name;
            }

            return girls;
        }
        const string boyloc = @"D:\Public engines\Special\Names\boynames.txt";
        const string girlloc = @"D:\Public engines\Special\Names\girlnames.txt";
        static int TotalMaleBirths, TotalFemaleBirths;
        static RatedName[] FemaleNames = Init();
        static RatedName[] MaleNames;
        public static string GetRandomMaleName(Random random)
        {
            int index = random.Next(TotalMaleBirths);
            int i = -1;
            while (index >= 0)
            {
                index -= MaleNames[++i].BirthCount;
            }


            return MaleNames[i].Name;
        }
        public static string GetRandomFemaleName(Random random)
        {
            int index = random.Next(TotalFemaleBirths);
            int i = -1;
            while (index >= 0)
            {
                index -= FemaleNames[++i].BirthCount;
            }
            return FemaleNames[i].Name;
        }

    }
    static class File
    {
        public static void WriteAllBytes(string Path, byte[] Bytes)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(Path, FileMode.Create)))
            {
                bw.Write(Bytes);
                bw.Close();
            }
        }
        public static void WriteAllDoubles(string Path, float[] Doubles)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(Path, FileMode.Create)))
            {
                foreach (float d in Doubles)
                {
                    bw.Write(d);
                }
            }
        }
        public static void WriteAllDoubles(string Path, float[,] Doubles)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(Path, FileMode.Create)))
            {
                foreach (float d in Doubles)
                {
                    bw.Write(d);
                }
            }
        }
        public static byte[] ReadAllBytes(string Path)
        {
            byte[] Bytes;
            using (FileStream f = new FileStream(Path, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(f))
                {
                    Bytes = new byte[f.Length];
                    br.Read(Bytes, 0, (int)f.Length);
                    //bw.Write(Bytes);
                    br.Close();
                }
                return Bytes;
            }
        }
        public static void ReadAllDoubles(string Path, float[] Doubles)
        {
            using (FileStream f = new FileStream(Path, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(f))
                {

                    for (int i = 0; i < Doubles.Length; i++)
                    {
                        Doubles[i] = br.Read();
                    }
                    //br.Close();
                }
            }
        }
        public static void ReadAllDoubles(string Path, float[,] Doubles)
        {
            using (FileStream f = new FileStream(Path, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(f))
                {

                    for (int i = 0; i < Doubles.GetLength(0); i++)
                    {
                        for (int j = 0; j < Doubles.GetLength(1); j++)
                        {
                            Doubles[i, j] = br.Read();
                        }
                    }
                    //br.Close();
                }
            }
        }
        public static byte[] Read(string filePath, int begin, int length)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                buffer = new byte[length];            // create buffer
                fileStream.Read(buffer, begin, length);
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
        public static void CreateDirectory(string directory) { if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); } }
        public static void RemoveDirectoryitems(string directory)
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }
        }
    }
    class Iterator
    {
        public int I1 = 0, I2 = 0, J1 = 0, J2 = 0, K1 = 0, K2 = 0, Directioni, Directionj, Directionk;
        int I, J, K;
        public Iterator(int Directioni, int Directionj, int Directionk)
        {
            I = (Directioni > 0) ? I1 - Directioni : I2 - Directioni;
            J = (Directionj > 0) ? J1 : J2;
            K = (Directionk > 0) ? K1 : K2;
            this.Directioni = Directioni;
            this.Directionj = Directionj;
            this.Directionk = Directionk;
        }
        public bool Iterate(out int i, out int j, out int k)
        {
            I += Directioni;
            if ((Directioni > 0) ? I <= I2 : I >= I1) { }
            else
            {
                I = (Directioni > 0) ? I1 : I2;
                J += Directionj;
                if ((Directionj > 0) ? J <= J2 : J >= J1) { }
                else
                {
                    J = (Directionj > 0) ? J1 : J2;
                    K += Directionk;
                    if ((Directionk > 0) ? K <= K2 : K >= K1) { }
                    else
                    {
                        K = (Directionk > 0) ? K1 : K2;
                        i = I;
                        j = J;
                        k = K;
                        return false;
                    }

                }

            }
            i = I;
            j = J;
            k = K;
            return true;
        }

    }
    class SimplexNoise
    {
        int precised = 3;
        float min = 0, max = 1, scale = 1, Power = 1;
        float[] offset;
        Noise[] noises;
        public float this[float x, float y]
        {
            get
            {
                x /= scale;
                y /= scale;
                float value = 0;
                for (int i = 0; i < precised; i++)
                {
                    value += (noises[i].Perlin(offset[i] * x, offset[i] * y) * Noise.basevalue + 0.5f) / offset[i];
                }
                float divby = (2 - (float)Math.Pow(2, -(precised - 1)));
                value /= divby;
                value = (float)Math.Pow(value, Power);
                value = value * (max - min) + min;
                return value;
            }
        }
        public SimplexNoise(int precised, float min, float max, float scale, int width, int height, int Seed, float Power)
        {
            this.precised = precised;
            this.max = max;
            this.min = min;
            this.scale = scale;
            this.Power = Power;
            offset = new float[precised];
            noises = new Noise[precised];
            for (int i = 0; i < precised; i++)
            {
                offset[i] = (float)Math.Pow(2, i);
                noises[i] = new Noise((int)(((width + 1) / scale) * offset[i]), (int)(((height + 1) / scale) * offset[i]), Seed);
            }
        }
    }
    class RidgidNoise
    {
        int precised = 3;
        float min = 0, max = 1, scale = 1, Power = 1;
        float[] offset;
        Noise[] noises;
        public float this[float x, float y]
        {
            get
            {
                x /= scale;
                y /= scale;
                float value = 0;
                for (int i = 0; i < precised; i++)
                {
                    float val = noises[i].Perlin(offset[i] * x, offset[i] * y) * Noise.basevalueTo1;
                    value += (val > 0 ? 1 - val : 1 + val) / offset[i];
                }
                float divby = (2 - (float)Math.Pow(2, -(precised - 1)));
                value /= divby;
                value = (float)Math.Pow(value, Power);
                value = value * (max - min) + min;
                return value;
            }
        }
        public RidgidNoise(int precised, float min, float max, float scale, int width, int height, int Seed, float Power)
        {
            this.precised = precised;
            this.max = max;
            this.min = min;
            this.scale = scale;
            this.Power = Power;
            offset = new float[precised];
            noises = new Noise[precised];
            for (int i = 0; i < precised; i++)
            {
                offset[i] = (float)Math.Pow(2, i);
                noises[i] = new Noise((int)(((width + 1) / scale) * offset[i]), (int)(((height + 1) / scale) * offset[i]), Seed);
            }
        }
    }
    public class Noise
    {
        int _width, _height;
        int _w2;
        float[] gradients;
        /// <summary>
        /// if you multiplie the output by that, it will be in range [-0.5,0.5]
        /// </summary>
        public const float basevalue = 0.707106769f;//basevalue = 0.5f / (float)Math.Pow(0.5, 0.5);//range = -sqrt(0.5) to sqrt(0.5)
        /// <summary>
        /// if you multiplie the output by that, it will be in range [-1,1]
        /// </summary>
        public const float basevalueTo1 = 0.707106769f * 2;//basevalue = 0.5f / (float)Math.Pow(0.5, 0.5);//range = -sqrt(0.5) to sqrt(0.5)
        public float GetZerobasednoise(float x, float y) => (Perlin(x, y) * basevalue + 0.5f);
        public Noise(int width, int height, int Seed)
        {
            if (width <= 0 || height <= 0) throw new Exception("Size is too small");
            _width = width;
            _height = height;
            _w2 = width * 2;
            Init(Seed);
        }
        public static float Lerp(float a, float b, float w) => a + (b - a) * w;
        public void Init(int Seed)
        {
            Random r = new Random(Seed);
            float rotation;
            int W = _width + 1;
            int H = _height + 1;

            int l = W * H * 2;
            gradients = new float[l];
            for (int i = 0; i < l; i += 2)
            {
                rotation = (float)r.NextDouble() * (Mathf.PI * 2);
                gradients[i] = (float)Math.Sin(rotation);//x
                gradients[i + 1] = (float)Math.Cos(rotation);//y
            }
        }
        // Computes the dot product of the distance and gradient vectors.
        float OldDotGridGradient(int ix, int iy, float x, float y)
        {
            // Compute the dot-product from gradients at each grid node
            return ((x - ix) * gradients[ix * 2 + iy * _w2] + (y - iy) * gradients[ix * 2 + iy * _w2 + 1]);
        }
        // Compute Perlin noise at coordinates x, y
        public float Perlin(float x, float y)
        {
            //https://www.redblobgames.com/maps/terrain-from-noise/
            // Determine grid cell coordinates
            int x0, x1, y0, y1;
            if (x == _width)
            {
                x0 = _width - 1;
                x1 = _width;
            }
            else
            {
                x0 = (int)x;
                x1 = x0 + 1;
            }
            if (y == _width)
            {
                y0 = _width - 1;
                y1 = _width;
            }
            else
            {
                y0 = (int)y;
                y1 = y0 + 1;
            }

            // Determine interpolation weights
            // Could also use higher order polynomial/s-curve here
            float sx = x - x0;
            float sy = y - y0;

            // Interpolate between grid point gradients
            float n0, n1, ix0, ix1, value;
            //n0 = OldDotGridGradient(x0, y0, x, y);
            //x0,y0
            n0 = (x - x0) * gradients[x0 * 2 + y0 * _w2] + sy * gradients[x0 * 2 + y0 * _w2 + 1];
            //n1 = OldDotGridGradient(x1, y0, x, y);
            //x1,y0
            n1 = (x - x1) * gradients[x1 * 2 + y0 * _w2] + sy * gradients[x1 * 2 + y0 * _w2 + 1];
            //ix0 = Lerp(n0, n1, sx);
            ix0 = n0 + (n1 - n0) * sx;
            //ix,iy,x,y
            //n0 = OldDotGridGradient(x0, y1, x, y);
            //x0,y1
            n0 = (x - x0) * gradients[x0 * 2 + y1 * _w2] + (y - y1) * gradients[x0 * 2 + y1 * _w2 + 1];
            //n1 = OldDotGridGradient(x1, y1, x, y);
            //x1,y1
            n1 = (x - x1) * gradients[x1 * 2 + y1 * _w2] + (y - y1) * gradients[x1 * 2 + y1 * _w2 + 1];
            //ix1 = Lerp(n0, n1, sx)
            ix1 = n0 + (n1 - n0) * sx;
            //value = Lerp(ix0, ix1, sy);
            value = ix0 + (ix1 - ix0) * sy;
            return value;
        }
    }

    /// <summary>
    /// Implementation of Mersenne Twister random number generator
    /// https://gist.github.com/adamveld12/6c0350d1cfd2da449dc6
    /// </summary>
    public class Random
    {
        const int MatrixLength = 624;
        private readonly uint[] _matrix = new uint[MatrixLength];
        private int _index = 0;


        public Random() : this((uint)(0xFFFFFFFF & DateTime.Now.Ticks)) { }
        public Random(int seed) : this((uint)seed) { }


        /// <summary>
        /// Initializes a new instance of the MersennePrimeRNG with a seed
        /// </summary>
        /// <param name="seed"></param>
        public Random(uint seed)
        {
            _matrix[0] = seed;
            for (int i = 1; i < MatrixLength; i++)
                _matrix[i] = (1812433253 * (_matrix[i - 1] ^ ((_matrix[i - 1]) >> 30) + 1));
        }
        /// <summary>
        /// Fills "buffer" with random bytes
        /// </summary>
        /// <param name="buffer">
        /// The byte array to be filled
        /// </param>
        public void NextBytes(byte[] buffer)
        {
            int l = buffer.Length;
            int lPlus = l % 4;
            l -= lPlus;
            int offset = 0;
            int value;
            while (offset < l)
            {
                value = Next();
                buffer[offset++] = (byte)(value >> 24);
                buffer[offset++] = (byte)(value >> 16);
                buffer[offset++] = (byte)(value >> 8);
                buffer[offset++] = (byte)value;
            }
            if (offset < l)
            {
                value = Next();
                buffer[offset++] = (byte)(value >> 24);
            }
            if (offset < l)
            {
                value = Next();
                buffer[offset++] = (byte)(value >> 16);
            }
            if (offset < l)
            {
                value = Next();
                buffer[offset++] = (byte)(value >> 8);
            }
        }

        /// <summary>
        /// Generates a new matrix table
        /// </summary>
        private void Generate()
        {
            for (int i = 0; i < MatrixLength; i++)
            {
                uint y = (_matrix[i] >> 31) + ((_matrix[(i + 1) & 623]) << 1);
                _matrix[i] = _matrix[(i + 397) & 623] ^ (y >> 1);
                if (y % 2 != 0)
                    _matrix[i] = (_matrix[i] ^ (2567483615));
            }
        }


        /// <summary>
        /// Generates and returns a random number
        /// </summary>
        /// <returns></returns>
        public int Next()
        {
            if (_index == 0)
                Generate();


            uint y = _matrix[_index];
            y = y ^ (y >> 11);
            y = (y ^ (y << 7) & (2636928640));
            y = (y ^ (y << 15) & (4022730752));
            y = (y ^ (y >> 18));


            _index = (_index + 1) % 623;
            return (int)(y % int.MaxValue);
        }
        /// <summary>
        /// Generates a random floating point number by dividing Next() by int.MaxValue
        /// </summary>
        /// <returns>
        /// a random float between 0.0 and 1.0
        /// </returns>
        public float NextFloat()
        {
            float i = Next() / (float)int.MaxValue;
            return i;
        }
        public Vector3 NextPoint()
            => new Vector3(Next() / (float)int.MaxValue, Next() / (float)int.MaxValue, Next() / (float)int.MaxValue);
        public Vector3 NextVector()
        {
            float r1 = Next() / (float)int.MaxValue * Mathf.PI2;
            float r2 = Next() / (float)int.MaxValue * Mathf.PI - Mathf.QuarterAngle;
            float cos2 = (float)Math.Cos(r2);
            return new Vector3((float)(cos2 * Math.Sin(r1)), (float)(cos2 * Math.Cos(r1)), (float)Math.Sin(r2));
        }
        /// <summary>
        /// Generates a random double-precising point number by dividing Next() by int.MaxValue
        /// </summary>
        /// <returns>
        /// a random double between 0.0 and 1.0
        /// </returns>
        public double NextDouble()
        {
            double i = Next() / (double)int.MaxValue;
            return i;
        }
        /// <summary>
        /// Generates and returns a random number
        /// </summary>
        /// <param name="max">The highest value - 1 that can be returned</param>
        /// <returns></returns>
        public int Next(int max)
        {
            var randomValue = Next();
            return randomValue % max;
        }
        public Color NextColor()
        {
            int value = Next();
            return Color.FromArgb(255, (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8));
        }
        public byte NextByte() => (byte)Next();
        /// <summary>
        /// Generates and returns a random number
        /// </summary>
        /// <param name="min">The lowest value returned</param>
        /// <param name="max">The highest value - 1 returned</param>
        /// <returns></returns>
        public int Next(int min, int max)
        {
            if (min > max)
                throw new ArgumentException("min cannot be greater than max", "min");
            int difference = max - min;
            return min + Next() % difference + min;
        }
    }
    /*
This file is part of libnoise-dotnet.
libnoise-dotnet is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

libnoise-dotnet is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with libnoise-dotnet.  If not, see <http://www.gnu.org/licenses/>.

Simplex Noise in 2D, 3D and 4D. Based on the example code of this paper:
http://staffwww.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf

From Stefan Gustavson, Linkping University, Sweden (stegu at itn dot liu dot se)
From Karsten Schmidt (slight optimizations & restructuring)

Some changes by Sebastian Lague for use in a tutorial series.
*/

    /*
     * Noise module that outputs 3-dimensional Simplex Perlin noise.
     * This algorithm has a computational cost of O(n+1) where n is the dimension.
     * 
     * This noise module outputs values that usually range from
     * -1.0 to +1.0, but there are no guarantees that all output values will exist within that range.
    */

    public class Noise3D
    {
        #region Values
        /// Initial permutation table
        static readonly int[] Source = {
            151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142,
            8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203,
            117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165,
            71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41,
            55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89,
            18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250,
            124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189,
            28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9,
            129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34,
            242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31,
            181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114,
            67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180
        };

        const int RandomSize = 256;
        const double Sqrt3 = 1.7320508075688772935;
        const double Sqrt5 = 2.2360679774997896964;
        int[] _random;

        /// Skewing and unskewing factors for 2D, 3D and 4D, 
        /// some of them pre-multiplied.
        const double F2 = 0.5 * (Sqrt3 - 1.0);

        const double G2 = (3.0 - Sqrt3) / 6.0;
        const double G22 = G2 * 2.0 - 1;

        const double F3 = 1.0 / 3.0;
        const double G3 = 1.0 / 6.0;

        const double F4 = (Sqrt5 - 1.0) / 4.0;
        const double G4 = (5.0 - Sqrt5) / 20.0;
        const double G42 = G4 * 2.0;
        const double G43 = G4 * 3.0;
        const double G44 = G4 * 4.0 - 1.0;

        /// <summary>
        /// Gradient vectors for 3D (pointing to mid points of all edges of a unit
        /// cube)
        /// </summary>
        static readonly sbyte[][] Grad3 = new sbyte[][]
        {
        new sbyte[] {1, 1, 0}, new sbyte[] {-1, 1, 0}, new sbyte[] {1, -1, 0},
        new sbyte[] {-1, -1, 0}, new sbyte[] {1, 0, 1}, new sbyte[] {-1, 0, 1},
        new sbyte[] {1, 0, -1}, new sbyte[] {-1, 0, -1}, new sbyte[] {0, 1, 1},
        new sbyte[] {0, -1, 1}, new sbyte[] {0, 1, -1}, new sbyte[] {0, -1, -1}
    };
        #endregion

        public Noise3D()
        {
            Randomize(0);
        }

        public Noise3D(int seed)
        {
            Randomize(seed);
        }

        public float Evaluate(Vector3 point) => Evaluate(point.x, point.y, point.z);
        /// <summary>
        /// Generates value, typically in range [-1, 1]
        /// </summary>
        public float Evaluate(double x, double y, double z)
        {
            double n0 = 0, n1 = 0, n2 = 0, n3 = 0;

            // Noise contributions from the four corners
            // Skew the input space to determine which simplex cell we're in
            double s = (x + y + z) * F3,
                xs = x + s,
                ys = y + s,
                zs = z + s;

            // for 3D
            int i = xs < 0 ? (int)xs - 1 : (int)xs,
                j = ys < 0 ? (int)ys - 1 : (int)ys,
                k = zs < 0 ? (int)zs - 1 : (int)zs;

            double t = (i + j + k) * G3;

            // The x,y,z distances from the cell origin
            double x0 = x - (i - t);
            double y0 = y - (j - t);
            double z0 = z - (k - t);

            // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
            // Determine which simplex we are in.
            // Offsets for second corner of simplex in (i,j,k)
            int i1, j1, k1;

            // coords
            int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords

            if (x0 >= y0)
            {
                if (y0 >= z0)
                {
                    // X Y Z order
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                }
                else if (x0 >= z0)
                {
                    // X Z Y order
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                }
                else
                {
                    // Z X Y order
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                }
            }
            else
            {
                // x0 < y0
                if (y0 < z0)
                {
                    // Z Y X order
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                else if (x0 < z0)
                {
                    // Y Z X order
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                else
                {
                    // Y X Z order
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                }
            }

            // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
            // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z),
            // and
            // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z),
            // where c = 1/6.

            // Offsets for second corner in (x,y,z) coords
            double x1 = x0 - i1 + G3;
            double y1 = y0 - j1 + G3;
            double z1 = z0 - k1 + G3;

            // Offsets for third corner in (x,y,z)
            double x2 = x0 - i2 + F3;
            double y2 = y0 - j2 + F3;
            double z2 = z0 - k2 + F3;

            // Offsets for last corner in (x,y,z)
            double x3 = x0 - 0.5;
            double y3 = y0 - 0.5;
            double z3 = z0 - 0.5;

            // Work out the hashed gradient indices of the four simplex corners
            int ii = i & 0xff;
            int jj = j & 0xff;
            int kk = k & 0xff;

            // Calculate the contribution from the four corners
            double t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0;
            if (t0 > 0)
            {
                t0 *= t0;
                int gi0 = _random[ii + _random[jj + _random[kk]]] % 12;
                sbyte[] b = Grad3[gi0];
                n0 = t0 * t0 * (b[0] * x0 + b[1] * y0 + b[2] * z0);
                //n0 = t0 * t0 * Dot(Grad3[gi0], x0, y0, z0);
            }
            double t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1;
            if (t1 > 0)
            {
                t1 *= t1;
                int gi1 = _random[ii + i1 + _random[jj + j1 + _random[kk + k1]]] % 12;
                sbyte[] b = Grad3[gi1];
                n1 = t1 * t1 * (b[0] * x1 + b[1] * y1 + b[2] * z1);
                //                n1 = t1 * t1 * Dot(Grad3[gi1], x1, y1, z1);
            }

            double t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2;
            if (t2 > 0)
            {
                t2 *= t2;
                int gi2 = _random[ii + i2 + _random[jj + j2 + _random[kk + k2]]] % 12;
                sbyte[] b = Grad3[gi2];
                n2 = t2 * t2 * (b[0] * x2 + b[1] * y2 + b[2] * z2);
                //                n2 = t2 * t2 * Dot(Grad3[gi2], x2, y2, z2);
            }

            double t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3;
            if (t3 > 0)
            {
                t3 *= t3;
                int gi3 = _random[ii + 1 + _random[jj + 1 + _random[kk + 1]]] % 12;
                sbyte[] b = Grad3[gi3];
                n3 = t3 * t3 * (b[0] * x3 + b[1] * y3 + b[2] * z3);
                //                n3 = t3 * t3 * Dot(Grad3[gi3], x3, y3, z3);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to stay just inside [-1,1]
            return (float)(n0 + n1 + n2 + n3) * 32;
        }


        void Randomize(int seed)
        {
            _random = new int[RandomSize * 2];

            if (seed != 0)
            {
                // Shuffle the array using the given seed
                // Unpack the seed into 4 bytes then perform a bitwise XOR operation
                // with each byte
                var F = new byte[4];
                UnpackLittleUint32(seed, ref F);

                for (int i = 0; i < Source.Length; i++)
                {
                    _random[i] = Source[i] ^ F[0];
                    _random[i] ^= F[1];
                    _random[i] ^= F[2];
                    _random[i] ^= F[3];

                    _random[i + RandomSize] = _random[i];
                }

            }
            else
            {
                for (int i = 0; i < RandomSize; i++)
                    _random[i + RandomSize] = _random[i] = Source[i];
            }
        }

        static double Dot(sbyte[] g, double x, double y, double z, double t)
        {
            return g[0] * x + g[1] * y + g[2] * z + g[3] * t;
        }

        static double Dot(sbyte[] g, double x, double y, double z)
        {
            return g[0] * x + g[1] * y + g[2] * z;
        }

        static double Dot(sbyte[] g, double x, double y)
        {
            return g[0] * x + g[1] * y;
        }

        static int FastFloor(double x)
        {
            return x >= 0 ? (int)x : (int)x - 1;
        }

        /// <summary>
        /// Unpack the given integer (int32) to an array of 4 bytes  in little endian format.
        /// If the length of the buffer is too smal, it wil be resized.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The output buffer.</param>
        static byte[] UnpackLittleUint32(int value, ref byte[] buffer)
        {
            if (buffer.Length < 4)
                Array.Resize(ref buffer, 4);

            buffer[0] = (byte)(value & 0x00ff);
            buffer[1] = (byte)((value & 0xff00) >> 8);
            buffer[2] = (byte)((value & 0x00ff0000) >> 16);
            buffer[3] = (byte)((value & 0xff000000) >> 24);

            return buffer;
        }

    }
    namespace OpenGL
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Transform
        {
            [FieldOffset(0)]
            public Vector3 position;
            [FieldOffset(12)]
            Vector3 _right;
            [FieldOffset(24)]
            Vector3 _direction;
            [FieldOffset(36)]
            Vector3 _up;
            [FieldOffset(48)]
            Matrix _matrix;
            public Matrix Matrix => _matrix;
            public Vector3 Direction => _direction;
            public Vector3 Right => _right;
            public Vector3 Up => _up;
            public void Rotate(Vector3 axis, float rotation)
            {
                //pass in right and direction to be secured they will be good
                _matrix = Matrix.Rotate(axis, _matrix, rotation);
                _right = _matrix.ApplyToReverse(R);
                _direction = _matrix.ApplyToReverse(D);
                _up = _matrix.ApplyToReverse(U);
            }
            public void Yaw(float rotation)
            {
                Vector3 v = _up;
                Rotate(_up, rotation);
            }
            public void Pitch(float rotation)
            {
                Rotate(_right, rotation);
            }
            public void Roll(float rotation)
            {
                Rotate(_direction, rotation);
            }
            public void MoveForward(float distance)
            {
                position += _direction * distance;
            }
            public void MoveSideward(float distance)
            {
                position += _right * distance;
            }
            static Vector3 R = new Vector3(1, 0, 0), D = new Vector3(0, 1, 0), U = new Vector3(0, 0, 1);
            public Transform(Vector3 position)
            {
                this.position = position;
                _right = R;
                _direction = D;
                _up = U;
                _matrix = Matrix.Normal;
            }
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct Matrix
        {
            public static Matrix Normal = new Matrix
            {
                XtoX = 1,
                YtoY = 1,
                ZtoZ = 1
            };
            public static Matrix Rotate(Vector3 axis, Matrix matrix, float Rotation)
            {
                axis.Normalize();
                float sin = (float)Math.Sin(Rotation);
                float cos = (float)Math.Cos(Rotation);
                float mincos = 1 - cos;
                //https://en.wikipedia.org/wiki/Rotation_matrix#Rotation_matrix_from_axis_and_angle
                Matrix rot = new Matrix
                {
                    XtoX = cos + axis.x * axis.x * mincos,
                    YtoX = axis.x * axis.y * mincos - axis.z * sin,
                    ZtoX = axis.x * axis.z * mincos + axis.y * sin,
                    XtoY = axis.y * axis.x * mincos + axis.z * sin,
                    YtoY = cos + axis.y * axis.y * mincos,
                    ZtoY = axis.y * axis.z * mincos - axis.x * sin,
                    XtoZ = axis.z * axis.x * mincos - axis.y * sin,
                    YtoZ = axis.z * axis.y * mincos + axis.x * sin,
                    ZtoZ = cos + axis.z * axis.z * mincos
                };
                matrix = matrix.ApplyTo(rot);
                //axis.Normalize();
                //Matrix lhs = axis;
                //Matrix cross = Cross(lhs, matrix);
                //return cos * matrix + sin * cross;
                return matrix;
            }
            public void MultiplyX(float value)
            {
                XtoX *= value;
                YtoX *= value;
                ZtoX *= value;
            }
            public void MultiplyY(float value)
            {
                XtoY *= value;
                YtoY *= value;
                ZtoY *= value;
            }
            public void MultiplyZ(float value)
            {
                XtoZ *= value;
                YtoZ *= value;
                ZtoZ *= value;
            }
            public Vector3 ApplyToReverse(Vector3 vector) => new Vector3
            {
                x = XtoX * vector.x + XtoY * vector.y + XtoZ * vector.z,
                y = YtoX * vector.x + YtoY * vector.y + YtoZ * vector.z,
                z = ZtoX * vector.x + ZtoY * vector.y + ZtoZ * vector.z,
            };
            public Vector3 ApplyTo(Vector3 vector) => new Vector3
            {
                x = XtoX * vector.x + YtoX * vector.y + ZtoX * vector.z,
                y = XtoY * vector.x + YtoY * vector.y + ZtoY * vector.z,
                z = XtoZ * vector.x + YtoZ * vector.y + ZtoZ * vector.z,
            };
            public Matrix ApplyTo(Matrix other)
            {
                return new Matrix
                {
                    XtoX = other.XtoX * XtoX + other.XtoY * YtoX + other.XtoZ * ZtoX,
                    YtoX = other.YtoX * XtoX + other.YtoY * YtoX + other.YtoZ * ZtoX,
                    ZtoX = other.ZtoX * XtoX + other.ZtoY * YtoX + other.ZtoZ * ZtoX,
                    XtoY = other.XtoX * XtoY + other.XtoY * YtoY + other.XtoZ * ZtoY,
                    YtoY = other.YtoX * XtoY + other.YtoY * YtoY + other.YtoZ * ZtoY,
                    ZtoY = other.ZtoX * XtoY + other.ZtoY * YtoY + other.ZtoZ * ZtoY,
                    XtoZ = other.XtoX * XtoZ + other.XtoY * YtoZ + other.XtoZ * ZtoZ,
                    YtoZ = other.YtoX * XtoZ + other.YtoY * YtoZ + other.YtoZ * ZtoZ,
                    ZtoZ = other.ZtoX * XtoZ + other.ZtoY * YtoZ + other.ZtoZ * ZtoZ,
                };
            }
            public static Matrix operator +(Matrix m1, Matrix m2) => new Matrix
            {
                XtoX = m1.XtoX + m2.XtoX,
                XtoY = m1.XtoY + m2.XtoY,
                XtoZ = m1.XtoZ + m2.XtoZ,
                YtoX = m1.YtoX + m2.YtoX,
                YtoY = m1.YtoY + m2.YtoY,
                YtoZ = m1.YtoZ + m2.YtoZ,
                ZtoX = m1.ZtoX + m2.ZtoX,
                ZtoY = m1.ZtoY + m2.ZtoY,
                ZtoZ = m1.ZtoZ + m2.ZtoZ,
            };
            public static Matrix operator *(float mult, Matrix matrix) => new Matrix
            {
                XtoX = matrix.XtoX * mult,
                YtoX = matrix.YtoX * mult,
                ZtoX = matrix.ZtoX * mult,
                XtoY = matrix.XtoY * mult,
                YtoY = matrix.YtoY * mult,
                ZtoY = matrix.ZtoY * mult,
                XtoZ = matrix.XtoZ * mult,
                YtoZ = matrix.YtoZ * mult,
                ZtoZ = matrix.ZtoZ * mult,
            };
            public static Matrix operator *(Matrix matrix, float mult) => new Matrix
            {
                XtoX = matrix.XtoX * mult,
                YtoX = matrix.YtoX * mult,
                ZtoX = matrix.ZtoX * mult,
                XtoY = matrix.XtoY * mult,
                YtoY = matrix.YtoY * mult,
                ZtoY = matrix.ZtoY * mult,
                XtoZ = matrix.XtoZ * mult,
                YtoZ = matrix.YtoZ * mult,
                ZtoZ = matrix.ZtoZ * mult,
            };
            public static Matrix Cross(Matrix lhs, Matrix rhs)
            {
                return new Matrix
                {
                    XtoX = lhs.XtoX * rhs.XtoX,
                    XtoY = lhs.XtoY * rhs.YtoX,
                    XtoZ = lhs.XtoZ * rhs.ZtoX,
                    YtoX = lhs.YtoX * rhs.XtoY,
                    YtoY = lhs.YtoY * rhs.YtoY,
                    YtoZ = lhs.YtoZ * rhs.ZtoY,
                    ZtoX = lhs.ZtoX * rhs.XtoZ,
                    ZtoY = lhs.ZtoY * rhs.YtoZ,
                    ZtoZ = lhs.ZtoZ * rhs.ZtoZ,
                };
            }
            public static implicit operator Matrix(Vector3 vector) => new Matrix { XtoX = vector.x, YtoY = vector.y, ZtoZ = vector.z };
            [FieldOffset(0)]
            public float XtoX;
            [FieldOffset(4)]
            public float ZtoX;
            [FieldOffset(8)]
            public float YtoX;
            [FieldOffset(12)]
            public float XtoY;
            [FieldOffset(16)]
            public float YtoY;
            [FieldOffset(20)]
            public float ZtoY;
            [FieldOffset(24)]
            public float XtoZ;
            [FieldOffset(28)]
            public float YtoZ;
            [FieldOffset(32)]
            public float ZtoZ;
        }
        public static class OpenGl
        {
            public static float Getrotation(float vx, float vy)
            {
                float l = Mathf.Sqrt(vx * vx + vy * vy);
                vx /= l;
                vy /= l;
                float rotation = (float)Math.Acos(vx);
                if (vy < 0)
                    rotation *= -1;//rotation = PI - rotation;
                if (rotation < 0)
                { rotation += Mathf.PI2; }
                return rotation;
            }
            public static Vector3 Getrotation(float vx, float vy, float vz)
            {
                float l = Mathf.Sqrt(vx * vx + vy * vy);
                vx /= l;
                vy /= l;
                Vector3 rotation = new Vector3
                {
                    x = (float)Math.Asin(vx)
                };
                if (vy < 0)
                    rotation.x -= Mathf.PI;//rotation = PI - rotation;
                if (rotation.x < 0)
                { rotation.x += Mathf.PI2; }
                rotation.y = -(float)Math.Asin(vz);
                rotation.z = 0;
                return rotation;
            }
            public static Vector3 Getrotation(Vector3 direction, Vector3 right)
            {
                Vector3 dir = direction;
                Vector3 up = Vector3.Cross(direction, right);
                float l = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
                direction.x /= l;
                direction.y /= l;
                Vector3 rotation = new Vector3
                {
                    x = (float)Math.Asin(direction.x)
                };
                if (direction.y < 0)
                    rotation.x -= Mathf.PI;//rotation = PI - rotation;
                if (rotation.x < 0)
                { rotation.x += Mathf.PI2; }
                rotation.y = -(float)Math.Asin(direction.z);
                rotation.z = right.x;
                return rotation;
            }
            /// <summary>
            /// rotates point with Rotation radians prependicular to axis
            /// </summary>
            /// <param name="axis">
            /// vector3 to do the rotation about
            /// </param>
            /// <param name="point">
            /// must be prependicular to axis
            /// </param>
            /// <param name="Rotation">
            /// rotation in radians
            /// </param>
            /// <returns>
            /// an rotated vector3 prependicular to axis
            /// </returns>
            public static Vector3 Rotate(Vector3 axis, Vector3 point, float Rotation)
            {
                axis.Normalize();
                Vector3 cross = Vector3.Cross(axis, point);
                float sin = (float)System.Math.Sin(Rotation);
                float cos = (float)System.Math.Cos(Rotation);
                return cos * point + sin * cross;
            }
            //Mesh sorted;
            static float[] CalculatePointDistances(Mesh mesh, Vector3 position)
            {
                Vector3[] v = mesh.vertices;
                int length = v.Length;
                //int tricount = length/3;
                float[] f = new float[length];
                float x = position.x, y = position.y, z = position.z;
                for (int i = 0; i < length; i++)
                {
                    Vector3 vector = v[i];
                    float dx = vector.x - x, dy = vector.y - y, dz = vector.z - z;
                    f[i] = dx * dx + dy * dy + dz * dz;
                }
                return f;
            }
            static float[] CalculateTriDistances(Mesh mesh, Vector3 pos)
            {
                float[] pointdists = CalculatePointDistances(mesh, pos);
                int[] tris = mesh.triangles;
                int tricount = tris.Length / 3;
                float[] f = new float[tricount];
                for (int i = 0, j = 0; i < tricount; i++, j += 3)
                {
                    f[i] = pointdists[tris[j]];
                }
                return f;
            }

            public static void Draw(TriangleDrawer graphics, Mesh mesh, Transform info, bool onlyborders, Shader shader, float Zoom)
            {
                if (mesh.triangles.Length > 0)
                {
                    #region initialisation
                    Vector3 position = info.position;
                    Vector3 direction = info.Direction;
                    //Vector3 direction = info.direction;
                    //Vector3 right = info.right;
                    Mesh todraw = mesh;
                    //Shader shader = todraw.shader;
                    int W = graphics.Bwidth, H = graphics.Bheight,
                        W2 = W / 2, H2 = H / 2;
                    float Multiplier = Math.Max(W2, H2) * Zoom;
                    bool shade = shader.Enabled;
                    float z = position.z, x = position.x, y = position.y,
                        LightR = shader.LigtR, LightG = shader.LightG, LightB = shader.LightB,
                        ShadeR = shader.ShadeR, ShadeG = shader.ShadeG, ShadeB = shader.ShadeB;
                    float ViewOffset = .5f / Zoom;
                    //the minimum difference in degrees the triangle has to be
                    float MinDegreesDiff = Mathf.QuarterAngle - (float)Math.Sin(ViewOffset);
                    //Vector3 rot = info.rotation;
                    //float rx = rot.x,
                    //    ry = rot.y,
                    //    rz = rot.z;
                    //float sinx = (float)Math.Sin(rx),
                    //      cosx = (float)Math.Cos(rx),
                    //      siny = (float)Math.Sin(ry),
                    //      cosy = (float)Math.Cos(ry),
                    //      sinz = (float)Math.Sin(rz),
                    //      cosz = (float)Math.Cos(rz);
                    #region getbounds
                    Rectangle bounds = graphics.Bounds;
                    int br = bounds.Right,
                        bb = bounds.Bottom,
                        bl = bounds.x,
                        bt = bounds.y;
                    #endregion


                    #endregion
                    #region Create Multiplication matrix
                    //https://en.wikipedia.org/wiki/Rotation_matrix
                    Matrix matrix = info.Matrix;
                    matrix.MultiplyX(Multiplier);
                    matrix.MultiplyZ(Multiplier);
                    float matrixmultXtoX = matrix.XtoX;
                    float matrixmultYtoX = matrix.YtoX;
                    float matrixmultZtoX = matrix.ZtoX;
                    float matrixmultXtoY = matrix.XtoY;
                    float matrixmultYtoY = matrix.YtoY;
                    float matrixmultZtoY = matrix.ZtoY;
                    float matrixmultXtoZ = matrix.XtoZ;
                    float matrixmultYtoZ = matrix.YtoZ;
                    float matrixmultZtoZ = matrix.ZtoZ;
                    //float r = Getrotation(right.x, right.y);
                    //rotate all the points instead of rotating the camera
                    //r *= -1;//inverse so the points will turn to the right position
                    //to inverse an rotation you've to put y*-1
                    //float cosr = right.x;
                    //float sinr = -right.y;
                    //x: cos * x - sin * y
                    //y: cos * y + sin * x
                    //matrixmultXtoX = cosr;
                    //matrixmultXtoY = sinr;
                    //matrixmultYtoX = -sinr;
                    //matrixmultYtoY = cosr;

                    #endregion
                    unsafe
                    {
                        Vector3[] vectors = todraw.vertices;
                        byte[] Colors = todraw.Colors;
                        int[] Tris = todraw.triangles;
                        int TriCount = Tris.Length / 3;
                        fixed (int* pTri = Tris, Pclr = graphics.colours)
                        {
                            fixed (byte* pColor = Colors)
                            {
                                fixed (Vector3* PointerVector = vectors)
                                {
                                    int* PointerTri = pTri;
                                    byte* PointerColour = pColor;
                                    for (int i = 0; i < TriCount; i++)//iterate for each tri
                                    {
                                        #region Get Values
                                        Vector3 v1 = *(PointerVector + *PointerTri++);
                                        Vector3 v2 = *(PointerVector + *PointerTri++);
                                        Vector3 v3 = *(PointerVector + *PointerTri++);
                                        byte red = *PointerColour++;
                                        byte green = *PointerColour++;
                                        byte blue = *PointerColour++;
                                        /*                                Vector3 v1 = vectors[Tris[k]],
                                                                                v2 = vectors[Tris[k + 1]],
                                                                                v3 = vectors[Tris[k + 2]];*/
                                        float
                                            DX1 = v1.x - x, DX2 = v2.x - x, DX3 = v3.x - x,
                                            DY1 = v1.y - y, DY2 = v2.y - y, DY3 = v3.y - y,
                                            DZ1 = v1.z - z, DZ2 = v2.z - z, DZ3 = v3.z - z;
                                        #endregion
                                        #region Multiply With Multiplication Matrix
                                        float outx1 = DX1 * matrixmultXtoX + DY1 * matrixmultYtoX + DZ1 * matrixmultZtoX,
                                            outy1 = DX1 * matrixmultXtoY + DY1 * matrixmultYtoY + DZ1 * matrixmultZtoY,
                                            outz1 = DX1 * matrixmultXtoZ + DY1 * matrixmultYtoZ + DZ1 * matrixmultZtoZ,
                                            outx2 = DX2 * matrixmultXtoX + DY2 * matrixmultYtoX + DZ2 * matrixmultZtoX,
                                            outy2 = DX2 * matrixmultXtoY + DY2 * matrixmultYtoY + DZ2 * matrixmultZtoY,
                                            outz2 = DX2 * matrixmultXtoZ + DY2 * matrixmultYtoZ + DZ2 * matrixmultZtoZ,
                                            outx3 = DX3 * matrixmultXtoX + DY3 * matrixmultYtoX + DZ3 * matrixmultZtoX,
                                            outy3 = DX3 * matrixmultXtoY + DY3 * matrixmultYtoY + DZ3 * matrixmultZtoY,
                                            outz3 = DX3 * matrixmultXtoZ + DY3 * matrixmultYtoZ + DZ3 * matrixmultZtoZ;
                                        #endregion
                                        if (outy1 <= 0 || outy2 <= 0 || outy3 <= 0) continue;//checking wether distances are > 0 because you don't want to draw things behind
                                        #region Transformation To Screen
                                        float
                                        x1 = (outx1 / outy1) + W2,//convert to screen coordinates
                                        x2 = (outx2 / outy2) + W2,
                                        x3 = (outx3 / outy3) + W2,//x3 = (outx3 / outy3) * Multiplier + W2,                                        x3 = (outx3 / outy3) * Multiplier + W2,
                                        y1 = (-outz1 / outy1) + H2,
                                        y2 = (-outz2 / outy2) + H2,
                                        y3 = (-outz3 / outy3) + H2,
                                        minx, maxx, miny, maxy;
                                        #endregion
                                        #region Calculate Triangle Properties
                                        if (x1 < x2)
                                        {
                                            minx = x1 < x3 ? x1 : x3;
                                            maxx = x2 > x3 ? x2 : x3;
                                        }
                                        else
                                        {
                                            minx = x2 < x3 ? x2 : x3;
                                            maxx = x1 > x3 ? x1 : x3;
                                        }
                                        if (y1 < y2)
                                        {
                                            miny = y1 < y3 ? y1 : y3;
                                            maxy = y2 > y3 ? y2 : y3;
                                        }
                                        else
                                        {
                                            miny = y2 < y3 ? y2 : y3;
                                            maxy = y1 > y3 ? y1 : y3;
                                        }
                                        #endregion
                                        if ((int)maxx != (int)minx && (int)maxy != (int)miny && maxx >= bl && maxy >= bt && minx <= br && miny <= bb)
                                        {
                                            #region Calculate Colors
                                            //shade
                                            if (shade)
                                            {
                                                #region Calculate Normals
                                                //Vector3 lhs = v2 - v1, rhs = v3 - v1;
                                                float lhsx = DX2 - DX1, lhsy = DY2 - DY1, lhsz = DZ2 - DZ1;
                                                float rhsx = DX3 - DX1, rhsy = DY3 - DY1, rhsz = DZ3 - DZ1;

                                                //Vector3 normal = new Vector3//direction the triangle is facing at
                                                float normalx = lhsy * rhsz - lhsz * rhsy,
                                                normaly = lhsz * rhsx - lhsx * rhsz,
                                                normalz = lhsx * rhsy - lhsy * rhsx;

                                                //Vector3.Cross(v2 - v1, v3 - v1);
                                                //float length = Mathf.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
                                                float length = (float)Math.Sqrt(normalx * normalx + normaly * normaly + normalz * normalz);
                                                //sometimes normal doesn't need to be normalized
                                                normalx /= length;
                                                normaly /= length;
                                                normalz /= length;
                                                float angle = Vector3.Angle(direction, new Vector3(normalx, normaly, normalz));
                                                if (angle < MinDegreesDiff)
                                                {
                                                    continue;//discard;
                                                }
                                                else
                                                {
                                                    //continue;
                                                }
                                                //normal.Normalize();
                                                normalx = (normalx + 1f) * .5f;
                                                #endregion
                                                //Vector3 opposite = -normal;//unblock this to get the opposite direction
                                                red = (byte)(red * Mathf.Lerp(ShadeR, LightR, normalx));
                                                green = (byte)(green * Mathf.Lerp(ShadeG, LightG, normalx));
                                                blue = (byte)(blue * Mathf.Lerp(ShadeB, LightB, normalx));
                                                /*if (normalx < 0)//12 || 23 || 31
                                                {//in shadow
                                                    red = (byte)(red * ShadeR);
                                                    green = (byte)(green * ShadeG);
                                                    blue = (byte)(blue * ShadeB);
                                                }
                                                else
                                                {//not in shadow
                                                    red = (byte)(red * LightR);
                                                    green = (byte)(green * LightG);
                                                    blue = (byte)(blue * LightB);
                                                }*/
                                            }
                                            #endregion
                                            #region TriangleCreation
                                            Tri tri = new Tri();
                                            //line12 = new Line(x1, y1, x2, y2);
                                            if (y1 > y2)//topy = smallest, bottomy biggest(y+h)
                                            {
                                                tri.l12topy = y2;
                                                tri.l12bottomy = y1;
                                            }
                                            else
                                            {
                                                tri.l12topy = y1;
                                                tri.l12bottomy = y2;
                                            }
                                            tri.l12dydx = (y2 - y1) / (x2 - x1);
                                            tri.l12YatX0 = x1 * -tri.l12dydx + y1;
                                            //line23 = new Line(x2, y2, x3, y3);
                                            if (y2 > y3)//topy = smallest, bottomy biggest(y+h)
                                            {
                                                tri.l23topy = y3;
                                                tri.l23bottomy = y2;
                                            }
                                            else
                                            {
                                                tri.l23topy = y2;
                                                tri.l23bottomy = y3;
                                            }
                                            tri.l23dydx = (y3 - y2) / (x3 - x2);
                                            tri.l23YatX0 = x2 * -tri.l23dydx + y2;
                                            //line31 = new Line(x3, y3, x1, y1);
                                            if (y3 > y1)//topy = smallest, bottomy biggest(y+h)
                                            {
                                                tri.l31topy = y1;
                                                tri.l31bottomy = y3;
                                            }
                                            else
                                            {
                                                tri.l31topy = y3;
                                                tri.l31bottomy = y1;
                                            }
                                            tri.l31dydx = (y1 - y3) / (x1 - x3);
                                            tri.l31YatX0 = x3 * -tri.l31dydx + y3;
                                            #endregion
                                            int color = new IntColor { A = 255, R = red, G = green, B = blue }.ToInt;
                                            if (onlyborders)
                                            {
                                                #region Calculate Things Needed to draw Triangles with Only Borders
                                                float xtop12, xtop23, xtop31, xbottom12, xbottom23, xbottom31;
                                                if (y1 < y2)
                                                {
                                                    xtop12 = x1;
                                                    xbottom12 = x2;
                                                }
                                                else
                                                {
                                                    xtop12 = x2;
                                                    xbottom12 = x1;
                                                }
                                                if (y2 < y3)
                                                {
                                                    xtop23 = x2;
                                                    xbottom23 = x3;
                                                }
                                                else
                                                {
                                                    xtop23 = x3;
                                                    xbottom23 = x2;
                                                }
                                                if (y3 < y1)
                                                {
                                                    xtop31 = x3;
                                                    xbottom31 = x1;
                                                }
                                                else
                                                {
                                                    xtop31 = x1;
                                                    xbottom31 = x3;
                                                }
                                                #endregion
                                                graphics.DrawTri(tri, color, DX1 * DX1 + DY1 * DY1 + DZ1 * DZ1, Pclr, xtop12, xbottom12, xtop23, xbottom23, xtop31, xbottom31);
                                            }
                                            else
                                            {
                                                #region Calculate Things Needed to fill Triangles
                                                tri.l31x = x3;
                                                tri.l23x = x2;
                                                tri.l12x = x1;
                                                tri.minx = minx;
                                                tri.miny = miny;
                                                tri.maxx = maxx;
                                                tri.maxy = maxy;
                                                tri.l31W0 = x3 == x1;
                                                tri.l23W0 = x2 == x3;
                                                tri.l12W0 = x1 == x2;
                                                #endregion
                                                graphics.FillTri(tri, color, DX1 * DX1 + DY1 * DY1 + DZ1 * DZ1, Pclr);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public struct Shader
        {
            /// <summary>
            /// if an triangle is shaded, the colors will be multiplied by this.
            /// </summary>
            public float LigtR, LightG, LightB;
            public float ShadeR, ShadeG, ShadeB;
            public bool Enabled;
        }
        public class Mesh
        {
            public Mesh Clone() => new Mesh { Colors = (byte[])Colors.Clone(), triangles = (int[])triangles.Clone(), vertices = (Vector3[])vertices.Clone() };
            //public Shader shader;
            public Vector3[] vertices;
            public int[] triangles;
            public byte[] Colors;
            public void InitializeColors(Color color)
            {
                int l = triangles.Length;
                Colors = new byte[l];
                byte r = color.R;
                byte g = color.G;
                byte b = color.B;
                int i = 0;
                while (i < l)
                {
                    Colors[i++] = r;
                    Colors[i++] = g;
                    Colors[i++] = b;
                }
            }
            public void SetSharedColor(Color color)
            {
                byte r = color.R;
                byte g = color.G;
                byte b = color.B;
                int i = 0;
                while (i < Colors.Length)
                {
                    Colors[i++] = r;
                    Colors[i++] = g;
                    Colors[i++] = b;
                }
            }

            public void Clear()
            {
                triangles = new int[0];
                Colors = new byte[0];
                vertices = new Vector3[0];
            }

            public Mesh(params Mesh[] meshes)
            {
                if (meshes.Length > 0)
                {
                    //edit triangle refs
                    int meshCount = meshes.Length;
                    int vertcount = meshes[0].vertices.Length, tricount = meshes[0].triangles.Length, colorcount = meshes[0].Colors.Length;
                    int[][] trisArr = new int[meshCount][];
                    //Vector3[][] vertsArr = new Vector3[0][];
                    //byte[][] colorsarr = new byte[0][];
                    trisArr[0] = (int[])meshes[0].triangles.Clone();
                    for (int i = 1; i < meshCount; i++)
                    {
                        Mesh m = meshes[i];
                        int[] trs = (int[])m.triangles.Clone();
                        int trilength = trs.Length;
                        for (int j = 0; j < trilength; j++)
                        {
                            trs[j] += vertcount;
                        }
                        vertcount += m.vertices.Length;
                        tricount += m.triangles.Length;
                        colorcount += m.Colors.Length;
                        trisArr[i] = trs;
                    }
                    //count will be the total count of vertices
                    //tri references corrected
                    //datashift
                    Vector3[] verts = new Vector3[vertcount];
                    byte[] colors = new byte[colorcount];
                    int[] tris = new int[tricount];
                    int byteoffset = 0, vertoffset = 0, trioffset = 0;
                    for (int i = 0; i < meshCount; i++)
                    {
                        Mesh m = meshes[i];
                        int[] trs = trisArr[i];
                        int trilength = trs.Length;
                        Buffer.BlockCopy(m.Colors, 0, colors, byteoffset, m.Colors.Length);
                        Generic.BlockCopy(m.vertices, 0, verts, vertoffset, m.vertices.Length);
                        Generic.BlockCopy(m.triangles, 0, tris, trioffset, m.triangles.Length);
                        byteoffset += m.Colors.Length;
                        vertoffset += m.vertices.Length;
                        trioffset += m.triangles.Length;
                    }
                    triangles = tris;
                    this.Colors = colors;
                    vertices = verts;
                }
                else Clear();
            }
            public Mesh()
            {
                vertices = new Vector3[0];
                triangles = new int[0];
                Colors = new byte[0];
            }
            public Mesh(int capacity)
            {
                vertices = new Vector3[capacity * 3];
                triangles = new int[capacity * 3];
                Colors = new byte[capacity * 3];
            }
            public Mesh(Vector3[] vectors, int[] triangles, byte[] colors)
            {
                vertices = vectors;
                this.triangles = triangles;
                Colors = colors;
            }
        }
        public struct Vector2
        {
            public static Vector2
                up = new Vector2(0, 1),
                down = new Vector2(0, -1),
                left = new Vector2(-1, 0),
                right = new Vector2(1, 0);
            public override string ToString()
            {
                return "X = " + x.ToString() + " Y = " + y.ToString();
            }
            public float x, y;

            public Vector2(float x, float y)
            {
                this.x = x;
                this.y = y;
            }
            public static Vector2 operator *(Vector2 v, float scale) => new Vector2 { x = v.x * scale, y = v.y * scale };
            public static Vector2 operator /(Vector2 v, float scale) => new Vector2 { x = v.x / scale, y = v.y / scale };
            public static Vector2 operator +(Vector2 v1, Vector2 v2) => new Vector2 { x = v1.x + v2.x, y = v1.y + v2.y };
            public static Vector2 operator -(Vector2 v1, Vector2 v2) => new Vector2 { x = v1.x - v2.x, y = v1.y - v2.y };
            public float Length => (float)Math.Sqrt(x * x + y * y);
            public void Normalize() => this /= Length;
#pragma warning disable IDE1006 // Naming Styles
            public Vector2 normalized => this / Length;
#pragma warning restore IDE1006 // Naming Styles
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct Vector3
        {
            public static Vector3 FromStream(BinaryReader reader) => new Vector3 { x = reader.ReadSingle(), y = reader.ReadSingle(), z = reader.ReadSingle() };
            public void WriteToStream(BinaryWriter writer)
            {
                writer.Write(x);
                writer.Write(y);
                writer.Write(z);
            }
            public static Vector3
                up = new Vector3(0, 1, 0),
                down = new Vector3(0, -1, 0),
                left = new Vector3(-1, 0, 0),
                right = new Vector3(1, 0, 0),
                forward = new Vector3(0, 0, 1),
                back = new Vector3(0, 0, -1);
            public override string ToString()
            {
                return "X = " + x.ToString() + " Y = " + y.ToString() + " Z = " + z.ToString();
            }
            public static float Angle(Vector3 v1, Vector3 v2)
            {
                Vector3 cross = Cross(v1, v2);
                return (float)Math.Atan2(cross.Length, Dot(v1, v2));
            }
            [FieldOffset(0)]
            public float x;
            [FieldOffset(4)]
            public float y;
            [FieldOffset(8)]
            public float z;
            public static Vector3 Lerp(Vector3 a, Vector3 b, float w) => new Vector3 { x = a.x + (b.x - a.x) * w, y = a.y + (b.y - a.y) * w, z = a.z + (b.z - a.z) * w };
            public static Vector3 Reflection(Vector3 Light, Vector3 mirrorDirection) => ReflectionNormalized(Light, mirrorDirection.normalized);
            /// <summary>
            /// calculates an mirrored vector
            /// </summary>
            /// <param name="Light"></param>
            /// <param name="mirrorDirection">must be normalized</param>
            /// <returns></returns>
            public static Vector3 ReflectionNormalized(Vector3 Light, Vector3 mirrorDirection)
            {
                //https://math.stackexchange.com/questions/13261/how-to-get-a-reflection-vector
                return Light - 2 * Dot(Light, mirrorDirection) * mirrorDirection;
            }
            public Vector3 Significance => new Vector3
            {
                x = x > 0 ? 1 : x < 0 ? -1 : 0,
                y = y > 0 ? 1 : y < 0 ? -1 : 0,
                z = z > 0 ? 1 : z < 0 ? -1 : 0
            };
            public Vector3(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            public Vector3(float x, float y)
            {
                this.x = x;
                this.y = y;
                z = 0;
            }
            public Vector3(float z)
            {
                x = 0;
                y = 0;
                this.z = z;
            }
            public float this[int index]
            {
                get => index == 0 ? x : index == 1 ? y : z;
                set
                {
                    switch (index)
                    {
                        case 0: x = value; return;
                        case 1: y = value; return;
                        case 2: z = value; return;
                    }
                }
            }
            public static bool operator ==(Vector3 v1, Vector3 v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
            public static bool operator !=(Vector3 v1, Vector3 v2) => v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
            public static Vector3 operator *(Vector3 v, float scale) => new Vector3 { x = v.x * scale, y = v.y * scale, z = v.z * scale };
            public static Vector3 operator *(float scale, Vector3 v) => new Vector3 { x = v.x * scale, y = v.y * scale, z = v.z * scale };
            public static Vector3 operator /(Vector3 v, float scale) => new Vector3 { x = v.x / scale, y = v.y / scale, z = v.z / scale };
            public static Vector3 operator +(Vector3 v1, Vector3 v2) => new Vector3 { x = v1.x + v2.x, y = v1.y + v2.y, z = v1.z + v2.z };
            public static Vector3 operator +(Vector3 v1, float additional) => new Vector3 { x = v1.x + additional, y = v1.y + additional, z = v1.z + additional };
            public static Vector3 operator -(Vector3 v1, Vector3 v2) => new Vector3 { x = v1.x - v2.x, y = v1.y - v2.y, z = v1.z - v2.z };
            public static Vector3 operator -(Vector3 v) => new Vector3 { x = -v.x, y = -v.y, z = -v.z };
            public float Length => (float)Math.Sqrt(x * x + y * y + z * z);
            public float LengthSquared => x * x + y * y + z * z;
            public void Normalize() => this /= Length;
            // projection of vector v1 onto v2
            public static Vector3 Projection(Vector3 v1, Vector3 v2)
            {
                //squared length of v2
                float v2_ls = v2.x * v2.x + v2.y * v2.y + v2.z * v2.z;
                float dot = v2.x * v1.x + v2.y * v1.y + v2.z * v1.z;
                return v2 * dot / v2_ls;
            }
            /// <summary>
            /// Calculate the dot (scalar) product of two vectors
            /// </summary>
            /// <param name="left">First operand</param>
            /// <param name="right">Second operand</param>
            /// <returns>The dot product of the two inputs</returns>
            public static float Dot(Vector3 left, Vector3 right)
            {

                return left.x * right.x + left.y * right.y + left.z * right.z;
            }
            //https://gamedev.stackexchange.com/questions/20815/most-efficient-bounding-sphere-vs-ray-collision-algorithms
            public static int FindIntersections(Vector3 Centre, float radius, Vector3 Startpoint, Vector3 Direction, out Vector3 collisionPoint1, out Vector3 collisionPoint2)
            {
                Vector3 v;
                Vector3 CA = Startpoint - Centre;
                float rSquared = radius * radius;
                float vSquared;
                if (Dot(CA, CA) <= rSquared)
                {
                    collisionPoint1 = Startpoint;
                    collisionPoint2 = new Vector3();
                    return 1;
                }
                else
                {
                    Direction.Normalize();
                    if (Dot(CA, Direction) <= 0)
                    {
                        v = CA - Projection(CA, Direction);
                        vSquared = v.Length;
                        if (vSquared <= rSquared)
                        {
                            Vector3 cv = Centre + v;
                            Vector3 difference = Direction.normalized * Mathf.Sqrt(rSquared - vSquared);
                            collisionPoint1 = cv - difference;
                            collisionPoint2 = cv + difference;
                            return vSquared == rSquared ? 1 : 2;
                        }
                        else
                        {
                            collisionPoint1 = new Vector3();
                            collisionPoint2 = new Vector3();
                            return 0;
                        }
                    }
                    else
                    {
                        collisionPoint1 = new Vector3();
                        collisionPoint2 = new Vector3();
                        return 0;
                    }
                }
            }
#pragma warning disable IDE1006 // Naming Styles
            public Vector3 normalized
#pragma warning restore IDE1006 // Naming Styles
            {
                get
                {
                    float scale = (float)Math.Sqrt(x * x + y * y + z * z);
                    return new Vector3
                    {
                        x = x / scale,
                        y = y / scale,
                        z = z / scale
                    };
                }
            }
            public Vector3 Floor => new Vector3(Mathf.Floor(x), Mathf.Floor(y), Mathf.Floor(z));
            public float AxisSum => x + y + z;
            public float AbsoluteAxisSum => x < 0 ? -x : x + y < 0 ? -y : y + z < 0 ? -z : z;
            public Vector3 Absolute => new Vector3 { x = (x < 0) ? -x : x, y = (y < 0) ? -y : y, z = (z < 0) ? -z : z };
            /// <summary>
            /// Calculates the cross product of two vectors.
            /// </summary>
            /// <param name="lhs">vector 1</param>
            /// <param name="rhs">vector 2</param>
            /// <returns></returns>
            public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
            {

                return new Vector3
                {
                    x = lhs.y * rhs.z - lhs.z * rhs.y,
                    y = lhs.z * rhs.x - lhs.x * rhs.z,
                    z = lhs.x * rhs.y - lhs.y * rhs.x
                };

            }
            public static float Distance(Vector3 p1, Vector3 p2)
                => Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y) + (p1.z - p2.z) * (p1.z - p2.z));
            public static float DistanceSquared(Vector3 p1, Vector3 p2)
                => (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y) + (p1.z - p2.z) * (p1.z - p2.z);
            public static Vector3 Nearest(Vector3 point, params Vector3[] others)
            {
                Vector3 nearest = others[0];
                float d = DistanceSquared(point, nearest);
                int l = others.Length;
                for (int i = 0; i < l; i++)
                {
                    float currentdistance = DistanceSquared(point, others[i]);
                    if (currentdistance < d)
                    {
                        d = currentdistance;
                        nearest = others[i];
                    }
                }
                return nearest;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Vector3))
                {
                    return false;
                }

                var vector = (Vector3)obj;
                return x == vector.x &&
                       y == vector.y &&
                       z == vector.z;
            }

            public override int GetHashCode()
            {
                var hashCode = 373119288;
                hashCode = hashCode * -1521134295 + x.GetHashCode();
                hashCode = hashCode * -1521134295 + y.GetHashCode();
                hashCode = hashCode * -1521134295 + z.GetHashCode();
                return hashCode;
            }
        }
        public class BitmapLocker : IDisposable
        {
            public long elapsed;
            public int Bwidth, Bheight;
            public int[] colours;
            Stopwatch stopwatch = new Stopwatch();
            public bool Locked { get; private set; }
            public BitmapLocker(int W, int H)
            {
                Bwidth = W;
                Bheight = H;
                colours = new int[W * H];
            }
            public BitmapSource Unlock()
            {
                if (Locked)
                {
                    //Marshal.Copy(colours, 0, data.Scan0, colours.Length);
                    //bitmap.UnlockBits(data);
                    Locked = false;
                    elapsed = stopwatch.ElapsedMilliseconds;
                    PixelFormat pf = PixelFormats.Bgr32;
                    //int is 32bits
                    //const int BitsPerInt = 32;
                    int rawStride = (Bwidth * pf.BitsPerPixel + 7) / 8;
                    //byte[] rawImage = new byte[rawStride * Bheight];

                    // Initialize the image with data.
                    //Random value = new Random();
                    //value.NextBytes(rawImage);

                    // Create a BitmapSource.
                    return BitmapSource.Create(Bwidth, Bheight,
                        96, 96, pf, null,
                        colours, rawStride);
                }
                else throw new Exception("This graphics is not locked");
            }
            public void Lock()
            {
                if (!Locked)
                {
                    stopwatch = Stopwatch.StartNew();
                    Locked = true;
                }
                else throw new Exception("This graphics is already locked");
            }
            public void Dispose() => colours = null;
        }
        public sealed class TriangleDrawer : BitmapLocker
        {
            float[] Distances;
            public TriangleDrawer(int W, int H) : base(W, H)
            {
                _bounds = new Rectangle(0, 0, W, H);
                //Distances = Enumerable.Repeat(float.PositiveInfinity, bitmap.Width * bitmap.Height).ToArray();
                #region Generate Float Array Filled With Inf
                int l = W * H;
                Distances = new float[l];
                unsafe
                {
                    fixed (float* d = Distances)
                    { //6 times *(d+i++) = float.PositiveInfinity: 27 ms
                        float* pointer = d;
                        float* end = d + l;
                        float* end6 = end - 6;
                        while (pointer < end6)
                        {
                            *pointer++ = float.PositiveInfinity;
                            *pointer++ = float.PositiveInfinity;
                            *pointer++ = float.PositiveInfinity;
                            *pointer++ = float.PositiveInfinity;
                            *pointer++ = float.PositiveInfinity;
                            *pointer++ = float.PositiveInfinity;
                        }
                        while (pointer < end)
                        {
                            *pointer++ = float.PositiveInfinity;
                        }
                    }
                }
                //System.Runtime.CompilerServices.Unsafe.InitBlock(ref a[0], valueToFill, (uint)a.Length);
                #endregion
            }
            public void Clear(IntColor color)
            {
                int c = color.ToInt;
                unsafe
                {
                    fixed (int* p = colours)
                    {
                        int* pointer = p;
                        int* endpointer = p + colours.Length;
                        while (pointer < endpointer)
                        {
                            *pointer++ = c;
                        }
                    }
                }
            }
            public Rectangle Bounds
            {
                get => _bounds;
                set => _bounds = value;
            }
            Rectangle _bounds = new Rectangle();
            public unsafe void FillTri(Tri tri, int color, float distance, int* colours)
            {
                fixed (float* DistancePointer = Distances)
                {
                    //float[] Distances = this.Distances;
                    Rectangle bounds = _bounds;
                    int bl = bounds.x;
                    int br = bounds.Right;
                    int bt = bounds.y;
                    int bb = bounds.Bottom;
                    int imageWidth = Bwidth;
                    //if (tri.minx == float.PositiveInfinity || tri.miny == float.PositiveInfinity) return;
                    float btop = bl > tri.miny ? bl : tri.miny;
                    float bbottom = bb < tri.maxy ? bb : tri.maxy;
                    //Line l12 = tri.line12, l23 = tri.line23, l31 = tri.line31;
                    for (int j = (int)btop; j < bbottom; j++)
                    {
                        float mostleft = tri.maxx, mostright = tri.minx;
                        if (j >= tri.l12topy && j < tri.l12bottomy)
                        {
                            float intersectx = tri.l12W0 ? tri.l12x : (j - tri.l12YatX0) / tri.l12dydx;//(y-b)/a
                            mostleft = intersectx;
                        }
                        if (j >= tri.l23topy && j < tri.l23bottomy)
                        {
                            float intersectx = tri.l23W0 ? tri.l23x : (j - tri.l23YatX0) / tri.l23dydx;//(y-b)/a
                            if (intersectx < mostleft)
                            {
                                mostright = mostleft;
                                mostleft = intersectx;
                            }
                            else
                                mostright = intersectx;
                        }
                        if (j >= tri.l31topy && j < tri.l31bottomy)
                        {
                            float intersectx = tri.l31W0 ? tri.l31x : (j - tri.l31YatX0) / tri.l31dydx;//(y-b)/a
                            if (intersectx < mostleft)
                            {
                                mostright = mostleft;
                                mostleft = intersectx;
                            }
                            else mostright = intersectx;
                        }

                        int bleft = bl > mostleft ? bl : (int)mostleft;
                        int bright = br < mostright ? br : (int)mostright;
                        if (bleft < bright)
                        {
                            //int index = (j * w + (int)bleft) * 4;

                            int EndPixelIndex = j * imageWidth + bright;
                            //unsafe
                            //{
                            int PixelIndex = j * imageWidth + bleft;
                            float* ptr = (DistancePointer + PixelIndex);
                            int* colourptr = (colours + PixelIndex);
                            float* endptr = (DistancePointer + EndPixelIndex);
                            do
                            {
                                if (*ptr > distance)
                                {
                                    *colourptr = color;
                                    *ptr = distance;
                                }
                                colourptr++;
                            }
                            while (++ptr < endptr);
                        }
                    }
                }
            }
            public void DrawImage(BitmapSource Image, int opacity)
            {
                int W = (int)Image.Width,
                    H = (int)Image.Height;

                int[] ImgColours = new int[W * H];
                Image.CopyPixels(ImgColours, W * 4, 0);
                const float MinimalOpacity = 1f / 255;
                unsafe
                {
                    fixed (int* img = ImgColours)
                    {
                        if (opacity == 255)
                            DrawImage(img, W, H);
                        else if (opacity < MinimalOpacity) return;
                        else DrawImage(img, W, H, opacity);
                    }
                }
            }
            public unsafe void DrawImage(int* image, int ImageWidth, int ImageHeight, int opacity)
            {
                int imagemultiplier = 255 - opacity;
                IntColor c1 = new IntColor(), c2 = new IntColor();
                fixed (int* colours = this.colours)
                {
                    int Awidth = Bwidth;
                    int* begin = colours;
                    for (int j = 0; j < ImageHeight; j++)
                    {
                        int* end = begin + ImageWidth - 1;
                        int* end6 = end - 6;
                        int* pointer = begin - 1;
                        while (pointer < end6)
                        {
                            c1.ToInt = *++pointer;
                            c2.ToInt = *image++;
                            *pointer = new IntColor { A = (byte)((c1.A * imagemultiplier + c2.A * opacity) / 255), R = (byte)((c1.R * imagemultiplier + c2.R * opacity) / 255), G = (byte)((c1.G * imagemultiplier + c2.G * opacity) / 255), B = (byte)((c1.B * imagemultiplier + c2.B * opacity) / 255) }.ToInt;
                            c1.ToInt = *++pointer;
                            c2.ToInt = *image++;
                            *pointer = new IntColor { A = (byte)((c1.A * imagemultiplier + c2.A * opacity) / 255), R = (byte)((c1.R * imagemultiplier + c2.R * opacity) / 255), G = (byte)((c1.G * imagemultiplier + c2.G * opacity) / 255), B = (byte)((c1.B * imagemultiplier + c2.B * opacity) / 255) }.ToInt;
                            c1.ToInt = *++pointer;
                            c2.ToInt = *image++;
                            *pointer = new IntColor { A = (byte)((c1.A * imagemultiplier + c2.A * opacity) / 255), R = (byte)((c1.R * imagemultiplier + c2.R * opacity) / 255), G = (byte)((c1.G * imagemultiplier + c2.G * opacity) / 255), B = (byte)((c1.B * imagemultiplier + c2.B * opacity) / 255) }.ToInt;
                            c1.ToInt = *++pointer;
                            c2.ToInt = *image++;
                            *pointer = new IntColor { A = (byte)((c1.A * imagemultiplier + c2.A * opacity) / 255), R = (byte)((c1.R * imagemultiplier + c2.R * opacity) / 255), G = (byte)((c1.G * imagemultiplier + c2.G * opacity) / 255), B = (byte)((c1.B * imagemultiplier + c2.B * opacity) / 255) }.ToInt;
                            c1.ToInt = *++pointer;
                            c2.ToInt = *image++;
                            *pointer = new IntColor { A = (byte)((c1.A * imagemultiplier + c2.A * opacity) / 255), R = (byte)((c1.R * imagemultiplier + c2.R * opacity) / 255), G = (byte)((c1.G * imagemultiplier + c2.G * opacity) / 255), B = (byte)((c1.B * imagemultiplier + c2.B * opacity) / 255) }.ToInt;
                            c1.ToInt = *++pointer;
                            c2.ToInt = *image++;
                            *pointer = new IntColor { A = (byte)((c1.A * imagemultiplier + c2.A * opacity) / 255), R = (byte)((c1.R * imagemultiplier + c2.R * opacity) / 255), G = (byte)((c1.G * imagemultiplier + c2.G * opacity) / 255), B = (byte)((c1.B * imagemultiplier + c2.B * opacity) / 255) }.ToInt;
                            //*++pointer = IntColor.Scale(new IntColor { ToInt = *pointer }, new IntColor { ToInt = *image++ }, imagemultiplier, opacity).ToInt;
                            //*++pointer = *image++ * opacity + *pointer * imagemultiplier;
                        }
                        while (pointer < end)
                        {
                            c1.ToInt = *++pointer;
                            c2.ToInt = *image++;
                            *pointer = new IntColor { A = (byte)((c1.A * imagemultiplier + c2.A * opacity) / 255), R = (byte)((c1.R * imagemultiplier + c2.R * opacity) / 255), G = (byte)((c1.G * imagemultiplier + c2.G * opacity) / 255), B = (byte)((c1.B * imagemultiplier + c2.B * opacity) / 255) }.ToInt;
                        }
                        begin += Bwidth;
                    }
                }
            }
            public unsafe void DrawImage(int* image, int ImageWidth, int ImageHeight)
            {
                image--;
                fixed (int* colours = this.colours)
                {
                    int Awidth = Bwidth;
                    int* begin = colours;
                    for (int j = 0; j < ImageHeight; j++)
                    {
                        int* end = begin + ImageWidth;
                        int* end6 = end - 6;
                        int* pointer = begin;
                        while (pointer < end6)
                        {
                            if (*++image != -1) *pointer = *image;
                            pointer++;
                            if (*++image != -1) *pointer = *image;
                            pointer++;
                            if (*++image != -1) *pointer = *image;
                            pointer++;
                            if (*++image != -1) *pointer = *image;
                            pointer++;
                            if (*++image != -1) *pointer = *image;
                            pointer++;
                            if (*++image != -1) *pointer = *image;
                            pointer++;
                        }
                        while (pointer < end)
                        {
                            if (*++image != -1) *pointer = *image;
                            pointer++;
                        }
                        begin += Bwidth;
                    }
                }
            }
            public unsafe void DrawTri(Tri tri, int color, float distance, int* colours, float xtop12, float xbottom12, float xtop23, float xbottom23, float xtop31, float xbottom31)
            {
                Rectangle bounds = _bounds;
                int bl = bounds.x;
                int br = bounds.Right;
                int bt = bounds.y;
                int bb = bounds.Bottom;
                fixed (float* Distances = this.Distances)
                {
                    DrawLine(tri.l12topy, tri.l12bottomy, tri.l12YatX0, tri.l12dydx, distance, color, bl, br, bt, bb, Distances, colours, Bwidth, xtop12, xbottom12);
                    DrawLine(tri.l23topy, tri.l23bottomy, tri.l23YatX0, tri.l23dydx, distance, color, bl, br, bt, bb, Distances, colours, Bwidth, xtop23, xbottom23);
                    DrawLine(tri.l31topy, tri.l31bottomy, tri.l31YatX0, tri.l31dydx, distance, color, bl, br, bt, bb, Distances, colours, Bwidth, xtop31, xbottom31);
                }
            }
            public void DrawFlatLine(int y, float x1, float x2, int bl, int br, int bt, int bb, float[] Distances, int[] colours, int Bwidth, int color, float distance)
            {
                if (y < 0 || y >= bb) return;
                int left;
                int right;
                if (x1 < x2)
                {
                    left = (int)x1;
                    right = (int)x2;
                }
                else
                {
                    left = (int)x2;
                    right = (int)x1;
                }
                left = left < bl ? bl : left;
                right = right > br ? br : right;
                int EndPixelIndex = y * Bwidth + right;
                for (int PixelIndex = y * Bwidth + left; PixelIndex < EndPixelIndex; PixelIndex++)
                {
                    if (Distances[PixelIndex] <= distance) continue;//if d == 0 || d > distance
                                                                    //int index = PixelIndex * 4;
                                                                    //draw color
                    colours[PixelIndex] = color;
                    Distances[PixelIndex] = distance;
                }

            }
            public void DrawLine(float topy, float bottomy, bool w0, float YatX0, float dydx, float distance, int color, float lineStraightX)
            {
                Rectangle bounds = _bounds;
                int bl = bounds.x;
                int br = bounds.Right;
                int bt = bounds.y;
                int bb = bounds.Bottom;
                DrawLineOld(topy, bottomy, w0, YatX0, dydx, distance, color, lineStraightX, bl, br, bt, bb, this.Distances, colours, Bwidth);
            }

            public static unsafe void DrawLine(float topy, float bottomy, float YatX0, float dydx, float distance, int color, int bl, int br, int bt, int bb, float* Distances, int* colours, int Bwidth, float xtop, float xbottom)
            {
                bool w0 = xtop == xbottom;
                int itopy = (int)topy, bottom = bottomy < 0 ? (int)bottomy - 1 : (int)bottomy;
                int top = topy % 1 == 0 || itopy == bottom ? (int)topy : (int)topy + 1;
                top = top < bt ? bt : top;
                bottom = bottom > bb - 1 ? bb - 1 : bottom;
                int lastx = (topy < bt) ?
                    w0 ? (int)xtop : (int)(-YatX0 / dydx) :
                    (int)xtop;//(y-b)/a
                bool downwards = dydx == 0 ? xtop > xbottom : dydx < 0;
                for (int j = top; j <= bottom; j++)
                {
                    float intersectx = (j == bottom) ? xbottom : w0 ? xtop : (j - YatX0) / dydx;//(y-b)/a
                    //float intersectx = w0 ? lineStraightX : (j - YatX0) / dydx;
                    int ix = (int)intersectx;
                    int minx, maxx;
                    //continue;
                    if (lastx > ix)//downwards)//if the line is downwards, ix>lastx
                    {
                        minx = ix;
                        maxx = lastx + 1;
                    }
                    else
                    {
                        minx = lastx;
                        maxx = ix + 1;
                    }
                    minx = minx < bl ? bl : minx;
                    maxx = maxx > br ? br : maxx;
                    int EndPixelIndex = j * Bwidth + maxx;
                    for (int PixelIndex = j * Bwidth + minx; PixelIndex < EndPixelIndex; PixelIndex++)
                    {
                        if (Distances[PixelIndex] <= distance) continue;//if d == 0 || d > distance
                        //int index = PixelIndex * 4;
                        //draw color
                        colours[PixelIndex] = color;
                        Distances[PixelIndex] = distance;
                    }
                    lastx = ix;
                }
            }

            public static void DrawLineOld(float topy, float bottomy, bool w0, float YatX0, float dydx, float distance, int color, float lineStraightX, int bl, int br, int bt, int bb, float[] Distances, int[] colours, int Bwidth)
            {
                //float[] Distances = this.Distances;
                //yield break;
                int top = (int)topy,
                    bottom = (int)bottomy;
                top = top < bt ? bt : top;
                bottom = bottom > bb ? bb : bottom;
                short lastx = (short)(w0 ? lineStraightX : (top - YatX0) / dydx);//(y-b)/a
                bool downwards = dydx < 0;
                for (int j = top; j < bottom; j++)
                {
                    float intersectx = w0 ? lineStraightX : (j - YatX0) / dydx;//(y-b)/a
                    short ix = (short)intersectx;
                    int minx, maxx;
                    //continue;
                    if (downwards)//if the line is downwards, ix>lastx
                    {
                        minx = ix;
                        maxx = lastx + 1;
                    }
                    else
                    {
                        minx = lastx;
                        maxx = ix + 1;
                    }
                    maxx = maxx > br ? br : maxx;
                    minx = minx < bl ? bl : minx;
                    //int index = (j * w + (int)bleft) * 4;
                    int EndPixelIndex = j * Bwidth + maxx;
                    for (int PixelIndex = j * Bwidth + minx; PixelIndex < EndPixelIndex; PixelIndex++)
                    {
                        if (Distances[PixelIndex] <= distance) continue;//if d == 0 || d > distance
                        //int index = PixelIndex * 4;
                        //draw color
                        colours[PixelIndex] = color;
                        Distances[PixelIndex] = distance;
                    }
                    lastx = ix;
                }
            }
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct Tri
        {
            //public float x1, x2, x3;
            //public float y1, y2, y3;
            [FieldOffset(0)]
            public float l12topy;
            [FieldOffset(4)]
            public float l12bottomy;
            [FieldOffset(8)]
            public bool l12W0;
            [FieldOffset(9)]
            public float l12dydx;
            [FieldOffset(13)]
            public float l12YatX0;
            [FieldOffset(17)]
            public float l12x;
            [FieldOffset(21)]
            public float l23topy;
            [FieldOffset(25)]
            public float l23bottomy;
            [FieldOffset(29)]
            public bool l23W0;
            [FieldOffset(30)]
            public float l23dydx;
            [FieldOffset(34)]
            public float l23YatX0;
            [FieldOffset(38)]
            public float l23x;
            [FieldOffset(42)]
            public float l31topy;
            [FieldOffset(46)]
            public float l31bottomy;
            [FieldOffset(50)]
            public bool l31W0;
            [FieldOffset(51)]
            public float l31dydx;
            [FieldOffset(55)]
            public float l31YatX0;
            [FieldOffset(59)]
            public float l31x;
            [FieldOffset(63)]
            public float minx;
            [FieldOffset(67)]
            public float miny;
            [FieldOffset(71)]
            public float maxx;
            [FieldOffset(75)]
            public float maxy;
            public Tri(float x1, float y1, float x2, float y2, float x3, float y3)
            {
                //line12 = new Line(x1, y1, x2, y2);
                if (y1 > y2)//topy = smallest, bottomy biggest(y+h)
                {
                    l12topy = y2;
                    l12bottomy = y1;
                }
                else
                {
                    l12topy = y1;
                    l12bottomy = y2;
                }
                l12W0 = x1 == x2;
                l12x = x1;
                if (x1 < x2)
                {
                    l12dydx = (y2 - y1) / (x2 - x1);
                    l12YatX0 = x1 * -l12dydx + y1;
                }
                else
                {
                    l12dydx = (y1 - y2) / (x1 - x2);
                    l12YatX0 = x2 * -l12dydx + y2;
                }
                //line23 = new Line(x2, y2, x3, y3);
                if (y2 > y3)//topy = smallest, bottomy biggest(y+h)
                {
                    l23topy = y3;
                    l23bottomy = y2;
                }
                else
                {
                    l23topy = y2;
                    l23bottomy = y3;
                }
                l23W0 = x2 == x3;
                l23x = x2;
                if (x2 < x3)
                {
                    l23dydx = (y3 - y2) / (x3 - x2);
                    l23YatX0 = x2 * -l23dydx + y2;
                }
                else
                {
                    l23dydx = (y2 - y3) / (x2 - x3);
                    l23YatX0 = x3 * -l23dydx + y3;
                }
                //line31 = new Line(x3, y3, x1, y1);
                if (y3 > y1)//topy = smallest, bottomy biggest(y+h)
                {
                    l31topy = y1;
                    l31bottomy = y3;
                }
                else
                {
                    l31topy = y3;
                    l31bottomy = y1;
                }
                l31W0 = x3 == x1;
                l31x = x3;
                if (x3 < x1)
                {
                    l31dydx = (y1 - y3) / (x1 - x3);
                    l31YatX0 = x3 * -l31dydx + y3;
                }
                else
                {
                    l31dydx = (y3 - y1) / (x3 - x1);
                    l31YatX0 = x1 * -l31dydx + y1;
                }

                minx =
                (x1 < x2) ?
                    x1 < x3 ?
                        x1 :
                        x3 :
                    x2 < x3 ?
                        x2 :
                        x3;
                miny =
                (y1 < y2) ?
                    y1 < y3 ?
                        y1 :
                        y3 :
                    y2 < y3 ?
                        y2 :
                        y3;
                maxx =
                (x1 > x2) ?
                    x1 > x3 ?
                        x1 :
                        x3 :
                    x2 > x3 ?
                        x2 :
                        x3;
                maxy =
                (y1 > y2) ?
                    y1 > y3 ?
                        y1 :
                        y3 :
                    y2 > y3 ?
                        y2 :
                        y3;
            }
        }
        public struct Line
        {
            //public float x1, x2;
            //public float y1, y2;
            public float topy, bottomy;
            //public float leftx, lefty, rightx, righty;
            public bool W0;
            public float dydx, YatX0, x;
            public Line(float x1, float y1, float x2, float y2)
            {
                if (y1 > y2)//topy = smallest, bottomy biggest(y+h)
                {
                    topy = y2;
                    bottomy = y1;
                }
                else
                {
                    topy = y1;
                    bottomy = y2;
                }
                W0 = x1 == x2;
                x = x1;
                if (x1 < x2)
                {
                    //leftx = x1;
                    //lefty = y1;
                    //rightx = x2;
                    //righty = y2;
                    dydx = (y2 - y1) / (x2 - x1);
                    YatX0 = x1 * -dydx + y1;
                }
                else
                {
                    //leftx = x2;
                    //lefty = y2;
                    //rightx = x1;
                    //righty = y1;
                    dydx = (y1 - y2) / (x1 - x2);
                    YatX0 = x2 * -dydx + y2;
                }
            }
        }
    }
}