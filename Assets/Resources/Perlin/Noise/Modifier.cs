using UnityEngine;
using System.Collections;

using LibNoisePerlin = LibNoise.Unity.Generator.Perlin;
using LibNoise.Unity;

namespace BDream.Noise {
	public class Modifier : Source {
		[SerializeField]
		Source _source;

		[SerializeField]
		AnimationCurve _curve;

		public override float GetFloat(Vector3 xyz) {

			return _curve.Evaluate(_source.GetFloat(xyz));
		}

		public override void SetSeed(int seed) {
			_source.SetSeed(seed);
		}

		public override bool IsValid() {
			return !(_source == null);
		}
	}
}