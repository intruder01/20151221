namespace OpenHTM.CLA
{
	/// <summary>
	/// Global variables and constants.
	/// </summary>
	public class Global
	{
		// Current (and maximum) time step of the context.
		// Classes usually have state vectors that use T as initial context reference.
		// A state vector works like a context 'queue' with the difference that we can access directly its elements through an index (between 0 and T).
		// This means that all state machines have their vectors with fixed lenght = T.
		// For example, if T=4 (i.e. 5 time steps assumming that array base is 0) then methods only can handle states up to the 'T-4' time step:
		//   State[T-4] => State[0] => state at 5 time steps ago
		//   State[T-3] => State[1]
		//   State[T-2] => State[2]
		//   State[T-1] => State[3]
		//   State[T]   => State[4] => current state
		public static int T = 4;

		/// <summary>
		/// If set to <c>true</c> a region will perform spatial learning
		/// during the spatial pooling phase.  If false the region will perform inferenece
		/// only during spatial pooling.  This value may be toggled on/off at any time
		/// during run.  When learning is finished the region can be frozen in place by
		/// toggling this false (predictions will still occur via previously learned data).
		/// </summary>
		public static bool SpatialLearning { get; set; }

		/// <summary>
		/// If set to <c>true</c> a region will perform temporal learning
		/// during the temporal pooling phase.  If false the region will perform inference
		/// only during temporal pooling.  This value may be toggled on/off at any time
		/// during run.  When learning is finished the region can be frozen in place by
		/// toggling this false (predictions will still occur via previously learned data).
		/// </summary>
		public static bool TemporalLearning { get; set; }
	}
}
