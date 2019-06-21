using System.Collections;
using System.Collections.Generic;
using openview;
using openview.OpenGL;
using gl = openview.OpenGL;
using System.Windows.Media;
//using System.Drawing;

namespace Planet3D
{
    public class ShapeSettings
    {
        /// <summary>
        /// the radius of the planet.
        /// </summary>
        public float PlanetRadius = 1;
        /// <summary>
        /// maximum height difference with average.
        /// </summary>
        public float MaxDifference = 0.5f;
        public Vector3 position;
        public NoiseSettings NoiseSettings;
    }
    public static class NoiseFilterFactory
    {
        public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
        {
            switch (settings.filterType)
            {
                case NoiseSettings.FilterType.Simple:
                    return new SimpleNoiseFilter(settings);
                case NoiseSettings.FilterType.Ridgid:
                    return new RidgidNoiseFilter(settings);
            }
            return null;
        }
    }
    public class ShapeGenerator
    {
        public ShapeSettings settings;
        public INoiseFilter noiseFilter;

        public ShapeGenerator(ShapeSettings settings)
        {
            this.settings = settings;
            noiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.NoiseSettings);
        }
        public float GetHeight(Vector3 PointOnUnitSphere)
        {
            float FirstLayerValue = noiseFilter.Evaluate(PointOnUnitSphere);
            return FirstLayerValue;
        }
        public Vector3 CalculatePointOnPlanet(Vector3 PointOnUnitSphere)
        {
            return PointOnUnitSphere * settings.PlanetRadius * (GetHeight(PointOnUnitSphere) + 1);
        }
    }
    public class MeshHolder
    {
        public MeshHolder Parent;
        //public ShapeSettings shapeSettings;
        public ShapeGenerator shapeGenerator;
        //public Vector3 position;
        public Vector3 LocalUp;
        public Vector3 AxisA;
        public Vector3 AxisB;
        public Vector3 MiddlePointOnUnitSphere;
        public int LastResolution;
        public Mesh LastMesh;
        public MeshHolder[] Childs;
        public float minx, miny, width;
        public MeshHolder(MeshHolder parent, ShapeGenerator shapeGenerator, Vector3 localUp, Vector3 axisA, Vector3 axisB, float minx, float miny, float width)
        {
            Parent = parent;
            this.shapeGenerator = shapeGenerator;
            LocalUp = localUp;
            AxisA = axisA;
            AxisB = axisB;
            ShapeSettings shapeSettings = shapeGenerator.settings;
            Vector3 PlanetCentre = shapeSettings.position;
            float radius = shapeSettings.PlanetRadius;
            Vector3 MiddlepointOnUnitCube = LocalUp*.5f + (minx + width*.5f - .5f) * AxisA + (miny + width * .5f - .5f) * AxisB;
            Vector3 MiddlePointOnUnitSphere = MiddlepointOnUnitCube.normalized;
            this.minx = minx;
            this.miny = miny;
            this.width = width;
        }

        public void DrawMesh(gl.Transform transform, Vector3 Camera, TriangleDrawer graphics, Shader shader, bool onlyborders, float zoom, float precision, int maxprecision)
        {
            float minx = this.minx, miny = this.miny, width = this.width;
                maxprecision--;
            ShapeGenerator shapeGenerator = this.shapeGenerator;
            ShapeSettings shapeSettings = shapeGenerator.settings;
            NoiseSettings noiseSettings = shapeSettings.NoiseSettings;
            ElevationSettings settings = noiseSettings.settings;
            Vector3 PlanetCentre = shapeSettings.position;
            //Vector3 pos = transform.position - PlanetCentre;
            float radius = shapeSettings.PlanetRadius;
            float r = settings.WaterHeight * shapeSettings.MaxDifference + radius;
            float Middleelev = shapeGenerator.noiseFilter.Evaluate(MiddlePointOnUnitSphere);
            Vector3 MiddlePoint = MiddlePoint = MiddlePointOnUnitSphere * (radius + Middleelev * shapeSettings.MaxDifference) + PlanetCentre;
            float distance = Vector3.Distance(Camera, MiddlePoint);
            if (distance == 0)
                return;
            if (minx > 0)
            {

            }
            int resolution = Mathf.FloorToPower((int)(precision / distance * width), 2);
            //resolution will be >0
            //#if false
            if (maxprecision > 0 && resolution > 16)//max 16x16=256
            {
                DrawChildMeshes(resolution, minx, miny, width, transform,Camera, graphics, shader, onlyborders, zoom, precision, maxprecision);
                return;
            }
            //#endif
            else if (maxprecision == 0)
            {

            }
            Vector3 Max00 = (LocalUp*.5f + (minx -.5f) * AxisA + (miny -.5f) * AxisB).normalized * r+PlanetCentre;
            Vector3 Max01 = (LocalUp*.5f + (minx - .5f) * AxisA + ((miny + width) -.5f) * AxisB).normalized * r + PlanetCentre;
            Vector3 Max10 = (LocalUp * .5f + ((minx + width) - .5f) * AxisA + (miny -.5f) * AxisB).normalized * r + PlanetCentre;
            Vector3 Max11 = (LocalUp * .5f + ((minx + width)- .5f) * AxisA + ((miny + width) - .5f) * AxisB).normalized * r + PlanetCentre;
            Vector3 nearest = Vector3.Nearest(Camera,Max00,Max01,Max10,Max11);
            //int intersections = Vector3.FindIntersections(PlanetCentre, r - Mathf.FloatEpsilon, nearest, Camera - nearest, out Vector3 nearestpoint, out Vector3 farthestpoint);
            //if (intersections > 0) return;//intersects with sphere
            if (LastResolution == resolution)
            {
                OpenGl.Draw(graphics, LastMesh, transform, onlyborders, shader, zoom);
                return;
            }
            #region initialisation
            int resolution1 = resolution + 1;
            Vector3[] vertices = new Vector3[resolution1 * resolution1];
            int cnt = resolution * resolution * 6;
            int[] triangles = new int[cnt];
            byte[] colors = new byte[cnt];
            int triIndex = 0;
            int i = 0;
            float wh = settings.WaterHeight;
            Color watercolor = settings.WaterColor;
            Elevation[] Elevations = settings.Elevations.ToArray();
            int elevcount = Elevations.Length;
            float Scale = width / resolution;
            #endregion
            for (int y = 0; y < resolution1; y++)
            {
                for (int x = 0; x < resolution1; x++)
                {
                    #region Calculate Vertices
                    Vector2 percent = new Vector2(x * Scale + minx, y * Scale + miny);
                    Vector3 pointOnUnitCube = LocalUp*.5f + (percent.x -.5f) * AxisA + (percent.y - .5f) * AxisB;
                    Vector3 PointOnUnitSphere = pointOnUnitCube.normalized;
                    float elev = shapeGenerator.noiseFilter.Evaluate(PointOnUnitSphere);
                    vertices[i] = PointOnUnitSphere * (radius + elev * shapeSettings.MaxDifference);// + PlanetCentre;
                    #endregion
                    if (x != resolution && y != resolution)
                    {
                        Color color = watercolor;
                        if (elev > wh + Mathf.FloatEpsilon)
                        {
                            for (int j = 0; j < elevcount; j++)
                            {
                                if (elev < Elevations[j].height + Mathf.FloatEpsilon)
                                {
                                    color = Elevations[j].color;
                                    break;
                                }
                            }
                        }
                        #region Calculate Colors
                        colors[triIndex] = color.R;
                        colors[triIndex + 1] = color.G;
                        colors[triIndex + 2] = color.B;
                        colors[triIndex + 3] = color.R;
                        colors[triIndex + 4] = color.G;
                        colors[triIndex + 5] = color.B;
                        #endregion
                        #region Calculate Triangles
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + resolution1 + 1;
                        triangles[triIndex + 2] = i + resolution1;
                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + resolution1 + 1;
                        #endregion
                        triIndex += 6;
                    }
                    i++;
                }
            }
            //mesh.Clear();
            LastMesh = new Mesh(vertices, triangles, colors);
            OpenGl.Draw(graphics, LastMesh, transform, onlyborders, shader, zoom);
            LastResolution = resolution;
            //mesh.shader = shader;
            //mesh.RecalculateNormals();
        }
        public void DrawChildMeshes(int res, float minx, float miny, float width, gl.Transform transform, Vector3 Camera, TriangleDrawer graphics, Shader shader, bool onlyborders, float zoom, float precision, int maxprecision)
        {
            const int Spread = 4, sq = Spread * Spread;
            res /= Spread;
            float step = width / Spread;
            MeshHolder[] childs = Childs;
            if (childs == null)
            {
                childs = new MeshHolder[sq];
                int k = 0;
                for (int j = 0; j < Spread; j++)
                {
                    float mny = miny + step * j;
                    for (int i = 0; i < Spread; i++)
                    {
                        float mnx = minx + step * i;
                        childs[k] = new MeshHolder(Parent = this, shapeGenerator, LocalUp, AxisA, AxisB, mnx, mny, step);
                        k++;
                    }
                }
                Childs = childs;
            }
            for (int i = 0; i < sq; i++)
            {
                childs[i].DrawMesh(transform, Camera, graphics, shader, onlyborders, zoom, precision, maxprecision);
            }
        }
    }
    public class TerrainFace : MeshHolder
    {
        public TerrainFace(ShapeGenerator shapeGenerator, Vector3 localup) : base(null, shapeGenerator, localup, new Vector3(localup.y, localup.z, localup.x), Vector3.Cross(localup, new Vector3(localup.y, localup.z, localup.x)), 0, 0, 1) { }
    }
    public class Planet
    {
        public bool Destroyed;
        public List<Planet> Colliding = new List<Planet>();
        public Vector3 Speed;
        public bool AutoUpdate = true;
        public ShapeSettings shapeSettings = new ShapeSettings
        {
            NoiseSettings = new NoiseSettings
            {
                BaseRougness = 1,
                filterType = NoiseSettings.FilterType.Simple,
                strength = 0.5f,
                persistence = 0.5f,
                numlayers = 5,
                roughness = 1
            },
            PlanetRadius = 10f
        };

        //public ColourSetings colourSettings;
        public Color color;
        public ShapeGenerator shapeGenerator;
        //public Mesh mesh => new Mesh(meshes);
        //public Mesh[] meshes = new Mesh[6];
        //MeshFilter[] meshfilters;
        public TerrainFace[] TerrainFaces;
        static Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        public void Draw(TriangleDrawer g, gl.Transform info, Vector3 Camera, bool onlyborders, Size screensize, float pixelprecision, int MaxResolution, Shader shader, float zoom)
        {
            int maxsize = screensize.Width > screensize.Height ? screensize.Width : screensize.Height;
            float precision = maxsize / pixelprecision * shapeSettings.PlanetRadius;
            for (int i = 0; i < 6; i++)
            {
                TerrainFaces[i].DrawMesh(info,Camera, g, shader, onlyborders, zoom, precision, 10);
            }
        }
        public void Initialize()
        {
            shapeGenerator = new ShapeGenerator(shapeSettings);
            //mesh = new Mesh(resolution * resolution);
            TerrainFaces = new TerrainFace[6];
            for (int i = 0; i < 6; i++)
            {
                TerrainFaces[i] = new TerrainFace(shapeGenerator, directions[i]);
            }
        }
        public void GeneratePlanet(int resolution)
        {
            Initialize();
            // GenerateColours();
        }


    }

    [System.Serializable]
    public class NoiseSettings
    {
        public enum FilterType
        {
            Simple,
            Ridgid
        }
        public FilterType filterType;
        public float strength = 1;
        public int numlayers = 1;
        public float BaseRougness = 1;
        public float roughness = 2;
        public float persistence = .5f;
        public Vector3 centre;
        public bool UseFirstLayerAsMask;
        public ElevationSettings settings;
    }
    public class ElevationSettings
    {
        public Color WaterColor;
        public float WaterHeight;
        public List<Elevation> Elevations = new List<Elevation>();
        public static List<Elevation> NormalElevations = new List<Elevation>(new Elevation[] {
            new Elevation(NormalColor.Green,0.6f),
            new Elevation(NormalColor.White,1f)
        });
    }
    public class Elevation
    {
        public Color color;
        public float height;
        public Elevation(Color color, float height)
        {
            this.color = color;
            this.height = height;
        }
    }
    public interface INoiseFilter
    {
        float Evaluate(Vector3 point);
    }
    public class SimpleNoiseFilter : INoiseFilter
    {
        NoiseSettings settings;
        Noise3D noise = new Noise3D();
        public SimpleNoiseFilter(NoiseSettings settings)
        {
            this.settings = settings;
        }
        public float Evaluate(Vector3 point)
        {
            float NoiseValue = 0;
            float frequency = settings.BaseRougness;
            float amplitude = 1;
            Vector3 centre = settings.centre;
            float px = point.x, py = point.y, pz = point.z;
            float divider = 0;
            for (int i = 0; i < settings.numlayers; i++)
            {
                float v = noise.Evaluate(px * frequency + centre.x, py * frequency + centre.y, pz * frequency + centre.z);
                v = (v + 1) * .5f;
                NoiseValue += v * amplitude;
                divider += amplitude;
                frequency *= settings.roughness;
                amplitude *= settings.persistence;
            }
            NoiseValue /= divider;
            if (NoiseValue > settings.settings.WaterHeight) return NoiseValue;
            else return settings.settings.WaterHeight;// * settings.strength;
        }
    }
    public class RidgidNoiseFilter : INoiseFilter
    {
        NoiseSettings settings;
        Noise3D noise = new Noise3D();
        public RidgidNoiseFilter(NoiseSettings settings)
        {
            this.settings = settings;
        }
        public float Evaluate(Vector3 point)
        {
            //float NoiseValue = (noise.Evaluate (point * settings.roughness + settings.centre) + 1) * .5f;
            float NoiseValue = 0;
            float frequency = settings.BaseRougness;
            float amplitude = 1;
            Vector3 centre = settings.centre;
            float px = point.x, py = point.y, pz = point.z;
            float divider = 0;
            for (int i = 0; i < settings.numlayers; i++)
            {
                float v = noise.Evaluate(px * frequency + centre.x, py * frequency + centre.y, pz * frequency + centre.z);
                v = v < 0 ? 1 + v : 1 - v;//absolute turned
                NoiseValue += v * v * amplitude;
                divider += amplitude;
                frequency *= settings.roughness;
                amplitude *= settings.persistence;
            }
            NoiseValue /= divider;
            if (NoiseValue > settings.settings.WaterHeight) return NoiseValue;
            else return settings.settings.WaterHeight;// * settings.strength;
        }
    }
}