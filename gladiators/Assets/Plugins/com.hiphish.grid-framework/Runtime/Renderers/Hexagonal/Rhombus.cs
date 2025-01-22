using UnityEngine;
using GridFramework.Grids;
using GridFramework.Rendering;

// Drawing Z-lines
//
// There needs to be a Z-line for each vertex of a hex, but we must avoid
// covering a hex more than once. The exact vertex depends on whether the
// rhombus is pointing upwards or downwards.
//
//      _
//    _/ \  All: east and west
//  _/ \_/  Left: south-west vertex (upwards) or north-west (down)
// / \_/ \  Right: north-east vertex (upwards) or south-east (down)
// \_/ \_/  Botton: south-east (up) or south-west (down)
// / \_/ \  Top: north-west (up) or north-east (down)
// \_/ \_/
// / \_/
// \_/

namespace GridFramework.Renderers.Hexagonal {
	/// <summary>
	///   A rhombic arrangement of hexagons with configurable direction.
	/// </summary>
	/// <remarks>
	///   <para>
	///     Hexagons are arranged in the rhombic pattern shifted up or down
	///     (right or left if the sides are pointed), depending on your
	///     settings.
	///   </para>
	/// </remarks>
	[AddComponentMenu("Grid Framework/Renderers/Hexagonal/Rhombus")]
	public sealed class Rhombus : HexRenderer {

#region  Types
		/// <summary>
		///   Direction of the rhombus (up/down or right/left).
		/// </summary>
		public enum RhombDirection {
			/// <summary>
			///   Every column is shifted upwards (pointed sides) or to the
			///   right (flat sides).
			/// </summary>
			Up,
			/// <summary>
			///   Every column is shifted downwards (pointed sides) or to the
			///   left (flat sides).
			/// </summary>
			Down
		}
#endregion  // Types

#region  Private variables
		[SerializeField] private int _bottom = -2;
		[SerializeField] private int _top    =  2;
		[SerializeField] private int _left   = -2;
		[SerializeField] private int _right  =  2;

		[SerializeField] private float _layerFrom = -2;
		[SerializeField] private float _layerTo   =  2;

		[SerializeField] private RhombDirection _direction;
#endregion  // Private variables

#region  Properties
		/// <summary>
		///   The direction of the rhombus, up- or downwards.
		/// </summary>
		public RhombDirection Direction {
			get => _direction;
			set {
				var previous = _direction;
				_direction= value;
				OnDirectionChanged(previous);
			}
		}

		/// <summary>
		///   Index of the bottom row.
		/// </summary>
		public int Bottom {
			get => _bottom;
			set {
				var previous = _bottom;
				_bottom = Mathf.Min(value, Top);
				OnHorizontalChanged(previous, Top);
			}
		}

		/// <summary>
		///   Index of the top row.
		/// </summary>
		public int Top {
			get => _top;
			set {
				var previous = _top;
				_top = Mathf.Max(value, Bottom);
				OnHorizontalChanged(Bottom, previous);
			}
		}

		/// <summary>
		///   Index of the left column.
		/// </summary>
		public int Left {
			get => _left;
			set {
				var previous = _left;
				_left = Mathf.Min(value, Right);
				OnVerticalChanged(previous, Right);
			}
		}

		/// <summary>
		///   Index of the right column.
		/// </summary>
		public int Right {
			get => _right;
			set {
				var previous = _right;
				_right = Mathf.Max(value, Left);
				OnVerticalChanged(Left, previous);
			}
		}

		/// <summary>
		///   First layer of the rendering.
		/// </summary>
		public float LayerFrom {
			get => _layerFrom;
			set {
				var previous = _layerFrom;
				_layerFrom = Mathf.Min(LayerTo, value);
				OnLayerChanged(previous, LayerTo);
			}
		}

		/// <summary>
		///   Last layer of the rendering.
		/// </summary>
		public float LayerTo {
			get => _layerTo;
			set {
				var previous = _layerTo;
				_layerTo = Mathf.Max(LayerFrom, value);
				OnLayerChanged(LayerFrom, previous);
			}
		}
#endregion

#region  Count
		protected override (int x, int y, int z) CountLines() {
			// The rhombic points are easier to count than herring points:
			//      _
			//    _/ \ - The number of horizontal lines is the number of columns
			//  _/ \_/   times (number of rows + 1).
			// / \_/ \ - The number of angled lines is two times the number of rows
			// \_/ \_/   times the number of columns+1 plus two times the number of
			// / \_/ \   columns-1.
			// \_/ \_/ - The number of cylindrical lines is two times the number of
			// / \_/     rows times number of +1 plus two times the number of
			// \_/       columns.
			var cs = Right - Left   + 1;  // columns
			var rs = Top   - Bottom + 1;  // rows

			// swap the role of horizontal and vertical for flat sides
			Swap<int>(ref cs, ref rs, FlatSides);

			var l = Mathf.FloorToInt(LayerTo) - Mathf.CeilToInt(LayerFrom) + 1; //layers

			var xCount = cs * (rs+1) * l;
			var yCount = (2 * rs * (cs+1) + cs-1) * l;
			var zCount =  2 * rs * (cs+1) + 2*cs;

			Swap<int>(ref xCount, ref yCount, FlatSides);
			return (xCount, yCount, zCount);
		}
#endregion  // Count

#region  Compute
		protected override void ComputeLines(LineSegment[][] lineSets) {
			int Front = Mathf.FloorToInt(LayerTo), Back = Mathf.CeilToInt(LayerFrom);

			var pointed   = Grid.Sides == HexGrid.Orientation.Pointed;
			var flat      = Grid.Sides == HexGrid.Orientation.Flat;
			var upwards   = Direction == RhombDirection.Up;
			var downwards = Direction == RhombDirection.Down;

			Vector3 EE, NE, NW, WW, SW, SE;

			if (pointed) {
				EE = CardinalToVertex(HexGrid.HexDirection.E );
				NE = CardinalToVertex(HexGrid.HexDirection.NE);
				NW = CardinalToVertex(HexGrid.HexDirection.NW);
				WW = CardinalToVertex(HexGrid.HexDirection.W );
				SW = CardinalToVertex(HexGrid.HexDirection.SW);
				SE = CardinalToVertex(HexGrid.HexDirection.SE);
			} else {
				EE = CardinalToVertex(HexGrid.HexDirection.SE);
				NE = CardinalToVertex(HexGrid.HexDirection.E );
				NW = CardinalToVertex(HexGrid.HexDirection.NE);
				WW = CardinalToVertex(HexGrid.HexDirection.NW);
				SW = CardinalToVertex(HexGrid.HexDirection.W );
				SE = CardinalToVertex(HexGrid.HexDirection.SW);
			}

			var (right, up, forward) = MakeBasisVectors(pointed, upwards);

			// Swap coordinates counter-clockwise for flat sides
			var leftEdge   = pointed ? Left    : -Top;
			var rightEdge  = pointed ? Right   : -Bottom;
			var bottomEdge = pointed ? Bottom  :  Left;
			var topEdge    = pointed ? Top     :  Right;

			// iterator variables
			int iterator_x = 0, iterator_y = 0, iterator_z = 0;

			if (FlatSides) {
				Swap<LineSegment[]>(ref lineSets[0], ref lineSets[1]);
			}

			System.Action<Vector3, int, int, int> drawHex = (hex, column, row, layer) => {
				System.Action<Vector3, Vector3> horizontalLine =
					(v1, v2) => ContributeLine(lineSets[0], hex, v1, v2, ref iterator_x);
				System.Action<Vector3, Vector3> verticalLine =
					(v1, v2) => ContributeLine(lineSets[1], hex, v1, v2, ref iterator_y);

				var topmost   = row == topEdge;
				var lowest    = row == bottomEdge;
				var rightmost = column == rightEdge;

				horizontalLine(SE, SW);
				verticalLine(SW, WW);
				verticalLine(WW, NW);
				if (topmost) {
					horizontalLine(NW, NE);
				}
				if (rightmost) {
					verticalLine(SE, EE);
					verticalLine(EE, NE);
					return;
				}
				if (lowest && (pointed ? upwards : downwards)) {
					verticalLine(SE, EE);
				} else if (topmost && (pointed ? downwards : upwards)) {
					verticalLine(EE, NE);
				}
			};

			System.Action<int, int> layerLines = (column, row) => {
				// See comment of the file for which lines to add for which hex
				var hex = DetermineOrigin(pointed, upwards, column, row, 0);

				lineSets[2][iterator_z++] = LayerLine(hex + EE, forward);
				lineSets[2][iterator_z++] = LayerLine(hex + WW, forward);

				(int value, int limit, Vector3 v1, Vector3 v2)[] specs = {
					(column, leftEdge,   SW, NW),
					(column, rightEdge,  NE, SE),
					(row,    bottomEdge, SE, SW),
					(row,    topEdge,    NW, NE),
				};
				foreach (var spec in specs) {
					var (value, limit, v1, v2) = spec;
					if (value == limit) {
						var shift = upwards ? (PointedSides ? v1 : v2) : (PointedSides ? v2 : v1);
						lineSets[2][iterator_z++] = LayerLine(hex + shift, forward);
					}
				}
			};

			var hexColumn = DetermineOrigin(pointed, upwards, leftEdge, bottomEdge, Back);
			for (var column = leftEdge; column <= rightEdge; ++column) {
				var hexRow = hexColumn;
				for (var row = bottomEdge; row <= topEdge; ++row) {
					var hexLayer = hexRow;
					for (var layer = Back; layer <= Front; ++layer) {
						drawHex(hexLayer, column, row, layer);
						hexLayer += forward;
					}
					hexRow += up;
					layerLines(column, row);
				}
				hexColumn += right;
			}

			if (FlatSides) {
				Swap<LineSegment[]>(ref lineSets[0], ref lineSets[1]);
			}
		}

		Vector3 DetermineOrigin(bool pointed, bool upwards, int left, int bottom, int back) {
			var rhomb = pointed ? new Vector3(left, bottom, back) : new Vector3(bottom, -left, back);
			return upwards ? Grid.RhombicUpToWorld(rhomb) : Grid.RhombicDownToWorld(rhomb);
		}

		private (Vector3 right, Vector3 up, Vector3 forward) MakeBasisVectors(bool pointed, bool upwards) {
			var zeroHex = upwards
			              ? Grid.RhombicUpToWorld(Vector3.zero)
			              : Grid.RhombicDownToWorld(Vector3.zero);

			var rVector = pointed ? Vector3.right : -Vector3.up;
			var uVector = pointed ? Vector3.up    :  Vector3.right;
			var right = (upwards
			        ? Grid.RhombicUpToWorld(rVector)
			        : Grid.RhombicDownToWorld(rVector))
			        - zeroHex;

			var up = (upwards
			     ? Grid.RhombicUpToWorld(uVector)
			     : Grid.RhombicDownToWorld(uVector))
			     - zeroHex;

			var forward = (upwards
			          ? Grid.RhombicUpToWorld( Vector3.forward)
			          : Grid.RhombicDownToWorld(Vector3.forward))
			          - zeroHex;

			return (right, up, forward);
		}

		private LineSegment LayerLine(Vector3 hex, Vector3 forward) {
			var from = hex + forward * LayerFrom;
			var   to = hex + forward * LayerTo;
			return new LineSegment(from, to);
		}
#endregion  // Compute

#region  Hook methods
		private void OnHorizontalChanged(int previousLeft, int previousRight) {
			if (previousLeft == Left && previousRight == Right) {
				return;
			}
			MarkAsDirty();
		}

		private void OnVerticalChanged(int previousBotton, int previousTop) {
			if (previousBotton == Left && previousTop == Right) {
				return;
			}
			MarkAsDirty();
		}

		private void OnLayerChanged(float previousFrom, float previousTo) {
			var changedFrom = Mathf.Abs(previousFrom - LayerFrom) < Mathf.Epsilon;
			var changedTo   = Mathf.Abs(previousTo   - LayerTo  ) < Mathf.Epsilon;
			if (changedFrom && changedTo) {
				return;
			}
			MarkAsDirty();
		}

		private void OnDirectionChanged(RhombDirection previous) {
			if (previous == Direction) {
				return;
			}
			MarkAsDirty();
		}
#endregion  // Hook methods
	}
}
