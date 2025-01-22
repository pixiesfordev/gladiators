using UnityEngine;
using GridFramework.Vectors;

namespace GridFramework.Grids {
	/// <summary>
	///   A standard three-dimensional rectangular grid.
	/// </summary>
	/// <remarks>
	///   <para>
	///     Your standard rectangular grid, the characterising values are its
	///     spacing and shearing, which can each be set for each axis
	///     individually.
	///   </para>
	/// </remarks>
	[AddComponentMenu("Grid Framework/Grids/RectGrid")]
	public sealed class RectGrid: BaseGrid {

#region  Private member variables
		[SerializeField] private Vector3 _spacing  = Vector3.one;
		[SerializeField] private Vector6 _shearing = Vector6.Zero;
#endregion  // Private member variables

#region  Accessors
		/// <summary>
		///   How large the grid boxes are.
		/// </summary>
		/// <value>
		///   The spacing of the grid.
		/// </value>
		/// <remarks>
		///   <para>
		///     How far apart the lines of the grid are. You can set each axis
		///     separately, but none may be less than `Mathf.Epsilon`, in order
		///     to prevent values that don't make any sense.
		///   </para>
		/// </remarks>
		public Vector3 Spacing {
			get => _spacing;
			set {
				var oldSpacing = _spacing;
				_spacing = Vector3.Max(value, Vector3.one * Mathf.Epsilon);
				OnSpacingChanged(oldSpacing, _spacing);
			}
		}

		/// <summary>
		///   How the axes are sheared.
		/// </summary>
		/// <value>
		///   Shearing vector of the grid.
		/// </value>
		/// <remarks>
		///   <para>
		///     How much the individual axes of the grid are skewed towards
		///     each other. For instance, this means the if _XY_ is set to _2_,
		///     then for each point with grid coordinates _(x, y)_ will be
		///     mapped to _(x, y + 2x)_, while the uninvolved _Z_ coordinate
		///     remains the same. For more information refer to the manual.
		///   </para>
		/// </remarks>
		public Vector6 Shearing {
			get  => _shearing;
			set {
				var oldShearing = _shearing;
				_shearing = value;
				OnShearingChanged(oldShearing, _shearing);
			}
		}
#endregion  // Accessors

#region  Computed properties
		/// <summary>
		///   Direction along the X-axis of the grid in world space.
		/// </summary>
		/// <value>
		///   Unit vector in grid scale along the grid's X-axis.
		/// </value>
		/// <remarks>
		///   <para>
		///     The X-axis of the grid in world space.
		///   </para>
		/// </remarks>
		public Vector3 Right {
			get => Axis(Vector3.right);
		}

		/// <summary>
		///   Direction along the Y-axis of the grid in world space.
		/// </summary>
		/// <value>
		///   Unit vector in grid scale along the grid's Y-axis.
		/// </value>
		/// <remarks>
		///   <para>
		///     The Y-axis of the grid in world space.
		///   </para>
		/// </remarks>
		public Vector3 Up {
			get => Axis(Vector3.up);
		}

		/// <summary>
		///   Direction along the Z-axis of the grid in world space.
		/// </summary>
		/// <value>
		///   Unit vector in grid scale along the grid's Z-axis.
		/// </value>
		/// <remarks>
		///   <para>
		///     The Z-axis of the grid in world space.
		///   </para>
		/// </remarks>
		public Vector3 Forward {
			get => Axis(Vector3.forward);
		}

		/// <summary>
		///   Common code for <c>Right</c>, <c>Up</c> and <c>Forward</c>.
		/// </summary>
		private Vector3 Axis(Vector3 axis) {
			return GridToWorld(axis) - GridToWorld(Vector3.zero);
		}
#endregion  // Computed properties

#region  Cached members
		private Matrix4x4 _gwMatrix = Matrix4x4.identity;
		private Matrix4x4 _wgMatrix = Matrix4x4.identity;
		
		protected override void ComputeCachedMembers() {
			var shearMatrix = Shearing.ShearMatrix();
			var rectMatrix  = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
			var scaleMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Spacing);

			_gwMatrix = rectMatrix * shearMatrix * scaleMatrix;
			_wgMatrix = _gwMatrix.inverse;
		}
#endregion  // Cached members

#region  Coordinate conversion
		/// <summary>
		///   Converts world coordinates to grid coordinates.
		/// </summary>
		/// <returns>
		///   Grid coordinates of the world point.
		/// </returns>
		/// <param name="world">
		///   Point in world space.
		/// </param>
		/// <remarks>
		///   <para>
		///     Takes in a position in wold space and calculates where in the
		///     grid that position is. The origin of the grid is the world
		///     position of its GameObject and its axes lie on the
		///     corresponding axes of the Transform.  Rotation is taken into
		///     account for this operation.
		///   </para>
		/// </remarks>
		public Vector3 WorldToGrid(Vector3 world) {
			UpdateCachedMembers();
			return _wgMatrix.MultiplyPoint3x4(world);
		}

		/// <summary>
		///   Converts grid coordinates to world coordinates.
		/// </summary>
		/// <returns>
		///   World coordinates of the Grid point.
		/// </returns>
		/// <param name="grid">
		///   Point in grid space.
		/// </param>
		/// <remarks>
		///   <para>
		///     The opposite of <see cref="WorldToGrid"/>, this returns the
		///     world position of a point in the grid. The origin of the grid
		///     is the world position of its GameObject and its axes lie on the
		///     corresponding axes of the Transform. Rotation is taken into
		///     account for this operation.
		///   </para>
		/// </remarks>
		public Vector3 GridToWorld(Vector3 grid) {
			UpdateCachedMembers();
			return _gwMatrix.MultiplyPoint3x4(grid);
		}
#endregion  // Coordinate conversion

#region  Hook methods
		/// <summary>
		///   This method is called when the <c>SpacingChanged</c> event has
		///   been triggered.
		/// </summary>
		/// <param name="oldSpacing">
		///   The spacing this grid had previously.
		/// </param>
		/// <param name="newSpacing">
		///   The spacing this grid has now.
		/// </param>
		private void OnSpacingChanged(Vector3 oldSpacing, Vector3 newSpacing) {
			var delta = oldSpacing - newSpacing;

			for (var i = 0; i < 3; ++i) {
				if (Mathf.Abs(delta[i]) > Mathf.Epsilon) {
					MarkAsDirty(true);
					return;
				}
			}
		}

		/// <summary>
		///   This method is called when the <c>ShearingChanged</c> event has
		///   been triggered.
		/// </summary>
		/// <param name="oldShearing">
		///   The shearing this grid had previously.
		/// </param>
		/// <param name="newShearing">
		///   The shearing this grid has now.
		/// </param>
		private void OnShearingChanged(Vector6 oldShearing, Vector6 newShearing) {
			var delta = oldShearing - newShearing;

			for (var i = 0; i < 6; ++i) {
				if (Mathf.Abs(delta[i]) > Mathf.Epsilon) {
					MarkAsDirty(true);
					return;
				}
			}
		}
#endregion  // Hook methods
	}
}
