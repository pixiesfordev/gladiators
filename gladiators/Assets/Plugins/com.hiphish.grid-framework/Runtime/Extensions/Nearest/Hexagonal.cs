using UnityEngine;
using HexGrid = GridFramework.Grids.HexGrid;

namespace GridFramework.Extensions.Nearest {
	/// <summary>
	///   Extension methods for finding the nearest vertex, face or cell in a
	///   hexagonal grid.
	/// </summary>
	public static class Hexgonal {

		/// <summary>
		///   The various coordinate systems supported by hexagonal grids.
		/// </summary>
		/// <remarks>
		///   <para>
		///     Cubic and barycentric coordinates are four-dimensional, the
		///     rest are three-dimensional.
		///   </para>
		/// </remarks>
		private enum CoordinateSystem {
			World,
			/// <remarks>
			///   This coordinate system does not look hexagonal and is only used as an intermediate
			///   step.
			/// </remarks>
			Grid,
			HerringboneUp,
			HerringboneDown,
			RhombicUp,
			RhombicDown,
			Cubic,
		};
#region  Nearest vertex
		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> World position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The rectangular grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		public static Vector3 NearestWorldVertex(this HexGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.World);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Herringbone up position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The rectangular grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		public static Vector3 NearestHerringboneUpVertex(this HexGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.HerringboneUp);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Herringbone down position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The rectangular grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		public static Vector3 NearestHerringboneDownVertex(this HexGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.HerringboneDown);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Rhombic up position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The rectangular grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		public static Vector3 NearestRhombicUpVertex(this HexGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.RhombicUp);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Rhombic down position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The rectangular grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		public static Vector3 NearestRhombicDownVertex(this HexGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.RhombicDown);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Cubic position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The rectangular grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		public static Vector3 NearestCubicVertex(this HexGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.Cubic);
		}

		private static Vector3 NearestVertex(HexGrid grid, Vector3 point, CoordinateSystem system) {
			// Vertices in the hex-grid are dual to faces in the triangular tessellation of the
			// grid. Convert the point to cubic coordinates
			var cubicPoint = grid.WorldToCubic(point);
			cubicPoint.x = Mathf.Floor(cubicPoint.x) + .5f;
			cubicPoint.y = Mathf.Floor(cubicPoint.y) + .5f;
			cubicPoint.z = Mathf.Floor(cubicPoint.z) + .5f;
			cubicPoint.w = Mathf.Round(cubicPoint.w);

			switch (system) {
				case CoordinateSystem.Cubic:
					return cubicPoint;
				case CoordinateSystem.HerringboneUp:
					return grid.CubicToHerringU(cubicPoint);
				case CoordinateSystem.HerringboneDown:
					return grid.CubicToHerringD(cubicPoint);
				case CoordinateSystem.RhombicUp:
					return grid.CubicToRhombic(cubicPoint);
				case CoordinateSystem.RhombicDown:
					return grid.CubicToRhombicD(cubicPoint);
				case CoordinateSystem.World:
					return grid.CubicToWorld(cubicPoint);
			}
			throw new System.ComponentModel.InvalidEnumArgumentException();
		}
#endregion  // Nearest vertex

#region  Nearest face
		/// <summary> Computes the position of the nearest face. </summary>
		/// <returns> World position of the nearest face. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <remarks>
		///   Since the face is enclosed by six vertices, the returned value is the point in between
		///   all six of the vertices. You also need to specify on which plane the face lies.
		/// </remarks>
		public static Vector3 NearestWorldFace(this HexGrid grid, Vector3 point) {
			return NearestFace(grid, point, CoordinateSystem.World);
		}

		/// <summary> Computes the position of the nearest face. </summary>
		/// <returns> Herringbone up position of the nearest face. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <remarks>
		///   Since the face is enclosed by six vertices, the returned value is the point in between
		///   all six of the vertices. You also need to specify on which plane the face lies.
		/// </remarks>
		public static Vector3 NearestHerringboneUpFace(this HexGrid grid, Vector3 point) {
			return NearestFace(grid, point, CoordinateSystem.HerringboneUp);
		}

		/// <summary> Computes the position of the nearest face. </summary>
		/// <returns> Herringbone down position of the nearest face. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <remarks>
		///   Since the face is enclosed by six vertices, the returned value is the point in between
		///   all six of the vertices. You also need to specify on which plane the face lies.
		/// </remarks>
		public static Vector3 NearestHerringboneDownFace(this HexGrid grid, Vector3 point) {
			return NearestFace(grid, point, CoordinateSystem.HerringboneDown);
		}

		/// <summary> Computes the position of the nearest face. </summary>
		/// <returns> Rhombic up position of the nearest face. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <remarks>
		///   Since the face is enclosed by six vertices, the returned value is the point in between
		///   all six of the vertices. You also need to specify on which plane the face lies.
		/// </remarks>
		public static Vector3 NearestRhombicUpFace(this HexGrid grid, Vector3 point) {
			return NearestFace(grid, point, CoordinateSystem.RhombicUp);
		}

		/// <summary> Computes the position of the nearest face. </summary>
		/// <returns> Rhombic down position of the nearest face. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <remarks>
		///   Since the face is enclosed by six vertices, the returned value is the point in between
		///   all six of the vertices. You also need to specify on which plane the face lies.
		/// </remarks>
		public static Vector3 NearestRhombicDownFace(this HexGrid grid, Vector3 point) {
			return NearestFace(grid, point, CoordinateSystem.RhombicDown);
		}

		/// <summary> Computes the position of the nearest face. </summary>
		/// <returns> Cubic position of the nearest face. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <remarks>
		///   Since the face is enclosed by six vertices, the returned value is the point in between
		///   all six of the vertices. You also need to specify on which plane the face lies.
		/// </remarks>
		public static Vector3 NearestCubicFace(this HexGrid grid, Vector3 point) {
			return NearestFace(grid, point, CoordinateSystem.Cubic);
		}

		private static Vector3 NearestFace(HexGrid grid, Vector3 point, CoordinateSystem system) {
			// Faces in the hex-grid are dual to vertices in the triangular
			// tessellation of the grid. Convert the point to cubic coordinates
			var cubic = grid.WorldToCubic(point);
			// We need the original cubic coordinates and the rounded ones, so
			// use a separate variable
			var rounded = new Vector4(
				Mathf.Round(cubic.x),
				Mathf.Round(cubic.y),
				Mathf.Round(cubic.z),
				Mathf.Round(cubic.w)
			);

			// Rounding all three coordinates does not guarantee that their sum
			// is zero. Therefore we will find the coordinate with the largest
			// change and compute its value from the other two instead of its
			// rounded value.
			var deltaX = Mathf.Abs(rounded.x - cubic.x);
			var deltaY = Mathf.Abs(rounded.y - cubic.y);
			var deltaZ = Mathf.Abs(rounded.z - cubic.z);

			if (deltaX > deltaY && deltaX > deltaZ) {
				rounded.x = -rounded.y - rounded.z;
			} else if (deltaY > deltaZ) {
				rounded.y = -rounded.x - rounded.z;
			} else {
				rounded.z = -rounded.x - rounded.y;
			}

			switch (system) {
				case CoordinateSystem.Cubic:
					return rounded;
				case CoordinateSystem.HerringboneUp:
					return grid.CubicToHerringU(rounded);
				case CoordinateSystem.HerringboneDown:
					return grid.CubicToHerringD(rounded);
				case CoordinateSystem.RhombicUp:
					return grid.CubicToRhombic(rounded);
				case CoordinateSystem.RhombicDown:
					return grid.CubicToRhombicD(rounded);
				case CoordinateSystem.World:
					return grid.CubicToWorld(rounded);
			}
			throw new System.ComponentModel.InvalidEnumArgumentException();
		}
#endregion  // Nearest face

#region  Nearest cell
		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> World position of the nearest box. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		/// 
		/// <remarks>
		///   Since a cell lies between vertices all three values will always have an offset
		///   compared to vertex coordinates.
		/// </remarks>
		public static Vector3 NearestWorldCell(this HexGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.World);
		}

		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> Herringbone up position of the nearest box. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		/// 
		/// <remarks>
		///   Since a cell lies between vertices all three values will always have an offset
		///   compared to vertex coordinates.
		/// </remarks>
		public static Vector3 NearestHerringboneUpCell(this HexGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.HerringboneUp);
		}

		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> Herringbone down position of the nearest box. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		/// 
		/// <remarks>
		///   Since a cell lies between vertices all three values will always have an offset
		///   compared to vertex coordinates.
		/// </remarks>
		public static Vector3 NearestHerringboneDownCell(this HexGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.HerringboneDown);
		}

		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> Rhombic up position of the nearest box. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		/// 
		/// <remarks>
		///   Since a cell lies between vertices all three values will always have an offset
		///   compared to vertex coordinates.
		/// </remarks>
		public static Vector3 NearestRhombicUpCell(this HexGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.RhombicUp);
		}

		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> Rhombic down position of the nearest box. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		/// 
		/// <remarks>
		///   Since a cell lies between vertices all three values will always have an offset
		///   compared to vertex coordinates.
		/// </remarks>
		public static Vector3 NearestRhombicDownCell(this HexGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.RhombicDown);
		}

		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> Cubic position of the nearest box. </returns>
		/// <param name="grid"> The hexagonal grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		/// 
		/// <remarks>
		///   Since a cell lies between vertices all three values will always have an offset
		///   compared to vertex coordinates.
		/// </remarks>
		public static Vector3 NearestCubic(this HexGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.Cubic);
		}

		private static Vector3 NearestCell(HexGrid grid, Vector3 point, CoordinateSystem system) {
			// Faces in the hex-grid are dual to vertices in the triangular tessellation of the
			// grid. Convert the point to cubic coordinates
			var cubicPoint = grid.WorldToCubic(point);
			cubicPoint.x = Mathf.Round(cubicPoint.x);
			cubicPoint.y = Mathf.Round(cubicPoint.y);
			cubicPoint.z = Mathf.Round(cubicPoint.z);
			cubicPoint.w = Mathf.Floor(cubicPoint.w) + .5f;

			switch (system) {
				case CoordinateSystem.Cubic:
					return cubicPoint;
				case CoordinateSystem.HerringboneUp:
					return grid.CubicToHerringU(cubicPoint);
				case CoordinateSystem.HerringboneDown:
					return grid.CubicToHerringD(cubicPoint);
				case CoordinateSystem.RhombicUp:
					return grid.CubicToRhombic(cubicPoint);
				case CoordinateSystem.RhombicDown:
					return grid.CubicToRhombicD(cubicPoint);
				case CoordinateSystem.World:
					return grid.CubicToWorld(cubicPoint);
			}
			throw new System.ComponentModel.InvalidEnumArgumentException();
		}
#endregion  // Nearest cell
	}
}

