using openview;
using System.Collections.Generic;
using System.Windows.Media;

namespace Molecules
{
    public struct Molecule
    {
        public Color colorFrozen;
        public Color colorMolten;
        public Color colorGas;
        public float MeltTemp;
        public float EvapTemp;
        public float Weight;
        public string Name;
        public static Molecule Water = new Molecule(Color.FromRgb(250, 250, 255), Color.FromRgb(128, 128, 255), Color.FromRgb(200, 200, 200), 0, 100, 1, "water"),
         Gold = new Molecule(Color.FromRgb(191, 144, 0), Color.FromRgb(191, 144, 0), Color.FromRgb(191, 144, 0), 1064.18f, 3243f, 19.3f, "gold"),
        Iron = new Molecule(Color.FromRgb(203, 205, 205), Color.FromRgb(224,209,120), Color.FromRgb(203, 205, 205), 1538f, 2862f, 7.874f, "iron"),
        Diamond = new Molecule(Color.FromRgb(255, 0, 255), Color.FromRgb(0, 255, 255), Color.FromRgb(203, 205, 205), 4700f, 4700f, 3.5f, "diamond"),
        Nickel = new Molecule(Color.FromRgb(189, 186, 174), Color.FromRgb(252,255,255), Color.FromRgb(189, 186, 174), 1455f, 2730f, 7.874f, "nickel");
        public static Molecule[] molecules = new Molecule[] {
            Water,
            Gold,
            Iron,
            Diamond,
            Nickel
        };
        public static Molecule GetRandomMolecule(Random r)
        {
            int i = r.Next() % molecules.Length;
            return molecules[i];
        }
        public static Molecule GetRandomMoltenMolecule(Random r,float temp)
        {
            List<Molecule> Molten = new List<Molecule>();
            int l = molecules.Length;
            for (int i = 0; i < l; i++)
            {
                Molecule molecule = molecules[i];
                if (molecule.MeltTemp < temp && molecule.EvapTemp > temp)//molten
                {
                    Molten.Add(molecule);
                }
            }
            if (Molten.Count == 0) return new Molecule();
            return Molten[r.Next() % Molten.Count];
        }
        public static Molecule GetRandomFrozenMolecule(Random r, float temp)
        {
            List<Molecule> Frozen = new List<Molecule>();
            int l = molecules.Length;
            for (int i = 0; i < l; i++)
            {
                Molecule molecule = molecules[i];
                if (molecule.MeltTemp > temp)//frozen
                {
                    Frozen.Add(molecule);
                }
            }
            if (Frozen.Count == 0) return new Molecule();
            return Frozen[r.Next() % Frozen.Count];
        }
        public Color GetColor(float temp)
        {
            return temp < MeltTemp ? colorFrozen : temp > EvapTemp ? colorGas : colorMolten;
        }
        public Molecule(Color colorFrozen, Color colorMolten, Color colorGas, float meltTemp, float evapTemp, float weight, string name)
        {
            this.colorFrozen = colorFrozen;
            this.colorMolten = colorMolten;
            this.colorGas = colorGas;
            this.Name = name;
            MeltTemp = meltTemp;
            EvapTemp = evapTemp;
            Weight = weight;
        }
    }
}