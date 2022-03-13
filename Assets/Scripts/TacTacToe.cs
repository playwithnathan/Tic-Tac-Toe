using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacTacToe : MonoBehaviour {
    public Button[] slots = new Button[9];

    public Image UIBackground;
    public Image UIGrid;

    public Image XHighlight;
    public Image OHighlight;

    public Text UIX;
    public Text UIO;

    public Text UIXWins;
    public Text UIOWins;

    public Color background;
    public Color grid;
    public Color highlighted;
    public Color XColor;
    public Color OColor;

    bool turn = false;
    int turnNumber = 0;

    bool[] claimedX = new bool[9];
    bool[] claimedO = new bool[9];

    int winsX = 0;
    int winsO = 0;

    private void Start() {
        foreach(Button slot in slots)
            slot.onClick.AddListener(() => OnClick(slot));
        ResetBoard();
    }

    private void Update() {
        // Set big X and O to their proper colors
        UIX.color = XColor;
        UIO.color = OColor;

        // Set all X's to their proper colors
        for(int i = 0; i < slots.Length; i++) {
            slots[i].image.color = background;
            Text text = slots[i].GetComponentInChildren<Text>();
            if(claimedX[i]) text.color = XColor;
            if(claimedO[i]) text.color = OColor;
        }

        // Set highlight color for whos turn it is
        if(turn) {
            XHighlight.color = background;
            OHighlight.color = highlighted;
        } else {
            OHighlight.color = background;
            XHighlight.color = highlighted;
        }

        // Set background to proper color
        UIBackground.color = background;

        // Set grid to proper color
        UIGrid.color = grid;
    }

    void OnClick(Button slot) {
        bool[] claimed = turn ? claimedO : claimedX;
        slot.GetComponentInChildren<Text>().text = turn ? "O" : "X";

        // Make slot uninteractable
        slot.interactable = false;

        // Add to claimed spots
        claimed.SetValue(true, int.Parse(slot.name.Split(' ')[1]));

        // Check if O has won
        if(WinCheck()) {
            winsO += turn ? 1 : 0;
            winsX += !turn ? 1 : 0;

            ResetBoard();
            return;
        }

        // Update the turn
        turn = turn ? false : true;

        // Update the total number of turns taken
        turnNumber++;

        // If there has been 9 turns taken then reset the board, its a draw
        if(turnNumber >= 9) ResetBoard();
    }

    bool WinCheck() {
        // Claimed list is based upon whos turn it is
        bool[] claimed = turn ? claimedO : claimedX;

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

        // Update wins
        UIXWins.text = winsX.ToString();
        UIOWins.text = winsO.ToString();

        // Set all claimed spots to false
        for(int i = 0; i < 9; i++) {
            claimedX.SetValue(false, i);
            claimedO.SetValue(false, i);
        }

        // Set all slots to be interactable and empty
        foreach(Button slot in slots) {
            slot.interactable = true;
            slot.GetComponentInChildren<Text>().text = "";
        }
    }
}