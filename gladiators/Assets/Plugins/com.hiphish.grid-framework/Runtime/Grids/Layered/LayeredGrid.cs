using UnityEngine;

namespace GridFramework.Grids {
	/// <summary>
	///   The parent class for all layered grids.
	/// </summary>
	/// <remarks>
	///   <para>
	///     This class serves as a parent for all grids composed out of
	///     two-dimensional grids stacked on top of each other (currently only
	///     hex- and polar grids).  These grids have a plane (orientation) and
	///     a "depth" (how densely stacked they are). Other than keeping common
	///     values and internal methods in one place, this class has not much
	///     practical use. I recommend yu ignore it, it is documented just for
	///     the sake of completion.
	///   </para>
	/// </remarks>
	public abstract class LayeredGrid : BaseGrid {
		[SerializeField]
		private float _depth = 1.0f;

		/// <summary>
		///   How far apart layers of the grid are.
		/// </summary>
		/// <value>
		///   Depth of grid layers.
		/// </value>
		/// <remarks>
		///   <para>
		///     Layered grids are made of an infinite number of two-dimensional
		///     grids stacked on top of each other. This determines how far
		///     apart those layers are. The value cannot be lower than
		///     <c>Mathf.Epsilon</c> in order to prevent contradictory values.
		///   </para>
		/// </remarks>
		public float Depth {
			get  => _depth;
			set {
				MarkAsDirty(Mathf.Abs(value - _depth) > Mathf.Epsilon);
				_depth = Mathf.Max(value, Mathf.Epsilon);
			}
		}

		/// <summary>
		///   Vector between two layers of the grid.
		/// </summary>
		public Vector3 Forward {
			get => Depth * transform.forward;
		}
	}
}
