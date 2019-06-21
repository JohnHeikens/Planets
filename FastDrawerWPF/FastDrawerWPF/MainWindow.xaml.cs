using openview;
using openview.OpenGL;
using gl = openview.OpenGL;
using Planet3D;
using sys = System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Threading;
/*using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Molecules;
using System.Drawing;*/

namespace FastDrawerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int seed = 1;
        public MainWindow()
        {
            float f = DateAndTime.Timer;
            InitializeComponent();
        }
        List<Planet> planets;
        bool DrawOnlyBorders = false, MotionBlur = false;
        gl.Transform transform = new gl.Transform(new Vector3());
        Vector3 speed;
        public const int ObjCount = 100;
        void Terrain()
        {
            Random random = new Random(seed);
            Planet[] planets = new Planet[ObjCount];
            for (int i = 0; i < ObjCount; i++)
            {
                int planetseed = random.Next();
                Planet planet;
                float r = random.NextFloat() * 20 + 1;
                float mult = random.NextFloat();
                mult = (1 - mult * mult);
                float mDiff = mult * Mathf.Sqrt(r);//if the radius is bigger, the planet gains more detail, more roughness, same height difference
                float roughness = mult * r * 0.25f;
                Vector3 pos = random.NextPoint() * 0x100;
                planet = new Planet
                {
                    Speed = random.NextVector() * .1f,
                    color = NormalColor.White,
                    shapeSettings = new ShapeSettings
                    {
                        position = pos * 0x10,
                        PlanetRadius = r,
                        MaxDifference = mDiff,
                        NoiseSettings = new NoiseSettings
                        {
                            centre = pos,
                            BaseRougness = roughness,
                            numlayers = 5,
                            roughness = 2f,
                            filterType = (NoiseSettings.FilterType)random.Next(2)
                        }
                    }
                };
                float h = random.NextFloat();
                h *= h;
                planet.shapeSettings.NoiseSettings.settings = new ElevationSettings
                {
                    WaterHeight = h,
                    WaterColor = random.NextColor()
                };
                int layercount = random.Next(10) + 1;//1 to 10
                Elevation[] elevations = new Elevation[layercount];
                for (int j = 0; j < layercount - 1; j++)
                {
                    int ToGo = layercount - j;
                    float f = random.NextFloat();
                    f = Mathf.Sqrt(f);
                    h = Mathf.Lerp(h, 1, f / ToGo);
                    elevations[j] = new Elevation(random.NextColor(), h);
            }
                elevations[layercount - 1] = new Elevation(random.NextColor(), 1);
                planet.shapeSettings.NoiseSettings.settings.Elevations = new List<Elevation>(elevations);
                planet.Initialize();
                planets[i] = planet;
            }
            this.planets = new List<Planet>(planets);
        }
        public bool Shade = true;
        int precision = 5;
        BitmapSource cockpit = new BitmapImage(new sys.Uri(sys.AppDomain.CurrentDomain.BaseDirectory + "cockpit.bmp"));
        BitmapSource LastDraw;
        bool DrawCockpit;
        float zoom = 1;
        void Draw()
        {
            Shader shader = Shade ?
                new Shader { ShadeB = 0.05f, ShadeG = 0.03f, ShadeR = 0.03f, LightB = 1f, LightG = 1f, LigtR = 1f, Enabled = true }
            : new Shader();
            int w = (int)SystemParameters.PrimaryScreenWidth;
            int h = (int)SystemParameters.PrimaryScreenHeight;
            using (TriangleDrawer g = new TriangleDrawer(w, h))
            {
                openview.Size size = new openview.Size(w, h);
                g.Lock();
                gl.Matrix matrix = transform.Matrix;
                for (int i = 0; i < planets.Count; i++)
                {
                    ShapeSettings shapeSettings = planets[i].shapeSettings;
                    Vector3 v = matrix.ApplyTo(shapeSettings.position - transform.position);
                    float x = v.x, y = v.y, z = v.z;
                    float r = shapeSettings.PlanetRadius + shapeSettings.MaxDifference;
                    y += r;
                    bool xCorrect = x > 0 ? x - r < y: x + r > -y;
                    bool zCorrect = z > 0 ? z - r < y : z + r > -y;
                    gl.Transform Applied = transform;
                    Applied.position = Applied.position - shapeSettings.position;
                    //Applied.position = shapeSettings.position- Applied.position;
                    if (y > 0 && xCorrect && zCorrect)
                    {
                        planets[i].Draw(g, Applied, transform.position, DrawOnlyBorders, size, precision, 1000 / precision, shader, zoom);
                    }
                }
                if(DrawCockpit)g.DrawImage(cockpit,255);
                if (MotionBlur)
                {
                    if (LastDraw != null)
                    {
                        g.DrawImage(LastDraw, 128);
                    }
                    LastDraw = g.Unlock();
                    Background = new ImageBrush(LastDraw);
                }
                else {
                    Background = new ImageBrush(g.Unlock());
                }
                Title = g.elapsed.ToString();

            }
        }
        public Mesh[] meshes = new Mesh[0];
        List<Key> keys = new List<Key>();
        bool Running;
        int NearestPlanet;
       void Run()
       {
            Running = true;
            Terrain();
            transform.position = new Vector3(0, 0, planets[0].shapeSettings.PlanetRadius * 2f) + planets[0].shapeSettings.position;
            int LastUpdate = DateAndTime.Milisecs;
            while (IsVisible)
            {
                Physics();
                Draw();
                DoEvents();
                int rest = LastUpdate + 30 - DateAndTime.Milisecs;
                if (rest > 1)
                    Thread.Sleep(rest);
                LastUpdate = DateAndTime.Milisecs;
                ProcessInput();
            }
            if(Application.Current!=null)
            Application.Current.Shutdown();
        }
        public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }
        public object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;
            return null;
        }
        bool OnPlanet;
        bool LastCollided;
        bool GravityOnPlayer = true;
        bool GravityOnPlanets = true;
        void Physics()
        {
            bool NewCollided = false;
            //Vector3 PlusSpeed = new Vector3();
            List<Planet> ps = planets;
            int l = planets.Count;
            Vector3 pos = transform.position;
            const float gravitational_constant = 0.01f;
            #region PlayerPhysics
            float dist = float.PositiveInfinity;
            int nearest = 0;
            if (GravityOnPlayer)
            {
                for (int i = 0; i < l; i++)//gravity planets on spacecraft
                {
                    ShapeSettings shapeSettings = planets[i].shapeSettings;
                    Vector3 planetpos = shapeSettings.position - pos;//points to left var
                    float r = shapeSettings.PlanetRadius + shapeSettings.MaxDifference * .5f;
                    float d = planetpos.Length;
                    if (d - r < dist)
                    {
                        dist = d;
                        nearest = i;
                    }
                    float weight = r * r * r;
                    planetpos *= gravitational_constant * weight / (d * d * d);//gravity = const * w/(d*d), extra division to normalize
                    speed += planetpos;
                }
            }
            Planet NearestPlanet = planets[nearest];
            ShapeSettings nearestSettings = NearestPlanet.shapeSettings;
            Vector3 Sum = transform.position + speed;
            Vector3 ppos = nearestSettings.position;
            Vector3 sub = Sum - ppos;//pointing to left variable
            float distanceToNearestPlanet = sub.Length;
            Vector3 normal = sub / distanceToNearestPlanet;
            //check wether colliding with nearest planet
            float val = NearestPlanet.shapeGenerator.noiseFilter.Evaluate(ppos + normal) * nearestSettings.MaxDifference + nearestSettings.PlanetRadius;
            const float bounce = -.5f, height = .005f;
            float Friction;
            if (val > distanceToNearestPlanet)//collide, bounce
            {
                NewCollided = true;
                Friction = 0.5f;
                //if (!LastCollided) speed *= bounce;
                //else speed = new Vector3();
                normal = (transform.position - ppos).normalized;
                val = planets[nearest].shapeGenerator.noiseFilter.Evaluate(ppos + normal) * nearestSettings.MaxDifference + nearestSettings.PlanetRadius;
                //transform.position = ppos + normal * (val + height);
            }
            else
            {
                Friction = 0.1f;
                transform.position += speed;
            }
            OnPlanet = distanceToNearestPlanet < nearestSettings.PlanetRadius + nearestSettings.MaxDifference;
            if (OnPlanet) speed = Vector3.Lerp(speed, NearestPlanet.Speed, Friction);//in space is no friction
            LastCollided = NewCollided;
            #endregion
            if (GravityOnPlanets)
            {
                for (int i = 0; i < l; i++)//gravity planets on eachother
                {
                    Planet planetA = planets[i];
                    ShapeSettings shapeSettingsA = planetA.shapeSettings;
                    Vector3 planetApos = shapeSettingsA.position;
                    float rA = shapeSettingsA.PlanetRadius + shapeSettingsA.MaxDifference * .5f;
                    float weightA = rA * rA * rA;
                    for (int j = i + 1; j < l; j++)
                    {
                        Planet planetB = planets[j];
                        ShapeSettings shapeSettingsB = planetB.shapeSettings;
                        Vector3 planetBpos = shapeSettingsB.position;
                        Vector3 FromBToA = planetApos - planetBpos;
                        float rB = shapeSettingsB.PlanetRadius + shapeSettingsB.MaxDifference * .5f;
                        float d = FromBToA.Length;//points to planetA
                        if (d < rA + rB)//planets collide
                        {
                            if (rA > rB)
                            {
                                //planetA.Colliding.Add(planetB);
                                planetB.Destroyed = true;
                            }
                            else
                            {
                                //planetB.Colliding.Add(planetA);
                                planetA.Destroyed = true;
                            }
                        }
                        else
                        {
                            float weightB = rB * rB * rB;
                            FromBToA *= gravitational_constant / (d * d * d);
                            planetB.Speed += FromBToA * weightA;
                            planetA.Speed -= FromBToA * weightB;
                        }
                    }
                }
            }
            for(int i = 0; i < l; i++)
            {
                Planet planetA = planets[i];
                if (planetA.Destroyed)
                {
                    planets.RemoveAt(i--);
                    l--;
                    continue;
                }
                int collidecount = planetA.Colliding.Count;
                if (collidecount > 0)
                {
                    ElevationSettings elevationSettingsA = planetA.shapeSettings.NoiseSettings.settings;
                    List<Elevation> BigPlanetElevations = elevationSettingsA.Elevations;
                    float rA = planetA.shapeSettings.PlanetRadius + planetA.shapeSettings.MaxDifference * .5f;
                    float mA = rA * rA * rA;
                    //other planets have collided, but their radius was lesser than this planet's radius
                    for (int j = 0; j < collidecount; j++)
                    {
                        Planet PlanetB = planetA.Colliding[j];
                        ElevationSettings elevationSettingsB = PlanetB.shapeSettings.NoiseSettings.settings;
                        //add height layers to this planet
                        Elevation[] elevations = elevationSettingsB.Elevations.ToArray();
                        int ElevationCount = elevations.Length;
                        Elevation BottomElevation = elevations[0];
                        float rB = PlanetB.shapeSettings.PlanetRadius + PlanetB.shapeSettings.MaxDifference * .5f;
                        float mB = rB * rB * rB;
                        float TotalMass = mB + mA;
                        float multiplierA = mA / TotalMass;
                        float multiplierB = mB / TotalMass;//   1/multA
                        float NewRadius = (float)sys.Math.Pow(TotalMass, 1 / 3);
                        float LastElevationHeight = BigPlanetElevations[BigPlanetElevations.Count - 1].height;
                        for(int k = 0; k < BigPlanetElevations.Count; k++)
                        {
                            BigPlanetElevations[k].height *= multiplierA;
                        }
                        for(int k = 0; k > ElevationCount; k++)
                        {
                            BigPlanetElevations.Add(new Elevation(elevations[k].color, elevations[k].height * multiplierB + multiplierA));
                            //elevations[0].
                        }
                        //add water to this planet
                        float waterheight = elevationSettingsB.WaterHeight;
                        Color waterColor = elevationSettingsB.WaterColor;
                        elevationSettingsA.WaterHeight = (elevationSettingsA.WaterHeight * mA + elevationSettingsB.WaterHeight * mB) / TotalMass;
                        elevationSettingsA.WaterColor.ScR = (elevationSettingsA.WaterColor.ScR * mA + elevationSettingsB.WaterColor.ScR * mB) / TotalMass;
                        elevationSettingsA.WaterColor.ScG = (elevationSettingsA.WaterColor.ScG * mA + elevationSettingsB.WaterColor.ScG * mB) / TotalMass;
                        elevationSettingsA.WaterColor.ScB = (elevationSettingsA.WaterColor.ScB * mA + elevationSettingsB.WaterColor.ScB * mB) / TotalMass;
                        planetA.shapeSettings.PlanetRadius = (planetA.shapeSettings.PlanetRadius * mA + PlanetB.shapeSettings.PlanetRadius * mB) / TotalMass;

                        planetA.Speed += PlanetB.Speed * mB / mA;
                    }
                }
                planetA.Colliding.Clear();
                planetA.shapeSettings.position += planets[i].Speed;
                for(int j = 0; j < 6; j++)
                {//force the planet to regenerate mesh
                    planetA.TerrainFaces[j].Childs = null;
                    planetA.TerrainFaces[j].LastMesh = null;
                    planetA.TerrainFaces[j].LastResolution = 0;
                }
                //planets[i].GeneratePlanet();
                //planets[i].
            }
        }
 
        void ProcessInput()
        {
            float step = 0.01f;//OnPlanet ? 0.0001f : 0.01f;
            if (keys.Contains(Key.LeftShift))
            {
                step *= 100f;
            }
            if (keys.Contains(Key.Space))
            {
                    step *= 1000f;
            }
            float rotstep = Mathf.PI * 0.05f;
            for (int i = 0; i < keys.Count; i++)
            {
                switch (keys[i])
                {
                    case Key.Escape:
                        if (MessageBox.Show("Do you really want to leave?", "Universe", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            Hide();
                        return;
                    case Key.M:
                        MotionBlur = !MotionBlur;
                        LastDraw = null;
                        break;
                    case Key.V:
                        DrawCockpit = !DrawCockpit;
                        keys.Remove(Key.V);
                        break;
                    case Key.I:
                        if (precision > 1) precision--;
                        break;
                    case Key.K:
                        precision++;
                        break;
                    case Key.N:
                        try
                        {
                            seed = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Seed"));
                            Terrain();
                            transform.position = planets[0].shapeSettings.position + new Vector3(0, 0, planets[0].shapeSettings.PlanetRadius * 2f);
                        }
                        catch { }
                        keys.Remove(Key.N);
                        break;
                    case Key.G:
                        string msgtext = seed.ToString();
                        if (MessageBox.Show(msgtext, "click OK to copy the seed", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        { Clipboard.SetText(msgtext); }
                        keys.Remove(Key.G);
                        break;
                    case Key.B:
                        DrawOnlyBorders = !DrawOnlyBorders;
                        keys.Remove(Key.B);
                        break;
                    case Key.C:
                        Shade = !Shade;
                        keys.Remove(Key.C);
                        break;
                    case Key.Enter:
                        seed = new Random().Next();
                        Terrain();
                        transform.position = planets[0].shapeSettings.position + new Vector3(0, 0, planets[0].shapeSettings.PlanetRadius * 2f);
                        break;
                    //https://en.wikipedia.org/wiki/Aircraft_principal_axes
                    case Key.E://roll
                        transform.Roll(-rotstep);
                        break;
                    case Key.Q:
                        transform.Roll(rotstep);
                        break;
                    case Key.A://yaw
                        transform.Yaw(-rotstep);
                        break;
                    case Key.D:
                        transform.Yaw(rotstep);
                        break;
                    case Key.W://pitch
                        transform.Pitch(-rotstep);
                        break;
                    case Key.S:
                        transform.Pitch(rotstep);
                        break;
                    case Key.Left:
                        speed -= transform.Right * step;
                        continue;
                    case Key.Right:
                        speed += transform.Right * step;
                        continue;
                    case Key.Up:
                        speed += transform.Direction * step;
                        continue;
                    case Key.Down:
                        speed *= 0.8f;
                        continue;
                    case Key.Home:
                        if (zoom < 8) zoom *= 1.25f;
                        else zoom = 10;
                        break;
                    case Key.End:
                        if (zoom > 1.25f) zoom *= 0.8f;
                        else zoom = 1f;
                        break;
                    case Key.PageUp:
                        break;
                    case Key.PageDown:
                        break;
                }
            }
        }
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Running) Run();
            if (keys.IndexOf(e.Key) == -1)
                keys.Add(e.Key);
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            keys.Remove(e.Key);
        }
    }
}