using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    public class Map
    {
        const int tailleWidth = 32;
        const int tailleHeight = 32;

        private int level;
        private String nameLevel;

        private int maxMapX;
        private int maxMapY;

        private int[,] map;

        private Objet mur;

        private Objet pacSamourai;

        private Objet fantome1;
        private Objet fantome2;
        private Objet fantome3;
        private Objet fantome4;

        public Map(int level)
        {
            this.level = level;
            nameLevel = "map" + level.ToString("D2");
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public String NameLevel
        {
            get { return nameLevel; }
            set { nameLevel = value; }
        }

        public int MaxMapX
        {
            get { return maxMapX; }
            set { maxMapX = value; }
        }

        public int MaxMapY
        {
            get { return maxMapY; }
            set { maxMapY = value; }
        }

        public int[,] Map
        {
            get { return map; }
            set { map = value; }
        }

        public void loadMap()
        {
            Boolean isMur = false, isPacsamourai = false, isFantome1 = false, isFantome2 = false, isFantome3 = false, isFantome4 = false;

            List<int[]> lignes = new List<int[]>();
            using (StreamReader reader = new StreamReader("Content/maps/"+ nameLevel + ".txt"))
            {
                string ligne;
                while ((ligne = reader.ReadLine()) != null)
                {
                    string[] strings = ligne.Split(' ');
                    int[] nombres = new int[strings.Length];
                    maxMapY = strings.Length;
                    for (int i = 0; i < maxMapY; i++)
                    {
                        if (strings[i] != "")
                            nombres[i] = int.Parse(strings[i]);
                    }

                    lignes.Add(nombres);
                }
                maxMapX = lignes.Count;

                map = new int[maxMapX, maxMapY];

                for (int x = 0; x < maxMapX; x++)
                {
                    for (int y = 0; y < maxMapY; y++)
                    {
                        map[x, y] = lignes[x][y];

                        if (map[x, y] == 0 && !isMur)
                        {
                            mur = new Objet(Content.Load<Texture2D>("mur01"));
                            isMur = true;
                        }
                            
                            

                    }
                }
            }
        }

        public void showMap()
        {

            for (int x = 0; x < maxMapX; x++)
            {
                for (int y = 0; y < maxMapY; y++)
                {
                    if (map[x, y] == 0)
                        
                    else if (map[x, y] == 1)

                }
            }
        }
    }
}
