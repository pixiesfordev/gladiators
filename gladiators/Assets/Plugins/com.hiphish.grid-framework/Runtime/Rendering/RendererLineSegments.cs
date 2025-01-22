using System.Collections.Generic;
using IndexOutOfRangeException = System.IndexOutOfRangeException;

namespace GridFramework.Rendering {
	/// <summary>
	///   Data class which contains the individual groups of renderer lines. There are always
	///   exactly three groups: X, Y and Z.
	/// </summary>
	/// <seealso cref="LineSegmentGroup"/>
	/// <seealso cref="LineSegment"/>
	public class RendererLineSegments {
		/// <summary>
		///   Create a new set of line segments with given groups.
		/// </summary>
		/// <param name="x"> Group of x-lines. </param>
		/// <param name="y"> Group of y-lines. </param>
		/// <param name="z"> Group of z-lines. </param>
		public RendererLineSegments(LineSegmentGroup x, LineSegmentGroup y, LineSegmentGroup z) {
			X = x; Y = y; Z = z;
		}

		/// <summary> Group of x-lines. </summary>
		public LineSegmentGroup X {get; private set;}

		/// <summary> Group of x-lines. </summary>
		public LineSegmentGroup Y {get; private set;}

		/// <summary> Group of x-lines. </summary>
		public LineSegmentGroup Z {get; private set;}

		/// <summary>
		///   Tuple of the individual groups, ordered as <c>x</c>, <c>y</c>, <c>z</c>.
		/// </summary>
		/// <seealso cref="X"/>
		/// <seealso cref="Y"/>
		/// <seealso cref="Z"/>
		public (LineSegmentGroup x, LineSegmentGroup y, LineSegmentGroup z) Groups
			=> (X, Y, Z);

		/// <summary>
		///   Tuple of line segments per group, ordered as <c>x</c>, <c>y</c>, <c>z</c>.
		/// </summary>
		/// <seealso cref="X"/>
		/// <seealso cref="Y"/>
		/// <seealso cref="Z"/>
		public (LineSegment[] x, LineSegment[] y, LineSegment[] z) Segments 
			=> (X.Segments, Y.Segments, Z.Segments);

		/// <summary>
		///   Tuple of Individual amounts of line segments per group, ordered as <c>x</c>, <c>y</c>,
		///   <c>z</c>.
		/// </summary>
		/// <seealso cref="X"/>
		/// <seealso cref="Y"/>
		/// <seealso cref="Z"/>
		public (int x, int y, int z) Lengths
			=> (X.Segments.Length, Y.Segments.Length, Z.Segments.Length);

		/// <summary>
		///   Total number of line segments across all groups.
		/// </summary>
		/// <seealso cref="Length"/>
		public int Length => X.Length + Y.Length + Z.Length;
		
		/// <summary>
		///   Indexer for the individual groups.
		/// </summary>
		/// <param name="i">
		///   Use <c>0</c> the x-group, <c>1</c> for the y group, or <c>3</c> for the z-group.
		///   Every other value throws an error.
		/// </param>
		/// <remarks>
		///   The indexer is added as a convenience for use in loops.
		/// </remarks>
		/// <seealso cref="X"/>
		/// <seealso cref="Y"/>
		/// <seealso cref="Z"/>
		public LineSegmentGroup this[int i] {
			get {
				switch (i) {
					case 0: return X;
					case 1: return Y;
					case 2: return Z;
				
					default:
						throw new IndexOutOfRangeException($"Only index 0, 1 and 2 allowed, given: {i}");
				}
			}
		}

		/// <summary>
		///   The default enumerator iterates over the individual groups from x to z.
		/// </summary>
		/// <seealso cref="X"/>
		/// <seealso cref="Y"/>
		/// <seealso cref="Z"/>
		public IEnumerator<LineSegmentGroup> GetEnumerator() {
			for (var i = 0; i < 3; ++i) {
				yield return this[i];
			}
		}

		public override string ToString() {
			return $"RendererLineSegments({X}, {Y}, {Z})";
		}
	}
}
