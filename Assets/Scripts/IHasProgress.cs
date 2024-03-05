using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress {

    public enum State {
        Normal,
        Warning
    }

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs {
        public bool shouldShowProgress = true;
        public float progressNormalized = 0f;
        public State state = State.Normal;
    }


}