using UnityEngine;
using System.Collections;

using LibNoisePerlin = LibNoise.Unity.Generator.Perlin;
using LibNoise.Unity;

namespace BDream.Noise {
	public class Mask : Source {
		[SerializeField]
		Source _mask;

		[SerializeField]
		Source _source;

		[SerializeField]
		[Range(0f, 1f)]
		float _threshold;

		[SerializeField]
		bool _invert = false;

		public override float GetFloat(Vector3 xyz) {
			if (_invert) {
				if (_mask.GetFloat(xyz) > _threshold) {
					return _source.GetFloat(xyz);
				}
			} else {
				if (_mask.GetFloat(xyz) < _threshold) {
					return _source.GetFloat(xyz);
				}
			}
			return 0f;
		}

		public override void SetSeed(int seed) {
			_mask.SetSeed(seed);
			_source.SetSeed(seed * seed);
		}

		public override bool IsValid() {
			return !(_mask == null || _source == null);
		}
	}
}