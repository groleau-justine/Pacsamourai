using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PAC_SAMURAI
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Pacsamourai : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        private Map useMap;

        private Fantome fantomeBleu, fantomeRose, fantomeRouge, fantomeVert;
        private Pacsamurai pacSamourai;
        private int scoreGame, lastPalier, viePac, better = 500;
        private Boolean endOfTheGame = false;

        private Objet mur;
        private Objet sushi, maki, cerises;

        //Gestion clavier
        private KeyboardState oldState;
        private Clavier clavier;

        //Ecritures
        private SpriteFont font, menuFont;
        
        private Timers timer;

        enum GameState
        {
            menuScreen, gameScreen, wonScreen, lostScreen, scoreScreen, commandScreen
        }
        GameState state, previousState;

        //Mouse pressed 
        Boolean mpressed;

        //Mouse location in window
        int mx, my;

        public Pacsamourai()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Création de la MAP
            useMap = new Map(0);

            base.Initialize();

            this.IsMouseVisible = true;

            //Récupération de l'état clavier et initialisation du clavier 
            oldState = Keyboard.GetState();
            clavier = new Clavier(pacSamourai, oldState, useMap);

            //Initialisation d'un objet timer
            this.timer = new Timers(useMap);

            //Gestion menu
            if (previousState == GameState.lostScreen)
            {
                state = GameState.lostScreen;
            }
            else if (previousState == GameState.wonScreen)
            {
                state = GameState.wonScreen;
            }
            else
            {
                state = GameState.menuScreen;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Variables contenant les textures du PacSamourai et des Fantômes
            List<Texture2D> texturePacSamourai = new List<Texture2D>();
            List<Texture2D> textureInvPacSamourai = new List<Texture2D>();
            List<Texture2D> textureFantomeBleu = new List<Texture2D>();
            List<Texture2D> textureFantomeRose = new List<Texture2D>();
            List<Texture2D> textureFantomeRouge = new List<Texture2D>();
            List<Texture2D> textureFantomeVert = new List<Texture2D>();
            List<Texture2D> textureInvFantome = new List<Texture2D>();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Chargement des dimensions de la fenêtre de jeu
            graphics.PreferredBackBufferWidth = 21*32;
            graphics.PreferredBackBufferHeight = 770;

            try
            {
                graphics.ApplyChanges();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            // Read the file and put it in useMap
            // Chargement des textures et du fichier de la MAP
            mur = new Objet(Content.Load<Texture2D>("mur01"));
            sushi = new Objet(Content.Load<Texture2D>("sushi"));
            maki = new Objet(Content.Load<Texture2D>("maki"));
            cerises = new Objet(Content.Load<Texture2D>("cerises"));

            //Chargement de l'ensemble des textures du PacSamourai
            //Texture mode normal
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_haut"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_droite"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_bas"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_gauche"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_hautO"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_droiteO"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_basO"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_gaucheO"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_mort"));

            //Texture mode invincible
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_hautI"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_droiteI"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_basI"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_gaucheI"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_hautIO"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_droiteIO"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_basIO"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_gaucheIO"));

            //Chargement de l'ensemble des textures des fantômes bleu, rose, rouge et vert
            //Texture mode normal
            textureFantomeBleu.Add(Content.Load<Texture2D>("fantomeBleu"));
            textureFantomeRose.Add(Content.Load<Texture2D>("fantomeRose"));
            textureFantomeRouge.Add(Content.Load<Texture2D>("fantomeRouge"));
            textureFantomeVert.Add(Content.Load<Texture2D>("fantomeVert"));

            //Texture mode peur
            textureInvFantome.Add(Content.Load<Texture2D>("fantomePeur"));

            useMap.loadMap();

            //Création du pacSamurai et des fantômes 
            fantomeBleu = new Fantome(textureFantomeBleu, textureInvFantome, 1, 'F', useMap);
            fantomeRose = new Fantome(textureFantomeRose, textureInvFantome, 2, 'R', useMap);
            fantomeRouge = new Fantome(textureFantomeRouge, textureInvFantome, 3, 'Q', useMap);
            fantomeVert = new Fantome(textureFantomeVert, textureInvFantome, 4, 'V', useMap);
            pacSamourai = new Pacsamurai(texturePacSamourai, textureInvPacSamourai, useMap);
            
            font = Content.Load<SpriteFont>("FontPacsamurai");
            menuFont = Content.Load<SpriteFont>("fontMenu");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            // Enregistre le score dans le fichier des scores lors de la fermeture de la fenêtre de jeu
            // Ce code est utilisé lorsqu'un joueur gagne une partie puis ferme le jeu avant d'avoir perdu
            if (endOfTheGame)
            {
                UpdateFileScore(scoreGame);
                endOfTheGame = false;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }
            
            //Gestion souris : associée au menu
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;

            // Lorsque le joueur a gagné
            if (pacSamourai.EndOfTheGame)
            {
                previousState = GameState.wonScreen;
                //Enregistre le score et le nombre de vies entre les parties
                scoreGame = pacSamourai.Score;
                lastPalier = pacSamourai.LastPalier;
                viePac = pacSamourai.Vies;

                Initialize();

                //Remet en place le score et le nombre de vies si le joueur recommence une partie après avoir gagné
                pacSamourai.Score = scoreGame;
                pacSamourai.LastPalier = lastPalier;
                pacSamourai.Vies = viePac;

                //Augmente la vitesse des fantômes
                better += 100;
                endOfTheGame = true;
            }
            // Lorsque le joueur a perdu
            else if (pacSamourai.Vies == 0)
            {
                previousState = GameState.lostScreen;
                //Mise à jour du fichier des scores
                UpdateFileScore(pacSamourai.Score);
                Initialize();

                //Réinitialise la vitesse des fantômes
                better = 500;
                endOfTheGame = false;
            }

            // Gestion du menu
            switch (state)
            {
                case GameState.menuScreen:
                    UpdateMenuScreen();
                    break;
                case GameState.gameScreen:
                    UpdateGamePlay(gameTime);
                    break;
                case GameState.wonScreen:
                    UpdateMenuScreen();
                    break;
                case GameState.lostScreen:
                    UpdateMenuScreen();
                    break;
                case GameState.commandScreen:
                    UpdateCommandScreen();
                    break;
                case GameState.scoreScreen:
                    UpdateScoreScreen();
                    break;
            }

            base.Update(gameTime);
        }

        // Fonction permettant de gérer les clicks sur le menu principal
        private void UpdateMenuScreen()
        {
            if (mpressed && isClickButton(mx, my, 258, 284, 154, 44))
            {
                state = GameState.gameScreen;
            }
            else if (mpressed && isClickButton(mx, my, 164, 376, 343, 45))
            {
                state = GameState.commandScreen;
            }
            else if (mpressed && isClickButton(mx, my, 245, 480, 182, 44))
            {
                state = GameState.scoreScreen;
            }
            else if (mpressed && isClickButton(mx, my, 242, 583, 186, 49))
            {
                 this.Exit();
            }
        }

        // Fonction Update du jeu Pacsamurai
        private void UpdateGamePlay(GameTime gameTime)
        {
            // Gestion du clavier
            clavier.update(gameTime);

            // Gestion des bonus
            timer.lancerTimerBonus(gameTime);

            // Gestion du pouvoir d'invincibilité et de la mort du Pacsamurai

            // Si le pacsamurai est dans un mode invincible
            if (pacSamourai.Invincible)
            {
                // On fait appel à la fonction suivante pour savoir s'il est toujours en mode invincible
                pacSamourai.Invincible = pacSamourai.Timer.gererTimerInvincible(gameTime);
            }
            // Si le pacsamurai est mort
            else if (pacSamourai.IsMort)
            {
                // On fait appel à la fonction suivante pour savoir s'il est toujours mort
                pacSamourai.IsMort = pacSamourai.Timer.gererTimerMort(gameTime);
                if (pacSamourai.IsMort)
                {
                    pacSamourai.Texture = pacSamourai.ListeTextures[8];
                    useMap.MapGame[pacSamourai.SonX, pacSamourai.SonY] = 'P';
                }
                else
                {
                    pacSamourai.Texture = pacSamourai.ListeTextures[pacSamourai.NumTexture];
                    pacSamourai.IsMort = false;
                }
            }
            else
            {
                // On remet sa texture en mode normal
                pacSamourai.Texture = pacSamourai.ListeTextures[pacSamourai.NumTexture];
            }

            // Gestion de l'IA des fantômes

            // Fantôme bleu
            if (pacSamourai.FantomePeur.Contains(fantomeBleu.TypeFantome))
            {
                fantomeBleu.IsPeur = fantomeBleu.Timer.gererTimerPeur(gameTime);
                fantomeBleu.choiceOfTexture();
            }

            if (!fantomeBleu.IsPeur)
            {
                // Timer qui permet de gérer le temps entre deux actions des fantômes
                if (fantomeBleu.Timer.lancerTimerFantome(gameTime, better))
                {
                    if (fantomeBleu.choixFantomeIA() && !pacSamourai.IsMort)
                    {
                        if (pacSamourai.Invincible)
                        {
                            // On fait appel à la fonction suivante pour savoir s'il est toujours en mode peur
                            pacSamourai.addFantome(fantomeBleu.TypeFantome, new Point(fantomeBleu.PositionXFantome, fantomeBleu.PositionYFantome));
                            fantomeBleu.IsPeur = fantomeBleu.Timer.gererTimerPeur(gameTime);
                            fantomeBleu.choiceOfTexture();
                        }
                        else
                        {
                            pacSamourai.MortDePac = true;
                        }
                    }
                }
            }
            else
            {
                fantomeBleu.IsPeur = fantomeBleu.Timer.gererTimerPeur(gameTime);
                if (!fantomeBleu.IsPeur)
                {
                    fantomeBleu.choiceOfTexture();
                }
            }

            // Fantôme rose
            if (pacSamourai.FantomePeur.Contains(fantomeRose.TypeFantome))
            {
                fantomeRose.IsPeur = fantomeRose.Timer.gererTimerPeur(gameTime);
                fantomeRose.choiceOfTexture();
            }

            if (!fantomeRose.IsPeur)
            {
                // Timer qui permet de gérer le temps entre deux actions des fantômes
                if (fantomeRose.Timer.lancerTimerFantome(gameTime, better))
                {
                    if (fantomeRose.choixFantomeIA() && !pacSamourai.IsMort)
                    {
                        if (pacSamourai.Invincible)
                        {
                            // On fait appel à la fonction suivante pour savoir s'il est toujours en mode peur
                            pacSamourai.addFantome(fantomeRose.TypeFantome, new Point(fantomeRose.PositionXFantome, fantomeRose.PositionYFantome));
                            fantomeRose.IsPeur = fantomeRose.Timer.gererTimerPeur(gameTime);
                            fantomeRose.choiceOfTexture();
                        }
                        else
                        {
                            pacSamourai.MortDePac = true;
                        }
                    }
                }
            }
            else
            {
                fantomeRose.IsPeur = fantomeRose.Timer.gererTimerPeur(gameTime);
                if (!fantomeRose.IsPeur)
                {
                    fantomeRose.choiceOfTexture();
                }
            }

            // Fantôme rouge
            if (pacSamourai.FantomePeur.Contains(fantomeRouge.TypeFantome))
            {
                fantomeRouge.IsPeur = fantomeRouge.Timer.gererTimerPeur(gameTime);
                fantomeRouge.choiceOfTexture();
            }

            if (!fantomeRouge.IsPeur)
            {
                // Timer qui permet de gérer le temps entre deux actions des fantômes
                if (fantomeRouge.Timer.lancerTimerFantome(gameTime, better))
                {
                    if (fantomeRouge.choixFantomeIA() && !pacSamourai.IsMort)
                    {
                        if (pacSamourai.Invincible)
                        {
                            // On fait appel à la fonction suivante pour savoir s'il est toujours en mode peur
                            pacSamourai.addFantome(fantomeRouge.TypeFantome, new Point(fantomeRouge.PositionXFantome, fantomeRouge.PositionYFantome));
                            fantomeRouge.IsPeur = fantomeRouge.Timer.gererTimerPeur(gameTime);
                            fantomeRouge.choiceOfTexture();
                        }
                        else
                        {
                            pacSamourai.MortDePac = true;
                        }
                    }
                }
            }
            else
            {
                fantomeRouge.IsPeur = fantomeRouge.Timer.gererTimerPeur(gameTime);
                if (!fantomeRouge.IsPeur)
                {
                    fantomeRouge.choiceOfTexture();
                }
            }

            // Fantôme vert
            if (pacSamourai.FantomePeur.Contains(fantomeVert.TypeFantome))
            {
                fantomeVert.IsPeur = fantomeVert.Timer.gererTimerPeur(gameTime);
                fantomeVert.choiceOfTexture();
            }

            if (!fantomeVert.IsPeur)
            {
                // Timer qui permet de gérer le temps entre deux actions des fantômes
                if (fantomeVert.Timer.lancerTimerFantome(gameTime, better))
                {
                    if (fantomeVert.choixFantomeIA() && !pacSamourai.IsMort)
                    {
                        if (pacSamourai.Invincible)
                        {
                            // On fait appel à la fonction suivante pour savoir s'il est toujours en mode peur
                            pacSamourai.addFantome(fantomeVert.TypeFantome, new Point(fantomeVert.PositionXFantome, fantomeVert.PositionYFantome));
                            fantomeVert.IsPeur = fantomeVert.Timer.gererTimerPeur(gameTime);
                            fantomeVert.choiceOfTexture();
                        }
                        else
                        {
                            pacSamourai.MortDePac = true;
                        }
                    }
                }
            }
            else
            {
                fantomeVert.IsPeur = fantomeVert.Timer.gererTimerPeur(gameTime);
                if (!fantomeVert.IsPeur)
                {
                    fantomeVert.choiceOfTexture();
                }
            }

            //Si le Pacsamurai a perdu une vie mais n'était pas déjà mort
            if (pacSamourai.MortDePac && !pacSamourai.IsMort)
            {
                pacSamourai.Vies--;
                pacSamourai.MortDePac = false;
                pacSamourai.IsMort = pacSamourai.Timer.gererTimerMort(gameTime);
                pacSamourai.Texture = pacSamourai.ListeTextures[8];
            }
        }

        // Fonction permettant de gérer les clicks sur le menu commande
        private void UpdateCommandScreen()
        {
            if (mpressed && isClickButton(mx, my, 469, 678, 144, 34))
            {
                state = GameState.menuScreen;
            }
        }

        //Mise à jour du fichier des scores
        private void UpdateFileScore(int score)
        {
            //Nom du dossier contenant le fichier des scores
            string pathString = @"Content/scores/scores.txt";

            if (!File.Exists(pathString))
            {
                File.WriteAllText(pathString, score.ToString());
            }
            else
            {
                File.AppendAllText(pathString, " ");
                File.AppendAllText(pathString, score.ToString());
            }
        }

        // Fonction permettant de créer le menu score
        private void CreateScoreScreen()
        {
            //Nom du dossier contenant le fichier des scores
            string pathString = @"Content/scores/scores.txt";

            //Si le fichier score existe
            if (File.Exists(pathString))
            {
                List<int> scores = new List<int>();
                //Lecture du fichier des scores et affichage dans le menu scores
                using (StreamReader reader = new StreamReader(pathString))
                {
                    string score;
                    while ((score = reader.ReadLine()) != null)
                    {
                        string[] strings = score.Split(' ');
                        for (int i = 0; i < strings.Length; i++)
                        {
                            scores.Add(int.Parse(strings[i]));
                        }
                    }
                }
                scores.Sort();
                scores.Reverse();

                //Affichage du titre "Scores"
                try
                {
                    spriteBatch.DrawString(menuFont, "Scores", new Vector2(200, 20), Color.White);
                }
                catch (Exception e)
                {
                    e.ToString();
                }

                int k = 1;
                //Affichage des dix meilleurs scores
                while (k <= scores.Count && k <= 10)
                {
                    string score;
                    if (scores[k - 1] != 0)
                    {
                        score = k + " : " + scores[k - 1].ToString() + " points";
                    }
                    else
                    {
                        score = k + " : " + scores[k - 1].ToString() + " point";
                    }

                    try
                    {
                        spriteBatch.DrawString(font, score, new Vector2(245, (k - 1) * 50 + 200), Color.White);
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }

                    k++;
                }

                //Affichage du "Retour"
                try
                {
                    spriteBatch.DrawString(font, "Retour", new Vector2(535, 700), Color.White);
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
        }

        // Fonction permettant de gérer les clicks sur le menu score
        private void UpdateScoreScreen()
        {
            if (mpressed && isClickButton(mx, my, 536, 700, 87, 25))
            {
                state = GameState.menuScreen;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin();

            // Gestion du menu
            switch (state)
            {
                case GameState.menuScreen:
                    spriteBatch.Draw(Content.Load<Texture2D>("menu"), GraphicsDevice.Viewport.TitleSafeArea, Color.White);
                    break;
                case GameState.gameScreen:
                    DrawGamePlay();
                    break;
                case GameState.wonScreen:
                    spriteBatch.Draw(Content.Load<Texture2D>("menuGagne"), GraphicsDevice.Viewport.TitleSafeArea, Color.White);
                    break;
                case GameState.lostScreen:
                    spriteBatch.Draw(Content.Load<Texture2D>("menuPerdu"), GraphicsDevice.Viewport.TitleSafeArea, Color.White);
                    break;
                case GameState.commandScreen:
                    spriteBatch.Draw(Content.Load<Texture2D>("menuCommandes"), GraphicsDevice.Viewport.TitleSafeArea, Color.White);
                    break;
                case GameState.scoreScreen:
                    CreateScoreScreen();
                    break;
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Fonction qui affiche la map
        private void DrawGamePlay()
        {
            //Tailles des tiles de la MAP
            const int tailleWidth = 32;
            const int tailleHeight = 32;

            //Chargement des textures de la MAP d'après le fichier .txt chargé
            for (int x = 0; x < useMap.MaxMapX; x++)
            {
                for (int y = 0; y < useMap.MaxMapY; y++)
                {
                    Vector2 coord = new Vector2(y * tailleWidth, x * tailleHeight);

                    switch (useMap.MapGame[x, y])
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
        }

        //Gérer le click droit sur les différents menus
        private Boolean isClickButton(float mx, float my, int xBouton, int yBouton, int widthBouton, int heightBouton)
        {
            if (mx >= xBouton && mx <= xBouton + widthBouton && my >= yBouton && my <= yBouton + heightBouton)
            {
                return true;
            }
            else
            { 
                return false; 
            }
        }
        
    }
}
