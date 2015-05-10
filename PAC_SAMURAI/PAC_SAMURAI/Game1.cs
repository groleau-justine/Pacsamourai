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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Map useMap;

        Objet mur;
        ObjetAnime pacSamourai;

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
            // TODO: Add your initialization logic here
            // Création de la MAP
            useMap = new Map(0);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Variable contenant les textures du PacSamourai
            List<Texture2D> texturePacSamourai = new List<Texture2D>();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Chargement des dimensions de la fenêtre de jeu
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();

            // Read the file and put it in useMap
            // Chargement des textures et du fichier de la MAP
            mur = new Objet(Content.Load<Texture2D>("mur01"));

            // Chargement de l'ensemble des textures du PacSamourai
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_haut"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_droite"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_bas"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_gauche"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_hautO"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_droiteO"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_basO"));
            texturePacSamourai.Add(Content.Load<Texture2D>("pacsamourai_gaucheO"));

            pacSamourai = new ObjetAnime(texturePacSamourai);
            useMap.loadMap();
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
            useMap.showMap(spriteBatch, mur, pacSamourai);

            base.Draw(gameTime);
        }
    }
}
