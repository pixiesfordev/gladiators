using UnityEngine;

namespace Scoz.Func {
    [System.Serializable]
    public struct MinMaxF {
        public float X;
        public float Y;
        public MinMaxF(float _x, float _y) {
            X = _x;
            Y = _y;
        }
        public override string ToString() {
            return "Min:" + X.ToString() + " Max:" + Y.ToString();
        }
        public float GetRandInRange() {
            return Random.Range(X, Y);
        }
        public float GetSub() {
            return X - Y;
        }
        public float GetSum() {
            return X + Y;
        }
    }
}