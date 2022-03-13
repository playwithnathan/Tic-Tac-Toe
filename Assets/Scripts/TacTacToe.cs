using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacTacToe : MonoBehaviour {
    public Transform[] slots = new Transform[9];

    public Image UIX;
    public Image UIO;

    public Text UIWinsX;
    public Text UIWinsO;
    public Text UIWinText;

    bool turn = false;
    int turnNumber = 0;

    int winsX = 0;
    int winsO = 0;

    void Update() {
        // If the mouse button is not pressed down then ignore
        if(!Input.GetMouseButtonDown(0)) return;

        foreach(Transform slot in slots) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 slotPos = slot.position;

            if((mousePos.x < slotPos.x + 1.5f && mousePos.x > slotPos.x - 1.5f) &&
            (mousePos.y < slotPos.y + 1.5f && mousePos.y > slotPos.y - 1.5f)) {
                TakeTurn(slot);
                break;
            }
        }
    }

    void TakeTurn(Transform slot) {
        // Reset game win text
        UIWinText.text = "";

        // Get x and o objects
        GameObject slotO = slot.GetChild(0).gameObject;
        GameObject slotX = slot.GetChild(1).gameObject;

        // If x or o object is already active then ignore this slot
        if(slotX.activeSelf) return;
        if(slotO.activeSelf) return;

        if(turn) {
            // O
            slotO.gameObject.SetActive(true);

            // Change UI text for whos turn it is
            UIO.gameObject.SetActive(false);
            UIX.gameObject.SetActive(true);

            // Check if O has won
            if(WinCheck()) {
                winsO += 1;
                UIWinText.text = "O Won!";

                ResetBoard();
                return;
            }

            // Set the turn to X
            turn = false;
        } else {
            // X
            slotX.gameObject.SetActive(true);
            
            // Change UI text for whos turn it is
            UIX.gameObject.SetActive(false);
            UIO.gameObject.SetActive(true);

            // Check if X has won
            if(WinCheck()) {
                winsX += 1;
                UIWinText.text = "X Won!";

                ResetBoard();
                return;
            }

            // Set the turn to O
            turn = true;
        }

        turnNumber++;

        if(turnNumber >= 9) {
            UIWinText.text = "Draw!";
            ResetBoard();
        }
    }

    bool WinCheck() {
        bool[] claimed = new bool[9];
        int child = turn ? 0 : 1;

        // Create array of claimed spaces
        for(int i = 0; i < slots.Length; i++) {
            bool activeChild = slots[i].GetChild(child).gameObject.activeSelf;
            if(activeChild) claimed[i] = true;
            else claimed[i] = false;
        }

        // Horizontal
        if(claimed[0] && claimed[1] && claimed[2]) return true;
        if(claimed[3] && claimed[4] && claimed[5]) return true;
        if(claimed[6] && claimed[7] && claimed[8]) return true;

        // Vertical
        if(claimed[0] && claimed[3] && claimed[6]) return true;
        if(claimed[1] && claimed[4] && claimed[7]) return true;
        if(claimed[2] && claimed[5] && claimed[8]) return true;

        // Diagonal
        if(claimed[0] && claimed[4] && claimed[8]) return true;
        if(claimed[2] && claimed[4] && claimed[6]) return true;

        return false;
    }

    void ResetBoard() {
        turn = false;
        turnNumber = 0;

        UIO.gameObject.SetActive(false);
        UIX.gameObject.SetActive(true);

        UIWinsX.text = winsX.ToString();
        UIWinsO.text = winsO.ToString();

        foreach(Transform slot in slots) {
            GameObject slotO = slot.GetChild(0).gameObject;
            GameObject slotX = slot.GetChild(1).gameObject;

            slotO.SetActive(false);
            slotX.SetActive(false);
        }
    }
}