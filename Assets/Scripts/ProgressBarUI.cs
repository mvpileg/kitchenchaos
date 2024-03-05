using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBarUI : MonoBehaviour {
 
    [SerializeField] private Image normalBarImage;
    [SerializeField] private Image warningBarImage;
    [SerializeField] private GameObject hasProgressGameObject;
    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.LogError("Game object " + hasProgressGameObject + " does not have component with IHasProgress");
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        normalBarImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        if (e.shouldShowProgress) {
            Show();
            switch (e.state) {
                case IHasProgress.State.Normal:
                    normalBarImage.fillAmount = e.progressNormalized;
                    warningBarImage.fillAmount = 0f;
                    break;
                case IHasProgress.State.Warning:
                    normalBarImage.fillAmount = 1f;
                    warningBarImage.fillAmount = e.progressNormalized;
                    break;
            }
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
