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
        private int level;
        private String nameLevel;

        private int maxMapX;
        private int maxMapY;

        private int[,] mapGame;

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

        public int[,] MapGame
        {
            get { return mapGame; }
            set { mapGame = value; }
        }

        public void loadMap()
        {
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

                mapGame = new int[maxMapX, maxMapY];

                for (int x = 0; x < maxMapX; x++)
                {
                    for (int y = 0; y < maxMapY; y++)
                    {
                        mapGame[x, y] = lignes[x][y];
                    }
                }
            }
        }

        public void showMap()
        {

        }
    }
}
