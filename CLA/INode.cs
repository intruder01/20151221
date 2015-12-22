using System;
using System.Drawing;

namespace OpenHTM.CLA
{
	/// <summary>
	/// A common interface to represent a region or sensor (file, network, etc).
	/// Since a higher region will receive inputs either from lower regions or sensors,
	/// it's better to handle these using a unique interface.
	/// </summary>
	public interface INode
	{
		/// <summary>
		/// A higher region in hierarchy which this node will feed-forward.
		/// </summary>
		/// 
		Region ParentRegion { get; set; }

		/// <summary>
		/// The width and height of the node.
		/// </summary>
		Size Size { get; set; }

		/// <summary>
		/// Indicates if the node was initialized or not.
		/// </summary>
		/// 
		bool Initialized { get; set; }

		/// <summary>
		/// Initialize the node.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Perfoms actions related to time step progression.
		/// </summary>
		void NextTimeStep();

		/// <summary>
		/// Get the output to the current time step.
		/// The output is a array representing current data that come from this node.
		/// </summary>
		int[,] GetOutput();
	}
}
