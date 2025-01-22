using Cysharp.Threading.Tasks;
using Scoz.Func;
using UnityEngine;

namespace Gladiators.Cuisine {
    public class CusineCard {

        public int Idx { get; private set; }
        public int ID { get; private set; }

        public bool IsFaceUp { get; set; }

        public bool IsMatched { get; set; }


        public CusineCard(int _idx, int _id) {
            Idx = _idx;
            ID = _id;
            IsFaceUp = false;
            IsMatched = false;
        }
        public async UniTask<Sprite> GetSprite() {
            var tcs = new UniTaskCompletionSource<Sprite>();

            AddressablesLoader.GetSpriteAtlas("CusineCard", atlas => {
                var sprite = atlas.GetSprite(ID.ToString());
                tcs.TrySetResult(sprite);
            });

            return await tcs.Task;
        }
    }
}
