using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dpGame;

public class ClickEvent :MonoBehaviour {
    Interaction interaction;
    dpGame.CharacterController character;
    public void setChrController (dpGame.CharacterController chr) {
        character = chr;
    }

    void Start() {
        interaction = Director.getInstance().currentSceneController as Interaction;

    }

    private void OnMouseDown()
    {   
        if (gameObject.name == "boat") {
            interaction.MoveBoat();
        }
        else {
            interaction.moveCharacters(character);
        }
    }

}