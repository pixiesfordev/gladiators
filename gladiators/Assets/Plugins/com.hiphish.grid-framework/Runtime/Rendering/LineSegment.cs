using System.Collections.Generic;
using UnityEngine;
using IndexOutOfRangeException = System.IndexOutOfRangeException;

namespace GridFramework.Rendering {
	/// <summary>
	///   Data class which represents a straight line segment in 3D space, effectively a pair of two
	///   points. The coordinates are in world space unless otherwise noted.
	/// </summary>
	/// <seealso cref="RendererLineSegments"/>
	/// <seealso cref="LineSegmentGroup"/>
	public class LineSegment {
		/// <summary>
		///   Create a new line segment with starting- and end point.
		/// </summary>
		/// <param name="from"> The starting point. </param>
		/// <param name="to"> The end point. </param>
		public LineSegment(Vector3 from, Vector3 to) {
			From = from; To = to;
		}

		/// <summary> Starting point of the line segment. </summary>
		public Vector3 From {get; private set;}
		/// <summary> End point of the line segment. </summary>
		public Vector3 To {get; private set;}

		/// <summary>
		///   Indexer for the end points of the segment.
		/// </summary>
		/// <param name="i">
		///   Use <c>0</c> for the starting point, <c>1</c> for end point.  Every other value throws
		///   an error.
		/// </param>
		/// <remarks>
		///   The indexer is added as a convenience for use in loops.
		/// </remarks>
		/// <seealso cref="From"/>
		/// <seealso cref="To"/>
		public Vector3 this[int i] {
			get {
				switch (i) {
					case 0: return From;
					case 1: return   To;
				
					default:
						throw new IndexOutOfRangeException($"Only index 0 and 1 allowed, given: {i}");
				}
			}
		}

		/// <summary>
		///   The default enumerator return the starting point and then the end point.
		/// </summary>
		/// <seealso cref="From"/>
		/// <seealso cref="To"/>
		public IEnumerator<Vector3> GetEnumerator () {
			yield return From;
			yield return To;
		}

		public override string ToString() {
			return $"LineSegment({From}, {To})";
		}
	}
}
