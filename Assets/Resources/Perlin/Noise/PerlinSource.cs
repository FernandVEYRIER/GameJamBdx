using UnityEngine;
using System.Collections;

using LibNoisePerlin = LibNoise.Unity.Generator.Perlin;
using LibNoise.Unity;

namespace BDream.Noise {
	public class PerlinSource : Source {
		[SerializeField]
		float _period = 10.0f;
		public float Period { get { return _period; } set { _period = value; } }
		
		[SerializeField]
		float _lacunarity = 2.0f;
		public float Lacunarity { get { return _lacunarity; } set { _lacunarity = value; } }
		
		[SerializeField]
		QualityMode _quality = QualityMode.Medium;
		public QualityMode Quality { get { return _quality; } set { _quality = value; } }
		
		[SerializeField]
		int _octaveCount = 6;
		public int OctaveCount { get { return _octaveCount; } set { _octaveCount = value; } }
		
		[SerializeField]
		float _persistence = 0.5f;
		public float Persistence { get { return _persistence; } set { _persistence = value; } }
		
		[SerializeField]
		int _seed = 0;
		public int Seed { get { return _seed; } set { _seed = value; } }
		
		LibNoisePerlin _perlin = new LibNoisePerlin();
		
		public override float GetFloat(Vector3 xyz) {
			return Mathf.Clamp((GetFloatRaw(xyz) + 1.5f) / 3f, 0f, 1f);
		}

		public override void SetSeed(int seed) {
			_seed = seed;
		}
		
		float GetFloatRaw(Vector3 xyz) {
			if (_period == 0.0f) {
				_period = 10.0f;
			}
			_perlin.Frequency = 1 / _period;
			_perlin.Lacunarity = _lacunarity;
			_perlin.Quality = _quality;
			_perlin.OctaveCount = _octaveCount;
			_perlin.Persistence = _persistence;
			_perlin.Seed = _seed;
			return (float)_perlin.GetValue(xyz.x, xyz.y, xyz.z); // value [-1.5,1.5] 
		}

		public override string ToString() {
			string s = "PerlinData:";
			s += "\n - period: " + _period;
			s += "\n - lacunarity: " + _lacunarity;
			s += "\n - quality: " + _quality;
			s += "\n - octave: " + _octaveCount;
			s += "\n - seed: " + _seed;
			return s;
		}

		public override bool IsValid() {
			return true;
		}
	}
}