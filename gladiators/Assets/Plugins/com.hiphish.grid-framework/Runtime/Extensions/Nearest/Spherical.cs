using System;
using UnityEngine;
using SphereGrid = GridFramework.Grids.SphereGrid;

namespace GridFramework.Extensions.Nearest {
	/// <summary>
	///   Extension methods for finding the nearest vertex or cell in a spherical grid.
	/// </summary>
	public static class Spherical {
		private enum CoordinateSystem {
			World,
			Grid,
			Spherical,
			Geographic,
		}

#region  Nearest vertex
		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> World position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The spherical grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <seealso cref="NearestGridVertex" />
		/// <seealso cref="NearestSphericalVertex" />
		/// <seealso cref="NearestGeographicVertex" />
		public static Vector3 NearestWorldVertex(this SphereGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.World);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Grid position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The spherical grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <seealso cref="NearestWorldVertex" />
		/// <seealso cref="NearestSphericalVertex" />
		/// <seealso cref="NearestGeographicVertex" />
		public static Vector3 NearestGridVertex(this SphereGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.Grid);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Spherical position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The spherical grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <seealso cref="NearestWorldVertex" />
		/// <seealso cref="NearestGridVertex" />
		/// <seealso cref="NearestGeographicVertex" />
		public static Vector3 NearestSphericalVertex(this SphereGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.Spherical);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Geographic position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The spherical grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		///
		/// <seealso cref="NearestWorldVertex" />
		/// <seealso cref="NearestGridVertex" />
		/// <seealso cref="NearestSphericalVertex" />
		public static Vector3 NearestGeographicVertex(this SphereGrid grid, Vector3 point) {
			return NearestVertex(grid, point, CoordinateSystem.Geographic);
		}
#endregion  // Nearest vertex

#region  Nearest cell
		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> World position of the nearest cell.  </returns>
		///
		/// <param name="grid"> The spherical grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		///
		/// <remarks>
		///     Since cell lies between vertices all three values will always be offset by +0.5
		///     compared to vertex coordinates.
		/// </remarks>
		///
		/// <seealso cref="NearestGridCell" />
		/// <seealso cref="NearestSphericalCell" />
		/// <seealso cref="NearestGeographicCell" />
		public static Vector3 NearestWorldCell(this SphereGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.World);
		}

		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> Grid position of the nearest cell.  </returns>
		///
		/// <param name="grid"> The spherical grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		///
		/// <remarks>
		///     Since cell lies between vertices all three values will always be offset by +0.5
		///     compared to vertex coordinates.
		/// </remarks>
		///
		/// <seealso cref="NearestWorldCell" />
		/// <seealso cref="NearestSphericalCell" />
		/// <seealso cref="NearestGeographicCell" />
		public static Vector3 NearestGridCell(this SphereGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.Grid);
		}

		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> Spherical position of the nearest cell.  </returns>
		///
		/// <param name="grid"> The spherical grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		///
		/// <remarks>
		///     Since cell lies between vertices all three values will always be offset by +0.5
		///     compared to vertex coordinates.
		/// </remarks>
		///
		/// <seealso cref="NearestWorldCell" />
		/// <seealso cref="NearestGridCell" />
		/// <seealso cref="NearestGeographicCell" />
		public static Vector3 NearestSphericalCell(this SphereGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.Spherical);
		}

		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> Geographic position of the nearest cell.  </returns>
		///
		/// <param name="grid"> The spherical grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		///
		/// <remarks>
		///     Since cell lies between vertices all three values will always be offset by +0.5
		///     compared to vertex coordinates.
		/// </remarks>
		///
		/// <seealso cref="NearestWorldCell" />
		/// <seealso cref="NearestGridCell" />
		/// <seealso cref="NearestSphericalCell" />
		public static Vector3 NearestGeographicCell(this SphereGrid grid, Vector3 point) {
			return NearestCell(grid, point, CoordinateSystem.Geographic);
		}
#endregion  // Nearest cell

#region  Helpers
		private static Vector3 NearestVertex(SphereGrid grid, Vector3 point, CoordinateSystem system) {
			var gridVertex = RoundPoint(grid.WorldToGrid(point));
			return gridToCoordinateSystem(grid, gridVertex, system);
		}

		private static Vector3 NearestCell(SphereGrid grid, Vector3 point, CoordinateSystem system) {
			var gridCell = RoundPoint(grid.WorldToGrid(point) - 0.5f * Vector3.one) + 0.5f * Vector3.one;
			return gridToCoordinateSystem(grid, gridCell, system);
		}

		private static Vector3 gridToCoordinateSystem(SphereGrid grid, Vector3 gridPoint, CoordinateSystem system) =>
			system switch {
				CoordinateSystem.World => grid.GridToWorld(gridPoint),
				CoordinateSystem.Grid => gridPoint,
				CoordinateSystem.Spherical => grid.GridToSpheric(gridPoint),
				CoordinateSystem.Geographic => grid.GridToGeographic(gridPoint),
				_ => throw new NotImplementedException(
					$"Unknown coordinate system {system}. This is a bug in Grid Framework, please report it"),
			};

		private static Vector3 RoundPoint(Vector3 point) {
			for (int i = 0; i < 3; i++) {
				point[i] = Mathf.Round(point[i]);  // could use Ceil or Floor to always round up or down
			}
			return point;
		}
#endregion  // Helpers
	}
}
