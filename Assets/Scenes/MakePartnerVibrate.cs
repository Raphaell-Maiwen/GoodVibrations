using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using System.Linq;

public class MakePartnerVibrate : MonoBehaviour
{
    //PlayerIndex playerIndex;
    Partner partner0;
    Partner partner1;

    GamePadState state0;
    GamePadState state1;

    //string[] buttons;

    float vibratingLength = 1f;
    float vibratingTime = 0f;
    bool something = true;

    float vibratingLength1 = 1f;
    float vibratingTime1 = 0f;
    bool something1 = true;

    enum button
    {
        A,
        B,
        X,
        Y
    }

    // Start is called before the first frame update
    void Awake() {
        //buttons = new string[]{"UpButton", "DownButton", "LeftButton", "RightButton"};

        partner0 = new Partner((PlayerIndex)0, RandomButton());
        partner1 = new Partner((PlayerIndex)1, RandomButton());

        //For testing purposes

        //partner0 = new Partner(0, buttons[Random.Range(0,4)]);

        state0 = GamePad.GetState(((PlayerIndex)0));
        state1 = GamePad.GetState((PlayerIndex)1);
    }

    void FixedUpdate() {
        //old
        /*GamePad.SetVibration((PlayerIndex)0, state1.Triggers.Left, state1.Triggers.Right);
        GamePad.SetVibration((PlayerIndex)1, state0.Triggers.Left, state0.Triggers.Right);*/

        //GamePad.SEt
    }

    // Update is called once per frame
    void Update() {
        //old
        /*state0 = GamePad.GetState((PlayerIndex)0);
        state1 = GamePad.GetState((PlayerIndex)1);*/

        partner0.state = GamePad.GetState(partner0.playerIndex);
        partner1.state = GamePad.GetState(partner1.playerIndex);

        if (partner0.state.Buttons.A == ButtonState.Pressed && partner0.currentButton == button.A && something) {
            Debug.Log("Yo");
            GamePad.SetVibration(partner1.playerIndex, 0.5f, 0.5f);
            something = false;
        }
        else if (partner0.state.Buttons.B == ButtonState.Pressed && partner0.currentButton == button.B && something) {
            Debug.Log("Yo");
            GamePad.SetVibration(partner1.playerIndex, 0.5f, 0.5f);
            something = false;
        }
        else if (partner0.state.Buttons.X == ButtonState.Pressed && partner0.currentButton == button.X && something) {
            Debug.Log("Yo");
            GamePad.SetVibration(partner1.playerIndex, 0.5f, 0.5f);
            something = false;
        }
        else if (partner0.state.Buttons.Y == ButtonState.Pressed && partner0.currentButton == button.Y && something) {
            Debug.Log("Yo");
            GamePad.SetVibration(partner1.playerIndex, 0.5f, 0.5f);
            something = false;
        }




        if (partner1.state.Buttons.A == ButtonState.Pressed && partner1.currentButton == button.A && something) {
            Debug.Log("Yo");
            GamePad.SetVibration(partner0.playerIndex, 0.5f, 0.5f);
            something1 = false;
        }
        else if (partner1.state.Buttons.B == ButtonState.Pressed && partner1.currentButton == button.B && something) {
            Debug.Log("Yo");
            GamePad.SetVibration(partner0.playerIndex, 0.5f, 0.5f);
            something1 = false;
        }
        else if (partner1.state.Buttons.X == ButtonState.Pressed && partner1.currentButton == button.X && something) {
            Debug.Log("Yo");
            GamePad.SetVibration(partner0.playerIndex, 0.5f, 0.5f);
            something1 = false;
        }
        else if (partner1.state.Buttons.Y == ButtonState.Pressed && partner1.currentButton == button.Y && something) {
            Debug.Log("Yo");
            GamePad.SetVibration(partner0.playerIndex, 0.5f, 0.5f);
            something1 = false;
        }

        //+Add some gauge
        if (!something) {
            vibratingTime += Time.deltaTime;
            if (vibratingTime >= vibratingLength) {
                something = true;
                GamePad.SetVibration(partner1.playerIndex, 0f, 0f);
                vibratingTime = 0f;
            }
        }


        if (!something1) {
            vibratingTime1 += Time.deltaTime;
            if (vibratingTime1 >= vibratingLength1) {
                something1 = true;
                GamePad.SetVibration(partner0.playerIndex, 0f, 0f);
                vibratingTime1 = 0f;
            }
        }
    }

    button RandomButton() {
        System.Array values = System.Enum.GetValues(typeof(button));
        System.Random r = new System.Random();
        button randomButton = (button)values.GetValue(r.Next(values.Length));

        return randomButton;
    }

    class Partner
    {
        public GamePadState state;
        public PlayerIndex playerIndex;

        public button currentButton;
        public bool leftVibrate = false;
        public bool rightVibrate = false;

        public Partner(PlayerIndex newIndex, button newCurrentButton) {
            playerIndex = newIndex;
            currentButton = newCurrentButton;
        }
    }
}