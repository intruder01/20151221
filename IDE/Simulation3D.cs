using System;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Xna.Framework;

namespace OpenHTM.IDE
{
	public delegate void SimEngineStarted (object sender, EventArgs e);

	/// <summary>
	/// This is the main class for start the 3D visualization
	/// </summary>
	internal class Simulation3D
	{
		//this worked
		//public static EventHandler<EventArgs> SimEngineStarted;


		public static event SimEngineStarted EngineStarted = delegate { };

		#region Fields

		private static Thread _thread;
	
		#endregion

		#region Properties

		public static bool IsActive { get; private set; }

		public static Simulation3DForm Form { get; private set; }

		public static Simulation3DEngine Engine { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Open the 3D visualization in a exclusive thread in order to it do not create
		/// a conflict with the application thread
		/// </summary>        
		public static void Start()
		{
			// If there is already a thread, kill it
			if (_thread != null && _thread.IsAlive)
			{
				_thread.Abort();
			}

			// Start a new thread calling startMethod method
			_thread = new Thread(StartMethod);
			_thread.TrySetApartmentState(ApartmentState.MTA);
			_thread.Start();

			IsActive = true;
		}

		/// <summary>
		/// Close the exclusive thread that has the 3D engine and the visualizer form running
		/// </summary>          
		public static void End()
		{
			if (_thread != null)
			{
				_thread.Abort();
			}

			IsActive = false;
		}

		/// <summary>
		/// Open the 3D engine and the visualizer form
		/// </summary> 
		private static void StartMethod()
		{
			// Open the visualizer form
			var form = new Simulation3DForm();
			Form = form;

			if (!Properties.Settings.Default.StealthMode)
			Form.Show();

			// Start the 3D engine
			Engine = new Simulation3DEngine ( form.GetDrawSurface () );

			//this worked
			//EventHandler<EventArgs> EngineStarted = SimEngineStarted;
			
			EngineStarted ( Engine, new EventArgs () );
	
			Engine.Run();

		}


		#endregion
	}
}
