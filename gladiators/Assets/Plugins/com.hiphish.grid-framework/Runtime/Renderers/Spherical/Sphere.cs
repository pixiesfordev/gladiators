using UnityEngine;
using GridFramework.Rendering;

namespace GridFramework.Renderers.Spherical {
	/// <summary>
	///   A sphere shape around the origin, consists of latitudes, longitudes and radial lines from
	///   the origin.
	/// </summary>
	/// <remarks>
	///   <para>
	///     It is possible to render only a partial shape by setting the starting and ending
	///     meridian and parallel accordingly. The radius can be set so that more than one sphere is
	///     drawn around the origin.
	///   </para>
	/// </remarks>
	[AddComponentMenu("Grid Framework/Renderers/Spherical/Sphere")]
	public sealed class Sphere : SphericalRenderer {

#region  Private variables
		[SerializeField] private Vector3 _from = new Vector3(0, 0, 0);
		[SerializeField] private Vector3 _to   = new Vector3(1f, 1f, 1f);

		[SerializeField] private int _smoothM = 1;
		[SerializeField] private int _smoothP = 1;

		[SerializeField] private bool _hasBeenInitialized;
#endregion  // Private variables

#region  Properties
		/// <summary>
		///   Distance of the start of radial lines from the origin.
		/// </summary>
		public float AltFrom {
			get => _from[0];
			set {
				_from[0] = Mathf.Max(0, value);
				_from[0] = Mathf.Min(_to[0], _from[0]);
				MarkAsDirty();
			}
		}

		/// <summary>
		///   Distance of the end of radial lines from the origin.
		/// </summary>
		public float AltTo {
			get => _to[0];
			set {
				_to[0] = Mathf.Max(value, _from[0]);
				MarkAsDirty();
			}
		}

		/// <summary>
		///   Which of longitude the rendering should start at.
		/// </summary>
		public float LonFrom {
			get => _from[1];
			set {
				_from[1] = Mathf.Max(0, value);
				_from[1] = Mathf.Min(_to[1], _from[1]);
				MarkAsDirty();
			}
		}

		/// <summary>
		///   Which of longitude the rendering should end at.
		/// </summary>
		public float LonTo {
			get => _to[1];
			set {
				_to[1] = Mathf.Max(value, _from[1]);
				_to[1] = Mathf.Min(_to[1], Grid.Parallels - 1);
				MarkAsDirty();
			}
		}

		/// <summary>
		///   Which of latitude the rendering should start at.
		/// </summary>
		public float LatFrom {
			get => _from[2];
			set {
				_from[2] = Float2Sector(value, Grid.Meridians);
				MarkAsDirty();
			}
		}

		/// <summary>
		///   Which of latitude the rendering should end at.
		/// </summary>
		public float LatTo {
			get => _to[2];
			set {
				_to[2] = Float2Sector(value, Grid.Meridians);
				MarkAsDirty();
			}
		}

		/// <summary>
		///   Subdivides the meridians to create a smoother look.
		/// </summary>
		/// <value>
		///   Smoothness of the grid segments.
		/// </value>
		/// <remarks>
		///   <para>
		///     Unity can only draw straight lines, so in order to get the meridians to look round
		///     this value breaks each meridian into smaller segments. The number of smoothness
		///     tells how many segments the circular line has been broken into. The amount of end
		///     points used is smoothness + 1, because we count both edges of the sector.
		///   </para>
		/// </remarks>
		public int SmoothM {
			get => _smoothM;
			set {
				_smoothM = Mathf.Max(value, 1);
				MarkAsDirty();
			}
		}

		/// <summary>
		///   Subdivides the parallels to create a smoother look.
		/// </summary>
		/// <value>
		///   Smoothness of the grid segments.
		/// </value>
		/// <remarks>
		///   <para>
		///     Unity can only draw straight lines, so in order to get the parallels to look round
		///     this value breaks each parallel into smaller segments. The number of smoothness
		///     tells how many segments the circular line has been broken into. The amount of end
		///     points used is smoothness + 1, because we count both edges of the sector.
		///   </para>
		/// </remarks>
		public int SmoothP {
			get => _smoothP;
			set {
				_smoothP = Mathf.Max(value, 1);
				MarkAsDirty();
			}
		}
#endregion  // Properties

#region  Count
		protected override (int x, int y, int z) CountLines() {
			// If the from-angle is greater than the to angle wrap the to-angle around once.
			var wrapAround = _from[2] > _to[2];

			if (wrapAround) {
				_to[2] += Grid.Meridians;
			}

			// Deltas for spheres, parallels and meridians
			var deltaS = Mathf.FloorToInt(AltTo) - Mathf.CeilToInt(AltFrom) + 1;
			var deltaP = Mathf.FloorToInt(LonTo) - Mathf.CeilToInt(LonFrom) + 1;
			var deltaM = Mathf.FloorToInt(LatTo) - Mathf.CeilToInt(LatFrom) + 1;

			if (Mathf.Abs(LonTo - Grid.Parallels) <= Mathf.Epsilon) {
				--deltaP;
			} if (Mathf.Abs( LatTo - Grid.Meridians) <= Mathf.Epsilon) {
				--deltaM;
			} if (Mathf.Abs(AltFrom) <= Mathf.Epsilon) {
				--deltaS;
			}

			// Smoothed deltas
			var deltaPs = Mathf.FloorToInt(LonTo*SmoothM) - Mathf.CeilToInt(LonFrom*SmoothM) + 2;
			var deltaMs = Mathf.FloorToInt(LatTo*SmoothP) - Mathf.CeilToInt(LatFrom*SmoothP) + 2;

			var xCount = deltaM *  deltaP;
			var yCount = deltaS * deltaM * deltaPs;
			var zCount = deltaS * deltaP * deltaMs;

			if (Mathf.Abs(LonFrom) <= Mathf.Epsilon) {
				++xCount;
			} if (Mathf.Abs(LonTo - Grid.Parallels) <= Mathf.Epsilon) {
				++xCount;
			} if (wrapAround) {
				_to[2] -= Grid.Meridians;
			}

			return (xCount, yCount, zCount);
		}
#endregion  // Count

#region  Compute
		protected override void ComputeLines(LineSegment[][] lineSegments) {
			ComputeSpokeLines(   lineSegments[0]);
			ComputeMeridianLines(lineSegments[1]);
			ComputeParallelLines(lineSegments[2]);
		}

		private void ComputeSpokeLines(LineSegment[] lines) {
			var o = transform.position;
			var up = transform.up;
			var right = transform.right;

			int firstParallel = Mathf.CeilToInt(LonFrom), lastParallel = Mathf.FloorToInt(LonTo);
			int firstMeridian = Mathf.CeilToInt(LatFrom), lastMeridian = Mathf.FloorToInt(LatTo);
			var hasMeridians = lastMeridian - firstMeridian >= 0
				&& !(lastMeridian == Grid.Meridians && firstMeridian == Grid.Meridians);

			var northLine = firstParallel == 0 && hasMeridians;
			var southLine = lastParallel == Grid.Parallels - 1 && hasMeridians;

			if (northLine) {
				++firstParallel;
			} if (southLine) {
				--lastParallel;
			} if (lastMeridian == Grid.Meridians) {
				--lastMeridian;
			}

			var i = 0;
			for (var p = firstParallel; p <= lastParallel; ++p) {
				var v_p = Quaternion.AngleAxis(-p * Grid.PolarDeg, -right) * up;
				for (var m = firstMeridian; m <= lastMeridian; ++m) {
					var v_m = Quaternion.AngleAxis(m * Grid.AzimuthDeg, -up) * v_p;
					var from = o + v_m * _from[0] * Grid.Radius;
					var   to = o + v_m *   _to[0] * Grid.Radius;
					lines[i++] = new LineSegment(from, to);
				}
			}

			// Lines at the poles
			if (northLine) { // North
				var from = o + up * _from[0] * Grid.Radius;
				var   to = o + up *   _to[0] * Grid.Radius;

				lines[i++] = new LineSegment(from, to);
			} if (southLine) { // South
				var from = o - up * _from[0] * Grid.Radius;
				var   to = o - up *   _to[0] * Grid.Radius;

				lines[i++] = new LineSegment(from, to);
			}
		}

		private void ComputeMeridianLines(LineSegment[] lines) {
			var o = transform.position;
			var up = transform.up;
			var forward = transform.forward;

			var firstSphere = Mathf.CeilToInt( AltFrom);
			var lastSphere  = Mathf.FloorToInt(AltTo  );

			var firstMeridian = Mathf.CeilToInt( LatFrom);
			var lastMeridian  = Mathf.FloorToInt(LatTo  );

			var firstParallel = Mathf.CeilToInt( LonFrom * SmoothM);
			var lastParallel  = Mathf.FloorToInt(LonTo   * SmoothM);

			if (firstSphere == 0) {
				++firstSphere;
			} if (lastMeridian == Grid.Meridians) {
				--lastMeridian;
			}

			var i = 0;
			for (var s = firstSphere; s <= lastSphere; ++s) {
				// Sphere construction vector
				var v_s = forward * s * Grid.Radius;

				for (var m = firstMeridian; m <= lastMeridian; ++m) {
					// Meridian construction vector
					Vector3 from, to;
					var v_m = Quaternion.AngleAxis(m * Grid.AzimuthDeg, -up) * v_s;
					var axis = Vector3.Cross(v_m, up);

					from = o + Quaternion.AngleAxis(90.0f - _from[1] * Grid.PolarDeg, axis) * v_m;
					for (var p = firstParallel; p <= lastParallel; ++p) {
						var v_p = Quaternion.AngleAxis(90.0f - p * Grid.PolarDeg/SmoothM, axis) * v_m;
						to = o + v_p;
						lines[i++] = new LineSegment(from, to);
						from = to;
					}
					to = o + Quaternion.AngleAxis(90.0f - _to[1] * Grid.PolarDeg, axis) * v_m;
					lines[i++] = new LineSegment(from, to);
				}
			}
		}

		private void ComputeParallelLines(LineSegment[] lines) {
			var o = transform.position;
			var up = transform.up;
			var right = transform.right;

			var firstSphere = Mathf.CeilToInt( AltFrom);
			var lastSphere  = Mathf.FloorToInt(AltTo  );

			var firstParallel = Mathf.Max(Mathf.CeilToInt( _from[1]), 1);
			var lastParallel  = Mathf.Min(Mathf.FloorToInt(  _to[1]),  Grid.Parallels-1);

			var firstMeridian = Mathf.CeilToInt(_from[2] * SmoothP);
			var lastMeridian  = Mathf.FloorToInt( _to[2] * SmoothP);

			if (firstSphere == 0) {
				++firstSphere;
			}

			var i = 0;
			for (var s = firstSphere; s <= lastSphere; ++s) {
				// Sphere construction vector
				var v_s = up * s * Grid.Radius;

				for (var p = firstParallel; p <= lastParallel; ++p) {
					Vector3 from, to;
					// Parallel construction vector
					var v_p = Quaternion.AngleAxis(p * Grid.PolarDeg, right) * v_s;

					from = o + Quaternion.AngleAxis(_from[2] * Grid.AzimuthDeg, -up) * v_p;
					for (var m = firstMeridian; m <= lastMeridian; ++m) {
						var v_m = Quaternion.AngleAxis(m * Grid.AzimuthDeg/SmoothP, -up) * v_p;
						to = o + v_m;
						lines[i++] = new LineSegment(from, to);
						from = to;
					}
					to = o + Quaternion.AngleAxis(_to[2]* Grid.AzimuthDeg, -up) * v_p;
					lines[i++] = new LineSegment(from, to);
				}
			}
		}
#endregion  // Compute

#region  Start
		private void Start() {
			if (_hasBeenInitialized) {
				return;
			}

			LonTo = Grid.Parallels - 1;
			LatTo = Grid.Meridians;
			_hasBeenInitialized = true;
		}
#endregion  // Start

#region  Helper
		private static float Float2Sector(float number, float amount) {
			if (number > amount) {
				return number % amount;
			} if (number < 0f) {
				return amount + (number % amount);
			}
			return number;
		}
#endregion  // Helper
	}
}
