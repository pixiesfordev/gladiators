using UnityEngine;
using System.Collections.Generic;
using GridFramework.Grids;
using LineSegment = GridFramework.Rendering.LineSegment;

namespace GridFramework.Renderers.Hexagonal {
	/// <summary>
	///   Base class for all hexagonal grid renderers.
	/// </summary>
	[RequireComponent(typeof(HexGrid))]
	public abstract class HexRenderer: GridRenderer {
		[SerializeField]
		private HexGrid _grid;

		private float oldRadius, oldDepth;
		private HexGrid.Orientation oldSides;

		/// <summary>
		///   Reference to the grid.
		/// </summary>
		protected HexGrid Grid {
			get {
				if (!_grid) {
					_grid = GetComponent<HexGrid>();
				}
				return _grid;
			}
		}

		/// <summary>
		///   Whether the grid has pointed sides.
		/// </summary>
		protected bool PointedSides {
			get => Grid.Sides == HexGrid.Orientation.Pointed;
		}

		/// <summary>
		///   Whether the grid has flat sides.
		/// </summary>
		protected bool FlatSides {
			get => !PointedSides;
		}

		protected static void ContributeLine(IList<LineSegment> lineSegments, Vector3 hex, Vector3 point1, Vector3 point2, ref int iterator) {
			lineSegments[iterator++] = new LineSegment(hex + point1, hex + point2);
		}

		protected static void ContributeLine(IList<LineSegment> lineSegments, Vector3 hex, Vector3 vertex, Vector3 back, Vector3 front, ref int i) {
			lineSegments[i++] = new LineSegment(hex + vertex + back, hex + vertex + front);
		}

		protected Vector3 CardinalToVertex(HexGrid.HexDirection direction) {
			Vector4 result;  // Use switch expression when Unity switches to C# 8.0
			switch (direction) {
				case HexGrid.HexDirection.E  : result = new Vector4( 2f/3f, -1f/3f, -1f/3f); break;
				case HexGrid.HexDirection.NE : result = new Vector4( 1f/3f,  1f/3f, -2f/3f); break;
				case HexGrid.HexDirection.NW : result = new Vector4(-1f/3f,  2f/3f, -1f/3f); break;
				case HexGrid.HexDirection.W  : result = new Vector4(-2f/3f,  1f/3f,  1f/3f); break;
				case HexGrid.HexDirection.SW : result = new Vector4(-1f/3f, -1f/3f,  2f/3f); break;
				case HexGrid.HexDirection.SE : result = new Vector4( 1f/3f, -2f/3f,  1f/3f); break;
				default: throw new System.ArgumentOutOfRangeException();
			}
			var vertex = Grid.CubicToWorld(result) - Grid.CubicToWorld(Vector4.zero);
			return vertex;
		}

		protected static void Swap<T>(ref T a, ref T b, bool condition = true) {
			if (!condition) {
				return;
			}
			var temp = a;
			a = b;
			b = temp;
		}

		protected static bool IsEven(int i) => i % 2 == 0;
		protected static bool IsOdd(int i) => !IsEven(i);

        protected override bool GridHasChanged() {
            var different = Mathf.Abs(Grid.Depth - oldDepth) > Mathf.Epsilon
            	|| Mathf.Abs(Grid.Radius - oldRadius) > Mathf.Epsilon
            	|| Grid.Sides != oldSides;

            if (different) {
            	oldDepth = Grid.Depth;
            	oldRadius = Grid.Radius;
            	oldSides = Grid.Sides;
            	return true;
            }

            return false;
        }
	}
}
