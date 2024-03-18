using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;


    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();        
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer > 0f) {
            return;
        }
        spawnRecipeTimer = spawnRecipeTimerMax;
        if (waitingRecipeSOList.Count >= waitingRecipesMax) {
            return;
        }
        AddNewRecipe();
    }

    private void AddNewRecipe() {
        RecipeSO newRecipe = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
        waitingRecipeSOList.Add(newRecipe);
        Debug.Log(newRecipe.name);
    }

    public bool DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        List<KitchenObjectSO> plateIngredients = plateKitchenObject.GetKitchenObjectSOList();

        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (Matches(waitingRecipeSO.kitchenObjectSOList, plateIngredients)) {
                Debug.Log("Player delivered correct recipe: " + waitingRecipeSO.name);
                waitingRecipeSOList.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    private bool Matches(List<KitchenObjectSO> kitchenObjectSOs1, List<KitchenObjectSO> kitchenObjectsSOs2) {
        if (kitchenObjectSOs1.Count != kitchenObjectsSOs2.Count) {
            return false;
        }
        foreach (KitchenObjectSO kitchenObjectSO in kitchenObjectSOs1) {
            if (!kitchenObjectsSOs2.Contains(kitchenObjectSO)) {
                return false;
            }
        }
        return true;
    }

}
