using UnityEngine;
using PolarGrid = GridFramework.Grids.PolarGrid;

namespace GridFramework.Extensions.Nearest {
	/// <summary>
	///   Extension methods for finding the nearest vertex, face or cell in a polar grid.
	/// </summary>
	public static class Polar {
		private enum CoordinateSystem {
			World,
			Grid,
			Cylindrical,
		}

#region  Nearest vertex
		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> World position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The polar grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <seealso cref="NearestGridVertex">
		/// <seealso cref="NearestCylindricalVertex">
		public static Vector3 NearestWorldVertex(this PolarGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.World);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Grid position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The polar grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <seealso cref="NearestWorldVertex">
		/// <seealso cref="NearestCylindricalVertex">
		public static Vector3 NearestGridVertex(this PolarGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.Grid);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Cylindrical position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The polar grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <seealso cref="NearestWorldVertex">
		/// <seealso cref="NearestGridVertex">
		public static Vector3 NearestCylindricalVertex(this PolarGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.Cylindrical);
		}
#endregion  // Nearest vertex

#region  Nearest face
		/// <summary> Computes the position of the nearest face. </summary>
		/// <returns> World osition of the nearest face.  </returns>
		/// <param name="grid"> The polar grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		///
		/// <remarks>
		///   Since the face is enclosed by four vertices, the returned value is the point in
		///   between all four of the vertices.
		/// </remarks>
		///
		/// <seealso cref="NearestCylindricalFace">
		/// <seealso cref="NearestGridFace">
		public static Vector3 NearestWorldFace(this PolarGrid grid, Vector3 point) {
			return NearestFace(grid, point, CoordinateSystem.World);
		}

		/// <summary> Computes the position of the nearest face. </summary>
		/// <returns> Cylindrical position of the nearest face.  </returns>
		/// <param name="grid"> The polar grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		///
		/// <remarks>
		///   Since the face is enclosed by four vertices, the returned value is the point in
		///   between all four of the vertices.
		/// </remarks>
		///
		/// <seealso cref="NearestWorldFace">
		/// <seealso cref="NearestGridFace">
		public static Vector3 NearestCylindricalFace(this PolarGrid grid, Vector3 point) {
			return NearestFace(grid, point, CoordinateSystem.Cylindrical);
		}

		/// <summary> Computes the position of the nearest face. </summary>
		/// <returns> Grid position of the nearest face.  </returns>
		/// <param name="grid"> The polar grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		///
		/// <remarks>
		///   Since the face is enclosed by four vertices, the returned value is the point in
		///   between all four of the vertices.
		/// </remarks>
		///
		/// <seealso cref="NearestWorldFace">
		/// <seealso cref="NearestCylindricalFace">
		public static Vector3 NearestGridFace(this PolarGrid grid, Vector3 point) {
			return NearestFace(grid, point, CoordinateSystem.Grid);
		}
#endregion  // Nearest face

#region  Nearest cell
		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> World position of the nearest cell.  </returns>
		/// <param name="grid"> The polar grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		/// 
		/// <remarks>
		///   Since cells lie between faces all three values will always have an offset of
		///   <c>0.5</c> compared to vertex coordinates.
		/// </remarks>
		///
		/// <seealso cref="NearestGridCell">
		/// <seealso cref="NearestCylindricalCell">
		public static Vector3 NearestWorldCell(this PolarGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.World);
		}

		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> Grid position of the nearest cell.  </returns>
		/// <param name="grid"> The polar grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		/// 
		/// <remarks>
		///   Since cells lie between faces all three values will always have an offset of
		///   <c>0.5</c> compared to vertex coordinates.
		/// </remarks>
		///
		/// <seealso cref="NearestWorldCell">
		/// <seealso cref="NearestCylindricalCell">
		public static Vector3 NearestGridCell(this PolarGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.Grid);
		}

		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> Cylindrical position of the nearest cell.  </returns>
		/// <param name="grid"> The polar grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		/// 
		/// <remarks>
		///   Since cells lie between faces all three values will always have an offset of
		///   <c>0.5</c> compared to vertex coordinates.
		/// </remarks>
		///
		/// <seealso cref="NearestWorldCell">
		/// <seealso cref="NearestGridCell">
		public static Vector3 NearestCylindricalCell(this PolarGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.Cylindrical);
		}
#endregion  // Nearest cell

#region  Helpers
		private static Vector3 NearestVertex(PolarGrid grid, Vector3 point, CoordinateSystem system) {
			var gridPoint = grid.WorldToGrid(point);
			for (var i = 0; i < 3; ++i) {
				gridPoint[i] = Mathf.Round(gridPoint[i]);
			}

			return GridToCoordinateSystem(grid, gridPoint, system);
		}

		private static Vector3 NearestFace(PolarGrid grid, Vector3 point, CoordinateSystem system) {
			Vector3 gridPoint = grid.WorldToGrid(point);

			gridPoint.x = Mathf.Floor(gridPoint.x) + .5f;
			gridPoint.y = Mathf.Floor(gridPoint.y) + .5f;
			gridPoint.z = Mathf.Round(gridPoint.z);

			return GridToCoordinateSystem(grid, gridPoint, system);
		}

		private static Vector3 NearestCell(PolarGrid grid, Vector3 point, CoordinateSystem system) {
			Vector3 gridPoint = grid.WorldToGrid(point);

			gridPoint.x = Mathf.Floor(gridPoint.x) + .5f;
			gridPoint.y = Mathf.Floor(gridPoint.y) + .5f;
			gridPoint.z = Mathf.Floor(gridPoint.z) + .5f;

			return GridToCoordinateSystem(grid, gridPoint, system);
		}

		private static Vector3 GridToCoordinateSystem(PolarGrid grid, Vector3 gridPoint, CoordinateSystem system) {
			switch (system) {
				case CoordinateSystem.Grid:
					return gridPoint;
				case CoordinateSystem.World:
					return grid.GridToWorld(gridPoint);
				case CoordinateSystem.Cylindrical:
					return grid.GridToPolar(gridPoint);
				default:
					var error = string.Format("Error: Coordinate system \"{0}\" unimplemented", system);
					throw new System.ComponentModel.InvalidEnumArgumentException(error);
			}
		}
#endregion  // Helpers
	}
}
