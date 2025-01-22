using UnityEngine;
using RectGrid = GridFramework.Grids.RectGrid;

namespace GridFramework.Extensions.Nearest {
	/// <summary>
	///   Extension methods for finding the nearest vertex or cell in a
	///   rectangular grid.
	/// </summary>
	public static class Rectangular {
#region  Nearest vertex
		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> World position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The rectangular grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		public static Vector3 NearestWorldVertex(this RectGrid grid, Vector3 point) {
			var gridPoint    = grid.WorldToGrid(point);
			var roundedPoint = RoundVector3(gridPoint);

			return grid.GridToWorld(roundedPoint);
		}

		/// <summary> Computes the position of the nearest vertex. </summary>
		/// <returns> Grid position of the nearest vertex. </returns>
		///
		/// <param name="grid"> The rectangular grid instance. </param>
		/// <param name="point"> Point in world space. </param>
		public static Vector3 NearestGridVertex(this RectGrid grid, Vector3 point) {
			var gridPoint    = grid.WorldToGrid(point);
			var roundedPoint = RoundVector3(gridPoint);

			return roundedPoint;
		}
#endregion  // Nearest Vertex

#region  Nearest cell
		/// <summary> Computes the position of the nearest cell. </summary>
		/// <returns> World position of the nearest cell. </returns>
		/// <param name="grid"> The rectangular grid instance.  </param>
		/// <param name="point"> Point in world space.  </param>
		/// 
		/// <remarks>
		///   <para>
		///     Since cell lies between vertices all three values will always have an offset of
		///     <c>+0.5</c> compared to vertex coordinates.
		///   </para>
		/// </remarks>
		public static Vector3 NearestWorldCell(this RectGrid grid, Vector3 point) {
			var shift     = .5f * Vector3.one;
			var gridPoint = grid.WorldToGrid(point);
			var shifted   = gridPoint - shift;
			var rounded   = RoundVector3(shifted);
			var gridCell  = rounded + shift;

			return grid.GridToWorld(gridCell);
		}

		/// <summary> Returns the grid position of the nearest cell. </summary>
		/// <returns> Grid position of the nearest cell. </returns>
		/// <param name="grid"> The rectangular grid instance.  </param>
		/// <param name="point"> Point in world space. </param>
		/// 
		/// <remarks>
		///   <para>
		///     Since cell lies between vertices all three values will always have <c>+0.5 *
		///     grid.Spacing</c> compared to vertex coordinates.
		///   </para>
		/// </remarks>
		public static Vector3 NearestGridCell(this RectGrid grid, Vector3 point) {
			var shift     = .5f * Vector3.one;

			var gridPoint = grid.WorldToGrid(point);
			var shifted   = gridPoint - shift;
			var rounded   = RoundVector3(shifted);
			var gridCell  = rounded + shift;

			return gridCell;
		}
#endregion  // Nearest cell

#region  Helpers
		private static Vector3 RoundVector3(Vector3 point) {
			for (var i = 0; i < 3; ++i) {
				point[i] = Mathf.Round(point[i]);
			}
			return point;
		}
#endregion  // Helpers
	}
}
