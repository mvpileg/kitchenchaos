using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBarUI : MonoBehaviour {
 
    [SerializeField] private Image barImage;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Start() {
        cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void CuttingCounter_OnProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e) {
        if (e.canCut) {
            barImage.fillAmount = e.progressNormalized;
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
