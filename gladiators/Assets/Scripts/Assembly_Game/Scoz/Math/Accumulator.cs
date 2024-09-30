namespace Scoz.Func {
    public class Accumulator {
        private int counter;
        private readonly object lockObj;
        public Accumulator() {
            counter = 0;
            lockObj = new object();
        }

        // 取得下一個流水號
        public int GetNextIdx() {
            lock (lockObj) {
                if (counter + 1 <= int.MaxValue) {
                    counter += 1;
                } else {
                    counter = 0;
                }
                return counter;
            }
        }
    }
}
