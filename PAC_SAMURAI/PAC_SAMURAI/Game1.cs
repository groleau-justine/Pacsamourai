using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
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

        private Pacsamurai pacSamourai;
        private Objet mur;
        private Objet sushi, maki, cerises;
        //ObjetAnime fantomeN;

        //Gestion clavier
        private KeyboardState oldState;
        private Clavier clavier;
        private SpriteFont font;
        
        private Timers timer;

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
            //List<Texture2D> textureFantomeN = new List<Texture2D>();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Chargement des dimensions de la fenêtre de jeu
            graphics.PreferredBackBufferWidth = 21*32;
            graphics.PreferredBackBufferHeight = 770;
            graphics.ApplyChanges();

            // Read the file and put it in useMap
            // Chargement des textures et du fichier de la MAP
            mur = new Objet(Content.Load<Texture2D>("mur01"));
            sushi = new Objet(Content.Load<Texture2D>("sushi"));
            maki = new Objet(Content.Load<Texture2D>("maki"));
            cerises = new Objet(Content.Load<Texture2D>("cerises"));

            // Chargement de l'ensemble des textures du PacSamourai
            //Texture mode normal
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_haut"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_droite"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_bas"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_gauche"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_hautO"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_droiteO"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_basO"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_gaucheO"));
            //Texture mode invincible
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_hautI"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_droiteI"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_basI"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_gaucheI"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_hautIO"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_droiteIO"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_basIO"));
            textureInvPacSamourai.Add(Content.Load<Texture2D>("pacsamourai_gaucheIO"));

            // Chargement de l'ensemble des textures du fantôme
            //textureFantomeN.Add(Content.Load<Texture2D>("fantomeN_bas"));
            //textureFantomeN.Add(Content.Load<Texture2D>("fantomeN_droite"));
            //textureFantomeN.Add(Content.Load<Texture2D>("fantomeN_bas"));
            //textureFantomeN.Add(Content.Load<Texture2D>("fantomeN_gauche"));

            
            //fantomeN = new ObjetAnime(textureFantomeN);
            useMap.loadMap();

            pacSamourai = new Pacsamurai(texturePacSamourai, textureInvPacSamourai, useMap);

            font = Content.Load<SpriteFont>("FontPacsamurai");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
                this.Exit();

            /***** GESTION DU CLAVIER *****/         
            clavier.update(gameTime);

            /*** Gestion des bonus ***/
            timer.lancerTimerBonus(gameTime);

            /*** Gestion du pouvoir d'invincibilité du Pacsamurai ***/

            // Si le pacsamurai est dans un mode invincible
            if (pacSamourai.Invincible)
            {
                // On fait appel à la fonction suivante pour savoir s'il est toujours en mode
                pacSamourai.Invincible = timer.gererTimerInvincible(gameTime);
            }
            else
            {
                //On remet sa texture en mode normal
                pacSamourai.Texture = pacSamourai.ListeTextures[pacSamourai.NumTexture];
            }

            // TODO: Add your update logic here
            base.Update(gameTime);
          
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            // Affichage de la MAP
            useMap.showMap(spriteBatch, mur, pacSamourai, sushi, maki, cerises, font); //Ajouter fantomeN...

            base.Draw(gameTime);
        }
    }
}
