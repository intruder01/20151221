using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OpenHTM.CLA;
using OpenHTM.CLA.Statistics;
//using OpenHTM.Shared.Interfaces;


namespace OpenHTM.IDE
{

	public delegate void SimEngineEvent_ObjectSelectionChanged ( object sender, EventArgs e );
	
	/// <summary>
	/// A delegate type for hooking up change notifications.
	/// </summary>

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	internal class Simulation3DEngine : Game
	{
		public event SimEngineEvent_ObjectSelectionChanged SelectionChangedEvent = delegate { };
		


		#region Fields

		#region HTM

		/// <summary>
		/// 2 dimensional Array to grab predition information
		/// </summary>
		private float[,] _predictions;

		/// <summary>
		/// 2 dimensional Array to grab predition information
		/// </summary>
		private float[,] _activeColumns;

		#endregion

		#region Graphics

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private IntPtr _drawSurface;
		private SpriteFont _spriteFont;
		private Color _clearColor = Color.CornflowerBlue;

		// Primitives
		private CubePrimitive _cube;
		private CoordinateSysPrimitive _coordinateSystem;
		private SquarePrimitve _bit;
		private LinePrimitive _connectionLine;

		// Global Matrices
		private Matrix _viewMatrix;
		private Matrix _projectionMatrix;

		// Colors
		private Dictionary<HtmCellColors, HtmColorInformation> _dictionaryCellColors;
		private Dictionary<HtmProximalSynapseColors, HtmColorInformation> _dictionaryProximalSynapseColors;
		private Dictionary<HtmDistalSynapseColors, HtmColorInformation> _dictionaryDistalSynapseColors;

		// Right Legend-Elements
		private HtmOverViewInformation _rightLegend;

		// Coordinates constants
		private const float _zHtmRegion = -0.0f;	//was-5.0f
		private const float _zHtmPlane = -0f; // Shift to the side //was -5.0f
		private const float _yHtmPlane = -5f; // Shift down
		
		private bool _contentLoaded;

		//input state varialbles
		KeyboardState keyState, prevKeyState;
		MouseState mouseState, prevMouseState;

		public Microsoft.Xna.Framework.Point mouseLocation;
		public Microsoft.Xna.Framework.Point mouseLocationClick;
		bool mouseLClick, mouseRClick;

		Color mouseOverColor = new Color ( 0, 0, 0 );
		Color selectedColor = Color.Red;
		Color color1 = new Color ( 0, 0, 0 );
		Color color2 = new Color ( 255, 255, 255 );
		int colorChangeMilliseconds = 0;
	


		#endregion

		#region Grid

		private Texture2D _gridTexture;
		private Vector2 _gridSize = new Vector2(14f);

		private const int _gridHeight = 7;
		private const int _gridWidth = 7;

		// Buffering space dimentions 
		private const int _gridHeightBuffer = 5;
		private const int _gridWidthBuffer = 10;

		#endregion

		#region Camera

		// Camera parameters for view matrix
		private Vector3 _lookAt = new Vector3(0,0,0);
		private Vector3 _posCamera = new Vector3 (0, 0, 30);

		// A yaw and pitch applied to the second viewport based on input
		private float _yawHtm;
		private float _pitchHtm;

		private float _yawCamera;
		private float _pitchCamera;

		private float _zoomCamera;

		private float _aspectRatio;
		private float sensitivity = 0.2f;
		private float _moveSpeedCamera = 1f;
		private float _rotateSpeedCamera = .002f;


		FPSCamera camera;
		#endregion


		#region Watch 

		public List<Selectable3DObject> WatchList = new List<Selectable3DObject> ();

		#endregion

		#endregion

		#region Properties

		/// <summary>
		/// Reference to HtmRegion
		/// </summary>
		public Region Region { get; set; }

		/// <summary>
		/// Reference to list of columsn from region for traversing
		/// </summary>
		public List<Column> HtmRegionColumns { get; private set; }

		#endregion

		#region Nested classes

		private struct HtmColorInformation
		{
			#region Fields

			public Color HtmColor;
			public string HtmInformation;

			#endregion

			#region Constructor

			public HtmColorInformation(Color color, string info)
			{
				this.HtmColor = color;
				this.HtmInformation = info;
			}

			#endregion
		}

		private class HtmOverViewInformation
		{
			#region Fields

			public string StepCount;
			public string ChosenHtmElement;
			public string PositionElement;
			public string ActivityRate;
			public string PrecisionRate;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the <see cref="HtmOverViewInformation"/> class.
			/// </summary>
			public HtmOverViewInformation()
			{
				this.StepCount = "";
				this.ChosenHtmElement = "Region";
				this.PositionElement = "";
				this.ActivityRate = "";
				this.PrecisionRate = "";
			}

			#endregion
		}

		private enum HtmCellColors
		{
			Learning,
			FalsePrediction,
			RightPrediction,
			Predicting,
			SequencePredicting,
			Active,
			Inactive,
			Selected,
			Inhibited
		}

		private enum HtmProximalSynapseColors
		{
			Default,
			Active,
			ActiveConnected
		}

		private enum HtmDistalSynapseColors
		{
			Default,
			Active
		}

		#endregion


		#region ArcBallCamera

		// File was taken from http://roy-t.nl/index.php/2010/02/21/xna-simple-arcballcamera/
		public class ArcBallCamera
		{
			#region Fields

			// We don't need an update method because the camera only needs updating
			// when we change one of it's parameters.
			// We keep track if one of our matrices is dirty
			// and reacalculate that matrix when it is accesed.
			private bool _viewMatrixDirty = true;
			private bool _projectionMatrixDirty = true;

			public float MinZoom = 1;
			public float MaxZoom = float.MaxValue;

			public float MinPitch = -MathHelper.PiOver2 + 0.3f;
			public float MaxPitch = MathHelper.PiOver2 - 0.3f;

			#endregion

			#region Properties

			public float Pitch
			{
				get
				{
					return this._pitch;
				}
				set
				{
					this._viewMatrixDirty = true;
					this._pitch = MathHelper.Clamp(value, this.MinPitch, this.MaxPitch);
				}
			}

			private float _pitch;

			public float Yaw
			{
				get
				{
					return this._yaw;
				}
				set
				{
					this._viewMatrixDirty = true;
					this._yaw = value;
				}
			}

			private float _yaw;

			public float FieldOfView
			{
				get
				{
					return this._fieldOfView;
				}
				set
				{
					this._projectionMatrixDirty = true;
					this._fieldOfView = value;
				}
			}

			private float _fieldOfView;

			public float AspectRatio
			{
				get
				{
					return this._aspectRatio;
				}
				set
				{
					this._projectionMatrixDirty = true;
					this._aspectRatio = value;
				}
			}

			private float _aspectRatio;

			public float NearPlane
			{
				get
				{
					return this._nearPlane;
				}
				set
				{
					this._projectionMatrixDirty = true;
					this._nearPlane = value;
				}
			}

			private float _nearPlane;

			public float FarPlane
			{
				get
				{
					return this._farPlane;
				}
				set
				{
					this._projectionMatrixDirty = true;
					this._farPlane = value;
				}
			}

			private float _farPlane;

			public float Zoom
			{
				get
				{
					return this._zoom;
				}
				set
				{
					this._viewMatrixDirty = true;
					this._zoom = MathHelper.Clamp(value, this.MinZoom, this.MaxZoom);
				}
			}

			private float _zoom = 1;

			public Vector3 Position
			{
				get
				{
					if (this._viewMatrixDirty)
					{
						this.ReCreateViewMatrix();
					}
					return this._position;
				}
			}

			private Vector3 _position;

			public Vector3 LookAt
			{
				get
				{
					return this._lookAt;
				}
				set
				{
					this._viewMatrixDirty = true;
					this._lookAt = value;
				}
			}

			private Vector3 _lookAt;

			public Matrix ViewProjectionMatrix
			{
				get
				{
					return this.ViewMatrix * this.ProjectionMatrix;
				}
			}

			public Matrix ViewMatrix
			{
				get
				{
					if (this._viewMatrixDirty)
					{
						this.ReCreateViewMatrix();
					}
					return this._viewMatrix;
				}
			}

			private Matrix _viewMatrix;

			public Matrix ProjectionMatrix
			{
				get
				{
					if (this._projectionMatrixDirty)
					{
						this.ReCreateProjectionMatrix();
					}
					return this._projectionMatrix;
				}
			}

			private Matrix _projectionMatrix;

			#endregion

			#region Constructor

			public ArcBallCamera(float aspectRation, Vector3 lookAt)
				: this(aspectRation, MathHelper.PiOver4, lookAt, Vector3.Up, 0.1f, float.MaxValue)
			{
			}

			public ArcBallCamera(float aspectRatio, float fieldOfView, Vector3 lookAt, Vector3 up, float nearPlane, float farPlane)
			{
				this._aspectRatio = aspectRatio;
				this._fieldOfView = fieldOfView;
				this._lookAt = lookAt;
				this._nearPlane = nearPlane;
				this._farPlane = farPlane;
			}

			#endregion

			#region Methods

			/// <summary>
			/// Recreates our view matrix, then signals that the view matrix
			/// is clean.
			/// </summary>
			private void ReCreateViewMatrix()
			{
				// Calculate the relative position of the camera                       
				this._position = Vector3.Transform(Vector3.Backward, Matrix.CreateFromYawPitchRoll(this._yaw, this._pitch, 0));

				// Convert the relative position to the absolute position
				this._position *= this._zoom;
				this._position += this._lookAt;

				// Calculate a new viewmatrix
				this._viewMatrix = Matrix.CreateLookAt(this._position, this._lookAt, Vector3.Up);
				this._viewMatrixDirty = false;
			}

			/// <summary>
			/// Recreates our projection matrix, then signals that the projection
			/// matrix is clean.
			/// </summary>
			private void ReCreateProjectionMatrix()
			{
				this._projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, this.AspectRatio, this._nearPlane, this._farPlane);
				this._projectionMatrixDirty = false;
			}

			/// <summary>
			/// Moves the camera and lookAt at to the right,
			/// as seen from the camera, while keeping the same height
			/// </summary>       
			public void MoveCameraRight(float amount)
			{
				Vector3 right = Vector3.Normalize(this.LookAt - this.Position); //calculate forward
				right = Vector3.Cross(right, Vector3.Up); //calculate the real right
				right.Y = 0;
				right.Normalize();
				this.LookAt += right * amount;
			}

			/// <summary>
			/// Moves the camera and lookAt forward,
			/// as seen from the camera, while keeping the same height
			/// </summary>       
			public void MoveCameraForward(float amount)
			{
				Vector3 forward = Vector3.Normalize(this.LookAt - this.Position);
				forward.Y = 0;
				forward.Normalize();
				this.LookAt += forward * amount;
			}

			#endregion
		}

		#endregion

		#region FPS Camera

		public class FPSCamera
		{
			#region Fields

			// We don't need an update method because the camera only needs updating
			// when we change one of it's parameters.
			// We keep track if one of our matrices is dirty
			// and reacalculate that matrix when it is accesed.
			private bool _viewMatrixDirty = true;
			private bool _projectionMatrixDirty = true;

			public float MinZoom = 1;
			public float MaxZoom = float.MaxValue;

			public float MinPitch = -MathHelper.PiOver2 + 0.3f;
			public float MaxPitch = MathHelper.PiOver2 - 0.3f;

			public float MinPosition = -1000f;
			public float MaxPosition = 1000f;
			
			#endregion

			#region Properties

			public float Pitch
			{
				get
				{
					return this._pitch;
				}
				set
				{
					this._viewMatrixDirty = true;
					this._pitch = MathHelper.Clamp ( value, this.MinPitch, this.MaxPitch );
				}
			}

			private float _pitch;

			public float Yaw
			{
				get
				{
					return this._yaw;
				}
				set
				{
					this._viewMatrixDirty = true;
					this._yaw = value;
				}
			}

			private float _yaw;

			public float FieldOfView
			{
				get
				{
					return this._fieldOfView;
				}
				set
				{
					this._projectionMatrixDirty = true;
					this._fieldOfView = value;
				}
			}

			private float _fieldOfView;

			public float AspectRatio
			{
				get
				{
					return this._aspectRatio;
				}
				set
				{
					this._projectionMatrixDirty = true;
					this._aspectRatio = value;
				}
			}

			private float _aspectRatio;

			public float NearPlane
			{
				get
				{
					return this._nearPlane;
				}
				set
				{
					this._projectionMatrixDirty = true;
					this._nearPlane = value;
				}
			}

			private float _nearPlane;

			public float FarPlane
			{
				get
				{
					return this._farPlane;
				}
				set
				{
					this._projectionMatrixDirty = true;
					this._farPlane = value;
				}
			}

			private float _farPlane;

			public float Zoom
			{
				get
				{
					return this._zoom;
				}
				set
				{
					this._viewMatrixDirty = true;
					this._zoom = MathHelper.Clamp ( value, this.MinZoom, this.MaxZoom );
				}
			}

			private float _zoom = 1;

			public Vector3 Position
			{
				get
				{
					if (this._viewMatrixDirty)
					{
						this.ReCreateViewMatrix ();
					}
					return this._position;
				}
				set
				{
					this._viewMatrixDirty = true;
					float valueX = MathHelper.Clamp ( value.X, this.MinPosition, this.MaxPosition );
					float valueY = MathHelper.Clamp ( value.Y, this.MinPosition, this.MaxPosition );
					float valueZ = MathHelper.Clamp ( value.Z, this.MinPosition, this.MaxPosition );
					this._position = new Vector3 ( valueX, valueY, valueZ );
				}
			}

			private Vector3 _position;

			public float MoveSpeed
			{
				get
				{
					return this._moveSpeed;
				}
				set
				{
					this._moveSpeed = value;
				}
			}
			private float _moveSpeed = 1f;


			public Vector3 LookAt
			{
				get
				{
					return this._lookAt;
				}
				set
				{
					this._viewMatrixDirty = true;
					this._lookAt = value;
				}
			}

			private Vector3 _lookAt;

			public Matrix ViewProjectionMatrix
			{
				get
				{
					return this.getViewMatrix * this.getProjectionMatrix;
				}
			}

			public Matrix getViewMatrix
			{
				get
				{
					if (this._viewMatrixDirty)
					{
						this.ReCreateViewMatrix ();
					}
					return this._viewMatrix;
				}
			}

			private Matrix _viewMatrix;

			public Matrix getProjectionMatrix
			{
				get
				{
					if (this._projectionMatrixDirty)
					{
						this.ReCreateProjectionMatrix ();
					}
					return this._projectionMatrix;
				}
			}

			private Matrix _projectionMatrix;

			#endregion

			#region Constructor

			public FPSCamera ( float aspectRatio, Vector3 lookAt )
				: this ( aspectRatio, MathHelper.PiOver4, lookAt, Vector3.Up, 0.1f, float.MaxValue )
			{
			}

			public FPSCamera ( float aspectRatio, float fieldOfView, Vector3 lookAt, Vector3 up, float nearPlane, float farPlane )
			{
				this._aspectRatio = aspectRatio;
				this._fieldOfView = fieldOfView;
				this._lookAt = lookAt;
				this._nearPlane = nearPlane;
				this._farPlane = farPlane;
			}

			#endregion

			#region Methods

			//http://ploobs.com.br/?p=1507

			private void ReCreateViewMatrix ()
			{
				Matrix cameraRotation = Matrix.CreateRotationX ( this._pitch ) * Matrix.CreateRotationY ( this._yaw );
				Vector3 cameraOriginalTarget = new Vector3 ( 0, 0, -1 );
				Vector3 cameraOriginalUpVector = new Vector3 ( 0, 1, 0 );
				Vector3 cameraRotatedTarget = Vector3.Transform ( cameraOriginalTarget, cameraRotation );
				Vector3 target = this._position + cameraRotatedTarget;
				Vector3 up = Vector3.Transform ( cameraOriginalUpVector, cameraRotation );
				this._viewMatrix = Matrix.CreateLookAt ( this._position, target, up );
			}

			/// <summary>
			/// Recreates our view matrix, then signals that the view matrix
			/// is clean.
			/// </summary>
			private void ReCreateViewMatrix_old ()
			{
				// Calculate the relative position of the camera                       
				this._position = Vector3.Transform ( Vector3.Backward, Matrix.CreateFromYawPitchRoll ( this._yaw, this._pitch, 0 ) );

				// Convert the relative position to the absolute position
				this._position *= this._zoom;
				this._position += this._lookAt;

				// Calculate a new viewmatrix
				this._viewMatrix = Matrix.CreateLookAt ( this._position, this._lookAt, Vector3.Up );
				this._viewMatrixDirty = false;
			}

			/// <summary>
			/// Recreates our projection matrix, then signals that the projection
			/// matrix is clean.
			/// </summary>
			private void ReCreateProjectionMatrix ()
			{
				this._projectionMatrix = Matrix.CreatePerspectiveFieldOfView ( MathHelper.PiOver4, this.AspectRatio, this._nearPlane, this._farPlane );
				this._projectionMatrixDirty = false;
			}

			/// <summary>
			/// Moves the camera and lookAt at to the right,
			/// as seen from the camera, while keeping the same height
			/// </summary>       
			public void MoveCameraRight ( float amount )
			{
				Vector3 right = Vector3.Normalize ( this.LookAt - this.Position ); //calculate forward
				right = Vector3.Cross ( right, Vector3.Up ); //calculate the real right
				right.Y = 0;
				right.Normalize ();
				this.LookAt += right * amount;
			}

			/// <summary>
			/// Moves the camera and lookAt forward,
			/// as seen from the camera, while keeping the same height
			/// </summary>       
			public void MoveCameraForward ( float amount )
			{
				Vector3 forward = Vector3.Normalize ( this.LookAt - this.Position );
				forward.Y = 0;
				forward.Normalize ();
				this.LookAt += forward * amount;
			}

			#endregion
		}

		//end of FPS Cam


		#endregion


		#region Constructor

		public Simulation3DEngine(IntPtr drawSurface)
		{
			this._graphics = new GraphicsDeviceManager(this);
			this._drawSurface = drawSurface;

			this._graphics.PreparingDeviceSettings += this.graphics_PreparingDeviceSettings;
			Control.FromHandle(this.Window.Handle).VisibleChanged += this.Game1_VisibleChanged;
		}

		#endregion

		#region Methods

		#region Form

		/// <summary> 
		/// Occurs when the original gamewindows' visibility changes and makes sure it stays invisible 
		/// </summary> 
		private void Game1_VisibleChanged(object sender, EventArgs e)
		{
			if (Control.FromHandle(this.Window.Handle).Visible)
			{
				Control.FromHandle(this.Window.Handle).Visible = false;
			}
		}

		#endregion

		#region Graphics device

		public void ResetGraphicsDevice()
		{
			// Avoid entering until hosting surface control is setup and device created
			if (this.GraphicsDevice != null && Simulation3D.Form.pictureBoxSurface.Width != 0 && Simulation3D.Form.pictureBoxSurface.Height != 0)
			{
				// Change back buffer size
				this._graphics.PreferredBackBufferHeight = Simulation3D.Form.pictureBoxSurface.Height;
				this._graphics.PreferredBackBufferWidth = Simulation3D.Form.pictureBoxSurface.Width;
				this._graphics.ApplyChanges();
			}
		}

		#endregion

		#region Content

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			if (!this._contentLoaded)
			{
				// Create a new SpriteBatch, which can be used to draw textures.
				this._spriteBatch = new SpriteBatch(this.GraphicsDevice);

				// Load the sprite font
				this.Content.RootDirectory = "./Content/";
				this._spriteFont = this.Content.Load<SpriteFont>("Simulation3DFont");

				// Load the grid texture
				this._gridTexture = LoadTexture2D(this.GraphicsDevice, "./Content/Simulation3DTileGrid.png");

				// Load Cell Colors for net
				this._dictionaryCellColors = new Dictionary<HtmCellColors, HtmColorInformation>();
				this._dictionaryCellColors.Add(HtmCellColors.Active, new HtmColorInformation(Color.Black, "Cell is activated"));
				this._dictionaryCellColors.Add(HtmCellColors.Inactive, new HtmColorInformation(Color.White, "Cell is inactive"));
				this._dictionaryCellColors.Add(HtmCellColors.Learning, new HtmColorInformation(Color.DarkGray, "Cell is learning"));
				this._dictionaryCellColors.Add(HtmCellColors.Predicting, new HtmColorInformation(Color.Orange, "Cell is predicting (t+2...)"));
				this._dictionaryCellColors.Add(HtmCellColors.SequencePredicting, new HtmColorInformation(Color.Violet, "Cell is sequence predicting (t+1)"));
				this._dictionaryCellColors.Add(HtmCellColors.RightPrediction, new HtmColorInformation(Color.Green, "Cell correctly predicted"));
				this._dictionaryCellColors.Add(HtmCellColors.FalsePrediction, new HtmColorInformation(Color.Red, "Cell falsely predicted"));
				this._dictionaryCellColors.Add(HtmCellColors.Selected, new HtmColorInformation(Color.Brown, "Cell prediction is lost"));
				this._dictionaryCellColors.Add(HtmCellColors.Inhibited, new HtmColorInformation(Color.Navy, "Column is inhibited"));

				this._dictionaryProximalSynapseColors = new Dictionary<HtmProximalSynapseColors, HtmColorInformation> ();
				this._dictionaryProximalSynapseColors.Add ( HtmProximalSynapseColors.Default, new HtmColorInformation ( Color.White, "Proximal synapse not active, not connected" ) );
				this._dictionaryProximalSynapseColors.Add ( HtmProximalSynapseColors.ActiveConnected, new HtmColorInformation ( Color.Green, "Proximal synapse is connected" ) );
				this._dictionaryProximalSynapseColors.Add ( HtmProximalSynapseColors.Active, new HtmColorInformation ( Color.Orange, "Proximal synapse is active" ) );

				this._dictionaryDistalSynapseColors = new Dictionary<HtmDistalSynapseColors, HtmColorInformation> ();
				this._dictionaryDistalSynapseColors.Add ( HtmDistalSynapseColors.Default, new HtmColorInformation ( Color.White, "Distal synapse not active" ) );
				this._dictionaryDistalSynapseColors.Add ( HtmDistalSynapseColors.Active, new HtmColorInformation ( Color.Black, "Distal synapse not active" ) );

				// Create OverviewElement
				this._rightLegend = new HtmOverViewInformation();

				// Get referencees for traversing regions
				this.Region = NetControllerForm.Instance.SelectedNode.Region;
				this.HtmRegionColumns = this.Region.Columns;

				// Prepare Array for 2-dim-content
				this._predictions = new float[this.Region.Size.Width,this.Region.Size.Height];
				this._activeColumns = new float[this.Region.Size.Width,this.Region.Size.Height];

				// Prepare Cube
				this._cube = new CubePrimitive(this.GraphicsDevice);
				this._coordinateSystem = new CoordinateSysPrimitive(this.GraphicsDevice);
				this._bit = new SquarePrimitve(this.GraphicsDevice);
				this._connectionLine = new LinePrimitive(this.GraphicsDevice);

				this.ResetCamera();

				this._contentLoaded = true;

				//SimulationEngineStarted(this, new EventArgs());

				//test only
				//01001_2x1 Cell select
				//test populating WatchList witho no windows displayed
				if (Properties.Settings.Default.StealthMode)
				{
					Ray ray = getPickingRay ( new System.Drawing.Point ( 331, 301 ) );

					PickHtmRegion ( ray, true );
					//WatchTreeForm.Instance.Handler_SimSelectionChanged ( WatchList, EventArgs.Empty );here
					WatchForm.Instance.Handler_SimSelectionChanged ( NetControllerForm.Instance.TopNode.Region, EventArgs.Empty );
				}
			}
		}

		public static Texture2D LoadTexture2D(GraphicsDevice graphicsDevice, string path)
		{
			return Texture2D.FromStream(graphicsDevice, new StreamReader(path).BaseStream);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		#endregion





		protected override void Initialize ()
		{
			this._aspectRatio = Simulation3D.Form.pictureBoxSurface.Width / Simulation3D.Form.pictureBoxSurface.Height;
			camera = new FPSCamera ( this._aspectRatio, this._lookAt );
			
			base.Initialize ();
		}
		#region Update

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			this.UpdateCamera();

			//store input variables for this scan
			keyState = Keyboard.GetState ();
			prevKeyState = keyState;
			mouseState = Mouse.GetState ();
			prevMouseState = mouseState;
			//mouseGetStateLocation = new System.Drawing.Point ( mouseState.X, mouseState.Y );
			mouseLClick = (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed) && (prevMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released);
			mouseRClick = (mouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed) && (prevMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released);


			//mouseOver color change
			colorChangeMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
			if (colorChangeMilliseconds >= 150)
			{
				if (mouseOverColor == color1)
					mouseOverColor = color2;
				else
					mouseOverColor = color1;

				colorChangeMilliseconds = 0;
			}

			if (keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.Up ) || keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.W ))      //Forward
			{
				AddToCameraPosition ( new Vector3 ( 0, 0, -sensitivity ) );
			}
			if (keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.Down ) || keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.S ))    //Backward
			{
				AddToCameraPosition ( new Vector3 ( 0, 0, sensitivity ) );
			}
			if (keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.Right ) || keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.D ))   //Right
			{
				AddToCameraPosition ( new Vector3 ( sensitivity, 0, 0 ) );
			}
			if (keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.Left ) || keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.A ))    //Left
			{
				AddToCameraPosition ( new Vector3 ( -sensitivity, 0, 0 ) );
			}
			if (keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.Q ))                                     //Up
			{
				AddToCameraPosition ( new Vector3 ( 0, sensitivity, 0 ) );
			}
			if (keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.Z ))                                     //Down
			{
				AddToCameraPosition ( new Vector3 ( 0, -sensitivity, 0 ) );
			}

			base.Update(gameTime);
		}

		
		/// <summary>
		/// Reset Camera to default rotation angle and zoom factor values.
		/// </summary>
		protected internal void ResetCamera()
		{
			//position camera relative to region size
			//Y=4 slightly raised, 
			//pitch =-10 look slightly down
			//Z=Size/3 + 3 - shift to right to give angled view (+ 3 provides shift for small regions)
			this._posCamera = new Vector3 ( -25, 4, GetSize().X/3 + 3 );
			
			// Reset rotation angle for camera
			this._yawCamera = (float)MathHelper.ToRadians(-90);
			this._pitchCamera = (float)MathHelper.ToRadians ( -10 );

			// Reset rotation angle for htm-objects
			this._yawHtm = 0f;
			this._pitchHtm = 0f;

			// Reset zoom
			this._zoomCamera = 35f;

			this.UpdateCamera();
		}

		/// <summary>
		/// Set camera zoom
		/// </summary>
		public void SetCameraZoom(int zoomFactor)
		{
			if (zoomFactor < 0)
			{
				// Zoom out
				this._zoomCamera -= 0.1f * this._zoomCamera;
			}
			else if (zoomFactor > 0)
			{
				// Zoom in
				this._zoomCamera += 0.1f * this._zoomCamera;
			}
		}

		/// <summary>
		/// Rotation angle for camera in world space
		/// </summary>
		public void RotateWorldSpaceCamera(float diffX, float diffY)
		{
			this._yawCamera += diffX * _rotateSpeedCamera;
			this._pitchCamera += diffY * _rotateSpeedCamera;
		}

		/// <summary>
		/// Rotation angle for htm-objects in world space
		/// </summary>
		public void RotateWorldSpaceHtmObjects(float diffX, float diffY)
		{
			this._yawHtm += diffX * _rotateSpeedCamera;
			this._pitchHtm += diffY * _rotateSpeedCamera;
		}

		public void AddToCameraPosition ( Vector3 vectorToAdd )
		{
			Matrix cameraRotation = Matrix.CreateRotationX ( this._pitchCamera ) * Matrix.CreateRotationY ( this._yawCamera );
			Vector3 rotatedVector = Vector3.Transform ( vectorToAdd, cameraRotation );
			this._posCamera += this._moveSpeedCamera * rotatedVector;
			//ReCreateViewMatrix ();


		}
		/// <summary>
		/// Move Camera according to rotation angle and zoom factor.
		/// </summary>
		private void UpdateCamera()
		{
			this._aspectRatio =
				(float) this.GraphicsDevice.PresentationParameters.BackBufferWidth /
				this.GraphicsDevice.PresentationParameters.BackBufferHeight;

			camera = new FPSCamera ( this._aspectRatio, this._lookAt );
			camera.Position = this._posCamera;
			camera.Zoom = this._zoomCamera;
			camera.Pitch = this._pitchCamera;
			camera.Yaw = this._yawCamera;
			this._viewMatrix = camera.getViewMatrix;
			this._projectionMatrix = camera.getProjectionMatrix;
		}


		#endregion

		#region Draw

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(this._clearColor);

			// Draw Legend:
			this.DrawLegend();

			// Draw HTM
			this.DrawHtmPlane();
			this.DrawHtmRegion(false);
			this.DrawHtmRegion(true);

			// Draw Prediction Plane
			this.DrawHtmRegionPredictionPlane();

			// Draw Active Columns Plane
			this.DrawHtmActiveColsPlane();

			// Draw CoordinateSystem:
			if (Simulation3D.Form.ShowCoordinateSystem)
			{
				this._coordinateSystem.Draw(this._viewMatrix, this._projectionMatrix);
			}



			base.Draw(gameTime);
		}

		/// <summary>
		/// Gets statistical info to fill HTMOverview for Legend
		/// </summary>
		/// <param name="statistics"></param>
		private void FillHtmOveriew(object element)
		{
			var statistics = new Statistics();

			if (element is Region)
			{
				var region = element as Region;
				statistics = region.Statistics;

				// Set Legend-Information on the right:
				this._rightLegend.ChosenHtmElement = "Region";
				this._rightLegend.PositionElement = "";
			}
			else if (element is Column)
			{
				var column = element as Column;
				statistics = column.Statistics;

				// Set Legend-Information on the right:
				this._rightLegend.ChosenHtmElement = "Column";
				this._rightLegend.PositionElement = column.PositionInRegion.ToString();
			}
			else if (element is Cell)
			{
				var cell = element as Cell;
				statistics = cell.Statistics;

				// Set Legend-Information on the right:
				this._rightLegend.ChosenHtmElement = "Cell";
				this._rightLegend.PositionElement = cell.Column.PositionInRegion.ToString() + "-: " + cell.Index;
			}

			this._rightLegend.StepCount = statistics.StepCounter.ToString();
			this._rightLegend.ActivityRate = statistics.ActivityRate.ToString();
			this._rightLegend.PrecisionRate = statistics.PredictPrecision.ToString();
		}

		/// <summary>
		/// Draws legend for HTM-Algorithm on the left and right side of the animation.
		/// </summary>
		private void DrawLegend()
		{
			var gridWidth = (int) this._gridSize.X;
			var gridHeight = (int) this._gridSize.Y;
			const int gridHeightBuffer = 5;
			const int gridWidthBuffer = 30;

			var startVectorLeft = new Vector2(20, 25);
			var startVectorRight = new Vector2(this.GraphicsDevice.PresentationParameters.BackBufferWidth - 250, 25);
			var startVectorRightTab = new Vector2(startVectorRight.X + 120.0f, startVectorRight.Y);

			// Draw left legend
			this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, null, null);
			this._spriteBatch.DrawString(this._spriteFont, "Legend", startVectorLeft, Color.Black);

			foreach (var item in this._dictionaryCellColors)
			{
				startVectorLeft.Y += gridHeight + gridHeightBuffer;
				this._spriteBatch.Draw(this._gridTexture, new Rectangle((int) startVectorLeft.X, (int) startVectorLeft.Y, gridWidth, gridHeight), item.Value.HtmColor);
				this._spriteBatch.DrawString(this._spriteFont, item.Value.HtmInformation, new Vector2(startVectorLeft.X + gridWidthBuffer, startVectorLeft.Y), Color.White);
			}

			// Draw right legend
			string str;
			this._spriteBatch.DrawString(this._spriteFont, "HTM Information", startVectorRight, Color.Black);
			startVectorRight.Y += gridHeight + gridHeightBuffer + gridHeightBuffer;
			startVectorRightTab.Y += gridHeight + gridHeightBuffer + gridHeightBuffer;
			this._spriteBatch.DrawString(this._spriteFont, "Steps: ", startVectorRight, Color.White);
			this._spriteBatch.DrawString(this._spriteFont, this._rightLegend.StepCount, startVectorRightTab, Color.White);
			startVectorRight.Y += gridHeight + gridHeightBuffer;
			startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			this._spriteBatch.DrawString(this._spriteFont, "Chosen: ", startVectorRight, Color.White);
			this._spriteBatch.DrawString(this._spriteFont, this._rightLegend.ChosenHtmElement, startVectorRightTab, Color.White);
			startVectorRight.Y += gridHeight + gridHeightBuffer;
			startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			this._spriteBatch.DrawString(this._spriteFont, "Position: ", startVectorRight, Color.White);
			this._spriteBatch.DrawString(this._spriteFont, this._rightLegend.PositionElement, startVectorRightTab, Color.White);
			startVectorRight.Y += gridHeight + gridHeightBuffer;
			startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			this._spriteBatch.DrawString(this._spriteFont, "Activity Rate: ", startVectorRight, Color.White);
			this._spriteBatch.DrawString(this._spriteFont, this._rightLegend.ActivityRate, startVectorRightTab, Color.White);
			startVectorRight.Y += gridHeight + gridHeightBuffer;
			startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			this._spriteBatch.DrawString(this._spriteFont, "Precision: ", startVectorRight, Color.White);
			this._spriteBatch.DrawString(this._spriteFont, this._rightLegend.PrecisionRate, startVectorRightTab, Color.White);

			//debug js
			//startVectorRight.Y += gridHeight + gridHeightBuffer;
			//startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			//this._spriteBatch.DrawString ( this._spriteFont, "MouseGetStateLoc: ", startVectorRight, Color.White );
			//string sMouseLoc = string.Format ( "X {0}  Y {1}", mouseGetStateLocation.X, mouseGetStateLocation.Y );
			//this._spriteBatch.DrawString ( this._spriteFont, sMouseLoc, startVectorRightTab, Color.White );

			startVectorRight.Y += gridHeight + gridHeightBuffer;
			startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			this._spriteBatch.DrawString ( this._spriteFont, "MouseLoc: ", startVectorRight, Color.White );
			str = string.Format ( "X {0}  Y {1}", mouseLocation.X, mouseLocation.Y );
			this._spriteBatch.DrawString ( this._spriteFont, str, startVectorRightTab, Color.White );

			//startVectorRight.Y += gridHeight + gridHeightBuffer;
			//startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			//this._spriteBatch.DrawString ( this._spriteFont, "MouseLocClick: ", startVectorRight, Color.White );
			//str = string.Format ( "X {0}  Y {1}", mouseLocationClick.X, mouseLocationClick.Y );
			//this._spriteBatch.DrawString ( this._spriteFont, str, startVectorRightTab, Color.White );

			//startVectorRight.Y += gridHeight + gridHeightBuffer;
			//startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			//this._spriteBatch.DrawString ( this._spriteFont, "minDistProximal: ", startVectorRight, Color.White );
			//str = string.Format ( "{0:0.000}", minDistanceProximal );
			//this._spriteBatch.DrawString ( this._spriteFont, str, startVectorRightTab, Color.White );

			//startVectorRight.Y += gridHeight + gridHeightBuffer;
			//startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			//this._spriteBatch.DrawString ( this._spriteFont, "pickDistProximal: ", startVectorRight, Color.White );
			//str = string.Format ( "{0:0.000}", pickDistanceProximal );
			//this._spriteBatch.DrawString ( this._spriteFont, str, startVectorRightTab, Color.White );

			//startVectorRight.Y += gridHeight + gridHeightBuffer;
			//startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			//this._spriteBatch.DrawString ( this._spriteFont, "minDistDistal: ", startVectorRight, Color.White );
			//str = string.Format ( "{0:0.000}", minDistanceDistal );
			//this._spriteBatch.DrawString ( this._spriteFont, str, startVectorRightTab, Color.White );

			//startVectorRight.Y += gridHeight + gridHeightBuffer;
			//startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			//this._spriteBatch.DrawString ( this._spriteFont, "pickDistDistal: ", startVectorRight, Color.White );
			//str = string.Format ( "{0:0.000}", pickDistanceDistal );
			//this._spriteBatch.DrawString ( this._spriteFont, str, startVectorRightTab, Color.White );

			startVectorRight.Y += gridHeight + gridHeightBuffer;
			startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			this._spriteBatch.DrawString ( this._spriteFont, "_aspectRatio: ", startVectorRight, Color.White );
			str = string.Format ( "{0:0.00}", _aspectRatio );
			this._spriteBatch.DrawString ( this._spriteFont, str, startVectorRightTab, Color.White );

			startVectorRight.Y += gridHeight + gridHeightBuffer;
			startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			this._spriteBatch.DrawString ( this._spriteFont, "_pitch _yaw: ", startVectorRight, Color.White );
			str = string.Format ( "{0:0.00} {1:0.00}", _pitchCamera, _yawCamera );
			this._spriteBatch.DrawString ( this._spriteFont, str, startVectorRightTab, Color.White );

			startVectorRight.Y += gridHeight + gridHeightBuffer;
			startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			this._spriteBatch.DrawString ( this._spriteFont, "_posCam: ", startVectorRight, Color.White );
			str = string.Format ( "X={0:0.0} Y={1:0.0} Z={2:0.0} ", _posCamera.X, _posCamera.Y, _posCamera.Z );
			//str = string.Format ( "{0:0.00}", _posCamera );
			this._spriteBatch.DrawString ( this._spriteFont, str, startVectorRightTab, Color.White );

			startVectorRight.Y += gridHeight + gridHeightBuffer;
			startVectorRightTab.Y += gridHeight + gridHeightBuffer;
			this._spriteBatch.DrawString ( this._spriteFont, "_lookAt: ", startVectorRight, Color.White );
			//str = string.Format ( "{0:0.00}", _lookAt );
			str = string.Format ( "X={0:0.0} Y={1:0.0} Z={2:0.0} ", _lookAt.X, _lookAt.Y, _lookAt.Z );
			this._spriteBatch.DrawString ( this._spriteFont, str, startVectorRightTab, Color.White );


			this._spriteBatch.End();
		}

		/// <summary>
		/// Draws the active cols as flat 2-d array-map
		/// </summary>
		private void DrawHtmActiveColsPlane()
		{
			if (!Simulation3D.Form.ShowActiveColumnGrid)
			{
				return;
			}

			// Compute starting point
			int x = this.GraphicsDevice.PresentationParameters.BackBufferWidth -
			        this._activeColumns.GetLength(1) * (_gridWidth + _gridWidthBuffer);
			int y = this.GraphicsDevice.PresentationParameters.BackBufferHeight -
			        this._activeColumns.GetLength(0) * (_gridHeight + _gridHeightBuffer) - 50;
			var startVectorLeft = new Vector2(x, y);

			this.DrawHtmActivationMap(startVectorLeft, this._activeColumns, "Active Columns");
		}

		/// <summary>
		/// Draws the region prediction as flat 2-d array-map
		/// </summary>
		private void DrawHtmRegionPredictionPlane()
		{
			if (!Simulation3D.Form.ShowPredictedGrid)
			{
				return;
			}

			int x = 10;
			int y = this.GraphicsDevice.PresentationParameters.BackBufferHeight -
			        this._activeColumns.GetLength(0) * (_gridHeight + _gridHeightBuffer) - 50;
			var startVectorLeft = new Vector2(x, y);
			this.DrawHtmActivationMap(startVectorLeft, this._predictions, "Region Prediction");
		}

		/// <summary>
		/// Draws a 2d-map on screen at wanted position
		/// </summary>
		/// <param name="startVectorLeft"></param>
		/// <param name="mapData"></param>
		/// <param name="title"> </param>
		private void DrawHtmActivationMap(Vector2 startVectorLeft, float[,] mapData, string title)
		{
			// Count active elements
			int activeCounter = 0;
			var gridLeftStart = (int) startVectorLeft.X;

			// Draw Prediction Legend
			this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
			                        SamplerState.AnisotropicClamp, null, null);
			this._spriteBatch.DrawString(this._spriteFont, title, startVectorLeft, Color.Black);
			// Go one more line down
			startVectorLeft.Y += _gridHeight + _gridHeightBuffer;

			for (int i = mapData.GetLength(0) - 1; i >= 0; i--)
			{
				// Go one more line down
				startVectorLeft.Y += _gridHeight + _gridHeightBuffer;
				for (int j = 0; j < mapData.GetLength(1); j++)
				{
					if (mapData[i, j] == 1)
					{
						activeCounter++;
					}

					// Adapt color
					float component = (1 - mapData[i, j]) * 255;
					var newColor = new Color(component, component, component);

					this._spriteBatch.Draw(this._gridTexture,
					                       new Rectangle((int) startVectorLeft.X, (int) startVectorLeft.Y,
					                                     _gridWidth, _gridHeight), newColor);
					startVectorLeft.X += _gridWidth + _gridWidthBuffer;
				}
				startVectorLeft.X = gridLeftStart;
			}

			string activeColumnsString = "Active Columns per step: " + activeCounter.ToString();
			startVectorLeft.Y += _gridHeight + _gridHeightBuffer;
			this._spriteBatch.DrawString(this._spriteFont, activeColumnsString, startVectorLeft, Color.Black);

			this._spriteBatch.End();
		}


		/// <summary>
		/// DrawHtmRegion
		/// </summary>
		/// <param name="inactiveCells"></param>
		private void DrawHtmRegion(bool inactiveCells)
		{
			Matrix worldTranslationZ = Matrix.CreateTranslation(new Vector3(0, 0, _zHtmRegion));
			Matrix worldTranslation;
			Matrix worldScale;
			Matrix worldRotate = Matrix.CreateRotationX(this._pitchHtm) * Matrix.CreateRotationY(this._yawHtm);
			var sumOf3DCoordinates = new Vector3();
			var regionCenter = new Vector3();
			int numberPointsToCalculateAverageOfCenter = 0;

			this.FillHtmOveriew(this.Region);

			try
			{
				foreach (var column in this.HtmRegionColumns)
				{
					int predictionCounter = 0;

					foreach (var cell in column.Cells)
					{
						if (column.IsDataGridSelected)
						{
							this.FillHtmOveriew(column);
						}

						if (cell.IsDataGridSelected)
						{
							this.FillHtmOveriew(cell);
						}

						//calculate cell world coordinates
						var translationVector = new Vector3(column.PositionInRegion.X, cell.Index, column.PositionInRegion.Y);
						worldTranslation = Matrix.CreateTranslation(translationVector) * worldTranslationZ;


						if (inactiveCells)
						{
							// Calculate region center.
							sumOf3DCoordinates += translationVector;
							numberPointsToCalculateAverageOfCenter++;
						}

						Color color;
						float alphaValue;
						this.GetColorFromCell(cell, out color, out alphaValue);

						if ((!inactiveCells && alphaValue < 1.0f) || (inactiveCells && alphaValue == 1.0f))
						{
							continue;
						}

						// Check for cell selection
						if (column.IsDataGridSelected)
						{
							alphaValue = 1.0f;
							color = this._dictionaryCellColors[HtmCellColors.Selected].HtmColor;
							worldScale = Matrix.CreateScale(new Vector3(0.5f, 0.5f, 0.5f));
						}
						else if (column.InhibitedState[Global.T])
						{
							alphaValue = 1.0f;
							color = this._dictionaryCellColors[HtmCellColors.Inhibited].HtmColor;
							worldScale = Matrix.CreateScale(new Vector3(0.3f, 0.3f, 0.3f));
						}

						if (cell.IsDataGridSelected)
						{
							alphaValue = 1.0f;
							worldScale = Matrix.CreateScale(new Vector3(0.5f, 0.5f, 0.5f));	//was 0.7
						}
						else
						{
							worldScale = Matrix.CreateScale(new Vector3(0.3f, 0.3f, 0.3f));
						}

						if (cell.PredictiveState[Global.T])
						{
							// Closed for region prediction visualization
							predictionCounter++;
						}

						if (cell.mouseSelected)
							color = Color.Red;

						if(cell.mouseOver)
						{
							color = mouseOverColor;
						}
					
						//apply scale factor 
						Matrix world = worldScale * worldTranslation * worldRotate;

						// Draw cube
						this._cube.Draw(world, this._viewMatrix, this._projectionMatrix, color, alphaValue);


						// Draw synapse connections
						this.DrawDistalSynapseConnections(ref worldTranslationZ, ref worldRotate, column, cell);
					}

					if (!inactiveCells)
					{
						// Send column indices with actual prediction value in 2-dimArray
						float result = predictionCounter / (float) this.Region.CellsPerColumn;
						this._predictions[column.PositionInRegion.X, column.PositionInRegion.Y] = result;
					}

					// Draw proximal synapse connections:
					this.DrawProximalSynapseConnections(ref worldRotate, column);

					// Define value to draw ColumnMap
					if (column.ActiveState[Global.T])
					{
						this._activeColumns[column.PositionInRegion.X, column.PositionInRegion.Y] = 1;
					}
					else
					{
						this._activeColumns[column.PositionInRegion.X, column.PositionInRegion.Y] = 0;
					}
				}
			}
			catch (Exception)
			{
			 
			}

			if (inactiveCells)
			{
				// Calculate region center to focus camera on.
				regionCenter = new Vector3(
					sumOf3DCoordinates.X / numberPointsToCalculateAverageOfCenter,
					sumOf3DCoordinates.Y / numberPointsToCalculateAverageOfCenter,
					sumOf3DCoordinates.Z / numberPointsToCalculateAverageOfCenter);
				regionCenter.Z += _zHtmRegion;
				this._lookAt = regionCenter;
			}
		}

		/// <summary>
		/// Helper Method to get color from cell activity
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="color"></param>
		/// <param name="alphaValue"></param>
		private void GetColorFromCell(Cell cell, out Color color, out float alphaValue)
		{
			color = this._dictionaryCellColors[HtmCellColors.Inactive].HtmColor;
			alphaValue = .1f; // All conditions can be false. 
			Simulation3DForm visualizerForm = Simulation3D.Form;

			try
			{
				// Currently predicting cells.
				if (visualizerForm.ShowPredictingCells && cell.PredictiveState[Global.T])
				{
					if (cell.IsSegmentPredicting)
					{
						// Sequence predicting cells (t+1).
						if (visualizerForm.ShowSeqPredictingCells)
						{
							alphaValue = 1f;
							color = this._dictionaryCellColors[HtmCellColors.SequencePredicting].HtmColor;
						}
					}
					else
					{
						// Lost predicting cells for t+k.
						alphaValue = 1f;
						color = this._dictionaryCellColors[HtmCellColors.Selected].HtmColor;

						// New predicting cells for t+k.
						foreach (var segment in cell.DistalSegments)
						{
							if (segment.ActiveState[Global.T])
							{
								color = this._dictionaryCellColors[HtmCellColors.Predicting].HtmColor;
							}
						}
					}
				}

				// Learning in t+0.
				if (visualizerForm.ShowLearningCells && cell.LearnState[Global.T])
				{
					alphaValue = 1f;
					color = this._dictionaryCellColors[HtmCellColors.Learning].HtmColor;
				}
				else // Learning cells are all active
					if (visualizerForm.ShowActiveCells && cell.ActiveState[Global.T])
					{
						alphaValue = 1f;
						color = this._dictionaryCellColors[HtmCellColors.Active].HtmColor;
					}

				// Sequence predicted cells.
				if (cell.GetSequencePredictingDistalSegment () != null)
				{
					// False predicted cells.
					if (visualizerForm.ShowFalsePredictedCells && !cell.ActiveState[Global.T])
					{
						alphaValue = 1f;
						color = this._dictionaryCellColors[HtmCellColors.FalsePrediction].HtmColor;
					}

					// Correctly predicting in t+0.
					if (visualizerForm.ShowCorrectPredictedCells && cell.ActiveState[Global.T])
					{
						alphaValue = 1f;
						color = this._dictionaryCellColors[HtmCellColors.RightPrediction].HtmColor;
					}
				}
			}
			catch (Exception)
			{
				
			}
		}

		/// <summary>
		/// Calculate size of the region in world coordinates.
		/// </summary>
		/// <returns>Vector3 size</returns>
		private Vector3 GetSize ()
		{
			Matrix worldTranslationZ = Matrix.CreateTranslation ( new Vector3 ( 0, 0, _zHtmRegion ) );
			Matrix worldTranslation;
			Matrix worldRotate = Matrix.CreateRotationX ( this._pitchHtm ) * Matrix.CreateRotationY ( this._yawHtm );
			Vector3 size = new Vector3 ();

			foreach (var column in this.HtmRegionColumns)
			{
				foreach (var cell in column.Cells)
				{
					//calculate cell world coordinates
					var translationVector = new Vector3 ( column.PositionInRegion.X, cell.Index, column.PositionInRegion.Y );
					worldTranslation = Matrix.CreateTranslation ( translationVector ) * worldTranslationZ;

					//Z extent for initial camera position
					size.X = Math.Max ( translationVector.X, size.X );
					size.Y = Math.Max ( translationVector.Y, size.Y );
					size.Z = Math.Max ( translationVector.Z, size.Z );
				}
			}
			return size;
		}


		/// <summary>
		/// Draw distal synapse connections for chosen cells.
		/// Attention! lateral movement of planes, regions causes Z-Value correction
		/// </summary>
		/// <param name="worldRotate"></param>
		/// <param name="column"></param>
		private void DrawProximalSynapseConnections(ref Matrix worldRotate, Column column)
		{
			try
			{
				// Draw Connections if existing
				if (column.IsDataGridSelected || (Simulation3D.Form.ShowSpatialLearning && column.ActiveState[Global.T]))
				{
					foreach (var synapse in column.ProximalSegment.Synapses)
					{
						if (column.Statistics.StepCounter > 0)
						{
							var proximalSynapse = synapse as ProximalSynapse;

							// Get the two vectors to draw line between
							var startPosition = new Vector3 ( column.PositionInRegion.X, 0, column.PositionInRegion.Y + _zHtmRegion );

							// Get input source position
							int x = proximalSynapse.InputSource.X;
							int y = -5;
							int z = proximalSynapse.InputSource.Y;
							var endPosition = new Vector3 ( x, y, z + _zHtmPlane );

							//// Check for color
							//if (proximalSynapse.IsActive ( Global.T ))
							//{
							//	if (proximalSynapse.IsConnected ())	// Active & connected.
							//		this._connectionLine.SetUpVertices ( startPosition, endPosition, Color.Green );
							//	else	// Active.
							//		this._connectionLine.SetUpVertices ( startPosition, endPosition, Color.Orange );
							//}
							//else // Not active.
							//{
							//	this._connectionLine.SetUpVertices ( startPosition, endPosition, Color.White );
							//}

							////debug js
							//if (proximalSynapse.mouseSelected)
							//{
							//	this._connectionLine.SetUpVertices ( startPosition, endPosition, Color.Red );
							//}
							//if (proximalSynapse.mouseOver)
							//{
							//	this._connectionLine.SetUpVertices ( startPosition, endPosition, mouseOverColor );
							//}

							Color color;
							float alphaValue;
							GetColorFromProximalSynapse ( proximalSynapse, out color, out alphaValue );
							this._connectionLine.SetUpVertices ( startPosition, endPosition, color );

							// Draw line
							this._connectionLine.Draw ( worldRotate, this._viewMatrix, this._projectionMatrix );
						}
					}
				}
			}
			catch (Exception)
			{
				
			}
		}


		/// <summary>
		/// Helper Method to get color from proximal synapse state
		/// </summary>
		/// <param name="proximalSynapse"></param>
		/// <param name="color"></param>
		/// <param name="alphaValue"></param>
		private void GetColorFromProximalSynapse ( ProximalSynapse proximalSynapse, out Color color, out float alphaValue )
		{
			color = this._dictionaryProximalSynapseColors[HtmProximalSynapseColors.Default].HtmColor;
			alphaValue = .1f; // All conditions can be false. 
			Simulation3DForm visualizerForm = Simulation3D.Form;

			try
			{
				if (proximalSynapse.IsActive ( Global.T ))
				{
					if (proximalSynapse.IsConnected ())	// Active & connected.
					{
						//this._connectionLine.SetUpVertices ( startPosition, endPosition, Color.Green );
						{
							alphaValue = 1f;
							color = this._dictionaryProximalSynapseColors[HtmProximalSynapseColors.ActiveConnected].HtmColor;
						}
					}
					else	// Active.
					{
						//this._connectionLine.SetUpVertices ( startPosition, endPosition, Color.Orange );
						alphaValue = 1f;
						color = this._dictionaryProximalSynapseColors[HtmProximalSynapseColors.Active].HtmColor;
					}
				}
				else // Not active.
				{
					//this._connectionLine.SetUpVertices ( startPosition, endPosition, Color.White );
					alphaValue = 1f;
					color = this._dictionaryProximalSynapseColors[HtmProximalSynapseColors.Default].HtmColor;
				}

				//selected
				if (proximalSynapse.mouseSelected)
				{
					color = selectedColor;
					alphaValue = 1f;
					//color = this._dictionaryProximalSynapseColors[HtmProximalSynapseColors.MouseSelected].HtmColor;
				}
				//mouseOver
				if (proximalSynapse.mouseOver)
				{
					color = mouseOverColor;
					alphaValue = 1f;
					//color = this._dictionaryProximalSynapseColors[HtmProximalSynapseColors.MouseOver].HtmColor;
				}
			}
			catch (Exception)
			{

			}
		}



		/// <summary>
		/// Draw distal synapse connections for chosen cells
		/// </summary>
		/// <param name="worldTranslation"></param>
		/// <param name="worldRotate"></param>
		/// <param name="column"></param>
		/// <param name="cell"></param>
		private void DrawDistalSynapseConnections(ref Matrix worldTranslation, 
			ref Matrix worldRotate, Column column, Cell cell)
		{
			try
			{
				// Draw Connections if existing
				if (cell.IsDataGridSelected || 
					(Simulation3D.Form.ShowTemporalLearning && cell.PredictiveState[Global.T]))
				{
					foreach (var segment in cell.DistalSegments)
					{
						foreach (var synapse in segment.Synapses)
						{
							var distalSynapse = synapse as DistalSynapse;

							// Get the two vectors to draw line between
							var startPosition = new Vector3(column.PositionInRegion.X,
								cell.Index, column.PositionInRegion.Y);

							// Get input source position
							int x = distalSynapse.InputSource.Column.PositionInRegion.X;
							int y = distalSynapse.InputSource.Index;
							int z = distalSynapse.InputSource.Column.PositionInRegion.Y;
							var endPosition = new Vector3(x, y, z);

							//Color color = distalSynapse.IsActive(Global.T) ? Color.Black : Color.White;
							Color color;
							float alphaValue;
							GetColorFromDistalSynapse ( distalSynapse, out color, out alphaValue );
							this._connectionLine.SetUpVertices(startPosition, endPosition, color);

							// Draw line
							this._connectionLine.Draw(worldTranslation * worldRotate,
								this._viewMatrix, this._projectionMatrix);
						}
					}
				}
			}
			catch (Exception)
			{
				// Is sometimes raised because of collections modification by another thread.
			}
		}


		
		/// <summary>
		/// Helper Method to get color from proximal synapse state
		/// </summary>
		/// <param name="distalSynapse"></param>
		/// <param name="color"></param>
		/// <param name="alphaValue"></param>
		private void  GetColorFromDistalSynapse ( DistalSynapse distalSynapse, out Color color, out float alphaValue )
		{
			color = this._dictionaryDistalSynapseColors[HtmDistalSynapseColors.Default].HtmColor;
			alphaValue = .1f; // All conditions can be false. 
			Simulation3DForm visualizerForm = Simulation3D.Form;

			//Color color = distalSynapse.IsActive ( Global.T ) ? Color.Black : Color.White;
			try
			{
				if (distalSynapse.IsActive ( Global.T ))
				{
					alphaValue = 1f;
					color = this._dictionaryDistalSynapseColors[HtmDistalSynapseColors.Active].HtmColor;
				}
				else	// Not Active.
				{
					alphaValue = 1f;
					color = this._dictionaryDistalSynapseColors[HtmDistalSynapseColors.Default].HtmColor;
				}

				//selected
				if (distalSynapse.mouseSelected)
				{
					color = selectedColor;
					alphaValue = 1f;
					//color = this._dictionaryProximalSynapseColors[HtmProximalSynapseColors.MouseSelected].HtmColor;
				}
				//mouseOver
				if (distalSynapse.mouseOver)
				{
					color = mouseOverColor;
					alphaValue = 1f;
					//color = this._dictionaryProximalSynapseColors[HtmProximalSynapseColors.MouseOver].HtmColor;
				}
			}
			catch (Exception)
			{

			}
		}


		/// <summary>
		/// Draws input bitmap. Attention: planes is translated also according to constants for better positioning
		/// </summary>
		private void DrawHtmPlane()
		{
			// Get input data from FileSensor. Attention: Draw rhythm happens very often!
			int[,] inputData = NetControllerForm.Instance.SelectedNode.Region.Input[Global.T];

			if (inputData != null)
			{
				const float alphaValue = 0.9f;

				int regionWidth = inputData.GetLength(0);
				int regionHeight = inputData.GetLength(1);

				Matrix worldTranslationBehindDown = Matrix.CreateTranslation(new Vector3(0, _yHtmPlane, 0)) * Matrix.CreateTranslation(new Vector3(0, 0, _zHtmPlane));
				float bitSquareScale = .3f;
				Matrix worldScale = Matrix.CreateScale(new Vector3(bitSquareScale, bitSquareScale, bitSquareScale));
				Matrix worldRotate = Matrix.CreateRotationX(this._pitchHtm) * Matrix.CreateRotationY(this._yawHtm);

				for (int x = 0; x < regionWidth; x++)
				{
					for (int z = 0; z < regionHeight; z++)
					{
						// All variables are on the method level.
						float cf = 1f;
						Matrix worldTranslation = Matrix.CreateTranslation(new Vector3(x * cf, 0, z * cf )) * worldTranslationBehindDown;
						Matrix world = worldScale * worldTranslation * worldRotate;
						Color color = inputData[x, z] == 0 ? Color.White : Color.Black;

						// Draw input bit square.
						this._bit.Draw(world, this._viewMatrix, this._projectionMatrix, color, alphaValue);
					}
				}
			}
		}

		#endregion

		#endregion

		#region Events

		/// <summary> 
		/// Event capturing the construction of a draw surface and makes sure this gets redirected to 
		/// a predesignated drawsurface marked by pointer _drawSurface 
		/// </summary> 
		private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
		{
			e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = this._drawSurface;

			e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = Simulation3D.Form.pictureBoxSurface.Width;
			e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = Simulation3D.Form.pictureBoxSurface.Height;
		}

#endregion

		#region Picking

		public Ray getPickingRay ( System.Drawing.Point mousePosition )
		{
			mouseLocationClick = new Point ( mousePosition.X, mousePosition.Y );

			Vector3 nearPoint = new Vector3 ( mousePosition.X, mousePosition.Y, 0.0f );
			Vector3 farPoint = new Vector3 ( mousePosition.X, mousePosition.Y, 0.999999f );

			Matrix worldRotate = Matrix.CreateRotationX ( this._pitchHtm ) * Matrix.CreateRotationY ( this._yawHtm );
			
			//nearPoint = this.GraphicsDevice.Viewport.Unproject ( nearPoint, this._projectionMatrix, this._viewMatrix, Matrix.Identity );
			//farPoint = this.GraphicsDevice.Viewport.Unproject ( farPoint, this._projectionMatrix, this._viewMatrix, Matrix.Identity );
			nearPoint = this.GraphicsDevice.Viewport.Unproject ( nearPoint, this._projectionMatrix, this._viewMatrix, worldRotate );
			farPoint = this.GraphicsDevice.Viewport.Unproject ( farPoint, this._projectionMatrix, this._viewMatrix, worldRotate );
			
			Vector3 direction = farPoint - nearPoint;
			direction.Normalize ();

			Ray ray = new Ray ( nearPoint, direction );
			return ray;

		}

		/// <summary>
		/// Pick graphic components.
		/// </summary>
		/// <param name="location"></param>
		/// <param name="bSelectionEnable"></param>
		public void Pick ( System.Drawing.Point location, bool bSelectionEnable )
		{
			//this.GraphicsDevice.Clear ( this._clearColor );

			////////////debug js
			////////////ray = Simulation3D.Engine.getPickingRay ( new System.Drawing.Point(440, 448 ));
			////////////ray = Simulation3D.Engine.getPickingRay ( new System.Drawing.Point ( 428, 444 ) );
			////////////ray = Simulation3D.Engine.getPickingRay ( new System.Drawing.Point ( 211, 332 ) );
			Ray ray = Simulation3D.Engine.getPickingRay ( location );


			//if (Properties.Settings.Default.StealthMode)
			//	ray = Simulation3D.Engine.getPickingRay ( new System.Drawing.Point ( 331, 301 ) );


			//// Draw Legend:
			//this.DrawLegend ();

			//// Draw HTM
			//this.PickHtmPlane ();

			this.PickHtmRegion ( ray, bSelectionEnable );
			//this.PickHtmRegion ( true );

			//// Draw Prediction Plane
			//this.PickHtmRegionPrediction ();

			//// Draw Active Columns Plane
			//this.PickHtmActiveCols ();
			
		}

		public void PickHtmRegion ( Ray ray, bool bSelectionEnable )
		{
			Matrix worldTranslationZ = Matrix.CreateTranslation ( new Vector3 ( 0, 0, _zHtmRegion ) );
			Matrix worldTranslation;
			Matrix worldScale;
			Matrix worldRotate = Matrix.CreateRotationX ( this._pitchHtm ) * Matrix.CreateRotationY ( this._yawHtm );
			float distance;
			float nearestDistance = float.MaxValue;
			Cell returnedCell = null;	//cell returned from function
			Cell nearestCell = null;	//keep track of nearest cell returned
			ProximalSynapse returnedProximalSynapse = null;
			ProximalSynapse nearestProximalSynapse = null;
			DistalSynapse returnedDistalSynapse = null;
			DistalSynapse nearestDistalSynapse = null;

			//this is a two-step process
			//Each Pick..() function retuns the "returned..." object which is the nearest object found for this call (eg for this cell).
			//Then, back in this function, the ultimate nearest object is determined (from all the object types) to be the ultimately picked object.

			foreach (Column column in this.HtmRegionColumns)
			{
				returnedProximalSynapse = null ;

				foreach (var cell in column.Cells)
				{
					returnedCell = null;
					returnedDistalSynapse = null;

					var translationVector = new Vector3 ( column.PositionInRegion.X, cell.Index, column.PositionInRegion.Y );

					worldScale = Matrix.CreateScale ( new Vector3 ( 0.2f, 0.2f, 0.2f ) );
					worldTranslation = Matrix.CreateTranslation ( translationVector ) * worldTranslationZ;
					//Matrix world = worldScale * worldTranslation * worldRotate;
					Matrix world = worldScale * worldTranslation;

					distance = PickCell ( ray, world, column, cell, ref returnedCell );
					TrackNearestObject ( distance, ref nearestDistance, returnedCell, ref nearestCell, returnedDistalSynapse, ref nearestDistalSynapse, returnedProximalSynapse, ref nearestProximalSynapse );

					//Pick synapse connections
					distance = PickDistalSynapseConnections ( ray, column, cell, ref returnedDistalSynapse );
					TrackNearestObject ( distance, ref nearestDistance, returnedCell, ref nearestCell, returnedDistalSynapse, ref nearestDistalSynapse, returnedProximalSynapse, ref nearestProximalSynapse );
				}
					
				// Pick proximal synapse connections:
				distance = PickProximalSynapseConnections ( ray, column, ref returnedProximalSynapse );
				TrackNearestObject ( distance, ref nearestDistance, returnedCell, ref nearestCell, returnedDistalSynapse, ref nearestDistalSynapse, returnedProximalSynapse, ref nearestProximalSynapse );
			}

			//find and process nearest object picked
			Selectable3DObject selObject = null;
			if (nearestDistance < float.MaxValue)
			{
				if (nearestCell != null)
				{
					selObject = nearestCell;
					selObject.mouseOver = true;
				}
				if (nearestDistalSynapse != null)
				{
					selObject = nearestDistalSynapse;
					selObject.mouseOver = true;
				}
				if (nearestProximalSynapse != null)
				{
					selObject = nearestProximalSynapse;
					selObject.mouseOver = true;
				}

				if (bSelectionEnable && selObject != null)
				{
					if (!selObject.mouseSelected)
						selObject.mouseSelected = true;
					else
						selObject.mouseSelected = false;	//toggle

					////SHIFT key - deselect
					//if (keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.LeftShift ) || keyState.IsKeyDown ( Microsoft.Xna.Framework.Input.Keys.RightShift ))
					//	selObject.mouseSelected = false;

					//update selected object list
					//UpdateSelectedObjectList ( selObject, selObject.mouseSelected ); start here
					UpdateSelectedObjectList ( this.Region, selObject.mouseSelected );
				}
			}

			//if (selObject != null)
			//{

			//	//test only
			//	DataSet ds = ViewListToDataset_Cells ( );
			//}
		}


		private float PickCell ( Ray ray, Matrix worldTranslation, Column column, Cell cell, ref Cell returnedCell )
		{
			cell.mouseOver = false;

			BoundingBox box = this._cube.GetBoundingBox ( worldTranslation );

			float? intersectDistance = ray.Intersects ( box );

			if (intersectDistance != null)
			{
				returnedCell = cell;
				return (float)intersectDistance;
			}
				
			return float.MaxValue;
		}


		private float PickProximalSynapseConnections ( Ray ray, Column column, ref ProximalSynapse returnedProximalSynapse )
		{
			float intersectDistance = float.MaxValue;
			float minDistance = float.MaxValue;

			returnedProximalSynapse = null;

			// Draw Connections if existing
			if (column.IsDataGridSelected || (Simulation3D.Form.ShowSpatialLearning && column.ActiveState[Global.T]))
			{
				Vector3 rayP1 = ray.Position;
				Vector3 rayP2 = rayP1 + ray.Direction;

				foreach (ProximalSynapse synapse in column.ProximalSegment.Synapses)
				{
					synapse.mouseOver = false;

					if (column.Statistics.StepCounter > 0)
					{
						//var proximalSynapse = synapse as ProximalSynapse;

						// Get the two vectors to draw line between
						var startPosition = new Vector3 ( column.PositionInRegion.X, 0, column.PositionInRegion.Y + _zHtmRegion );

						// Get input source position
						int x = synapse.InputSource.X;
						int y = (int)_yHtmPlane;
						int z = synapse.InputSource.Y;
						var endPosition = new Vector3 ( x, y, z + _zHtmPlane );
							
						bool intersect;
						Vector3 Line1ClosestPt = new Vector3 ();
						Vector3 Line2ClosestPt = new Vector3 ();
						intersect = Math3D.ClosestPointsLineSegmentToLine ( out Line1ClosestPt, out Line2ClosestPt, startPosition, endPosition, rayP1, rayP2, 0.1f, out intersectDistance );

						if (intersect && intersectDistance < minDistance)
						{
							minDistance = intersectDistance;
							returnedProximalSynapse = synapse;
						}
					}
				}
			}

			return minDistance;
		}


		private float PickDistalSynapseConnections ( Ray ray, Column column, Cell cell, ref DistalSynapse returnedDistalSynapse )
		{
			float intersectDistance;
			float minDistance = float.MaxValue;

			returnedDistalSynapse = null;

			// Draw Connections if existing
			if (cell.IsDataGridSelected ||
				(Simulation3D.Form.ShowTemporalLearning && cell.PredictiveState[Global.T]))
			{
				Vector3 rayP1 = ray.Position;
				Vector3 rayP2 = rayP1 + ray.Direction;

				foreach (DistalSegment segment in cell.DistalSegments)
				{
					foreach (DistalSynapse synapse in segment.Synapses)
					{
						synapse.mouseOver = false;

						var distalSynapse = synapse as DistalSynapse;

						// Get the two vectors to draw line between
						var startPosition = new Vector3 ( column.PositionInRegion.X, cell.Index, column.PositionInRegion.Y + _zHtmRegion );

						// Get input source position
						int x = distalSynapse.InputSource.Column.PositionInRegion.X;
						int y = distalSynapse.InputSource.Index;
						int z = distalSynapse.InputSource.Column.PositionInRegion.Y;
						var endPosition = new Vector3 ( x, y, z + _zHtmPlane );

						bool intersect;
						Vector3 Line1ClosestPt = new Vector3 ();
						Vector3 Line2ClosestPt = new Vector3 ();
						intersect = Math3D.ClosestPointsLineSegmentToLine ( out Line1ClosestPt, out Line2ClosestPt, startPosition, endPosition, rayP1, rayP2, 0.1f, out intersectDistance );

						if (intersect && intersectDistance < minDistance)
						{
							minDistance = intersectDistance;
							returnedDistalSynapse = synapse;
						}
					}
				}
			}

			return minDistance;
		}


		//Keep track of nearest object by setting others to null.
		private void TrackNearestObject ( float distance, ref float nearestDistance, Cell returnedCell, ref Cell nearestCell, DistalSynapse returnedDistalSynapse, ref DistalSynapse nearestDistalSynapse, ProximalSynapse returnedProximalSynapse, ref ProximalSynapse nearestProximalSynapse )
		{
			if (distance < nearestDistance)
			{
				nearestDistance = distance;

				if (returnedCell != null)
				{
					nearestCell = returnedCell;
					nearestDistalSynapse = null;
					nearestProximalSynapse = null;
				}
				if (returnedDistalSynapse != null)
				{
					nearestCell = null;
					nearestDistalSynapse = returnedDistalSynapse;
					nearestProximalSynapse = null;
				}
				if (returnedProximalSynapse != null)
				{
					nearestCell = null;
					nearestDistalSynapse = null;
					nearestProximalSynapse = returnedProximalSynapse;
				}
			}
		}



		/// <summary>
		/// Add or remove selected object to ViewList
		/// </summary>
		/// <param name="obj"></param>
		public void UpdateSelectedObjectList(object obj, bool add)
		{
			//if (obj != null)
			//{
			//	Selectable3DObject o = (Selectable3DObject)obj;
			//	if (add == true)
			//		WatchList.Add ( o );
			//	else
			//		WatchList.Remove ( o );
			//}

			//trigger SelectionChangedEvent
			//SelectionChangedEvent ( WatchList, EventArgs.Empty );
			SelectionChangedEvent ( NetControllerForm.Instance.TopNode.Region, EventArgs.Empty );
		}



		#endregion
	}
}
