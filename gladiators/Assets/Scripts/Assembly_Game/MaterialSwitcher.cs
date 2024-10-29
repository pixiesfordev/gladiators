using Cysharp.Threading.Tasks;
using Scoz.Func;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSwitcher : MonoBehaviour {
    public Material newMaterial;
    public Image uiImage1;
    public Image uiImage2;

    void Start() {

        UniTask.Void(async () => {
            await UniTask.Delay(3000);
            SwitchMaterial();
        });

    }

    public void SwitchMaterial() {
        // 切換材質
        WriteLog.Log("change");
        uiImage1.material = Instantiate(newMaterial);
        uiImage2.material = Instantiate(newMaterial);
        SetMetallicValue();
    }
    public void SetMetallicValue() {
        UniTask.Void(async () => {
            var mat = uiImage1.material;
            float value = 1f;
            while (value > 0) {
                value -= 0.05f;
                mat.SetFloat("_Metallic", value);
                await UniTask.Delay(50);
            }
        });
    }
}
