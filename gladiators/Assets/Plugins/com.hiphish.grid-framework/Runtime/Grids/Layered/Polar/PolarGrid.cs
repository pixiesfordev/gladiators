using UnityEngine;

namespace GridFramework.Grids {
	/// <summary> A polar grid based on cylindrical coordinates. </summary>
	/// <remarks>
	///   <para>
	///     A grid based on cylindrical coordinates. The characterising values are <see
	///     cref="Radius"/>, <see cref="Sectors"/> and <see cref="Depth"/>. The angle values are
	///     derived from <see cref="Sectors"/> and we use radians internally. The coordinate systems
	///     used are either a grid-based coordinate system based on the defining values or a regular
	///     cylindrical coordinate system. If you want polar coordinates just ignore the height
	///     component of the cylindrical coordinates.
	///   </para>
	///   <para>
	///     It is important to note that the components of polar and grid coordinates represent the
	///     radius, radian angle and height. Which component represents what depends on the grid
	///     plane. The user manual has a handy table for that purpose.
	///   </para>
	/// </remarks>
	[AddComponentMenu("Grid Framework/Grids/PolarGrid")]
	public sealed class PolarGrid : LayeredGrid {

#region  Private variables
		[SerializeField] private float _radius  = 1f;
		[SerializeField] private int   _sectors = 8;
#endregion  // Private variables

#region  Accessors
		/// <summary> The radius of the inner-most circle of the grid. </summary>
		/// <value> The radius.  </value>
		/// <remarks>
		///   The radius of the innermost circle and how far apart the other circles are. The value
		///   cannot go below <c>Mathf.Epsilon</c>.
		/// </remarks>
		public float Radius {
			get => _radius;
			set {
				MarkAsDirty(Mathf.Abs(value - _radius) > Mathf.Epsilon);
				_radius = Mathf.Max(value, Mathf.Epsilon);
			}
		}
		
		/// <summary> The amount of sectors per circle. </summary>
		/// <value> Amount of sectors. </value>
		/// <remarks>
		///   The amount of sectors the circles are divided into. The minimum values is 1, which
		///   means one full circle.
		/// </remarks>
		public int Sectors {
			get => _sectors;
			set {
				MarkAsDirty(value != _sectors);
				_sectors = Mathf.Max(value, 1);
			}
		}
#endregion  // Accessors

#region  Computed properties
		/// <summary> The angle of a sector in radians. </summary>
		/// <remarks>
		///   <para>
		///     This is a read-only value derived from <c>Sectors</c>. It gives you the angle within
		///     a sector in radians and it is a shorthand writing for
		///   </para>
		///   <code>
		///     (2f * Mathf.PI) / Sectors
		///   </code>
		///   <para>
		///     When assigning a value only values of the form <c>2π / 2^n</c> are valid, everything
		///     else will be rounded to the nearest possible value.
		///   </para>
		/// </remarks>
		public float Radians {
			get => 2f * Mathf.PI / Sectors;
			set => Sectors = Mathf.RoundToInt(2f * Mathf.PI / value);
		}
		
		/// <summary> The angle of a sector in degrees. </summary>
		/// <remarks>
		///   <para>
		///     The same as <see cref="Radians"><c>Radians</c></see> except in degrees, it’s a
		///     shorthand writing for
		///   </para>
		///   <code>
		///     360f / Sectors
		///   </code>
		///   <para>
		///     When assigning a value only values of the form <c>360 / 2^n</c> are valid,
		///     everything else will be rounded to the nearest possible value.
		///   </para>
		/// </remarks>
		public float Degrees {
			get => 360f / Sectors;
			set => Sectors = Mathf.RoundToInt(360f / value);
		}

		/// <summary> Quaternion that rotates by one sector. </summary>
		public Quaternion Rotation {
			get => Quaternion.AngleAxis(Degrees, -Forward);
		}

		/// <summary>
		///   Vector that points straight one radius to the right from the center of the grid.
		/// </summary>
		public Vector3 Right {
			get => Radius * transform.right;
		}
#endregion  // Computed properties

#region  Cached members
		/// <summary> Matrix that transforms from world to plane. </summary>
		private Matrix4x4 _wpMatrix = Matrix4x4.identity;

		/// <summary> Matrix that transforms from plane to world. </summary>
		private Matrix4x4 _pwMatrix = Matrix4x4.identity;

		protected override void ComputeCachedMembers() {
			var t = transform.position;
			var r = transform.rotation;
			var s = Vector3.one;

			_pwMatrix = Matrix4x4.TRS(t, r, s);
			_wpMatrix = _pwMatrix.inverse;
		}
#endregion  // Cached members

#region  Grid <-> World coordinate transformation
		/// <summary> Converts from world to grid coordinates. </summary>
		/// <returns> Grid coordinates of the world point. </returns>
		/// <param name="world"> Point in world space. </param>
		/// <remarks>
		///   Converts a point from world space to grid space. The first coordinate represents the
		///   distance from the radial axis as multiples of <see cref="Radius"/>, the second one the
		///   sector and the thrid one the distance from the main plane as multiples of <see
		///   cref="Depth"/>. This order applies to XY-grids only, for the other two orientations
		///   please consult the manual.
		/// </remarks>
		public Vector3 WorldToGrid(Vector3 world) =>
			PolarToGrid(WorldToPolar(world));
		
		/// <summary> Converts from grid to world coordinates. </summary>
		/// <returns> World coordinates of the grid point. </returns>
		/// <param name="grid"> Point in grid space. </param>
		/// <remarks> Converts a point from grid space to world space. </remarks>
		public Vector3 GridToWorld(Vector3 grid) =>
			PolarToWorld(GridToPolar(grid));
#endregion  // Grid <-> World coordinate transformation

#region  Polar <-> World coordinate transformation
		/// <summary> Converts from world to polar coordinates. </summary>
		/// <returns> Point in polar space. </returns>
		/// <param name="worldPoint"> Point in world space. </param>
		/// <remarks>
		///   Converts a point from world space to polar space. The first coordinate represents the
		///   distance from the radial axis, the second one the angle in radians and the third one
		///   the distance from the main plane. This order applies to XY-grids only, for the other
		///   two orientations please consult the manual.
		/// </remarks>
		public Vector3 WorldToPolar(Vector3 worldPoint) {
			// First transform the point into planar coordinates
			UpdateCachedMembers();
			var p = _wpMatrix.MultiplyPoint3x4(worldPoint);

			// Then turn the point from Cartesian coordinates into polar
			// coordinates.
			var r = Mathf.Sqrt(Mathf.Pow(p.x, 2f) + Mathf.Pow(p.y, 2f));
			var a = Mathf.Atan2(p.y, p.x) + (p.y >= 0 ? 0 : 2 * Mathf.PI);
			var z = p.z;

			return new Vector3(r, a, z);
		}
		
		/// <summary> Converts from polar to world coordinates. </summary>
		/// <returns> Point in world space. </returns>
		/// <param name="polarPoint"> Point in polar space. </param>
		/// <remarks> Converts a point from polar space to world space. </remarks>
		public Vector3 PolarToWorld(Vector3 polarPoint) {
			UpdateCachedMembers();
			var r = polarPoint.x;
			var a = polarPoint.y;

			var x = r * Mathf.Cos(a);
			var y = r * Mathf.Sin(a);
			var z = polarPoint.z;
			var plane = new Vector3(x, y, z);

			return _pwMatrix.MultiplyPoint3x4(plane);
		}
#endregion  // Polar <-> World coordinate transformation

#region  Grid <-> Polar coordinate transformation
		/// <summary> Converts a point from grid to polar space. </summary>
		/// <returns> Point in polar space. </returns>
		/// <param name="gridPoint"> Point in grid space. </param>
		/// <remarks>
		///   Converts a point from grid to polar space. The main difference is that grid
		///   coordinates are dependent on the grid's parameters, while polar coordinates are not.
		/// </remarks>
		public Vector3 GridToPolar(Vector3 gridPoint) {
			gridPoint.y = Float2Sector(gridPoint.y);
			var scale = new Vector3(Radius, Radians, Depth);
			var polar = Vector3.Scale(gridPoint, scale);
			return polar;
		}
		
		/// <summary> Converts a point from polar to grid space. </summary>
		/// <returns> Point in grid space. </returns>
		/// <param name="polarPoint"> Point in polar space. </param>
		/// <remarks>
		///   Converts a point from polar to grid space. The main difference is that grid
		///   coordinates are dependent on the grid's parameters, while polar coordinates are not.
		/// </remarks>
		public Vector3 PolarToGrid(Vector3 polarPoint) {
			var relativeVector = new Vector3(Radius, Float2Rad(Radians), Depth);
			for (int i = 0; i <=2; i++){
				polarPoint[i] = polarPoint[i]/relativeVector[i];
			}
			return polarPoint;
		}
        #endregion  // Grid <-> Polar coordinate transformation

        #region  Private Conversions
        /// <summary>
        ///   Interprets a float as radians; loops if value exceeds 2π and runs in
        ///   reverse for negative values.
        /// </summary>
        private static float Float2Rad(float number) =>
        	number >= 0 ? number % (2 * Mathf.PI) : 2 * Mathf.PI + (number % Mathf.PI);

        /// <summary>
        ///   Interprets a float as sector; loops if value exceeds [sectors] and
        ///   runs in reverse for negative values.
        /// </summary>
        private float Float2Sector(float number) =>
			number >= 0 ? number % Sectors : Sectors + (number % Sectors);
#endregion  // Private Conversions
	}
}
