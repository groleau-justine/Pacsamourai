using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    //Classe contenant les méthodes afin de créer et d'afficher la MAP
    public class Map
    {
        //Tailles des tiles de la MAP
        const int tailleWidth = 32;
        const int tailleHeight = 32;

        private int level;
        private String nameLevel;

        //Tailles de la MAP
        private int maxMapX;
        private int maxMapY;

        //Données de la MAP
        private char[,] mapGame;

        Boolean bonus;
        private int xBonus;
        private int yBonus;

        public Boolean Bonus
        {
            get { return bonus; }
            set { bonus = value; }
        }

        public Map(int level)
        {
            this.level = level;
            nameLevel = "map" + level.ToString("D2");
            bonus = false;
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

        public char[,] MapGame
        {
            get { return mapGame; }
            set { mapGame = value; }
        }

        //Charger les données du fichier de la MAP dans le tableau mapGame
        public void loadMap()
        {
            List<char[]> lignes = new List<char[]>();
            //Lecture du fichier de la MAP et enregistrement des infos dans le tableau mapGame
            using (StreamReader reader = new StreamReader("Content/maps/"+ nameLevel + ".txt"))
            {
                string ligne;
                while ((ligne = reader.ReadLine()) != null)
                {
                    string[] strings = ligne.Split(' ');
                    char[] characters = new char[strings.Length];

                    //Taille (colonnes) de la MAP
                    maxMapY = strings.Length;
                    for (int i = 0; i < maxMapY; i++)
                    {
                        if (strings[i] != "")
                            characters[i] = char.Parse(strings[i]);
                    }

                    lignes.Add(characters);
                }

                //Taille (lignes) de la MAP
                maxMapX = lignes.Count;

                mapGame = new char[maxMapX, maxMapY];
                
                //Création du tableau mapGame
                for (int x = 0; x < maxMapX; x++)
                {
                    for (int y = 0; y < maxMapY; y++)
                    {
                        mapGame[x, y] = lignes[x][y];
                    }
                }
            }
        }

        //Afficher la MAP d'après le fichier .txt chargée précédemment
        public void showMap(SpriteBatch spriteBatch, Objet mur, Pacsamurai pacSamourai, Fantome fantomeBleu, Fantome fantomeRose, Fantome fantomeRouge, Fantome fantomeVert, Objet sushi, Objet maki, Objet cerises, SpriteFont font)
        {
            spriteBatch.Begin();

            //Chargement des textures de la MAP
            for (int x = 0; x < MaxMapX; x++)
            {
                for (int y = 0; y < MaxMapY; y++)
                {
                    Vector2 coord = new Vector2(y * tailleWidth, x * tailleHeight);

                    switch (MapGame[x, y])
                    {
                        case '0':
                            spriteBatch.Draw(mur.Texture, coord, Color.White);
                            break;
                        case '1':
                            spriteBatch.Draw(mur.Texture, coord, Color.Black);
                            break;
                        //Pourquoi le '2'?
                        case '2':
                            spriteBatch.Draw(mur.Texture, coord, Color.Black);
                            break;
                        case 'S':
                            spriteBatch.Draw(sushi.Texture, coord, Color.White);
                            break;
                        case 'M':
                            spriteBatch.Draw(maki.Texture, coord, Color.White);
                            break;
                        case 'B':
                            spriteBatch.Draw(cerises.Texture, coord, Color.White);
                            break;
                        case 'F':
                            spriteBatch.Draw(fantomeBleu.Texture, coord, Color.White);
                            break;
                        case 'R':
                            spriteBatch.Draw(fantomeRose.Texture, coord, Color.White);
                            break;
                        case 'Q':
                            spriteBatch.Draw(fantomeRouge.Texture, coord, Color.White);
                            break;
                        case 'V':
                            spriteBatch.Draw(fantomeVert.Texture, coord, Color.White);
                            break;
                        case 'P':
                            spriteBatch.Draw(pacSamourai.Texture, coord, Color.White);
                            break;
                    }
                }
            }

            //Affichage du texte dans le jeu          
            String texteVie = String.Format("Vie : {0}", pacSamourai.Vies);
            String texteScore = String.Format("Score : {0}", pacSamourai.Score);
            spriteBatch.DrawString(font, texteVie, new Vector2(10, 32 * 22), Color.White);
            spriteBatch.DrawString(font, texteScore, new Vector2(4 * 32, 22 * 32), Color.White);

            spriteBatch.End();
        }

        //Affichage des bonus
        public void drawBonus(Random aleatoire)
        {
            char bonusChoisi = GenerateurBonus.recupererBonus(aleatoire);

            if (!bonusChoisi.Equals('v'))
            {
                //On récupère les cases vides ou il est possible de poser un bonus
                List<Vector2> positionsPossiblesBonus = new List<Vector2>();

                for (int x = 0; x < MaxMapX; x++)
                {
                    for (int y = 0; y < MaxMapY; y++)
                    {
                        if ('1'.Equals(MapGame[x, y]))
                        {
                            positionsPossiblesBonus.Add(new Vector2(x, y));
                        }
                    }
                }

                if (positionsPossiblesBonus.Count != 0)
                {
                    int position = aleatoire.Next(0, positionsPossiblesBonus.Count);
                    List<Vector2> caseMap = positionsPossiblesBonus.GetRange(position, 1);
                    Vector2 caseBonus = caseMap[0];
                    xBonus = (int)caseBonus.X;
                    yBonus = (int)caseBonus.Y;

                    //On modifie la Map afin quelle prenne en compte le bonus
                    this.MapGame[xBonus, yBonus] = 'B';
                }
            }    
        }

        //Remove des bonus
        public void removeBonus()
        {
            this.MapGame[xBonus, yBonus] = '1';
        }
    }
}
